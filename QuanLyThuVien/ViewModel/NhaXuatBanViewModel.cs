using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public ICommand DeleteCommmand { get; set; }

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

        private string _txtNhaXuatBan = "";
        public string TenNhaXuatBan { get => _txtNhaXuatBan; set { _txtNhaXuatBan = value; OnPropertyChanged(); } }

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

                SelectedItem.TenNhaXuatBan = TenNhaXuatBan;
                SelectedItem.Email = Email;
                SelectedItem.SoDienThoai = SoDienThoai;
                SelectedItem.DiaChi = DiaChi;
            });
        }

        private void LoadNhaXuatBan()
        {

            List = new ObservableCollection<NhaXuatBan>();
            var lNhaXuatBan = Dataprovider.Ins.DB.NhaXuatBans;

            foreach (var item in lNhaXuatBan)
            {

                NhaXuatBan _nhaxuatban = new NhaXuatBan();
                _nhaxuatban.Id = item.Id;
                _nhaxuatban.TenNhaXuatBan = item.TenNhaXuatBan;
                List.Add(_nhaxuatban);
            }
        }

    }
}

