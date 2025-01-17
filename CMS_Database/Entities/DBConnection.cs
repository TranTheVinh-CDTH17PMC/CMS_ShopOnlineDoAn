namespace CMS_Database.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DBConnection : DbContext
    {
        public DBConnection()
            : base("name=DBConnection")
        {
            
        }

        public virtual DbSet<CongViec> CongViec { get; set; }
        public virtual DbSet<CTPhieuNhap> CTPhieuNhap { get; set; }
        public virtual DbSet<CTPhieuXuat> CTPhieuXuat { get; set; }
        public virtual DbSet<DonViTinh> DonViTinh { get; set; }
        public virtual DbSet<KhachHang> KhachHang { get; set; }
        public virtual DbSet<LoaiNV> LoaiNV { get; set; }
        public virtual DbSet<LoaiSP> LoaiSP { get; set; }
        public virtual DbSet<NguyenLieu> NguyenLieu { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCap { get; set; }
        public virtual DbSet<NhanVien> NhanVien { get; set; }
        public virtual DbSet<PhieuNhap> PhieuNhap { get; set; }
        public virtual DbSet<PhieuXuat> PhieuXuat { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<ThanhPham> ThanhPham { get; set; }
        public virtual DbSet<HoaDon> HoaDon { get; set; }
        public virtual DbSet<CTHoaDon> CTHoaDon { get; set; }
        public virtual DbSet<PhanQuyen> PhanQuyen { get; set; }
        public virtual DbSet<ListController> ListController { get; set; }
        public virtual DbSet<TemplatePrint> TemplatePrint { get; set; }
        public virtual DbSet<DoiDiem> DoiDiem { get; set; }
        public virtual DbSet<KhuyenMai> KhuyenMai { get; set; }
        public virtual DbSet<CTKhuyenmai> CTKhuyenmai { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonViTinh>()
                .HasMany(e => e.NguyenLieu)
                .WithOptional(e => e.DonViTinh)
                .HasForeignKey(e => e.IdDVT);

            modelBuilder.Entity<DonViTinh>()
                .HasMany(e => e.ThanhPham)
                .WithOptional(e => e.DonViTinh)
                .HasForeignKey(e => e.IdDVT);

            //modelBuilder.Entity<KhachHang>()
            //    .HasMany(e => e.HoaDon)
            //    .WithOptional(e => e.KhachHang)
            //    .HasForeignKey(e => e.IdKhachHang);

            modelBuilder.Entity<LoaiNV>()
                .HasMany(e => e.NhanVien)
                .WithOptional(e => e.LoaiNV)
                .HasForeignKey(e => e.IdLoaiNV);

            modelBuilder.Entity<LoaiSP>()
                .HasMany(e => e.NguyenLieu)
                .WithOptional(e => e.LoaiSP)
                .HasForeignKey(e => e.IdLoai);

            modelBuilder.Entity<LoaiSP>()
                .HasMany(e => e.ThanhPham)
                .WithOptional(e => e.LoaiSP)
                .HasForeignKey(e => e.IdLoai);

            modelBuilder.Entity<NguyenLieu>()
                .HasMany(e => e.CTPhieuNhap)
                .WithOptional(e => e.NguyenLieu)
                .HasForeignKey(e => e.IdNguyenLieu);
            modelBuilder.Entity<NguyenLieu>()
               .HasMany(e => e.CTPhieuXuat)
               .WithOptional(e => e.NguyenLieu)
               .HasForeignKey(e => e.IdNguyenLieu);

            modelBuilder.Entity<NhaCungCap>()
                .HasMany(e => e.PhieuNhap)
                .WithOptional(e => e.NhaCungCap)
                .HasForeignKey(e => e.IdNhaCungCap);

            modelBuilder.Entity<NhanVien>()
                .HasMany(e => e.CongViec)
                .WithOptional(e => e.NhanVien)
                .HasForeignKey(e => e.IdNhanVien);

            modelBuilder.Entity<NhanVien>()
                .HasMany(e => e.KhachHang)
                .WithOptional(e => e.NhanVien)
                .HasForeignKey(e => e.IdNVTao);

            modelBuilder.Entity<NhanVien>()
                .HasMany(e => e.PhieuNhap)
                .WithOptional(e => e.NhanVien)
                .HasForeignKey(e => e.IdNhanVien);

            modelBuilder.Entity<NhanVien>()
                .HasMany(e => e.PhieuXuat)
                .WithOptional(e => e.NhanVien)
                .HasForeignKey(e => e.IdNhanVien);

            modelBuilder.Entity<NhanVien>()
               .HasMany(e => e.HoaDon)
               .WithOptional(e => e.NhanVien)
               .HasForeignKey(e => e.IdNhanVien);

            modelBuilder.Entity<PhieuNhap>()
                .HasMany(e => e.CTPhieuNhap)
                .WithOptional(e => e.PhieuNhap)
                .HasForeignKey(e => e.IdPhieuNhap);

            modelBuilder.Entity<PhieuXuat>()
                .HasMany(e => e.CTPhieuXuat)
                .WithOptional(e => e.PhieuXuat)
                .HasForeignKey(e => e.IdPhieuXuat);

            modelBuilder.Entity<ThanhPham>()
                .HasMany(e => e.CTHoaDon)
                .WithOptional(e => e.ThanhPham)
                .HasForeignKey(e => e.IdThanhPham);
            modelBuilder.Entity<NguyenLieu>()
                .HasMany(e => e.CTPhieuXuat)
                .WithOptional(e => e.NguyenLieu)
                .HasForeignKey(e => e.IdNguyenLieu);
            modelBuilder.Entity<HoaDon>()
               .HasMany(e => e.CTHoaDon)
               .WithOptional(e => e.HoaDon)
               .HasForeignKey(e => e.IdHoaDon);
        }
    }
}
