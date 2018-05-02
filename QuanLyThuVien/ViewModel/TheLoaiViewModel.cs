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
    class TheLoaiViewModel: BaseViewModel
    {
        private ObservableCollection<TheLoai> _list;
        public ObservableCollection<TheLoai> List { get => _list; set { _list = value; OnPropertyChanged(); } }


        public ICommand PLoadCommand { get; set; }
        public ICommand AddCommmand { get; set; }
        public ICommand EditCommmand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private TheLoai _SelectedItem;
        public TheLoai SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged(); if (_SelectedItem != null) TxtTheLoai = SelectedItem.TenTheLoai; } }

        private String _txtTheLoai = "";
        public string TxtTheLoai { get => _txtTheLoai; set { _txtTheLoai = value; OnPropertyChanged(); } }


        public TheLoaiViewModel()
        {
            PLoadCommand = new RelayCommand<Page>((p) => { return true; }, (p) => {
                if (p == null)
                    return;
                LoadTheLoai();

            });
            AddCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TxtTheLoai) || string.IsNullOrWhiteSpace(TxtTheLoai))
                    return false;
                var displaylist = Dataprovider.Ins.DB.TheLoais.Where(x => x.TenTheLoai == TxtTheLoai);
                if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            }, (p) => {
                var TheLoai = new TheLoai() { TenTheLoai = TxtTheLoai };
                Dataprovider.Ins.DB.TheLoais.Add(TheLoai);
                Dataprovider.Ins.DB.SaveChanges();
                List.Add(TheLoai);
            });

            EditCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TxtTheLoai) || string.IsNullOrWhiteSpace(TxtTheLoai) || SelectedItem == null)
                    return false;
                var displaylist = Dataprovider.Ins.DB.TheLoais.Where(x => x.TenTheLoai == TxtTheLoai);
                if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            }, (p) => {
                var TheLoai = Dataprovider.Ins.DB.TheLoais.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                TheLoai.TenTheLoai = TxtTheLoai;
                Dataprovider.Ins.DB.SaveChanges();

                SelectedItem.TenTheLoai = TxtTheLoai;
            });

            DeleteCommand = new RelayCommand<object>((p) => { if (SelectedItem == null) return false; return true; }, (p) => {
                MessageBoxResult dr = MessageBox.Show("Nếu bạn xoá ngôn ngữ " + SelectedItem.TenTheLoai + " thì những tài liệu có ngôn ngữ  " + SelectedItem.TenTheLoai + " cũng sẽ bị xoá. Bạn có chắc chắn muốn xoá không?", "Cảnh Báo", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (dr == MessageBoxResult.No)
                    return;
                try
                {
                    XoaDuLieu.XoaTheLoai(SelectedItem);
                    List.Remove(SelectedItem);
                    SelectedItem = null;

                }
                catch (Exception e)
                {

                    MessageBox.Show(e.ToString());
                }



            });
        }

        private void LoadTheLoai()
        {
            List = new ObservableCollection<TheLoai>(Dataprovider.Ins.DB.TheLoais);
        }

    }
}
