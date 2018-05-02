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
    class TacGiaViewModel: BaseViewModel
    {
        private ObservableCollection<TacGia> _list;
        public ObservableCollection<TacGia> List { get => _list; set { _list = value; OnPropertyChanged(); } }


        public ICommand PLoadCommand { get; set; }
        public ICommand AddCommmand { get; set; }
        public ICommand EditCommmand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private TacGia  _SelectedItem;
        public TacGia SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged(); if (_SelectedItem != null) TxtTacGia = SelectedItem.TenTacGia; } }

        private String _txtTacGia = "";
        public string TxtTacGia { get => _txtTacGia; set { _txtTacGia = value; OnPropertyChanged(); } }


        public TacGiaViewModel()
        {
            PLoadCommand = new RelayCommand<Page>((p) => { return true; }, (p) => {
                if (p == null)
                    return;
                LoadTacGia();
                var a = p.Parent;
            });
            AddCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TxtTacGia) || string.IsNullOrWhiteSpace(TxtTacGia))
                    return false;
                var displaylist = Dataprovider.Ins.DB.TacGias.Where(x => x.TenTacGia == TxtTacGia);
                if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            }, (p) => {
                var tacgia = new TacGia() { TenTacGia = TxtTacGia };
                Dataprovider.Ins.DB.TacGias.Add(tacgia);
                Dataprovider.Ins.DB.SaveChanges();
                List.Add(tacgia);
            });

            EditCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TxtTacGia) || string.IsNullOrWhiteSpace(TxtTacGia) || SelectedItem == null)
                    return false;
                var displaylist = Dataprovider.Ins.DB.TacGias.Where(x => x.TenTacGia == TxtTacGia);
                if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            }, (p) => {
                var tacgia = Dataprovider.Ins.DB.TacGias.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                tacgia.TenTacGia = TxtTacGia;
                Dataprovider.Ins.DB.SaveChanges();
                SelectedItem.TenTacGia = TxtTacGia;
            });
            DeleteCommand = new RelayCommand<object>((p) => { if (SelectedItem == null) return false; return true; }, (p) => {
                MessageBoxResult dr = MessageBox.Show("Nếu bạn xoá ngôn ngữ " + SelectedItem.TenTacGia + " thì những tài liệu có ngôn ngữ  " + SelectedItem.TenTacGia + " cũng sẽ bị xoá. Bạn có chắc chắn muốn xoá không?", "Cảnh Báo", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (dr == MessageBoxResult.No)
                    return;
                try
                {
                    List.Remove(SelectedItem);
                    XoaDuLieu.XoaTacGia(SelectedItem);
                    SelectedItem = null;
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.ToString());
                }
                
                
            });
        }

        private void LoadTacGia()
        {
            List = new ObservableCollection<TacGia>(Dataprovider.Ins.DB.TacGias);
        }

    }
}
