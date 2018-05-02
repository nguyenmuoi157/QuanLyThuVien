using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyThuVien.ViewModel
{
   public class NavTaiLieuDatacontex:BaseViewModel
    {
        public ICommand LoadContent { get; set; }

        public ICommand BtnTaiLieu { get; set; }
        public ICommand BtnNhaXuatBan { get; set; }
        public ICommand BtnTacGia { get; set; }
        public ICommand BtnNgonNgu { get; set; }
        public ICommand BtnTheLoai { get; set; }
        public ICommand BtnViTri { get; set; }

        //public object MainContent { get; set; }
        public Frame MainFrame { get; set; }

        public NavTaiLieuDatacontex()
        {


            LoadContent = new RelayCommand<Frame>((p)=> { return true; },(p)=> {
                if (p == null)
                    return;
                MainFrame = p;
               p.Content = new PageQuanLyTaiLieu();
                //MainContent = p.Content;
            });

            BtnTaiLieu = new RelayCommand<object>((p) => { return true; }, (p) => {
                
                MainFrame.Content = new PageQuanLyTaiLieu();
                //MainContent = MainFrame.Content;
            });
            BtnNhaXuatBan = new RelayCommand<object>((p) => { return true; }, (p) => {
                MainFrame.Content = new PageNhaXuatBan();
                //MainContent = MainFrame.Content;

            });
            BtnTacGia = new RelayCommand<object>((p) => { return true; }, (p) => {
                MainFrame.Content = new PageTacGia();
                //MainContent = MainFrame.Content;

            });
            BtnNgonNgu = new RelayCommand<object>((p) => { return true; }, (p) => {
                MainFrame.Content = new PageNgonNgu();

                //MainContent = MainFrame.Content;

            });
            BtnTheLoai = new RelayCommand<object>((p) => { return true; }, (p) => {
                MainFrame.Content = new PageTheLoai();
                //MainContent = MainFrame.Content;

            });
            BtnViTri = new RelayCommand<object>((p) => { return true; }, (p) => {
                MainFrame.Content = new PageViTri();
                //MainContent = MainFrame.Content;

            });

        }
    }
}
