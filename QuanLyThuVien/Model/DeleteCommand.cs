using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
  public class XoaDuLieu
    {
        public static void XoaPhieuMuon(PhieuMuon phieuMuon)
        {
            Dataprovider.Ins.DB.PhieuMuons.Remove(phieuMuon);

        }
        public static void XoaChiTietPhieuMuon(ChiTietPhieuMuon chiTiet)
        {
            foreach (PhieuMuon item in Dataprovider.Ins.DB.PhieuMuons.Where(x => x.MaPhieu == chiTiet.Id))
            {
                XoaPhieuMuon(item);
            }
            Dataprovider.Ins.DB.ChiTietPhieuMuons.Remove(chiTiet);
        }

        public static void XoaNguoiDung(NguoiDung nguoiDung)
        {
            //xoá chi tiết trước
            foreach (ChiTietPhieuMuon item in Dataprovider.Ins.DB.ChiTietPhieuMuons.Where(x => x.MaNhanVien == nguoiDung.Id))
            {
                XoaChiTietPhieuMuon(item);
            }
            Dataprovider.Ins.DB.NguoiDungs.Remove(nguoiDung);
            Dataprovider.Ins.DB.SaveChanges();

        }
        public static void XoaQuyenHeThong(QuyenHeThong quyen)
        {
            foreach (NguoiDung item in Dataprovider.Ins.DB.NguoiDungs.Where(x => x.IdQuyen == quyen.Id))
            {
                XoaNguoiDung(item);
            }
            Dataprovider.Ins.DB.QuyenHeThongs.Remove(quyen);
            Dataprovider.Ins.DB.SaveChanges();
        }

        public static void XoaDocGia(DocGia docGia)
        {
            foreach (ChiTietPhieuMuon item in Dataprovider.Ins.DB.ChiTietPhieuMuons.Where(x => x.MaTheBanDoc == docGia.Id))
            {
                XoaChiTietPhieuMuon(item);
            }
            Dataprovider.Ins.DB.DocGias.Remove(docGia);
            Dataprovider.Ins.DB.SaveChanges();
        }


        public static void XoaTaiLieu(TaiLieu taiLieu)
        {
            foreach (PhieuMuon item in Dataprovider.Ins.DB.PhieuMuons.Where(x => x.MaSach == taiLieu.Id))
            {
                XoaPhieuMuon(item);
            }
            Dataprovider.Ins.DB.TaiLieux.Remove(taiLieu);
        }

        public static void XoaViTri(ViTri viTri)
        {
            foreach (TaiLieu item in Dataprovider.Ins.DB.TaiLieux.Where(x => x.MaViTri == viTri.Id))
            {
                XoaTaiLieu(item);
            }
            
            Dataprovider.Ins.DB.ViTris.Remove(viTri);
            Dataprovider.Ins.DB.SaveChanges();

        }
        public static void XoaNgonNgu(NgonNgu ngonNgu)
        {
            foreach (TaiLieu item in Dataprovider.Ins.DB.TaiLieux.Where(x => x.MaNgonNgu == ngonNgu.Id))
            {
                XoaTaiLieu(item);
            }
            Dataprovider.Ins.DB.NgonNgus.Remove(ngonNgu);
            Dataprovider.Ins.DB.SaveChanges();

        }
        public static void XoaTacGia(TacGia tacGia)
        {
            foreach (TaiLieu item in Dataprovider.Ins.DB.TaiLieux.Where(x => x.MaTacGia == tacGia.Id))
            {
                XoaTaiLieu(item);
            }

            Dataprovider.Ins.DB.TacGias.Remove(tacGia);
            Dataprovider.Ins.DB.SaveChanges();

        }
        public static void XoaNhaXuatBan(NhaXuatBan nhaXuatBan)
        {
            foreach (TaiLieu item in Dataprovider.Ins.DB.TaiLieux.Where(x => x.MaNhaXuatBan == nhaXuatBan.Id))
            {
                XoaTaiLieu(item);
            }
            Dataprovider.Ins.DB.NhaXuatBans.Remove(nhaXuatBan);
            Dataprovider.Ins.DB.SaveChanges();

        }
        public static void XoaTheLoai(TheLoai theLoai)
        {
            foreach (TaiLieu item in Dataprovider.Ins.DB.TaiLieux.Where(x => x.MaTheLoai == theLoai.Id))
            {
                XoaTaiLieu(item);
            }
            Dataprovider.Ins.DB.TheLoais.Remove(theLoai);
            Dataprovider.Ins.DB.SaveChanges();
        }
    }
}
