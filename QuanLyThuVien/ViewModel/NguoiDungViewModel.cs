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
using System.Windows.Media.Imaging;

namespace QuanLyThuVien.ViewModel
{
   public class NguoiDungViewModel: BaseViewModel
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
        private string _AnhDaiDien;
        private BitmapImage _ImageSource;
        private NguoiDung _SelectedItem;
        private QuyenHeThong _SelectedItemQuyen;
        private string noimage = @"..\..\ResoucesXAML\no_img.jpg";
        #endregion

        #region property
        public ObservableCollection<NguoiDung> ListNguoiDung { get => _ListNguoiDung; set { _ListNguoiDung = value; OnPropertyChanged(); } }
        public string TenNguoiDung { get=>_TenNguoiDung; set { _TenNguoiDung = value;OnPropertyChanged(); } }
        public string Email { get=>_Email; set { _Email = value;OnPropertyChanged(); } }
        public string PasswordOrg { get=>_PasswordOrg; set { _PasswordOrg = value; } }
        public string RePassword { get=>_RePassword; set { _RePassword = value; } }
        public string Quyen { get => _Quyen; set { _Quyen = value;OnPropertyChanged(); } }
        public string AnhDaiDien { get => _AnhDaiDien; set { _AnhDaiDien = value; OnPropertyChanged(); } }
        public BitmapImage ImageSource { get => _ImageSource; set { _ImageSource = value;OnPropertyChanged(); } }

        public string FilePath { get; set; }

        public NguoiDung SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {   TenNguoiDung  = SelectedItem.TenNguoiDung;
                    Email = SelectedItem.Email;
                    SelectedItemQuyen = SelectedItem.QuyenHeThong;
                    AnhDaiDien = SelectedItem.AnhDaiDien;
                    if (!string.IsNullOrEmpty(AnhDaiDien))
                    {
                        ImageSource = Base64ToImg(AnhDaiDien);
                    }
                    else
                        LoadImage(noimage);

                   
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

            #region nút thêm
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
                    AnhDaiDien = AnhDaiDien
                   
                };
                Dataprovider.Ins.DB.NguoiDungs.Add(nguoidung);
                Dataprovider.Ins.DB.SaveChanges();
                ListNguoiDung.Add(nguoidung);
            });
            #endregion

            #region nút sửa
            EditCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TenNguoiDung) || SelectedItem == null)
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
                nguoiDung.AnhDaiDien = AnhDaiDien;//ImageSource;
                Dataprovider.Ins.DB.SaveChanges();
                SelectedItem = nguoiDung;
            });
            #endregion

            #region Nút load hình ảnh
            LoadImageCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                OpenFileDialog openFile = new OpenFileDialog();
                bool? dr = openFile.ShowDialog();
                if (dr == true)
                {
                    openFile.Multiselect = false;
                    FilePath = openFile.FileName;
                    LoadImage(FilePath);
                }
            });
            #endregion

            #region nút xoá người dùng

            DeleteCommmand = new RelayCommand<object>((p)=> { if (SelectedItem == null) return false; return true; },(p)=> {
                var ten = SelectedItem.TenNguoiDung;
                MessageBoxResult dr = MessageBox.Show("Nếu bạn xoá người dùng " + ten+ " thì những phiếu mượn có tên "+ten+ " cũng sẽ bị xoá. Bạn có chắc chắn muốn xoá không?", "Cảnh Báo",MessageBoxButton.YesNo,MessageBoxImage.Warning,MessageBoxResult.No);
                if (dr == MessageBoxResult.No)
                    return;

                //var ls = Dataprovider.Ins.DB.ChiTietPhieuMuons.Where(x=>x.MaNhanVien == SelectedItem.Id);
                //if (ls == null)
                //    return;
                //foreach (var item in ls)
                //{
                //    foreach (var item1 in item.PhieuMuons)
                //    {
                //        Dataprovider.Ins.DB.PhieuMuons.Remove(item1);
                //    }

                //    Dataprovider.Ins.DB.ChiTietPhieuMuons.Remove(item);
                //}
                //Dataprovider.Ins.DB.NguoiDungs.Remove(SelectedItem);
                //Dataprovider.Ins.DB.SaveChanges();
                try
                {
                    XoaDuLieu.XoaNguoiDung(SelectedItem);
                    ListNguoiDung.Remove(SelectedItem);
                    SelectedItem = null;
                    MessageBox.Show("Đã xoá người dùng " + ten);
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.ToString());
                }
                
            });

            #endregion
        }
        private void LoadNguoiDung()
        {
            ListNguoiDung = new ObservableCollection<NguoiDung>(Dataprovider.Ins.DB.NguoiDungs);

            ListQuyen = new ObservableCollection<QuyenHeThong>(Dataprovider.Ins.DB.QuyenHeThongs);
        }

        private string ImgToBase64(string imagePath)
        {
            FileStream fs;
            string strbyte;
            fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            
                byte[] picbyte = new byte[fs.Length];
                fs.Read(picbyte, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                strbyte = Convert.ToBase64String(picbyte);
            
            return strbyte;
        }
        private BitmapImage Base64ToImg(string baseString)
        {
            byte[] imgBytes = Convert.FromBase64String(baseString);
            MemoryStream ms;
            BitmapImage imageSource;
            ms = new MemoryStream(imgBytes, 0, imgBytes.Length);
            
                imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.StreamSource = ms;
                imageSource.EndInit();
            
            return imageSource;
        }

        private void LoadImage(string _FilePath)
        {

            if (string.IsNullOrEmpty(FilePath))
            {
                FilePath = @"..\..\ResoucesXAML\no_img.jpg";
            }
            AnhDaiDien = ImgToBase64(FilePath);
            ImageSource = Base64ToImg(AnhDaiDien);
        }

        #endregion
    }
}
