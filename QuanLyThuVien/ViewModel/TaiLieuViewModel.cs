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
    public class TaiLieuViewModel: BaseViewModel
    {
        #region command
        public ICommand PLoadCommand { get; set; }
        public ICommand AddCommmand { get; set; }
        public ICommand EditCommmand { get; set; }
        public ICommand DeleteCommmand { get; set; }
        public ICommand PasswordChanged { get; set; }
        public ICommand LoadImageCommand { get; set; }
        // PasswordChanged
        #endregion

        #region thuoc tinh
        private ObservableCollection<TaiLieu> _ListTaiLieu;
        private ObservableCollection<TacGia> _ListTacGia;
        private ObservableCollection<NhaXuatBan> _ListNhaXuatBan;
        private ObservableCollection<NgonNgu> _ListNgonNgu;
        private ObservableCollection<TheLoai> _ListTheLoai;
        private ObservableCollection<ViTri> _ListViTri;
        

        private string _NhanDe;
        private string _SoTrang;
        private string _GiaBia;
        private string _SoLuong;
        private string _AnhBia;


        private BitmapImage _ImageSource;
        private string noimage = @"..\..\ResoucesXAML\no_img.jpg";


        private TaiLieu _SelectedItem;
        private TacGia _SelectedTacGia;
        private NhaXuatBan _SelectedNhaXuatBan;
        private NgonNgu _SelectedNgonNgu;
        private TheLoai _SelectedTheLoai;
        private ViTri _SelectedViTri;
        

        #endregion

        #region property
        public ObservableCollection<TaiLieu> ListTaiLieu { get => _ListTaiLieu; set { _ListTaiLieu = value; OnPropertyChanged(); } }
        public string SoTrang { get => _SoTrang; set { _SoTrang = value; OnPropertyChanged(); } }
        public string NhanDe { get => _NhanDe; set { _NhanDe = value; OnPropertyChanged(); } }
        public string GiaBia { get => _GiaBia; set { _GiaBia = value; OnPropertyChanged(); } }
        public string SoLuong { get => _SoLuong; set { _SoLuong = value; OnPropertyChanged(); } }

        public string AnhBia { get => _AnhBia; set { _AnhBia = value; OnPropertyChanged(); } }
        public BitmapImage ImageSource { get => _ImageSource; set { _ImageSource = value; OnPropertyChanged(); } }

        public TacGia SelectedTacGia { get => _SelectedTacGia; set { _SelectedTacGia = value; OnPropertyChanged(); } }
        public NhaXuatBan SelectedNhaXuatBan { get => _SelectedNhaXuatBan; set { _SelectedNhaXuatBan = value; OnPropertyChanged(); } }
        public NgonNgu SelectedNgonNgu { get => _SelectedNgonNgu; set { _SelectedNgonNgu = value; OnPropertyChanged(); } }
        public TheLoai SelectedTheLoai { get => _SelectedTheLoai; set { _SelectedTheLoai = value; OnPropertyChanged(); } }
        public ViTri SelectedViTri { get => _SelectedViTri; set { _SelectedViTri = value; OnPropertyChanged(); } }


        public string FilePath { get; set; }

        public TaiLieu SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value; OnPropertyChanged();
                if (_SelectedItem != null)
                {
                    
                    NhanDe = SelectedItem.TuaDe;
                    GiaBia = SelectedItem.GiaBia.ToString();
                    SoLuong = SelectedItem.SoLuong.ToString();
                    SoTrang = SelectedItem.SoTrang.ToString();
                    SelectedNgonNgu = SelectedItem.NgonNgu;
                    SelectedNhaXuatBan = SelectedItem.NhaXuatBan;
                    SelectedTacGia = SelectedItem.TacGia;
                    SelectedTheLoai = SelectedItem.TheLoai;
                    SelectedViTri = SelectedItem.ViTri;
                    AnhBia = SelectedItem.AnhBia;
                    if (!string.IsNullOrEmpty(AnhBia))
                    {
                        ImageSource = Base64ToImg(AnhBia);
                    }
                    else
                        LoadImage(noimage);
                }
            }
        }

        



        public ObservableCollection<TacGia> ListTacGia { get => _ListTacGia; set { _ListTacGia = value; OnPropertyChanged(); } }
        public ObservableCollection<NhaXuatBan> ListNhaXuatBan { get => _ListNhaXuatBan; set { _ListNhaXuatBan = value; OnPropertyChanged(); } }
        public ObservableCollection<NgonNgu> ListNgonNgu { get => _ListNgonNgu; set { _ListNgonNgu = value; OnPropertyChanged(); } }
        public ObservableCollection<TheLoai> ListTheLoai { get => _ListTheLoai; set { _ListTheLoai = value; OnPropertyChanged(); } }
        public ObservableCollection<ViTri> ListViTri { get => _ListViTri; set { _ListViTri = value; OnPropertyChanged(); } }
        


        #endregion

        #region method
        public TaiLieuViewModel()
        {
            PLoadCommand = new RelayCommand<Page>((p) => { return true; }, (p) => {
                if (p == null)
                    return;
                LoadTaiLieu();
            });


            AddCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(NhanDe)|| SelectedNgonNgu==null || SelectedTacGia==null|| SelectedNhaXuatBan==null|| SelectedTheLoai==null|| SelectedViTri==null||string.IsNullOrEmpty(AnhBia))
                {
                    return false;
                }

                var displaylist = Dataprovider.Ins.DB.TaiLieux.Where(x => x.TuaDe == NhanDe);
                 if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            }, (p) => {

                string id = GetPass();
                while (Dataprovider.Ins.DB.TaiLieux.Where(x=>x.Id ==id).Count()!=0)
                {
                    id = GetPass();
                }
                int.TryParse(SoLuong, out int sl);
                int.TryParse(SoTrang, out int st);
                float.TryParse(GiaBia, out float gb);
                var tailieu = new TaiLieu()
                {
                    Id = id,
                    TuaDe = NhanDe,
                    SoLuong = sl,
                    SoTrang = st,
                    GiaBia = gb,
                    MaNgonNgu = SelectedNgonNgu.Id,
                    MaTacGia = SelectedTacGia.Id,
                    MaNhaXuatBan = SelectedNhaXuatBan.Id,
                    MaTheLoai = SelectedTheLoai.Id, 
                    MaViTri = SelectedViTri.Id,
                    AnhBia = AnhBia
                };
                Dataprovider.Ins.DB.TaiLieux.Add(tailieu);
                Dataprovider.Ins.DB.SaveChanges();
                ListTaiLieu.Add(tailieu);
            });

            EditCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(NhanDe) || SelectedNgonNgu == null || SelectedTacGia == null || SelectedNhaXuatBan == null || SelectedTheLoai == null || SelectedViTri == null || string.IsNullOrEmpty(AnhBia)||SelectedItem==null)
                    return false;
                var displaylist = Dataprovider.Ins.DB.TaiLieux.Where(x => x.Id == SelectedItem.Id);
                if (displaylist == null || displaylist.Count() == 0)
                    return false;
                return true;
            }, (p) => {
                int.TryParse(SoLuong, out int sl);
                int.TryParse(SoTrang, out int st);
                float.TryParse(GiaBia, out float gb);

                var tailieu = Dataprovider.Ins.DB.TaiLieux.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                tailieu.TuaDe = NhanDe;
                tailieu.SoLuong = sl;
                tailieu.SoTrang = st;
                tailieu.GiaBia = gb;
                tailieu.MaNgonNgu = SelectedNgonNgu.Id;
                tailieu.MaTacGia = SelectedTacGia.Id;
                tailieu.MaNhaXuatBan = SelectedNhaXuatBan.Id;
                tailieu.MaViTri = SelectedViTri.Id;
                tailieu.AnhBia = AnhBia;
                Dataprovider.Ins.DB.SaveChanges();
                SelectedItem = tailieu;
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

            DeleteCommmand = new RelayCommand<object>((p)=> { if (SelectedItem == null) return false; return true; },(p)=> {
                MessageBoxResult dr = MessageBox.Show("Nếu bạn xoá tài liệu " + SelectedItem.TuaDe + " thì những phiếu mượn có tên " + SelectedItem.TuaDe + " cũng sẽ bị xoá. Bạn có chắc chắn muốn xoá không?", "Cảnh Báo", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (dr == MessageBoxResult.No)
                {
                    return;
                }
                try
                {
                    XoaDuLieu.XoaTaiLieu(SelectedItem);
                    ListTaiLieu.Remove(SelectedItem);
                    Dataprovider.Ins.DB.SaveChanges();
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.ToString());
                }

            });
        }
        private void LoadTaiLieu()
        {
            ListTaiLieu = new ObservableCollection<TaiLieu>(Dataprovider.Ins.DB.TaiLieux);
            ListNgonNgu = new ObservableCollection<NgonNgu>(Dataprovider.Ins.DB.NgonNgus);
            ListTacGia = new ObservableCollection<TacGia>(Dataprovider.Ins.DB.TacGias);
            ListNhaXuatBan = new ObservableCollection<NhaXuatBan>(Dataprovider.Ins.DB.NhaXuatBans);
            ListTheLoai = new ObservableCollection<TheLoai>(Dataprovider.Ins.DB.TheLoais);
            ListViTri = new ObservableCollection<ViTri>(Dataprovider.Ins.DB.ViTris);
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
            builder.Append(RandomString(4, true));
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
}
