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
  public  class ViTriViewModel: BaseViewModel
    {
        private ObservableCollection<ViTri> _list;
        public ObservableCollection<ViTri> List { get => _list; set { _list = value; OnPropertyChanged(); } }


        public ICommand PLoadCommand { get; set; }
        public ICommand AddCommmand { get; set; }
        public ICommand EditCommmand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private ViTri _SelectedItem;
        public ViTri SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged(); if (_SelectedItem != null) { TxtViTri = SelectedItem.SoGiaSach.ToString();TxtSucChua = SelectedItem.SucChua.ToString(); } } }

        private string _txtViTri = "";
        public string TxtViTri { get => _txtViTri; set { _txtViTri = value; OnPropertyChanged(); } }
        private string _TxtSucChua = "";
        public string TxtSucChua { get => _TxtSucChua; set { _TxtSucChua = value; OnPropertyChanged(); } }


        public ViTriViewModel()
        {
            PLoadCommand = new RelayCommand<Page>((p) => { return true; }, (p) => {
                if (p == null)
                    return;
                LoadViTri();

            });
            AddCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TxtViTri) || string.IsNullOrEmpty(TxtSucChua))
                    return false;
                if (!int.TryParse(TxtViTri, out int sogia))
                    return false;
                var displaylist = Dataprovider.Ins.DB.ViTris.Where(x => x.SoGiaSach == sogia);
                if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            }, (p) => {
                var ViTri = new ViTri() { SoGiaSach = int.Parse(TxtViTri), SucChua = int.Parse(TxtSucChua) };
                Dataprovider.Ins.DB.ViTris.Add(ViTri);
                Dataprovider.Ins.DB.SaveChanges();
                List.Add(ViTri);
            });

            EditCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TxtViTri) || string.IsNullOrWhiteSpace(TxtViTri) || SelectedItem == null)
                    return false;
                var displaylist = Dataprovider.Ins.DB.ViTris.Where(x => x.SoGiaSach.ToString() == TxtViTri);
                if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            }, (p) => {
                var ViTri = Dataprovider.Ins.DB.ViTris.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                ViTri.SoGiaSach = int.Parse(TxtViTri);
                ViTri.SucChua = int.Parse(TxtSucChua);
                Dataprovider.Ins.DB.SaveChanges();

                SelectedItem.SoGiaSach = int.Parse(TxtViTri);
                SelectedItem.SucChua = int.Parse(TxtSucChua);
            });

            #region deletecommand
            DeleteCommand = new RelayCommand<object>((p)=> { if (SelectedItem == null) { return false; } return true; },(p)=> {
                var ten = SelectedItem.SoGiaSach;
                MessageBoxResult dr = MessageBox.Show("Nếu bạn xoá giá sách số " + ten + " thì những tài liệu thuộc giá " + ten + " cũng sẽ bị xoá. Bạn có chắc chắn muốn xoá không?", "Cảnh Báo", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (dr == MessageBoxResult.No)
                    return;
                try
                {
                    XoaDuLieu.XoaViTri(SelectedItem);
                    List.Remove(SelectedItem);
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.ToString());
                }


            });
            #endregion
        }

        private void LoadViTri()
        {
            List = new ObservableCollection<ViTri>(Dataprovider.Ins.DB.ViTris);
        }
    }
}
