using System;
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
        public ICommand HoaDonCommand { get; set; }
        public ICommand SanPhamCommand { get; set; }
        public ICommand NhanVienCommand { get; set; }
        public ICommand KhachHangCommand { get; set; }

        public bool Isloaded = false;
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand DangXuatCommand { get; set; }
        private string _textContent;
        public string textContent { get => _textContent; set { _textContent = value; OnPropertyChanged(); } }
        public MainViewModel()
        {
            //Command call other form
            
            HoaDonCommand = new RelayCommand<object>(p => { return true; }, p => { HoaDonWindow wd = new HoaDonWindow(); wd.ShowDialog(); });
            SanPhamCommand= new RelayCommand<object>(p => { return true; }, p => { SanPhamWindow wd = new SanPhamWindow(); wd.ShowDialog(); });
            NhanVienCommand= new RelayCommand<object>(p => {
                if (state.role!=4)
                {
                    return false;
                }
                return true;
            }, p => { NhanVienWindow wd = new NhanVienWindow(); wd.ShowDialog(); });
            KhachHangCommand = new RelayCommand<object>(p => { return true; }, p => { KhachHangWindow wd = new KhachHangWindow(); wd.ShowDialog(); });

            //end 
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
              {
                  Isloaded = true;
                  if (p == null)
                  {
                      return;
                  }
                  check(p);
              });
            DangXuatCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { check(p); });
            //List<NhaCungCap> nhaCungCaps = new List<NhaCungCap>();
            //nhaCungCaps = DataProvider.Ins.db.NhaCungCaps.ToList();
            //textContent = "Hello World!!! This text is loaded from database (table NhaCungCap): <" + nhaCungCaps[0].TenNCC + "> " +
            //    "Test connect successfully";
        }
        void check(Window p)
        {
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
            }
            else
            {
                p.Close();
            }
        }
    }
}
