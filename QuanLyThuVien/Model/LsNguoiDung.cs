using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    class LsNguoiDung
    {
        private NguoiDung _nguoiDung;
        public NguoiDung NguoiDung { get => _nguoiDung; set => _nguoiDung = value; }
        private int _STT;
        public int STT { get => _STT; set => _STT = value; }
    }
}
