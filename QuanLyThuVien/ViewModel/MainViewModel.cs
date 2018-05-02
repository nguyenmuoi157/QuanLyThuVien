using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using QuanLyThuVien.Model;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.ObjectModel;

namespace QuanLyThuVien.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public ICommand WindowsLoadCommand { get; set; }
        public ICommand SelectItemCommand { get; set; }
        public ICommand FrameLoadCommand { get; set; }
        public ICommand SelectSubMenuCommand { get; set; }
        public ICommand HomePageSelectCommand { get; set; }
        public ICommand TimKiemCommand { get; set; }


        public string TuKhoaTimKiem { get; set; }
        private BitmapImage _ImageSource;
        public BitmapImage ImageSource { get => _ImageSource; set { _ImageSource = value; OnPropertyChanged(); } }
        private string _TenNguoiDung;
        public string TenNguoiDung { get => _TenNguoiDung; set { _TenNguoiDung = value; OnPropertyChanged(); } }

        public int luachon;
        public int Idnguoidung { get; set; }
        public Frame FrameMain{ get;set;}

        public QuyenHeThong matranquyen;

        public MainViewModel()
        {


            WindowsLoadCommand = new RelayCommand<Window>((p)=> { return true; },(p)=> {
                if (p == null)
                    return;
                p.Hide();
                LoginWindows login = new LoginWindows();
                login.ShowDialog();

                if (login.DataContext == null)
                    return;
                var loginDatacontex = login.DataContext as LoginWindowViewModel;
                if (loginDatacontex.IsLogin)
                {
                    Idnguoidung = loginDatacontex.IdNguoiDung;
                    p.Show();
                    TenNguoiDung = "Xin Chào : " + Dataprovider.Ins.DB.NguoiDungs.Where(x => x.Id == Idnguoidung).SingleOrDefault().TenNguoiDung;
                    string anh = Dataprovider.Ins.DB.NguoiDungs.Where(x=>x.Id== Idnguoidung).SingleOrDefault().AnhDaiDien;
                    ImageSource = Base64ToImg(anh);
                    TenNguoiDung = "Xin Chào : " + loginDatacontex.UserName;

                    int idquyen = Dataprovider.Ins.DB.NguoiDungs.Where(x => x.Id == Idnguoidung).SingleOrDefault().IdQuyen;
                    matranquyen = Dataprovider.Ins.DB.QuyenHeThongs.Where(x => x.Id == idquyen).SingleOrDefault();
                }
                else
                    p.Close();
            });
            FrameLoadCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                FrameMain = p;
                FrameMain.Content = new PageTaiLieu();
            });
            SelectItemCommand = new RelayCommand<ListView>((p) => { return true; }, (p) => {
                int selectedIndex = p.SelectedIndex;
               
                switch (selectedIndex)
                {

                    case 0:

                        FrameMain.Content = new PageTaiLieu();
                        luachon = 1;
                        break;
                    case 1:
                        if (matranquyen.BanDoc == false)
                        {
                            MessageBox.Show("bạn không có quyền để thực hiện chức năng này, liên hệ admin để thêm quyền");
                            return;
                        }
                        FrameMain.Content = new PageBanDoc();
                        luachon = 2;
                        break;
                    case 2:
                        if (matranquyen.Muon == false)
                        {
                            MessageBox.Show("bạn không có quyền để thực hiện chức năng này, liên hệ admin để thêm quyền");
                            return;
                        }
                        FrameMain.Content = new PageMuon();
                        luachon = 3;
                        break;
                    case 3:
                        if (matranquyen.Tra == false)
                        {
                            MessageBox.Show("bạn không có quyền để thực hiện chức năng này, liên hệ admin để thêm quyền");
                            return;
                        }
                        FrameMain.Content = new PageTra();
                        luachon = 4;
                        break;
                    case 4:
                        if (matranquyen.NguoiDung == false)
                        {
                            MessageBox.Show("bạn không có quyền để thực hiện chức năng này, liên hệ admin để thêm quyền");
                            return;
                        }
                        FrameMain.Content = new NavNguoiDung();
                        luachon = 5;
                        break;
                    default:
                        if (matranquyen.PhanQuyen == false)
                        {
                            MessageBox.Show("bạn không có quyền để thực hiện chức năng này, liên hệ admin để thêm quyền");
                            return;
                        }
                        FrameMain.Content = new PagePhanQuyen();
                        break;
                }
            });

            HomePageSelectCommand = new RelayCommand<ListView>((p) => { return true; }, (p) => {
                int selectedIndex = p.SelectedIndex;

                switch (selectedIndex)
                {
                    case 0:
                        FrameMain.Content = new PageTrangchu();
                        break;
                    default:
                        break;
                }
            });
            TimKiemCommand = new RelayCommand<Frame>((p)=> { return true; },(p)=> {
               
                if (p == null)
                {
                    return;
                }
                #region tìm kiếm ở trang tài liệu
                if (luachon == 1)
                {
                    TimKiemTaiLieu(p);
                }
                #endregion
                else if(luachon ==5)
                    {
                    return;
                    }
                else
                {
                    switch(luachon)
                    {
                        case 2:
                            TimKiemBanDoc(p);
                            break;
                        case 4:
                            TimKiemTra(p);
                            break;
                    }
                }
            });
            
        }

        private void LoadImage()
        {
            //string imgBase64 = Dataprovider.Ins.DB.NguoiDungs
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

        private void TimKiemBanDoc(Frame p)
        {
            var t = p.Content as PageBanDoc;
            var tt = t.DataContext as QuanLyBanDocViewModel;
            //var ttt = tt.MainFrame.Content as PageQuanLyTaiLieu;
            //var tttt = ttt.DataContext as TaiLieuViewModel;
            if (string.IsNullOrEmpty(TuKhoaTimKiem))
            {
                tt.ListDocGia = new ObservableCollection<DocGia>(Dataprovider.Ins.DB.DocGias);
                //tt.ListTaiLieu = new ObservableCollection<TaiLieu>(Dataprovider.Ins.DB.TaiLieux);
                return;
            }
            tt.ListDocGia = new ObservableCollection<DocGia>(Dataprovider.Ins.DB.DocGias.Where(x => x.TenDocGia.Contains(TuKhoaTimKiem) || x.CMTND.Contains(TuKhoaTimKiem)
            || x.DiaChi.Contains(TuKhoaTimKiem) || x.Email.Contains(TuKhoaTimKiem) || x.Id.Contains(TuKhoaTimKiem)
            || x.SoDienThoai.Contains(TuKhoaTimKiem)
            ));
            tt.SelectedItem = null;
        }
        private void TimKiemTra( Frame p)
        {
            var t = p.Content as PageTra;
            var tt = t.DataContext as TraViewModel;
            //var ttt = tt.MainFrame.Content as PageQuanLyTaiLieu;
            //var tttt = ttt.DataContext as TaiLieuViewModel;
            if (string.IsNullOrEmpty(TuKhoaTimKiem))
            {
                tt.ListPhieuMuon = new ObservableCollection<ChiTietPhieuMuon>(Dataprovider.Ins.DB.ChiTietPhieuMuons);
                //tt.ListTaiLieu = new ObservableCollection<TaiLieu>(Dataprovider.Ins.DB.TaiLieux);
                return;
            }
            tt.ListPhieuMuon = new ObservableCollection<ChiTietPhieuMuon>(Dataprovider.Ins.DB.ChiTietPhieuMuons.Where(x => x.DocGia.TenDocGia.Contains(TuKhoaTimKiem) || x.MaTheBanDoc.Contains(TuKhoaTimKiem)
            || x.NguoiDung.TenNguoiDung.Contains(TuKhoaTimKiem) || x.Id.Contains(TuKhoaTimKiem)
            ));
            tt.SelectedPhieuMuon = null;
        }
        private void TimKiemTaiLieu(Frame p)
        {
            var t = p.Content as PageTaiLieu;
            var tt = t.DataContext as NavTaiLieuDatacontex;
            var ttt = tt.MainFrame.Content as PageQuanLyTaiLieu;
            var tttt = ttt.DataContext as TaiLieuViewModel;
            if (string.IsNullOrEmpty(TuKhoaTimKiem))
            {
                tttt.ListTaiLieu = new ObservableCollection<TaiLieu>(Dataprovider.Ins.DB.TaiLieux);
                return;
            }
            tttt.ListTaiLieu = new ObservableCollection<TaiLieu>(Dataprovider.Ins.DB.TaiLieux.Where(x => x.TuaDe.Contains(TuKhoaTimKiem) || x.NgonNgu.TenNgonNgu.Contains(TuKhoaTimKiem)
            || x.NhaXuatBan.TenNhaXuatBan.Contains(TuKhoaTimKiem) || x.TacGia.TenTacGia.Contains(TuKhoaTimKiem) || x.TheLoai.TenTheLoai.Contains(TuKhoaTimKiem)
            || x.Id.Contains(TuKhoaTimKiem)
            ));
            tttt.SelectedItem = null;
        }
    }
}
