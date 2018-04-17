using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using QuanLyThuVien.Model;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using System.Windows;

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
        public ICommand LoadImageCommand { get; set; }
        // PasswordChanged
        #endregion

        #region thuoc tinh
        private ObservableCollection<NguoiDung> _ListNguoiDung;
        private ObservableCollection<QuyenHeThong> _ListQuyen;
        private string _Email;
        private string _TenNguoiDung;
        private string _PasswordOrg;
        private string _RePassword;
        private string _Quyen;
        private string _ImageSource= @".\ResoucesXAML\no_img.jpg";
        private NguoiDung _SelectedItem;
        private QuyenHeThong _SelectedItemQuyen;
        #endregion

        #region property
        public ObservableCollection<NguoiDung> ListNguoiDung { get => _ListNguoiDung; set { _ListNguoiDung = value; OnPropertyChanged(); } }
        public string TenNguoiDung { get=>_TenNguoiDung; set { _TenNguoiDung = value;OnPropertyChanged(); } }
        public string Email { get=>_Email; set { _Email = value;OnPropertyChanged(); } }
        public string PasswordOrg { get=>_PasswordOrg; set { _PasswordOrg = value; } }
        public string RePassword { get=>_RePassword; set { _RePassword = value; } }
        public string Quyen { get => _Quyen; set { _Quyen = value;OnPropertyChanged(); } }
        public string ImageSource { get => _ImageSource; set { _ImageSource = value;OnPropertyChanged(); } }

        public NguoiDung SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged();
                if (_SelectedItem != null)
                {   TenNguoiDung  = SelectedItem.TenNguoiDung;
                    Email = SelectedItem.Email;
                    SelectedItemQuyen = SelectedItem.QuyenHeThong;
                    if (!string.IsNullOrEmpty(SelectedItem.HinhAnh))
                    {
                        ImageSource = SelectedItem.HinhAnh;
                    }
                }
            }
        }

        public QuyenHeThong SelectedItemQuyen { get => _SelectedItemQuyen; set { _SelectedItemQuyen = value;OnPropertyChanged(); } }

        public ObservableCollection<QuyenHeThong> ListQuyen { get => _ListQuyen; set { _ListQuyen = value;OnPropertyChanged(); } }

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
                if(PasswordOrg!=RePassword)
                {
                    return false;
                }
                return true;
            }, (p) => {
                var nguoidung = new NguoiDung() {
                    TenNguoiDung = TenNguoiDung,
                    Email = Email,
                    IdQuyen = SelectedItemQuyen.Id,
                    MatKhau = PasswordOrg,
                    HinhAnh = ImageSource
                   
                };
                Dataprovider.Ins.DB.NguoiDungs.Add(nguoidung);
                Dataprovider.Ins.DB.SaveChanges();
                ListNguoiDung.Add(nguoidung);
            });

            EditCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TenNguoiDung) || string.IsNullOrWhiteSpace(TenNguoiDung) || SelectedItem == null)
                    return false;
                var displaylist = Dataprovider.Ins.DB.NguoiDungs.Where(x => x.Id == SelectedItem.Id);
                if (displaylist == null || displaylist.Count() == 0)
                    return false;
                return true;
            }, (p) => {
                var nguoiDung = Dataprovider.Ins.DB.NguoiDungs.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                nguoiDung.TenNguoiDung = TenNguoiDung;
                nguoiDung.Email = Email;
                nguoiDung.IdQuyen = SelectedItemQuyen.Id;
                nguoiDung.HinhAnh = ImageSource;
                Dataprovider.Ins.DB.SaveChanges();
                SelectedItem.TenNguoiDung = TenNguoiDung;
            });

            LoadImageCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                OpenFileDialog openFile = new OpenFileDialog();
                bool? dr = openFile.ShowDialog();
                if (dr == true)
                {
                    ImageSource = openFile.FileName;
                    openFile.Multiselect = false;
                    try
                    {
                       
                        File.Delete(@"..\..\Resouces\"+openFile.SafeFileName);
                        
                    }
                    catch (Exception)
                    {

                        
                    }
                    File.Copy(openFile.FileName, @"..\..\Resouces\" + openFile.SafeFileName);


                }
            });
        }
        private void LoadNguoiDung()
        {
            ListNguoiDung = new ObservableCollection<NguoiDung>(Dataprovider.Ins.DB.NguoiDungs);

            ListQuyen = new ObservableCollection<QuyenHeThong>(Dataprovider.Ins.DB.QuyenHeThongs);
        }

        #endregion
    }
}
