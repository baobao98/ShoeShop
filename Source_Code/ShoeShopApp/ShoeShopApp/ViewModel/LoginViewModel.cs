using ShoeShopApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShoeShopApp.ViewModel
{
    public class state
    {
        public static int idcur;
        public static int role;
    }
    public class LoginViewModel:BaseViewModel
    {
        public bool IsLogin { get; set; }
        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        public String IDNV { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand RegistrationCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }


        // mọi thứ xử lý sẽ nằm trong này
        public LoginViewModel()
        {
            IsLogin = false;
            Password = "";
            UserName = "";
            RegistrationCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { registration(p); });
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            //LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { MessageBox.Show("hishishsi"); });
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            IDNV = "Error";
        }
        void registration (Window p)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
        }
        void Login(Window p)
        {
            if (p == null)
                return;

            /*
             baobao
             123
             */

            //string passEncode = MD5Hash(Base64Encode(Password));
                 
            int accCount = DataProvider.Ins.db.TaiKhoans.Where(x => x.TenDN == UserName && x.MatKhau == Password &&x.isDeleted==false).Count();
            if (accCount > 0)
            {
                IsLogin = true;
                int idtk = DataProvider.Ins.db.TaiKhoans.Where(x => x.TenDN == UserName && x.MatKhau == Password).SingleOrDefault().ID;
                var idnhanvien = DataProvider.Ins.db.NhanViens.Where(x => x.TaiKhoan == idtk).SingleOrDefault();
                if (idnhanvien != null)
                {
                    state.idcur = idnhanvien.MaNV;
                    state.role = idnhanvien.MaLoaiNV;
                    IDNV=idnhanvien.HoVaTen;
                }
                p.Close();
            }
            else
            {
                IsLogin = false;
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
            }
        }

        //public static string Base64Encode(string plainText)
        //{
        //    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        //    return System.Convert.ToBase64String(plainTextBytes);
        //}



        //public static string MD5Hash(string input)
        //{
        //    StringBuilder hash = new StringBuilder();
        //    MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
        //    byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        hash.Append(bytes[i].ToString("x2"));
        //    }
        //    return hash.ToString();
        //}



    }
}
