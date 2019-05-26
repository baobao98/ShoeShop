using ShoeShopApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShoeShopApp.ViewModel
{
    public class NhanVienViewModel : BaseViewModel
    {
        //Command 
        public ICommand AddCommand { get; set; }
        //public ICommand EditCommand { get; set; }
        //public ICommand DeleteCommand { get; set; }
        //public ICommand OKCommand { get; set; }

        private ObservableCollection<NhanVien> _List;
        public ObservableCollection<NhanVien> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<LoaiNV> _LoaiNV;
        public ObservableCollection<LoaiNV> LoaiNV { get => _LoaiNV; set { _LoaiNV = value; OnPropertyChanged(); } }

        private NhanVien _SeletedItem;
        public NhanVien SeletedItem { get => _SeletedItem; set { _SeletedItem = value; OnPropertyChanged();
                if (SeletedItem != null) {
                    HoVaTen = SeletedItem.HoVaTen;
                    CMND = SeletedItem.CMND;
                    SDT = SeletedItem.SDT;
                    NgaySinh = SeletedItem.NgaySinh;
                    DiaChi = SeletedItem.DiaChi;
                } } }

        private string _HoVaTen;
        public string HoVaTen { get => _HoVaTen; set { _HoVaTen = value; OnPropertyChanged(); } }

        private string _CMND;
        public string CMND { get => _CMND; set { _CMND = value; OnPropertyChanged(); } }

        private string _SDT;
        public string SDT { get => _SDT; set { _SDT = value; OnPropertyChanged(); } }

        private DateTime? _NgaySinh;
        public DateTime? NgaySinh { get => _NgaySinh; set { _NgaySinh = value; OnPropertyChanged(); } }

        private string _DiaChi;
        public string DiaChi { get => _DiaChi; set { _DiaChi = value; OnPropertyChanged(); } }

        //Nhấn vào binding lên ô trên combobox
        private LoaiNV _SelectedLoaiNV;
        public LoaiNV SelectedLoaiNV
        {
            get => _SelectedLoaiNV;

            set
            {
                _SelectedLoaiNV = value;
                OnPropertyChanged();
            }
        }

        public NhanVienViewModel()
        {

            List = new ObservableCollection<NhanVien>(DataProvider.Ins.db.NhanViens.Where(h => h.isDeleted == false));
            LoaiNV = new ObservableCollection<LoaiNV>(DataProvider.Ins.db.LoaiNVs.Where(h => h.isDeleted == false));

            AddCommand = new RelayCommand<object>(
              (p) =>
              {
                  if (string.IsNullOrEmpty(HoVaTen))
                      return false;

                  if (SelectedLoaiNV == null )
                      return false;
                  
                  return true;
              },
              (p) =>
              {
                  DataProvider.Ins.db.NhanViens.Add(new NhanVien()
                  {
                      HoVaTen = HoVaTen,
                      CMND = CMND,
                      DiaChi = DiaChi,
                      SDT = SDT,
                      NgaySinh = NgaySinh,
                      isDeleted = false
                  });

              }
              );

            //OKCommand = new RelayCommand<object>(
            //   (p) =>
            //   {
            //       if (SelectedLoaiNV != null)
            //           return true;
            //       return false;
            //   },
            //   p =>
            //   {
            //       int MaNhanvienCurrent = 1;
            //       int Maloainv = SelectedLoaiNV.MaLoaiNV;
            //       NhanVien nv = new NhanVien { MaNV = MaNhanvienCurrent, NgaySinh = NgaySinh, isDeleted = false };
            //       DataProvider.Ins.db.NhanViens.Add(nv);
            //       DataProvider.Ins.db.SaveChanges();// cập nhật trong database
            //       List.Add(nv);// cập nhật trên list
            //       // add account
            //       //HoaDon hoaDonNew = new HoaDon();
            //       //hoaDonNew = DataProvider.Ins.db.HoaDons.Where(hd => hd.MaKH == Makh && hd.MaNV == MaNhanvienCurrent).SingleOrDefault();
            //       //ChiaTietHoaDon chiaTietHoaDon = new ChiaTietHoaDon { MaHD = hoaDonNew.MaHD, MaSP = SelectedSanPham.MaSP, SoLuong = SoLuong };
            //       //DataProvider.Ins.db.ChiaTietHoaDons.Add(chiaTietHoaDon);
            //       //DataProvider.Ins.db.SaveChanges();// cập nhật trong database
            //   }
            //   );
        }
    }
}
