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
    public class LoaiSanPhamViewModel:BaseViewModel
    {
        //Command 
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        private ObservableCollection<LoaiSP> _List;
        public ObservableCollection<LoaiSP> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<NhaCungCap> _NhaCungCap;
        public ObservableCollection<NhaCungCap> NhaCungCap { get => _NhaCungCap; set { _NhaCungCap = value; OnPropertyChanged(); } }

        // Nhấn vào binding lên ô trên
        private LoaiSP _SeletedItem;
        public LoaiSP SeletedItem
        {
            get => _SeletedItem;

            set
            {
                _SeletedItem = value;
                OnPropertyChanged();
                if (_SeletedItem != null)
                {
                    TenLoaiSP = SeletedItem.TenLoaiSP;
                    SelectedNhaCungCap = SeletedItem.NhaCungCap;
                    NoiSanXuat = SeletedItem.NoiSanXuat;
                }
            }
        }

        // Nhấn vào binding lên ô trên combobox
        private NhaCungCap _SelectedNhaCungCap;
        public NhaCungCap SelectedNhaCungCap
        {
            get => _SelectedNhaCungCap;

            set
            {
                _SelectedNhaCungCap = value;
                OnPropertyChanged();
            }
        }


        // Binding textbox 
        //--Begin 
        private string _TenLoaiSP;
        public string TenLoaiSP { get => _TenLoaiSP; set { _TenLoaiSP = value; OnPropertyChanged(); } }

        private string _NoiSanXuat;
        public string NoiSanXuat { get => _NoiSanXuat; set { _NoiSanXuat = value; OnPropertyChanged(); } }
        //--end

        public LoaiSanPhamViewModel()
        {
            List = new ObservableCollection<LoaiSP>(DataProvider.Ins.db.LoaiSPs.Where(sp => sp.isDeleted == false));
            NhaCungCap= new ObservableCollection<NhaCungCap>(DataProvider.Ins.db.NhaCungCaps);

            AddCommand = new RelayCommand<object>(
               (p) =>
               {
                   if (SelectedNhaCungCap == null || TenLoaiSP == null || NoiSanXuat == null)
                       return false;
                   return true;
               },
               p =>
               {
                     LoaiSP loaiSP = new LoaiSP { TenLoaiSP = TenLoaiSP, NoiSanXuat = NoiSanXuat, MaNCC = SelectedNhaCungCap.MaNCC};

                   DataProvider.Ins.db.LoaiSPs.Add(loaiSP);
                   DataProvider.Ins.db.SaveChanges();// cập nhật trong database

                    List.Add(loaiSP);// cập nhật trên list
                }
               );
            EditCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SeletedItem == null || SelectedNhaCungCap == null)
                    {
                        return false;
                    }
                    var displaylist = DataProvider.Ins.db.LoaiSPs.Where(u => u.MaLoaiSP == SeletedItem.MaLoaiSP);
                    if (displaylist != null && displaylist.Count() != 0)
                    {
                        return true;
                    }
                    return false;
                },
                (p) =>
                {
                    LoaiSP loaiSP = DataProvider.Ins.db.LoaiSPs.Where(x => x.MaLoaiSP == SeletedItem.MaLoaiSP).SingleOrDefault();
                    loaiSP.TenLoaiSP = TenLoaiSP;
                    loaiSP.MaNCC = SelectedNhaCungCap.MaNCC;
                    loaiSP.NoiSanXuat = NoiSanXuat;

                    DataProvider.Ins.db.SaveChanges();

                    SeletedItem.TenLoaiSP = TenLoaiSP;
                }
                );
            DeleteCommand = new RelayCommand<object>(
                (p) =>
                {
                    if(SeletedItem==null)
                    {
                        return false;
                    }
                    return true;
                },
                (p) =>
                {
                    LoaiSP loaiSP = DataProvider.Ins.db.LoaiSPs.Where(x => x.MaLoaiSP == SeletedItem.MaLoaiSP).SingleOrDefault();
                    loaiSP.isDeleted = true;
                    DataProvider.Ins.db.SaveChanges();
                    List.Remove(loaiSP);// cập nhật trên list
                }
                );
        }
    }
}
