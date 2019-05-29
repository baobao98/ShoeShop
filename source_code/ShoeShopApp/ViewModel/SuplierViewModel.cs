using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Input;
using ShoeShopApp.Model;

namespace ShoeShopApp.ViewModel
{
    public class SuplierViewModel:BaseViewModel
    {
        private ObservableCollection<NhaCungCap> _List;
        public ObservableCollection<NhaCungCap> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private NhaCungCap _SelectedItem;
        public NhaCungCap SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.TenNCC;
                }
            }
        }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }


        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public SuplierViewModel()
        {
            List = new ObservableCollection<NhaCungCap>(DataProvider.Ins.db.NhaCungCaps.Where(x=>x.isDeleted==false));

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName))
                    return false;

                var displayList = DataProvider.Ins.db.NhaCungCaps.Where(x => x.TenNCC == DisplayName && x.isDeleted==false);
                if (displayList == null || displayList.Count() != 0)
                    return false;

                return true;

            }, (p) =>
            {
                var displayList = DataProvider.Ins.db.NhaCungCaps.Where(x => x.TenNCC == DisplayName && x.isDeleted == true).SingleOrDefault();
                if (displayList!=null)
                {
                    displayList.isDeleted = false;
                    DataProvider.Ins.db.SaveChanges();
                    List.Add(displayList);
                }
                else
                {
                    var NhaCC = new NhaCungCap() { TenNCC = DisplayName, isDeleted = false };

                    DataProvider.Ins.db.NhaCungCaps.Add(NhaCC);
                    DataProvider.Ins.db.SaveChanges();

                    List.Add(NhaCC);
                }
                
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName) || SelectedItem == null)
                    return false;

                var displayList = DataProvider.Ins.db.NhaCungCaps.Where(x => x.TenNCC == DisplayName);
                if (displayList == null || displayList.Count() != 0)
                    return false;

                return true;

            }, (p) =>
            {
                var NhaCC = DataProvider.Ins.db.NhaCungCaps.Where(x => x.MaNCC == SelectedItem.MaNCC).SingleOrDefault();
                NhaCC.TenNCC = DisplayName;
                DataProvider.Ins.db.SaveChanges();

                List.Remove(SelectedItem);
                List.Add(NhaCC);
                SelectedItem = NhaCC;
            });
            DeleteCommand=new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName) || SelectedItem == null)
                    return false;
                return true;

            }, (p) =>
            {
                String s = MessageBox.Show("Thông Báo!!!", "bạn thật sự muốn xóa!!!", MessageBoxButton.YesNo, MessageBoxImage.Stop).ToString();
                if (s == "Yes")
                {
                    var NhaCC = DataProvider.Ins.db.NhaCungCaps.Where(x => x.MaNCC == SelectedItem.MaNCC).SingleOrDefault();
                    NhaCC.isDeleted = true;
                    DataProvider.Ins.db.SaveChanges();

                    List.Remove(SelectedItem);
                }
                return;
            });
        }
    }
}
