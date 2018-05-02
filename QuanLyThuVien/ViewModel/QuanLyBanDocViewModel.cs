using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using QuanLyThuVien.Model;

namespace QuanLyThuVien.ViewModel
{
    class QuanLyBanDocViewModel: BaseViewModel
    {
        #region command
        public ICommand PLoadCommand { get; set; }
        public ICommand AddCommmand { get; set; }
        public ICommand EditCommmand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand LoadImageCommand { get; set; }
        // PasswordChanged
        #endregion

        #region thuoc tinh
        private ObservableCollection<DocGia> _ListDocGia;
        private ObservableCollection<GioiTinh123> _ListGioiTinh;


        private string _HoTen;
        private string _SoDienThoai;
        private string _Email;
        private string _ChungMinhThu;
        private string _NgheNghiep;
        private string _DiaChi;
        private DateTime? _NgaySinh;
        private string _AnhBia;

        private BitmapImage _ImageSource;
        private string noimage = @"..\..\ResoucesXAML\no_img.jpg";

        private GioiTinh123 _SelectedGioiTinh;
        
        #endregion

        #region property
        public ObservableCollection<DocGia> ListDocGia { get => _ListDocGia; set { _ListDocGia = value; OnPropertyChanged(); } }
        public ObservableCollection<GioiTinh123> ListGioiTinh { get => _ListGioiTinh; set { _ListGioiTinh = value; OnPropertyChanged(); } }
        public string SoDienThoai { get => _SoDienThoai; set { _SoDienThoai = value; OnPropertyChanged(); } }
        public string HoTen { get => _HoTen; set { _HoTen = value; OnPropertyChanged(); } }
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }
        public string ChungMinhThu { get => _ChungMinhThu; set { _ChungMinhThu = value; OnPropertyChanged(); } }
        public string NgheNghiep { get => _NgheNghiep; set { _NgheNghiep = value; OnPropertyChanged(); } }
        public string DiaChi { get => _DiaChi; set { _DiaChi = value; OnPropertyChanged(); } }
        public DateTime? NgaySinh { get => _NgaySinh; set { _NgaySinh = value; OnPropertyChanged(); } }




        public string AnhBia { get => _AnhBia; set { _AnhBia = value; OnPropertyChanged(); } }
        public BitmapImage ImageSource { get => _ImageSource; set { _ImageSource = value; OnPropertyChanged(); } }

        public GioiTinh123 SelectedGioiTinh { get => _SelectedGioiTinh; set { _SelectedGioiTinh = value; OnPropertyChanged(); } }



        public string FilePath { get; set; }

        private DocGia _SelectedItem;
        public DocGia SelectedItem {get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    HoTen = SelectedItem.TenDocGia;
                    SoDienThoai = SelectedItem.SoDienThoai;
                    Email = SelectedItem.Email;
                    ChungMinhThu = SelectedItem.CMTND;
                    NgaySinh = SelectedItem.NgaySinh;
                    NgheNghiep = SelectedItem.NgheNghiep;
                    DiaChi = SelectedItem.DiaChi;
                    AnhBia = SelectedItem.AnhDaiDien;
                    SelectedGioiTinh = ListGioiTinh.Where(x =>x.Value == SelectedItem.GioiTinh).Single();
                    if (!string.IsNullOrEmpty(AnhBia))
                    {
                        ImageSource = Base64ToImg(AnhBia);
                    }
                    else
                        LoadImage(noimage);
                }
            }
        }



        #endregion

        #region method
        public QuanLyBanDocViewModel()
        {
            PLoadCommand = new RelayCommand<Page>((p) => { return true; }, (p) => {
                if (p == null)
                    return;
                LoadDocGia();
            });


            AddCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(HoTen)|| string.IsNullOrEmpty(SoDienThoai) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(ChungMinhThu) || string.IsNullOrEmpty(NgheNghiep)|| string.IsNullOrEmpty(DiaChi)||string.IsNullOrEmpty(AnhBia)||SelectedGioiTinh==null)
                {
                    return false;
                }

                return true;
            }, (p) => {
                var DocGia = new DocGia()
                {
                    Id = Guid.NewGuid().ToString(),
                    TenDocGia = HoTen,
                    SoDienThoai = SoDienThoai,
                    Email = Email,
                    CMTND = ChungMinhThu,
                    NgaySinh = NgaySinh,
                    NgheNghiep = NgheNghiep,
                    DiaChi = DiaChi,
                    GioiTinh = SelectedGioiTinh.Value,
                    AnhDaiDien = AnhBia

                };
                Dataprovider.Ins.DB.DocGias.Add(DocGia);
                Dataprovider.Ins.DB.SaveChanges();
                ListDocGia.Add(DocGia);
            });

            EditCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(HoTen) || string.IsNullOrEmpty(SoDienThoai) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(ChungMinhThu) || string.IsNullOrEmpty(NgheNghiep) || string.IsNullOrEmpty(DiaChi) || string.IsNullOrEmpty(AnhBia) || SelectedGioiTinh == null||SelectedItem==null)
                    return false;
                var displaylist = Dataprovider.Ins.DB.DocGias.Where(x => x.Id == SelectedItem.Id);
                if (displaylist == null || displaylist.Count() == 0)
                    return false;
                return true;
            }, (p) => {
                var DocGia = Dataprovider.Ins.DB.DocGias.Where(x => x.Id == SelectedItem.Id).Single();
                DocGia.TenDocGia = HoTen;
                DocGia.SoDienThoai = SoDienThoai;
                DocGia.Email = Email;
                DocGia.CMTND = ChungMinhThu;
                DocGia.NgaySinh = NgaySinh;
                DocGia.NgheNghiep = NgheNghiep;
                DocGia.DiaChi = DiaChi;
                DocGia.GioiTinh = SelectedGioiTinh.Value;

                Dataprovider.Ins.DB.SaveChanges();
                SelectedItem = DocGia;
            });

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
            DeleteCommand = new RelayCommand<object>((p)=> { if (SelectedItem == null) return false; return true;},(p)=> {
                var ten = SelectedItem.TenDocGia;
                MessageBoxResult dr = MessageBox.Show("Nếu bạn xoá đọc giả " + ten + " thì những phiếu mượn có tên " + ten + " cũng sẽ bị xoá. Bạn có chắc chắn muốn xoá không?", "Cảnh Báo", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (dr == MessageBoxResult.No)
                    return;
                try
                {
                    XoaDuLieu.XoaDocGia(SelectedItem);
                    ListDocGia.Remove(SelectedItem);
                    SelectedItem = null;
                    //MessageBox.Show("Đã xoá người dùng " + ten);
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.ToString());
                }


            });

        }
        private void LoadDocGia()
        {
            ListDocGia = new ObservableCollection<DocGia>(Dataprovider.Ins.DB.DocGias);
            ListGioiTinh = new ObservableCollection<GioiTinh123>
            {
                new GioiTinh123(true, "Nam"),
                new GioiTinh123(false, "Nữ")
            };

        }
        #region tạo base64 từ ảnh và chuyển base64 thành ảnh
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
        #endregion
        private void LoadImage(string _FilePath)
        {

            if (string.IsNullOrEmpty(FilePath))
            {
                FilePath = @"..\..\ResoucesXAML\no_img.jpg";
            }
            AnhBia = ImgToBase64(FilePath);
            ImageSource = Base64ToImg(AnhBia);
        }
        #region tạo một chuỗi ngẫu nhiên làm mã
        public string GetPass()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(8, false));
            builder.Append(RandomNumber(1000, 9999));
            return builder.ToString();
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        #endregion

        #endregion
    }

    public class GioiTinh123 : BaseViewModel
    {
        private bool? _value;
        private string _GioiTinh;

        public bool? Value { get => _value; set { _value = value; OnPropertyChanged(); } }
        public string GioiTinh { get => _GioiTinh; set { _GioiTinh = value; OnPropertyChanged(); } }

        public GioiTinh123(bool? gt, string gioitinh)
        {
            Value = gt;
            GioiTinh = gioitinh;
        }
    }
}
