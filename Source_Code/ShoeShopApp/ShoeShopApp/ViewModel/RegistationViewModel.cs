using ShoeShopApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShoeShopApp.ViewModel
{
    public class RegistationViewModel:BaseViewModel
    {
        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }
        private string _Passwordag;
        public string Passwordag { get => _Passwordag; set { _Passwordag = value; OnPropertyChanged(); } }
        public ICommand OKCommand { get; set; }
        public ICommand PasswordChangedCommand1 { get; set; }
        public ICommand PasswordChangedCommand2 { get; set; }
        public RegistationViewModel()
        {
            UserName = "";
            Password = "";
            Passwordag = "";
            PasswordChangedCommand1 = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            PasswordChangedCommand2 = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Passwordag = p.Password; });
            OKCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { regis(p); });
        }
        void regis(Window p)
        {
            if (p == null)
                return;
            if (Password != Passwordag)
            {
                MessageBox.Show("mk khong trung nhau");
            }
            var acclist = DataProvider.Ins.db.TaiKhoans.Where(x => x.TenDN == UserName && x.isDeleted == false).SingleOrDefault();
            if(acclist!=null)
            {
                MessageBox.Show("tai khoan ton tai");
            }
            else
            {
                var TaiK = new TaiKhoan() { TenDN = UserName, MatKhau = Password, isDeleted = false };

                DataProvider.Ins.db.TaiKhoans.Add(TaiK);
                DataProvider.Ins.db.SaveChanges();
                MessageBox.Show("dk thanh cong!");
                p.Close();
            }
        }
    }
}
