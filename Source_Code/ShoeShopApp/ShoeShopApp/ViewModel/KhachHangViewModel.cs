using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ShoeShopApp.Model;

namespace ShoeShopApp.ViewModel
{
    public class KhachHangViewModel: BaseViewModel
    {
        //Command 
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        private ObservableCollection<KhachHang> _List;
        public ObservableCollection<KhachHang> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        // Nhấn vào binding lên ô trên
        private KhachHang _SeletedItem;
        public KhachHang SeletedItem
        {
            get => _SeletedItem;

            set
            {
                _SeletedItem = value;
                OnPropertyChanged();
                if (_SeletedItem != null)
                {
                    TenKH = SeletedItem.TenKH;
                    SDT = SeletedItem.SDT;
                    DiaChi = SeletedItem.DiaChi;
                }
            }
        }

       
        // Binding textbox 
        //--Begin 
        private string _TenKH;
        public string TenKH { get => _TenKH; set { _TenKH = value; OnPropertyChanged(); } }

        private string _SDT;
        public string SDT { get => _SDT; set { _SDT = value; OnPropertyChanged(); } }

        private string _DiaChi;
        public string DiaChi { get => _DiaChi; set { _DiaChi = value; OnPropertyChanged(); } }

        //--end

        public KhachHangViewModel()
        {
            List = new ObservableCollection<KhachHang>(DataProvider.Ins.db.KhachHangs.Where(k => k.isDeleted == false));

            AddCommand = new RelayCommand<object>(
               (p) =>
               {
                   if (TenKH == null || SDT == null || DiaChi == null)
                       return false;
                   return true;
               },
               p =>
               {
                   KhachHang khachHang = new KhachHang { TenKH = TenKH, SDT = SDT, DiaChi = DiaChi };

                   DataProvider.Ins.db.KhachHangs.Add(khachHang);
                   DataProvider.Ins.db.SaveChanges();// cập nhật trong database

                   List.Add(khachHang);// cập nhật trên list
               }
               );
            EditCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SeletedItem == null)
                    {
                        return false;
                    }
                    var displaylist = DataProvider.Ins.db.KhachHangs.Where(k => k.MaKH == SeletedItem.MaKH);
                    if (displaylist != null && displaylist.Count() != 0)
                    {
                        return true;
                    }
                    return false;
                },
                (p) =>
                {
                    var khachHang = DataProvider.Ins.db.KhachHangs.Where(x => x.MaKH == SeletedItem.MaKH).SingleOrDefault();
                    khachHang.TenKH = TenKH;
                    khachHang.SDT = SDT;
                    khachHang.DiaChi = DiaChi;

                    DataProvider.Ins.db.SaveChanges();

                    SeletedItem.TenKH = TenKH;
                }
                );
            DeleteCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SeletedItem == null)
                    {
                        return false;
                    }
                    return true;
                },
                (p) =>
                {
                    KhachHang khachHang = DataProvider.Ins.db.KhachHangs.Where(x => x.MaKH == SeletedItem.MaKH).SingleOrDefault();
                    khachHang.isDeleted = true;
                    DataProvider.Ins.db.SaveChanges();
                    List.Remove(khachHang);// cập nhật trên list
                }
                );
        }
    }
}
