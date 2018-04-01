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
using System.Windows.Shapes;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for MainWindows.xaml
    /// </summary>
    public partial class MainWindows : Window
    {
        public MainWindows()
        {
            InitializeComponent();
            MainContent.Children.Add(new UserControlDaTaBroad());
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MovieCusorMenu(index);

            switch (index)
            {
                case 0:
                    MainContent.Children.Clear();
                    MainContent.Children.Add(new UserControlDaTaBroad());
                    break;
                default:
                    break;
            }
        }

        private void MovieCusorMenu(int index)
        {
            TransitionSlide.OnApplyTemplate();
            GridSlider.Margin = new Thickness(0, 150 + (60 * index), 0, 0);
        }
    }
}
