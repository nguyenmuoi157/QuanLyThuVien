using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
namespace QuanLyThuVien.ViewModel
{
   public class ControlBarViewModel:BaseViewModel
    {
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MaximineWindowCommand { get; set; }
        public ICommand MinimineWindowCommand { get; set; }
        public ICommand MoveWindowCommand { get; set; }
        public ControlBarViewModel()
        {
            CloseWindowCommand = new RelayCommand<UserControl>((p)=> { return true; },(p)=> {
               FrameworkElement window =  GetWindow(p);
                var w = window as Window;
                if (w!=null)
                {
                   
                    w.Close();
                }


            });
            MaximineWindowCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => {
                FrameworkElement window = GetWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    if (w.WindowState == WindowState.Normal)
                    {
                        w.WindowState = WindowState.Maximized;
                    }
                    else w.WindowState = WindowState.Normal;
                }


            });
            MinimineWindowCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => {
                FrameworkElement window = GetWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    if (w.WindowState == WindowState.Minimized)
                    {
                        w.WindowState = WindowState.Maximized;
                    }
                    else w.WindowState = WindowState.Minimized;
                }


            });
            MoveWindowCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                FrameworkElement window = GetWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }

            });
        }

        FrameworkElement GetWindow(FrameworkElement p)
        {
            FrameworkElement parent = p;
            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }

            return parent;
        }
    }
}
