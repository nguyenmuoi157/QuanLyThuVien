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
   public class PhanQuyenViewModel : BaseViewModel
    {


        #region danh sách các phiếu mượn hiện ở listview
        //gồm tên bạn đọc, ngày mượn, hạn trả, trạng thái
        private ObservableCollection<QuyenHeThong> _ListQuyen;
        public ObservableCollection<QuyenHeThong> ListQuyen { get => _ListQuyen; set { _ListQuyen = value; OnPropertyChanged(); } }
        private QuyenHeThong _SelectedQuyen;
        public QuyenHeThong SelectedQuyen { get => _SelectedQuyen; set { _SelectedQuyen = value; OnPropertyChanged(); LoadNguoiDung(); } }
        private void LoadNguoiDung()
        {
            var nd = Dataprovider.Ins.DB.NguoiDungs.Where(x => x.Id == SelectedQuyen.Id).SingleOrDefault();
            ListModul = new ObservableCollection<PhanQuyen>
            {
                new PhanQuyen() { Name = "Quản Lý Bạn Đọc", IsCheck = nd.QuyenHeThong.BanDoc },
                new PhanQuyen() { Name = "Quản Lý Mượn", IsCheck = nd.QuyenHeThong.Muon },
                new PhanQuyen() { Name = "Quản Lý Trả", IsCheck = nd.QuyenHeThong.Tra },
                new PhanQuyen() { Name = "Quản Lý Người Dùng", IsCheck = nd.QuyenHeThong.NguoiDung }
            };
        }

        private ObservableCollection<PhanQuyen> _ListModul;
        public ObservableCollection<PhanQuyen> ListModul { get => _ListModul; set { _ListModul = value; OnPropertyChanged(); } }


        #endregion

        #region danh sách các quyển sách mà độc giả được chọn đã mượn
        // danh sách này hiện tên và tác giả của cách quyển sách đã mượn
        //public ObservableCollection<PhieuMuon> _ListSach;
        //public ObservableCollection<PhieuMuon> ListSach { get => _ListSach; set { _ListSach = value; OnPropertyChanged(); } }
        #endregion

        #region các command
        public ICommand PLoadCommand { get; set; }
        public ICommand EditCommmand { get; set; }
        #endregion

        #region hàm tạo, viết mọi thứ trong đây
        public PhanQuyenViewModel()
        {
            #region hoàm load dữ liệu
            PLoadCommand = new RelayCommand<Page>((p) => { return true; }, (p) =>
            {
                if (p == null)
                    return;
                LoadDulieu();
            });
            #endregion

            #region edit commamd
            EditCommmand = new RelayCommand<object>((p) =>
            {
                if (SelectedQuyen == null)
                    return false;
                return true;
            }, (p) =>
            {
                var a = Dataprovider.Ins.DB.QuyenHeThongs.Where(x => x.Id == SelectedQuyen.Id).SingleOrDefault();
                a.BanDoc = ListModul[0].IsCheck;
                a.Muon = ListModul[1].IsCheck;
                a.Tra = ListModul[2].IsCheck;
                a.NguoiDung = ListModul[3].IsCheck;
                Dataprovider.Ins.DB.SaveChanges();
                //LoadDulieu();
            });
            #endregion
        }
        #endregion

        #region hàm load các list từ database
        private void LoadDulieu()
        {
           
            ListQuyen = new ObservableCollection<QuyenHeThong>(Dataprovider.Ins.DB.QuyenHeThongs);
        }


    }

    public class PhanQuyen: BaseViewModel
    {
        private string _Name;
        private bool? _IsCheck;

        public bool? IsCheck { get => _IsCheck; set { _IsCheck = value;OnPropertyChanged(); } }
        public string Name { get => _Name; set { _Name = value;OnPropertyChanged(); } }
    }
}
#endregion