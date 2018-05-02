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
    class TraViewModel : BaseViewModel
    {
  
        
        private ObservableCollection<bool?> TT;

        #region danh sách các phiếu mượn hiện ở listview
        //gồm tên bạn đọc, ngày mượn, hạn trả, trạng thái
        private ObservableCollection<ChiTietPhieuMuon> _ListPhieuMuon;
        public ObservableCollection<ChiTietPhieuMuon> ListPhieuMuon { get => _ListPhieuMuon; set { _ListPhieuMuon = value; OnPropertyChanged(); } }
        private ChiTietPhieuMuon _SelectedPhieuMuon;
        public ChiTietPhieuMuon SelectedPhieuMuon { get => _SelectedPhieuMuon; set { _SelectedPhieuMuon = value; OnPropertyChanged(); LoadBanDoc(); } }

        private ObservableCollection<PhieuMuon> _list;
        public ObservableCollection<PhieuMuon> lists { get => _list; set { _list = value; OnPropertyChanged(); } }
        //private TrangThai _Selectedls;
        //public TrangThai Selectedls { get => _Selectedls; set { _Selectedls = value; OnPropertyChanged(); LoadBanDoc(); } }

        private ObservableCollection<ChiTietPhieuMuon> _DocGia;
        public ObservableCollection<ChiTietPhieuMuon> DocGia { get =>_DocGia; set { _DocGia = value; OnPropertyChanged(); } }
        private void LoadBanDoc()
        {

            DocGia = new ObservableCollection<ChiTietPhieuMuon>{SelectedPhieuMuon};
            lists = new ObservableCollection<PhieuMuon>(Dataprovider.Ins.DB.PhieuMuons.Where(x => x.MaPhieu == SelectedPhieuMuon.Id));
            LoadDataCu();
        }

        private void LoadDataCu()
        {
            TT = new ObservableCollection<bool?>();
            foreach (var item in lists)
            {
                TT.Add(item.TrangThai);
            }
        }

        #endregion

        #region danh sách các quyển sách mà độc giả được chọn đã mượn
        // danh sách này hiện tên và tác giả của cách quyển sách đã mượn
        public ObservableCollection<PhieuMuon>_ListSach;
        public ObservableCollection<PhieuMuon> ListSach { get => _ListSach; set { _ListSach = value; OnPropertyChanged(); } }
        #endregion

        #region các command
        public ICommand PLoadCommand { get; set; }
        public ICommand EditCommmand { get; set; }
        public ICommand DeleteCommmand { get; set; }
        #endregion

        #region hàm tạo, viết mọi thứ trong đây
        public TraViewModel()
        {
            #region hoàm load dữ liệu
            PLoadCommand = new RelayCommand<Page>((p) => { return true; }, (p) =>
            {
                if (p == null)
                    return;
                CheckStatus();
                LoadDulieu();
            });
            #endregion

            #region edit commamd
            EditCommmand = new RelayCommand<object>((p) =>
            {
                if (SelectedPhieuMuon == null)
                    return false;
                return true;
            }, (p) =>
            {
                var phieu = Dataprovider.Ins.DB.PhieuMuons.Where(x=>x.MaPhieu == SelectedPhieuMuon.Id).ToList();
                var sosanh = TT;
                for (int i = 0; i < phieu.Count(); i++)
                {
                    
                    if (phieu[i].TrangThai==false && sosanh[i]==true )
                    {
                        var masach = phieu[i].MaSach;
                        var taiLieu = Dataprovider.Ins.DB.TaiLieux.Where(x => x.Id == masach).SingleOrDefault();
                        taiLieu.SoLuong -= phieu[i].SoLuong;
                    }
                    if(phieu[i].TrangThai == true && sosanh[i] == false)
                    {
                        var masach = phieu[i].MaSach;
                        var taiLieu = Dataprovider.Ins.DB.TaiLieux.Where(x => x.Id == masach).SingleOrDefault();
                        taiLieu.SoLuong += phieu[i].SoLuong;
                    }
                }
                Dataprovider.Ins.DB.SaveChanges();
                LoadDataCu();
                LoadDulieu();
                CheckStatus();
            });
            #endregion

            #region hoàm xoá phiếu mượn

            DeleteCommmand = new RelayCommand<object>((p)=> { if (SelectedPhieuMuon == null) return false; return true; },(p)=> {

                MessageBoxResult dr = MessageBox.Show("Bạn có chắc chắn muốn xoá phiếu mượn có mã là :"+SelectedPhieuMuon.Id+ " không", "Thông Báo",MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.No);
                if (dr == MessageBoxResult.No)
                    return;
                var ls = Dataprovider.Ins.DB.PhieuMuons.Where(x=>x.MaPhieu == SelectedPhieuMuon.Id);
                foreach (var item in ls)
                {
                    Dataprovider.Ins.DB.PhieuMuons.Remove(item);
                }
                Dataprovider.Ins.DB.ChiTietPhieuMuons.Remove(SelectedPhieuMuon);

                Dataprovider.Ins.DB.SaveChanges();
            }); 
            #endregion

        }
        #endregion

        #region hàm load các list từ database
        private void LoadDulieu()
        {
            var ls = Dataprovider.Ins.DB.ChiTietPhieuMuons;
            foreach (var item in ls)
            {
                if (item.PhieuMuons.Count == 0)
                    Dataprovider.Ins.DB.ChiTietPhieuMuons.Remove(item);
            }
            Dataprovider.Ins.DB.SaveChanges();
            ListPhieuMuon = new ObservableCollection<ChiTietPhieuMuon>(Dataprovider.Ins.DB.ChiTietPhieuMuons);

        }

        private void CheckStatus()
        {
            var listchitiet = Dataprovider.Ins.DB.ChiTietPhieuMuons;
            foreach (var item in listchitiet)
            {
                var ls = Dataprovider.Ins.DB.PhieuMuons.Where(x => x.MaPhieu == item.Id);
                bool tradu = false, trathieu = false, chuatra = true;
                int sachdatra = 0, sachchuatra = ls.Count();
                foreach (var item1 in ls)
                {
                    if (item1.TrangThai == true)
                    {
                        sachdatra++;
                        sachchuatra--;
                    }
                }
                tradu = sachchuatra == 0 ? true : false;
                trathieu = sachdatra != 0 && sachchuatra != 0 ? true : false;
                chuatra = sachdatra == 0 ? true : false;


              if (tradu)
                {
                    item.TrangThai = "Trả Đủ";
                }
              if (trathieu)
                {
                    item.TrangThai = "Trả Thiếu";
                }
              if (chuatra)
                {
                    item.TrangThai = "Chưa Trả";
                }

                if ((item.HanTra - DateTime.Today).Value.TotalDays < 0 && chuatra)
                {
                    item.TrangThai = "Quá Hạn";
                }
            }
            var a = ListPhieuMuon;
            Dataprovider.Ins.DB.SaveChanges();
        }
    }
    #endregion

    public class TrangThai : BaseViewModel
    {
        private PhieuMuon _phieumuon;
        private bool? _trangthai;

        public bool? Trangthai { get => _trangthai; set { _trangthai = value;OnPropertyChanged(); } }
        public PhieuMuon Phieumuon { get => _phieumuon; set { _phieumuon = value; OnPropertyChanged(); } }
    }
}

