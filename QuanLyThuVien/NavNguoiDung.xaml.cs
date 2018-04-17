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

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for NavNguoiDung.xaml
    /// </summary>
    public partial class NavNguoiDung : Page
    {
        public NavNguoiDung()
        {
            InitializeComponent();
            MainContent.Content = new PageNguoiDung();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TransitionSlide.OnApplyTemplate();
            int index = int.Parse(((Button)e.Source).Uid);
            GridCusor.Margin = new Thickness(10 + (index * 150), 0, 0, 0);
            switch (index)
            {
                case 0:
                    MainContent.Content = new PageNguoiDung();
                    break;
                case 1:
                    MainContent.Content = new PageQuyenHeThong();
                    break;
               
                default:
                    break;
            }
        }
    }
}
