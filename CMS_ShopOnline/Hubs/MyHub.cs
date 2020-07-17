using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading;
using CMS_ShopOnline.Helpers;
using System;

namespace CMS_ShopOnline.Hubs
{
    [HubName("myHub")]
    public class MyHub : Hub
    {
        private static IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
        public static List<UserOnline> UserOnlines = new List<UserOnline>();
        
        public override System.Threading.Tasks.Task OnConnected()
        {
            //lấy list user đang đăng nhập
            var userlogin = Helper.CurrentUser.Id;
            if (UserOnlines.Where(x => x.UserLogginId == userlogin).FirstOrDefault() == null)
            {
                UserOnline user = new UserOnline()
                {
                    Name = Context.User.Identity.Name,
                    ConnectionID = Context.ConnectionId,
                    UserLogginId = userlogin
                };
                UserOnlines.Add(user);
                Clients.Others.userConnected(user.UserLogginId);
            }
            return base.OnConnected();
        }
        public override System.Threading.Tasks.Task OnReconnected()
        {

            return base.OnReconnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {

            if (stopCalled)
            {
                if (UserOnlines.Any(x => x.ConnectionID == Context.ConnectionId))
                {
                    UserOnline user = UserOnlines.First(x => x.ConnectionID == Context.ConnectionId);

                    //xoa khoi danh sach cac user dang dang nhap
                    UserOnlines.RemoveAll(x => x.ConnectionID == Context.ConnectionId);

                    user.ConnectionID = null;
                    int count = 5;
                    bool isDisconnected = true;
                    while (count > 0)
                    {
                        if (string.IsNullOrEmpty(user.ConnectionID))
                        {
                            Thread.Sleep(1000);
                            count--;
                        }
                        else
                        {
                            isDisconnected = false;
                            break;
                        }
                    }

                    if (isDisconnected)
                    {
                        Clients.Others.userLeft(user.UserLogginId);
                        UserOnlines.Remove(user);
                    }
                }
            }

            return base.OnDisconnected(stopCalled);
        }
        public static void Noti(string NameTask, string Controller, string Action,string Areas, string UserIDCreate,int? Idtask ,string DateTime,int IdPx)
        {
            var listStaffCanView = UserOnlines.Select(x => x.ConnectionID).ToList();
            context.Clients.Clients(listStaffCanView).updatedClients(NameTask, Controller, Action, Areas, UserIDCreate, Idtask, DateTime,IdPx);  
        }
        public static void PurchaseOder(int? Id, DateTime NgayTao,int? IdNhanVien, int? IdKhachHang, string TenNV, string TenKH, string GhiChu, string TrangThai, double? TongTien)
        {
            var listStaffCanView = UserOnlines.Select(x => x.ConnectionID).ToList();
            context.Clients.Clients(listStaffCanView).updatedPurchaseOder(Id,  NgayTao,  IdNhanVien,  IdKhachHang,  TenNV,  TenKH, GhiChu ,  TrangThai,  TongTien);  
        }
        public class UserOnline
        {
            public string Name;
            public string ConnectionID;
            public int UserLogginId;
        }

    }
}