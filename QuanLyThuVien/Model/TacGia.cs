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
    
    public partial class TacGia: BaseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TacGia()
        {
            this.TaiLieux = new HashSet<TaiLieu>();
        }
    
        public int Id { get; set; }
        private string _TenTacGia;
        public string TenTacGia { get=>_TenTacGia; set { _TenTacGia = value; OnPropertyChanged(); } }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaiLieu> TaiLieux { get; set; }
    }
}