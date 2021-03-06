﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QuanLyThuVien.Model;
using System.Windows.Controls;
using System.Windows;

namespace QuanLyThuVien.ViewModel
{
    
   public class NgonNguViewModel: BaseViewModel
    {
        private ObservableCollection<NgonNgu> _listNgonNgu;
        public ObservableCollection<NgonNgu> ListNgonNgu { get => _listNgonNgu; set{ _listNgonNgu = value;OnPropertyChanged(); } }


        public ICommand PLoadCommand { get; set; }
        public ICommand BtnAddCommmand{get;set;}
        public ICommand EditCommmand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private NgonNgu _SelectedItem;
        public NgonNgu SelectedItem { get=>_SelectedItem; set { _SelectedItem = value;OnPropertyChanged(); if (_SelectedItem != null) TxtNgonNgu = SelectedItem.TenNgonNgu; } }

        private String _txtNgonNgu = "";
        public string TxtNgonNgu{ get => _txtNgonNgu; set { _txtNgonNgu = value; OnPropertyChanged(); } }

       

        public NgonNguViewModel()
        {
            PLoadCommand = new RelayCommand<Page>((p) => { return true; }, (p) => {
                if (p == null)
                    return;
                LoadNgonNgu();

            });
            BtnAddCommmand = new RelayCommand<object>((p) => {
            if (string.IsNullOrEmpty(TxtNgonNgu) || string.IsNullOrWhiteSpace(TxtNgonNgu))
                return false;
                var displaylist = Dataprovider.Ins.DB.NgonNgus.Where(x => x.TenNgonNgu == TxtNgonNgu);
                if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            },(p)=> {
                    var ngonngu = new NgonNgu() { TenNgonNgu = TxtNgonNgu };
                    Dataprovider.Ins.DB.NgonNgus.Add(ngonngu);
                    Dataprovider.Ins.DB.SaveChanges();
                    ListNgonNgu.Add(ngonngu);
                });

            EditCommmand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(TxtNgonNgu) || string.IsNullOrWhiteSpace(TxtNgonNgu)|| SelectedItem==null)
                    return false;
                var displaylist = Dataprovider.Ins.DB.NgonNgus.Where(x => x.TenNgonNgu == TxtNgonNgu);
                if (displaylist == null || displaylist.Count() != 0)
                    return false;
                return true;
            }, (p) => {
                var ngonngu = Dataprovider.Ins.DB.NgonNgus.Where(x=>x.Id == SelectedItem.Id).SingleOrDefault();
                ngonngu.TenNgonNgu = TxtNgonNgu;
                Dataprovider.Ins.DB.SaveChanges();

                SelectedItem.TenNgonNgu = TxtNgonNgu;
            });
            DeleteCommand = new RelayCommand<object>((p) => { if (SelectedItem == null) return false; return true; }, (p) => {
                MessageBoxResult dr = MessageBox.Show("Nếu bạn xoá ngôn ngữ " + SelectedItem.TenNgonNgu + " thì những tài liệu có ngôn ngữ  " + SelectedItem.TenNgonNgu + " cũng sẽ bị xoá. Bạn có chắc chắn muốn xoá không?", "Cảnh Báo", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (dr == MessageBoxResult.No)
                    return;
                try
                {
                    ListNgonNgu.Remove(SelectedItem);
                    XoaDuLieu.XoaNgonNgu(SelectedItem);


                }
                catch (Exception e)
                {

                    MessageBox.Show(e.ToString());
                }



            });
        }

        private void LoadNgonNgu()
        {
            ListNgonNgu = new ObservableCollection<NgonNgu>(Dataprovider.Ins.DB.NgonNgus);
        }
    }
 }

