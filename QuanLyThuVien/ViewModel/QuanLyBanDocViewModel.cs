using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace QuanLyThuVien.ViewModel
{
    

    class QuanLyBanDocViewModel: BaseViewModel
    {
        public ICommand ThemMoiCommand { get; set; }


    public QuanLyBanDocViewModel()
    {
            ThemMoiCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                ThemBanDocWindows them = new ThemBanDocWindows();
                them.ShowDialog();
               
            });

        }

}
}
