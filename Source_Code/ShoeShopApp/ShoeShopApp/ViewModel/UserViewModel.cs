using ShoeShopApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ShoeShopApp.ViewModel
{
    public class UserViewModel:BaseViewModel
    {
        private ObservableCollection<TaiKhoan> _List;
        public ObservableCollection<TaiKhoan> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private TaiKhoan _SelectedItem;
        public TaiKhoan SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    UserName = SelectedItem.TenDN;
                    Password = SelectedItem.MatKhau;
                }
            }
        }

        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }

        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }


        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public UserViewModel()
        {
            List = new ObservableCollection<TaiKhoan>(DataProvider.Ins.db.TaiKhoans.Where(x => x.isDeleted == false));
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(UserName) || String.IsNullOrEmpty(Password))
                    return false;

                var displayList = DataProvider.Ins.db.TaiKhoans.Where(x => x.TenDN == UserName && x.isDeleted == false);
                if (displayList == null || displayList.Count() != 0)
                    return false;

                return true;

            }, (p) =>
            {
                var displayList = DataProvider.Ins.db.TaiKhoans.Where(x => (x.TenDN == UserName &&x.MatKhau==Password && x.isDeleted == true)).SingleOrDefault();
                if (displayList == null)
                {
                    var TaiK = new TaiKhoan() { TenDN = UserName, MatKhau = Password, isDeleted = false };

                    DataProvider.Ins.db.TaiKhoans.Add(TaiK);
                    DataProvider.Ins.db.SaveChanges();

                    List.Add(TaiK);
                }
                else 
                {
                    displayList.isDeleted = false;
                    DataProvider.Ins.db.SaveChanges();
                    List.Add(displayList);
                }

            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(UserName) ||string.IsNullOrEmpty(Password) || SelectedItem == null)
                    return false;

                var displayList = DataProvider.Ins.db.TaiKhoans.Where(x => x.TenDN == UserName &&x.MatKhau==Password);
                if (displayList == null || displayList.Count() != 0)
                    return false;

                return true;

            }, (p) =>
            {
                var NhaCC = DataProvider.Ins.db.TaiKhoans.Where(x => x.TenDN == SelectedItem.TenDN).SingleOrDefault();
                NhaCC.TenDN = UserName;
                NhaCC.MatKhau = Password;
                DataProvider.Ins.db.SaveChanges();

                List.Remove(SelectedItem);
                List.Add(NhaCC);
                SelectedItem = NhaCC;
            });
            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(UserName) || SelectedItem == null)
                    return false;
                return true;

            }, (p) =>
            {
                String s = MessageBox.Show("Thông Báo!!!", "bạn thật sự muốn xóa!!!", MessageBoxButton.YesNo, MessageBoxImage.Stop).ToString();
                if (s == "Yes")
                {
                    var NhaCC = DataProvider.Ins.db.TaiKhoans.Where(x =>x.ID  == SelectedItem.ID &&x.isDeleted==false).SingleOrDefault();
                    NhaCC.isDeleted = true;
                    DataProvider.Ins.db.SaveChanges();

                    List.Remove(SelectedItem);
                }
                return;
            });
        }
    }
}
