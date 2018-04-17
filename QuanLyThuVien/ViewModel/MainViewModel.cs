using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using QuanLyThuVien.Model;
namespace QuanLyThuVien.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public ICommand WindowsLoadCommand { get; set; }
        public ICommand SelectItemCommand { get; set; }
        public ICommand FrameLoadCommand { get; set; }
        public ICommand SelectSubMenuCommand { get; set; }
        public ICommand HomePageSelectCommand { get; set; }
        

        public Frame frame{ get;set;}
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
                    p.Show();
                else
                    p.Close();
            });
            FrameLoadCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                frame = p;
                frame.Content = new PageTrangchu();
            });
            SelectItemCommand = new RelayCommand<ListView>((p) => { return true; }, (p) => {
                int selectedIndex = p.SelectedIndex;
               
                switch (selectedIndex)
                {
                    case 0:
                        frame.Content = new PageTrangchu();
                        break;
                    case 1:
                        frame.Content = new PageTaiLieu();
                        break;
                    case 2:
                        frame.Content = new PageBanDoc();
                        break;
                    case 3:
                        frame.Content = new PageMuon();
                        break;
                    case 4:
                        frame.Content = new PageTra();
                        break;
                    default:
                        frame.Content = new NavNguoiDung();
                        break;
                }
            });
            SelectSubMenuCommand = new RelayCommand<ListView>((p) => { return true; }, (p) => {
                int selectedIndex = p.SelectedIndex;

                switch (selectedIndex)
                {
                    case 0:
                        frame.Content = new PageTaiLieu();
                        break;
                    case 1:
                        frame.Content = new PageExpanTaiLieu();
                        break;
                  
                    default:
                        break;
                }
            });
            HomePageSelectCommand = new RelayCommand<ListView>((p) => { return true; }, (p) => {
                int selectedIndex = p.SelectedIndex;

                switch (selectedIndex)
                {
                    case 0:
                        frame.Content = new PageTrangchu();
                        break;
                    default:
                        break;
                }
            });

            
        }
    }
}
