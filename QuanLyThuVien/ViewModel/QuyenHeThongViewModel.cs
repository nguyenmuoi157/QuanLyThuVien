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
    class QuyenHeThongViewModel: BaseViewModel
    {
        private ObservableCollection<QuyenHeThong> _list;
        public ObservableCollection<QuyenHeThong> List { get => _list; set { _list = value; OnPropertyChanged(); } }


        public ICommand PLoadCommand { get; set; }
        public ICommand AddCommmand { get; set; }
        public ICommand EditCommmand { get; set; }
        public ICommand DeleteCommmand { get; set; }

        private QuyenHeThong _SelectedItem;
        public QuyenHeThong SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged(); if (_SelectedItem != null) TxtQuyenHeThong = SelectedItem.TenQuyen; } }

        private String _txtQuyenHeThong = "";
        public string TxtQuyenHeThong { get => _txtQuyenHeThong; set { _txtQuyenHeThong = value; OnPropertyChanged(); } }


        public QuyenHeThongViewModel()
        {
            PLoadCommand = new RelayCommand<Page>((p) => { return true; }, (p) => {
                if (p == null)
                    return;
                LoadQuyenHeThong();

            });
            AddCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TxtQuyenHeThong) || string.IsNullOrWhiteSpace(TxtQuyenHeThong))
                {
                    return false;
                }

                var displaylist = Dataprovider.Ins.DB.QuyenHeThongs.Where(x => x.TenQuyen == TxtQuyenHeThong);
                if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            }, (p) => {
                var quyenhethong = new QuyenHeThong() { TenQuyen = TxtQuyenHeThong};
                Dataprovider.Ins.DB.QuyenHeThongs.Add(quyenhethong);
                Dataprovider.Ins.DB.SaveChanges();
                List.Add(quyenhethong);
            });

            EditCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TxtQuyenHeThong) || string.IsNullOrWhiteSpace(TxtQuyenHeThong) || SelectedItem == null)
                    return false;
                var displaylist = Dataprovider.Ins.DB.QuyenHeThongs.Where(x => x.TenQuyen == TxtQuyenHeThong);
                if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            }, (p) => {
                var quyenhethong = Dataprovider.Ins.DB.QuyenHeThongs.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                quyenhethong.TenQuyen = TxtQuyenHeThong;
                Dataprovider.Ins.DB.SaveChanges();

                SelectedItem.TenQuyen = TxtQuyenHeThong;
            });

            DeleteCommmand = new RelayCommand<object>((p)=> { if (SelectedItem == null) return false; return true; },(p)=> {
                MessageBoxResult dr = MessageBox.Show("Nếu bạn xoá quyền " + SelectedItem.TenQuyen + " thì những người dùng có quyền  " + SelectedItem.TenQuyen + " cũng sẽ bị xoá. Bạn có chắc chắn muốn xoá không?", "Cảnh Báo", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (dr == MessageBoxResult.No)
                    return;
                try
                {
                    XoaDuLieu.XoaQuyenHeThong(SelectedItem);
                  
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.ToString());
                }
                


            });
        }

        private void LoadQuyenHeThong()
        {

            List = new ObservableCollection<QuyenHeThong>();
            var lquyen = Dataprovider.Ins.DB.QuyenHeThongs;

            foreach (var item in lquyen)
            {

                QuyenHeThong _quyenhethong = new QuyenHeThong();
                _quyenhethong.Id = item.Id;
                _quyenhethong.TenQuyen = item.TenQuyen;
                List.Add(_quyenhethong);
            }
        }
    }
}
