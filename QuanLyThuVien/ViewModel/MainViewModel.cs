using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace QuanLyThuVien.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public ICommand WindowsLoadCommand { get; set; }
        public MainViewModel()
        {
            WindowsLoadCommand = new RelayCommand<object>((p)=> { return true; },(p)=> {
                LoginWindows login = new LoginWindows();
                login.ShowDialog();
            });
        }
    }
}
