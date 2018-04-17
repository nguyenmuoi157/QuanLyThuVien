using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QuanLyThuVien.Model;

namespace QuanLyThuVien.ViewModel
{
    class LoginWindowViewModel:BaseViewModel
    {
        public bool IsLogin{get;set;}
        private String _UserName = "";
        private String _PassWord = "";
        
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
        public string PassWord { get => _PassWord; set { _PassWord = value; OnPropertyChanged(); } }


        public ICommand BtnLoginClickCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }


        public LoginWindowViewModel()
        {
            IsLogin = false;
            BtnLoginClickCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Login(p);
            });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                PassWord = p.Password;
            });
        }

        void Login(Window p)
        {
            if (p == null)
                return;
             string pass = MD5Hash(PassWord);
            var acc = Dataprovider.Ins.DB.NguoiDungs.Where(x=>(x.Email== UserName||x.TenNguoiDung== UserName) && x.MatKhau== pass).Count();

            if (acc>0)
                {
                    IsLogin = true;
                    p.Close();
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu, vui lòng thử lại");
                }

            
        }
        public string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

    }
}
