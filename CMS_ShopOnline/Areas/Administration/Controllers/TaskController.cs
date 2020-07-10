using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_ShopOnline.Areas.Administration.Models;
using CMS_ShopOnline.Hubs;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.Administration.Controllers
{
    public class TaskController : Controller
    {
        
        private readonly ICongViec CongViec;
        public TaskController()
        {
            CongViec = new CongViecRepository();
        }
        public TaskController(ICongViec _CongViec)
        {
            CongViec = _CongViec;
        }
        // GET: Administration/CongViec
        public ActionResult Index()
        {
            ViewBag.listcongviec = CongViec.GetAll();
            return View();
        }
        public static void CreateTask(string NameTask, string Controller, string Action, string Areas, int UserIDCreate,int IdPx)
        {
            try
            {
                Delete();
                ICongViec _CongViec = new CongViecRepository();
                var task = new CongViec();
                task.IdNhanVien = UserIDCreate;
                task.IdPhieuXuat = IdPx;
                task.Ten = NameTask;
                task.NgayTao = DateTime.Now;
                task.Controller = Controller;
                task.Action = Action;
                task.Area = Areas;
                task.IsDelete = false;
                _CongViec.Insert(task);
                _CongViec.Save();
                MyHub.Noti(NameTask, Controller, Action, Areas,Helpers.Helper.CurrentUser.TenNV, task.Id, Convert.ToString(task.NgayTao), IdPx);
            }
            catch(Exception e)
            {

            }
           
        }
        public static void Delete()
        {
            ICongViec _CongViec = new CongViecRepository();
            var q = _CongViec.SelectAll();
            foreach(var item in q)
            {
                if( ngay(item.NgayTao) >= 2)
                {
                    _CongViec.Delete(item.Id);
                    _CongViec.Save();
                }
            }
        }
        public static int ngay(DateTime ngaynhap)
        {
            DateTime datetimenow = DateTime.Now;
            int i = 0;
            TimeSpan diff1 = datetimenow.Subtract(ngaynhap);
            i = diff1.Days;
            return i;
        }
        // GET: Administration/CongViec/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Administration/CongViec/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administration/CongViec/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Administration/CongViec/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Administration/CongViec/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Administration/CongViec/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Administration/CongViec/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
