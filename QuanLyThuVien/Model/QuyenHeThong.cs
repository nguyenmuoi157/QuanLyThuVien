
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
    
public partial class QuyenHeThong:BaseViewModel
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public QuyenHeThong()
    {

        this.NguoiDungs = new HashSet<NguoiDung>();

    }


        private int _Id;

        private string _TenQuyen;

        private Nullable<bool> _TaiLieu;

        private Nullable<bool> _BanDoc;

        private Nullable<bool> _Muon;

        private Nullable<bool> _Tra;

        private Nullable<bool> _NguoiDung;

        private Nullable<bool> _PhanQuyen;

        public int Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }
        public string TenQuyen { get => _TenQuyen; set { _TenQuyen = value; OnPropertyChanged(); } }
        public bool? TaiLieu { get => _TaiLieu; set { _TaiLieu = value; OnPropertyChanged(); } }
        public bool? BanDoc { get => _BanDoc; set { _BanDoc = value; OnPropertyChanged(); } }
        public bool? Muon { get => _Muon; set { _Muon = value; OnPropertyChanged(); } }
        public bool? Tra { get => _Tra; set { _Tra = value; OnPropertyChanged(); } }
        public bool? NguoiDung { get => _NguoiDung; set { _NguoiDung = value; OnPropertyChanged(); } }
        public bool? PhanQuyen { get => _PhanQuyen; set { _PhanQuyen = value; OnPropertyChanged(); } }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
    }

}
