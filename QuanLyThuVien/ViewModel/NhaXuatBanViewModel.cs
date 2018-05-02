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
    class NhaXuatBanViewModel: BaseViewModel
    {
        private ObservableCollection<NhaXuatBan> _list;
        public ObservableCollection<NhaXuatBan> List { get => _list; set { _list = value; OnPropertyChanged(); } }


        public ICommand PLoadCommand { get; set; }
        public ICommand AddCommmand { get; set; }
        public ICommand EditCommmand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private NhaXuatBan _SelectedItem;
        public NhaXuatBan SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged();
                if (_SelectedItem != null)
                {
                    TenNhaXuatBan = SelectedItem.TenNhaXuatBan;
                    Email = SelectedItem.Email;
                    SoDienThoai = SelectedItem.SoDienThoai;
                    DiaChi = SelectedItem.DiaChi;
                }
            }
        }

        private string _TenNhaXuatBan = "";
        public string TenNhaXuatBan { get => _TenNhaXuatBan; set { _TenNhaXuatBan = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _SoDienThoai;
        public string SoDienThoai { get => _SoDienThoai; set { _SoDienThoai = value; OnPropertyChanged(); } }

        private string _DiaChi;
        public string DiaChi { get => _DiaChi; set { _DiaChi = value; OnPropertyChanged(); } }





        public NhaXuatBanViewModel()
        {
            PLoadCommand = new RelayCommand<Page>((p) => { return true; }, (p) => {
                if (p == null)
                    return;
                LoadNhaXuatBan();

            });
            AddCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TenNhaXuatBan)|| string.IsNullOrEmpty(Email)|| string.IsNullOrEmpty(SoDienThoai)|| string.IsNullOrEmpty(DiaChi))
                    return false;
                var displaylist = Dataprovider.Ins.DB.NhaXuatBans.Where(x => x.TenNhaXuatBan == TenNhaXuatBan);
                if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            }, (p) => {
                NhaXuatBan nhaxuatban = new NhaXuatBan
                {
                    TenNhaXuatBan = TenNhaXuatBan,
                    Email = Email,
                    SoDienThoai = SoDienThoai,
                    DiaChi = DiaChi
                };

                Dataprovider.Ins.DB.NhaXuatBans.Add(nhaxuatban);
                Dataprovider.Ins.DB.SaveChanges();

                List.Add(nhaxuatban);
            });

            EditCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TenNhaXuatBan) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(SoDienThoai) || string.IsNullOrEmpty(DiaChi) || SelectedItem == null)
                    return false;
                var displaylist = Dataprovider.Ins.DB.NhaXuatBans.Where(x => x.TenNhaXuatBan == TenNhaXuatBan);
                if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            }, (p) => {
                NhaXuatBan nhaxuatban = Dataprovider.Ins.DB.NhaXuatBans.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                nhaxuatban.TenNhaXuatBan = TenNhaXuatBan;
                nhaxuatban.Email = Email;
                nhaxuatban.SoDienThoai = SoDienThoai;
                nhaxuatban.DiaChi = DiaChi;
                Dataprovider.Ins.DB.SaveChanges();

                SelectedItem = nhaxuatban;
            });

            DeleteCommand = new RelayCommand<object>((p) => { if (SelectedItem == null) return false; return true; }, (p) => {
                MessageBoxResult dr = MessageBox.Show("Nếu bạn xoá ngôn ngữ " + SelectedItem.TenNhaXuatBan + " thì những tài liệu có ngôn ngữ  " + SelectedItem.TenNhaXuatBan + " cũng sẽ bị xoá. Bạn có chắc chắn muốn xoá không?", "Cảnh Báo", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (dr == MessageBoxResult.No)
                    return;
                try
                {
                    XoaDuLieu.XoaNhaXuatBan(SelectedItem);
                    List.Remove(SelectedItem);
                    SelectedItem = null;

                }
                catch (Exception e)
                {

                    MessageBox.Show(e.ToString());
                }



            });
        }

        private void LoadNhaXuatBan()
        {

            List = new ObservableCollection<NhaXuatBan>(Dataprovider.Ins.DB.NhaXuatBans);
            
        }

    }
}

