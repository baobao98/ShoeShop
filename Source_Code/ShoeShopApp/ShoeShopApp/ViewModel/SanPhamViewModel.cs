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
    public class SanPhamViewModel:BaseViewModel
    {
        private ObservableCollection<SanPham> _List;
        public ObservableCollection<SanPham> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<LoaiSP> _LoaiSP;
        public ObservableCollection<LoaiSP> LoaiSP { get => _LoaiSP; set { _LoaiSP = value; OnPropertyChanged(); } }

        private SanPham _SelectedItem;
        public SanPham SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    TenSP = SelectedItem.TenSP;
                    //MaLoaiSP = SelectedItem.MaLoaiSP;
                    Gia = SelectedItem.Gia;
                    Mau = SelectedItem.Mau;
                    SoLuong = SelectedItem.SoLuong;
                    Anh = SelectedItem.Anh;
                    SelectedLoaiSP = SelectedItem.LoaiSP;
                }
            }
        }

        private LoaiSP _SelectedLoaiSP;
        public LoaiSP SelectedLoaiSP
        {
            get => _SelectedLoaiSP;
            set
            {
                _SelectedLoaiSP = value;
                OnPropertyChanged();
            }
        }

        private string _TenSP;
        public string TenSP { get => _TenSP; set { _TenSP = value; OnPropertyChanged(); } }
        
        private decimal _Gia;
        public decimal Gia { get => _Gia; set { _Gia = value; OnPropertyChanged(); } }

        private string _Mau;
        public string Mau { get => _Mau; set { _Mau = value; OnPropertyChanged(); } }

        private int? _SoLuong;
        public int? SoLuong { get => _SoLuong; set { _SoLuong = value; OnPropertyChanged(); } }
        
        private string _Anh;
        public string Anh { get => _Anh; set { _Anh = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public SanPhamViewModel()
        {
            List = new ObservableCollection<SanPham>(DataProvider.Ins.db.SanPhams.Where(sp=>sp.isDeleted==false));
            LoaiSP = new ObservableCollection<LoaiSP>(DataProvider.Ins.db.LoaiSPs);

            AddCommand = new RelayCommand<object>((p) =>
            {
                //if (string.IsNullOrEmpty(TenSP))
                //    return false;
                if (TenSP != null && Mau!= null && SoLuong!=null)
                    return true;
                return false;

            }, (p) =>
            {
                var Sanpham = new SanPham() { TenSP = TenSP, MaLoaiSP = SelectedLoaiSP.MaLoaiSP, Gia = Gia, Mau = Mau, SoLuong = SoLuong};

                DataProvider.Ins.db.SanPhams.Add(Sanpham);
                DataProvider.Ins.db.SaveChanges();

                List.Add(Sanpham);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;

                var displayList = DataProvider.Ins.db.SanPhams.Where(x => x.MaSP == SelectedItem.MaSP);
                if (displayList != null && displayList.Count() != 0)
                    return true;

                return false;

            }, (p) =>
            {
                var SanPham = DataProvider.Ins.db.SanPhams.Where(x => x.MaSP == SelectedItem.MaSP).SingleOrDefault();
                SanPham.TenSP = TenSP;
                //#SanPham.MaLoaiSP = MaLoaiSP;
                SanPham.MaLoaiSP = SelectedLoaiSP.MaLoaiSP;
                SanPham.Gia = Gia;
                SanPham.Mau = Mau;
                SanPham.SoLuong = SoLuong;
                DataProvider.Ins.db.SaveChanges();

                SelectedItem.TenSP = TenSP;
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;

                var displayList = DataProvider.Ins.db.SanPhams.Where(x => x.MaSP == SelectedItem.MaSP);
                if (displayList != null && displayList.Count() != 0)
                    return true;

                return false;
            }, (p) =>
            {
                var SanPham = DataProvider.Ins.db.SanPhams.Where(x => x.MaSP == SelectedItem.MaSP).SingleOrDefault();
                SanPham.isDeleted = true;
                DataProvider.Ins.db.SaveChanges();


                List.Remove(SelectedItem);
                //SelectedItem.TenSP = TenSP;
            });

        }
    }
}
