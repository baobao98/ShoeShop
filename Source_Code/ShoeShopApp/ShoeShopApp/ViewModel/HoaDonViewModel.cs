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
    public class HoaDonViewModel: BaseViewModel
    {
        //Command 
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand OKCommand { get; set; }

        private ObservableCollection<HoaDon> _List;
        public ObservableCollection<HoaDon> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<KhachHang> _KhachHang;
        public ObservableCollection<KhachHang> KhachHang { get => _KhachHang; set { _KhachHang = value; OnPropertyChanged(); } }

        private ObservableCollection<NhanVien> _NhanVien;
        public ObservableCollection<NhanVien> NhanVien { get => _NhanVien; set { _NhanVien = value; OnPropertyChanged(); } }

        private ObservableCollection<SanPham> _SanPham;
        public ObservableCollection<SanPham> SanPham { get => _SanPham; set { _SanPham = value; OnPropertyChanged(); } }

        // Nhấn vào binding lên ô trên
        private HoaDon _SeletedItem;
        public HoaDon SeletedItem
        {
            get => _SeletedItem;

            set
            {
                _SeletedItem = value;
                OnPropertyChanged();
                if (_SeletedItem != null)
                {
                    SelectedKhachHang = SeletedItem.KhachHang;
                    SelectedNhanVien = SeletedItem.NhanVien;
                    NgayLapHD = SeletedItem.NgayLapHD;
                    ChiaTietHoaDon chiaTietHoaDon = new ChiaTietHoaDon();
                    chiaTietHoaDon = DataProvider.Ins.db.ChiaTietHoaDons.Where(ct => ct.MaHD == SeletedItem.MaHD).SingleOrDefault();
                    SoLuong = chiaTietHoaDon.SoLuong;
                    SelectedSanPham = DataProvider.Ins.db.SanPhams.Where(s => s.MaSP == chiaTietHoaDon.MaSP).SingleOrDefault();
                }
            }
        }
        // Nhấn vào binding lên ô trên combobox
        private KhachHang _SelectedKhachHang;
        public KhachHang SelectedKhachHang
        {
            get => _SelectedKhachHang;

            set
            {
                _SelectedKhachHang = value;
                OnPropertyChanged();
            }
        }
        // Nhấn vào binding lên ô trên combobox
        private NhanVien _SelectedNhanVien;
        public NhanVien SelectedNhanVien
        {
            get => _SelectedNhanVien;

            set
            {
                _SelectedNhanVien = value;
                OnPropertyChanged();
            }
        }
        // Nhấn vào binding lên ô trên combobox
        private SanPham _SelectedSanPham;
        public SanPham SelectedSanPham
        {
            get => _SelectedSanPham;

            set
            {
                _SelectedSanPham = value;
                OnPropertyChanged();
            }
        }


        // Binding textbox 
        //--Begin 

        private DateTime? _NgayLapHD;
        public DateTime? NgayLapHD { get => _NgayLapHD; set { _NgayLapHD = value; OnPropertyChanged(); } }

        private int? _SoLuong;
        public int? SoLuong { get => _SoLuong; set { _SoLuong = value; OnPropertyChanged(); } }

        //--end

        public HoaDonViewModel()
        {

            List = new ObservableCollection<HoaDon>(DataProvider.Ins.db.HoaDons.Where(h => h.isDeleted == false));
            //truyền danh sách khách hàng
            KhachHang = new ObservableCollection<KhachHang>(DataProvider.Ins.db.KhachHangs.Where(h => h.isDeleted == false));
            //truyền danh sách nhân viên
            NhanVien = new ObservableCollection<NhanVien>(DataProvider.Ins.db.NhanViens.Where(h => h.isDeleted == false));
            //truyền danh sách sản phẩm
            SanPham = new ObservableCollection<SanPham>(DataProvider.Ins.db.SanPhams.Where(h => h.isDeleted == false));



            OKCommand = new RelayCommand<object>(
               (p) =>
               {
                   if (SelectedKhachHang != null || SelectedKhachHang != null || NgayLapHD != null || SoLuong != null || SelectedSanPham != null)
                       return true;
                   return false;
               },
               p =>
               {
                   int MaNhanvienCurrent = state.idcur;
                   int Makh = SelectedKhachHang.MaKH;
                   HoaDon hoaDon = new HoaDon { MaKH = Makh, MaNV = MaNhanvienCurrent, NgayLapHD = NgayLapHD, isDeleted = false };
                   DataProvider.Ins.db.HoaDons.Add(hoaDon);
                   DataProvider.Ins.db.SaveChanges();// cập nhật trong database
                   List.Add(hoaDon);// cập nhật trên list
                   // add chi tiet hoa don
                   HoaDon hoaDonNew = new HoaDon();
                   hoaDonNew = DataProvider.Ins.db.HoaDons.Where(hd => hd.MaKH == Makh && hd.MaNV == MaNhanvienCurrent).SingleOrDefault();
                   ChiaTietHoaDon chiaTietHoaDon = new ChiaTietHoaDon { MaHD = hoaDonNew.MaHD, MaSP = SelectedSanPham.MaSP, SoLuong = SoLuong };
                   DataProvider.Ins.db.ChiaTietHoaDons.Add(chiaTietHoaDon);
                   DataProvider.Ins.db.SaveChanges();// cập nhật trong database
               }
               );

            AddCommand = new RelayCommand<object>(
              (p) =>
              {
                  if (SelectedKhachHang == null || SelectedKhachHang == null || NgayLapHD == null || SoLuong == null || SelectedSanPham == null)
                      return false;
                  return true;
              },
              (p) =>
              {
                  SelectedKhachHang = null;
                  SelectedNhanVien = null;
                  NgayLapHD = null;
                  SoLuong = null;
                  SelectedSanPham = null;
              }
              );


            //EditCommand = new RelayCommand<object>(
            //   (p) =>
            //   {
            //       return true;
            //   },
            //   (p) =>
            //   {
            //       SelectedKhachHang = SeletedItem.KhachHang;
            //       SelectedNhanVien = SeletedItem.NhanVien;
            //       NgayLapHD = SeletedItem.NgayLapHD;


            //   }
            //   );

            //OKCommand = new RelayCommand<object>(
            //   (p) =>
            //   {
            //       if (SeletedItem == null)
            //       {
            //           return false;
            //       }
            //       var displaylist = DataProvider.Ins.db.HoaDons.Where(u => u.MaHD == SeletedItem.MaHD);
            //       if (displaylist != null && displaylist.Count() != 0)
            //       {
            //           return true;
            //       }
            //       return false;
            //   },
            //   (p) =>
            //   {
            //       var hoaDon = DataProvider.Ins.db.HoaDons.Where(x => x.MaHD == SeletedItem.MaHD).SingleOrDefault();
            //       //Suplier.DisplayName = DisplayName;
            //       //Suplier.Phone = Phone;
            //       //Suplier.Address = Address;
            //       //Suplier.Email = Email;
            //       //Suplier.MoreInfo = MoreInfo;
            //       //Suplier.ContractDate = ContractDate          

            //       DataProvider.Ins.db.SaveChanges();

            //       //SeletedItem.DisplayName = DisplayName;
            //   }
            //   );

        }
    }
}
