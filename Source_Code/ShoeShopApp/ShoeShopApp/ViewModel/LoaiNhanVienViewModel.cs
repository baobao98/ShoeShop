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
    public class LoaiNhanVienViewModel:BaseViewModel
    {
        //Command 
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        private ObservableCollection<LoaiNV> _List;
        public ObservableCollection<LoaiNV> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        // Nhấn vào binding lên ô trên
        private LoaiNV _SeletedItem;
        public LoaiNV SeletedItem
        {
            get => _SeletedItem;

            set
            {
                _SeletedItem = value;
                OnPropertyChanged();
                if (_SeletedItem != null)
                {
                    TenLoaiNV = SeletedItem.TenLoaiNV;
                    TienThuong = SeletedItem.TienThuong;
                }
            }
        }


        // Binding textbox 
        //--Begin 
        private string _TenLoaiNV;
        public string TenLoaiNV { get => _TenLoaiNV; set { _TenLoaiNV = value; OnPropertyChanged(); } }

        private decimal? _TienThuong;
        public decimal? TienThuong { get => _TienThuong; set { _TienThuong = value; OnPropertyChanged(); } }

     

        //--end

        public LoaiNhanVienViewModel()
        {
            List = new ObservableCollection<LoaiNV>(DataProvider.Ins.db.LoaiNVs.Where(k => k.isDeleted == false));

            AddCommand = new RelayCommand<object>(
               (p) =>
               {
                   if (TenLoaiNV == null)
                       return false;
                   return true;
               },
               p =>
               {
                   LoaiNV loaiNV = new LoaiNV { TenLoaiNV = TenLoaiNV, TienThuong = TienThuong };

                   DataProvider.Ins.db.LoaiNVs.Add(loaiNV);
                   DataProvider.Ins.db.SaveChanges();// cập nhật trong database

                   List.Add(loaiNV);// cập nhật trên list
               }
               );
            EditCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SeletedItem == null)
                    {
                        return false;
                    }
                    var displaylist = DataProvider.Ins.db.LoaiNVs.Where(k => k.MaLoaiNV == SeletedItem.MaLoaiNV);
                    if (displaylist != null && displaylist.Count() != 0)
                    {
                        return true;
                    }
                    return false;
                },
                (p) =>
                {
                    var loaiNV = DataProvider.Ins.db.LoaiNVs.Where(x => x.MaLoaiNV == SeletedItem.MaLoaiNV).SingleOrDefault();
                    loaiNV.TenLoaiNV = TenLoaiNV;
                    loaiNV.TienThuong = TienThuong;

                    DataProvider.Ins.db.SaveChanges();

                    SeletedItem.TenLoaiNV = TenLoaiNV;
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
                    var loaiNV = DataProvider.Ins.db.LoaiNVs.Where(x => x.MaLoaiNV == SeletedItem.MaLoaiNV).SingleOrDefault();
                    loaiNV.isDeleted = true;
                    DataProvider.Ins.db.SaveChanges();
                    List.Remove(loaiNV);// cập nhật trên list
                }
                );
        }
    }
}
