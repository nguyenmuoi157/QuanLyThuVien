using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using QuanLyThuVien.Model;
using System.Collections.ObjectModel;

namespace QuanLyThuVien.ViewModel
{
    class NguoiDungViewModel: BaseViewModel
    {
        #region command
        public ICommand PLoadCommand{get; set;}
        public ICommand AddCommmand { get; set; }
        public ICommand EditCommmand { get; set; }
        public ICommand DeleteCommmand { get; set; }
        public ICommand PasswordChanged { get; set; }

        // PasswordChanged
        #endregion

        #region thuoc tinh
        private ObservableCollection<NguoiDung> _ListNguoiDung;
        private string _Email;
        private string _TenNguoiDung;
        private string _PasswordOrg;
        private string _RePassword;
        private NguoiDung _SelectedItem;
        private NguoiDung _SelectedItemCbb;
        #endregion

        #region property
        public ObservableCollection<NguoiDung> ListNguoiDung { get => _ListNguoiDung; set { _ListNguoiDung = value; OnPropertyChanged(); } }
        public string TenNguoiDung { get=>_TenNguoiDung; set { _TenNguoiDung = value;OnPropertyChanged(); } }
        public string Email { get=>_Email; set { _Email = value;OnPropertyChanged(); } }
        public string PasswordOrg { get=>_PasswordOrg; set { _PasswordOrg = value; } }
        public string RePassword { get=>_RePassword; set { _RePassword= value; } }
        
        public NguoiDung SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged();
                if (_SelectedItem != null)
                {   TenNguoiDung  = SelectedItem.TenNguoiDung;
                    Email = SelectedItem.Email;
                    //SelectedItemCbb.QuyenHeThong.TenQuyen = SelectedItem.QuyenHeThong.TenQuyen;
                }
            }
        }

        public NguoiDung SelectedItemCbb { get => _SelectedItemCbb; set { _SelectedItemCbb = value;OnPropertyChanged(); } }

        #endregion
        #region method
        public NguoiDungViewModel()
        {
            PLoadCommand = new RelayCommand<Page>((p)=> { return true; },(p)=> {
                if (p == null)
                    return;
                LoadNguoiDung();
            });


            AddCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TenNguoiDung) || string.IsNullOrWhiteSpace(TenNguoiDung))
                    return false;
                var displaylist = Dataprovider.Ins.DB.NguoiDungs.Where(x => x.TenNguoiDung == TenNguoiDung);
                if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            }, (p) => {
                var nguoidung = new NguoiDung() {TenNguoiDung = TenNguoiDung };
                Dataprovider.Ins.DB.NguoiDungs.Add(nguoidung);
                Dataprovider.Ins.DB.SaveChanges();
                ListNguoiDung.Add(nguoidung);
            });

            EditCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TenNguoiDung) || string.IsNullOrWhiteSpace(TenNguoiDung) || SelectedItem == null)
                    return false;
                var displaylist = Dataprovider.Ins.DB.NguoiDungs.Where(x => x.TenNguoiDung == TenNguoiDung);
                if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            }, (p) => {
                var nguoiDung = Dataprovider.Ins.DB.NguoiDungs.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                nguoiDung.TenNguoiDung = TenNguoiDung;
                Dataprovider.Ins.DB.SaveChanges();

                SelectedItem.TenNguoiDung = TenNguoiDung;
            });
        }


        private void LoadNguoiDung()
        {

            var lis = Dataprovider.Ins.DB.NguoiDungs;
            ListNguoiDung = new ObservableCollection<NguoiDung>();

           
            foreach (var item in lis)
            {
               
                ListNguoiDung.Add(item);
            }
        }

        #endregion
    }
}
