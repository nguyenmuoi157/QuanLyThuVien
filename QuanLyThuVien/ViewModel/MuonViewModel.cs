using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QuanLyThuVien.Model;

namespace QuanLyThuVien.ViewModel
{
    class MuonViewModel: BaseViewModel
    {
        #region các danh sách
        private ObservableCollection<DanhSachMuonSach> _list;
        public ObservableCollection<DanhSachMuonSach> List1 { get => _list; set { _list = value; OnPropertyChanged(); } }
       
        private ObservableCollection<TaiLieu> _ListSach;
        public ObservableCollection<TaiLieu> ListSach { get => _ListSach; set { _ListSach = value; OnPropertyChanged(); } }


        private ObservableCollection<DocGia> _ListID;
        public ObservableCollection<DocGia> ListID { get => _ListID; set { _ListID = value; OnPropertyChanged(); } }

        public ObservableCollection<NguoiDung> _ListNguoiDung;
        public ObservableCollection<NguoiDung> ListNguoiDung { get => _ListNguoiDung; set { _ListNguoiDung = value; OnPropertyChanged(); } }
        #endregion

        #region các command
        public ICommand PLoadCommand { get; set; }
        public ICommand AddCommmand { get; set; }
        public ICommand EditCommmand { get; set; }
        public ICommand DeleteCommmand { get; set; }
        public ICommand ThemSachCommand { get; set; }
        
        #endregion

        #region các selectedItem
        private DanhSachMuonSach _SelectedItem;
        public DanhSachMuonSach SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged();} }

        private TaiLieu _SelectedSach;
        public TaiLieu SelectedSach { get=>_SelectedSach; set { _SelectedSach = value;OnPropertyChanged(); } }

        public DocGia _SelectedID;
        public DocGia SelectedID { get => _SelectedID; set { _SelectedID = value; OnPropertyChanged(); } }

        public NguoiDung _SelectedNguoiDung;
        public NguoiDung SelectedNguoiDung { get => _SelectedNguoiDung; set { _SelectedNguoiDung = value; OnPropertyChanged(); } }
        #endregion

        #region các thuộc tính binding cho textbox
        private string _SoLuong = "";
        public string SoLuong { get => _SoLuong; set { _SoLuong = value; OnPropertyChanged(); } }

        private string _NgayMuon = "";
        public string NgayMuon { get => _NgayMuon; set { _NgayMuon = value; OnPropertyChanged(); } }

        private string _NgayTra = "";
        public string NgayTra { get => _NgayTra; set { _NgayTra = value; OnPropertyChanged(); } }

        private string _TinhTrang = "";
        public string TinhTrang { get => _TinhTrang; set { _TinhTrang = value; OnPropertyChanged(); } }
        
        #endregion

        #region hàm tạo, viết mọi thứ trong đây
        public MuonViewModel()
        {
            PLoadCommand = new RelayCommand<Page>((p) => { return true; }, (p) => {
                if (p == null)
                    return;
                LoadDulieu();
            });

            #region add command

            AddCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(SoLuong)||SelectedID==null||SelectedSach==null||string.IsNullOrEmpty(NgayMuon)||string.IsNullOrEmpty(NgayTra))
                    return false;
                return true;
            }, (p) => {
                //TimeSpan hantra = DateTime.Parse(NgayTra)-DateTime.Parse(NgayMuon);
                var chitietphieumuon = new ChiTietPhieuMuon() {
                    Id = Guid.NewGuid().ToString(),
                    MaTheBanDoc = SelectedID.Id,
                    NgayMuon = DateTime.Parse(NgayMuon),
                    HanTra = DateTime.Parse(NgayTra),
                    MaNhanVien = SelectedNguoiDung.Id,
                    TrangThai= "Chưa Trả"
                };
                Dataprovider.Ins.DB.ChiTietPhieuMuons.Add(chitietphieumuon);
                Dataprovider.Ins.DB.SaveChanges();

                string id = chitietphieumuon.Id;
                foreach (var item in List1)
                {
                    TaiLieu taiLieu = Dataprovider.Ins.DB.TaiLieux.Where(x => x.Id == item.MaSach).SingleOrDefault();
                    taiLieu.SoLuong -= item.SoLuong;
                    var phieumuon = new PhieuMuon() { Id=Guid.NewGuid().ToString(),MaPhieu = id, MaSach = item.MaSach, TinhTrang = item.TinhTrang, SoLuong = item.SoLuong,TrangThai=false};
                    Dataprovider.Ins.DB.PhieuMuons.Add(phieumuon);
                }
                Dataprovider.Ins.DB.SaveChanges();
                MessageBox.Show("Đã thêm phiếu mượn cho độc giả: "+SelectedID.TenDocGia);
                List1.Clear();
            });

            #endregion
            

            #region thêm sách mượn command

            ThemSachCommand = new RelayCommand<object>((p) => {
                if (SelectedSach == null)
                {
                    return false;
                }

                if (!int.TryParse(SoLuong, out int a))
                {
                    return false;
                }

                return true; }, (p) => {
                int? soluongcon = Dataprovider.Ins.DB.TaiLieux.Where(x => x.Id == SelectedSach.Id).SingleOrDefault().SoLuong;
                if (soluongcon == 0)
                {
                    MessageBox.Show("Sách đã hết, vui lòng chọn sách khác");
                    return;
                }else if(soluongcon< int.Parse(SoLuong))
                    {
                        MessageBox.Show("Sách Không Đủ");
                        return;
                    }
                    List1.Add(new DanhSachMuonSach() {SoLuong = int.Parse(SoLuong), TuaDe = SelectedSach.TuaDe,TinhTrang = TinhTrang, MaSach = SelectedSach.Id});
                });
            #endregion

            DeleteCommmand = new RelayCommand<object>((p) => { if (SelectedItem == null) return false; return true; }, (p) => {
                List1.RemoveAt(List1.IndexOf(SelectedItem));
            });
        }
        #endregion


        #region hàm load các list từ database
        private void LoadDulieu()
        {
            List1 = new ObservableCollection<DanhSachMuonSach>();
            ListSach = new ObservableCollection<TaiLieu>(Dataprovider.Ins.DB.TaiLieux);
            ListID = new ObservableCollection<DocGia>(Dataprovider.Ins.DB.DocGias);
            ListNguoiDung = new ObservableCollection<NguoiDung>(Dataprovider.Ins.DB.NguoiDungs);
        }
        #endregion

    }

    public class DanhSachMuonSach: BaseViewModel
    {
        private string _TuaDe;
        private string _TinhTrang;
        private int _SoLuong;
        private string _MaSach;

        public string TuaDe { get => _TuaDe; set { _TuaDe = value;OnPropertyChanged(); } }
        public string TinhTrang { get => _TinhTrang; set { _TinhTrang = value; OnPropertyChanged(); } }
        public string MaSach { get => _MaSach; set { _MaSach = value; OnPropertyChanged(); } }
        public int SoLuong { get => _SoLuong; set { _SoLuong = value; OnPropertyChanged(); } }

    }
}
