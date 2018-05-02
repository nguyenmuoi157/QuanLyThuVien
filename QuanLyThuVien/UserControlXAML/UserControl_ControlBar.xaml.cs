using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuanLyThuVien.ViewModel;
namespace QuanLyThuVien.UserControlXAML
{
 



    public partial class UserControl_ControlBar : UserControl
    {
        public ControlBarViewModel ViewModel { get; set; }
        public UserControl_ControlBar()
        {
            InitializeComponent();
            ViewModel = new ControlBarViewModel();
            this.DataContext = ViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/nguyenmuoi157/QuanLyThuVien");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/konoha.akatshuki");
        }
    }
}
