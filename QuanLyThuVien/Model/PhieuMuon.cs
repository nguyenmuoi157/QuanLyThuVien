
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using QuanLyThuVien.ViewModel;

namespace QuanLyThuVien.Model
{

using System;
    using System.Collections.Generic;

    public partial class PhieuMuon : BaseViewModel
    {

        private string _Id;
        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        private string _MaPhieu;
        public string MaPhieu { get => _MaPhieu; set { _MaPhieu = value;OnPropertyChanged(); } }

        private string _maSach;
        public string MaSach { get => _maSach; set { _maSach = value; OnPropertyChanged(); } }

        private string _tinhTrang;
        public string TinhTrang { get => _tinhTrang; set { _tinhTrang = value; OnPropertyChanged(); } }

        private Nullable<int> _SoLuong;
        public int? SoLuong { get => _SoLuong; set { _SoLuong = value; OnPropertyChanged(); } }

        private Nullable<bool> _TrangThai;
        public bool? TrangThai { get => _TrangThai; set { _TrangThai = value; OnPropertyChanged(); } }



        public virtual ChiTietPhieuMuon ChiTietPhieuMuon { get; set; }

        public virtual TaiLieu TaiLieu { get; set; }
    }

}
