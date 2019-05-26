﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoeShopApp.Model;
using System.Windows;
using System.Windows.Input;


namespace ShoeShopApp.ViewModel
{
    public class MainViewModel: BaseViewModel
    {
        public bool Isloaded = false;
        public ICommand LoadedWindowCommand { get; set; }
        private string _textContent;
        public string textContent { get => _textContent; set { _textContent = value; OnPropertyChanged(); } }
        public MainViewModel()
        {
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
              {
                  Isloaded = true;
                  if (p == null)
                  {
                      return;
                  }
                  
                  p.Hide();
                  LoginWindow loginWindow = new LoginWindow();
                  loginWindow.ShowDialog();
                  if (loginWindow.DataContext == null)
                  {
                      return;
                  }
                  var loginVM = loginWindow.DataContext as LoginViewModel;
                  if (loginVM.IsLogin)
                  {
                      p.Show();
                      int a = 1;
                      load(a);
                  }
                  else
                  {
                      p.Close();
                  }
              });
            //List<NhaCungCap> nhaCungCaps = new List<NhaCungCap>();
            //nhaCungCaps = DataProvider.Ins.db.NhaCungCaps.ToList();
            //textContent = "Hello World!!! This text is loaded from database (table NhaCungCap): <" + nhaCungCaps[0].TenNCC + "> " +
            //    "Test connect successfully";
        }
        void load(int a)
        {
            List<NhaCungCap> nhaCungCaps = new List<NhaCungCap>();
            nhaCungCaps = DataProvider.Ins.db.NhaCungCaps.ToList();
            textContent = "Hello World!!! This text is loaded from database (table NhaCungCap): <" + nhaCungCaps[0].TenNCC + "> " +
                "Test connect successfully"+a.ToString();
        }
    }
}
