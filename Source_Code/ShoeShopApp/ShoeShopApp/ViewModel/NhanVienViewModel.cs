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
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private bool _pEnable = false;
        public bool pEnable { get => _pEnable; set { _pEnable = value; OnPropertyChanged(); } }

        private ObservableCollection<NhanVien> _List;
        public ObservableCollection<NhanVien> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<LoaiNV> _LoaiNV;
        public ObservableCollection<LoaiNV> LoaiNV { get => _LoaiNV; set { _LoaiNV = value; OnPropertyChanged(); } }

        private NhanVien _SelectedItem;
        public NhanVien SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged();
                if (_SelectedItem != null) {
                    HoVaTen = SelectedItem.HoVaTen;
                    CMND = SelectedItem.CMND;
                    SDT = SelectedItem.SDT;
                    NgaySinh = SelectedItem.NgaySinh;
                    DiaChi = SelectedItem.DiaChi;
                    SelectedLoaiNV = SelectedItem.LoaiNV;
                    pEnable = false;
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

        private string _TenDN;
        public string TenDN { get => _TenDN; set { _TenDN = value; OnPropertyChanged(); } }

        private string _MatKhau;
        public string MatKhau { get => _MatKhau; set { _MatKhau = value; OnPropertyChanged(); } }

        //Nhấn vào binding lên ô trên combobox
        private LoaiNV _SelectedLoaiNV;
        public LoaiNV SelectedLoaiNV
        {
            get => _SelectedLoaiNV;

            set
            {
                _SelectedLoaiNV = value;
                OnPropertyChanged();
                if (_SelectedLoaiNV.TenLoaiNV == "Thu Ngân")
                {
                    pEnable = true;
                }
                else
                {
                    pEnable = false;
                }
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
                  var nv = new NhanVien()
                  {
                      HoVaTen = HoVaTen,
                      CMND = CMND,
                      DiaChi = DiaChi,
                      SDT = SDT,
                      NgaySinh = NgaySinh,
                      MaLoaiNV = SelectedLoaiNV.MaLoaiNV,
                      isDeleted = false
                  };

                  if (!string.IsNullOrEmpty(TenDN) && !string.IsNullOrEmpty(MatKhau))
                  {
                      var acc = new TaiKhoan()
                      {
                          TenDN = TenDN,
                          MatKhau = MatKhau,
                          isDeleted = false
                      };

                      DataProvider.Ins.db.TaiKhoans.Add(acc);
                  }
                  DataProvider.Ins.db.NhanViens.Add(nv);
                  DataProvider.Ins.db.SaveChanges();
                  List.Add(nv);
              }
              );

            EditCommand = new RelayCommand<object>(
              (p) =>
              {
                  if (string.IsNullOrEmpty(HoVaTen) || SelectedItem == null || SelectedLoaiNV == null)
                      return false;

                  var displaylist = DataProvider.Ins.db.NhanViens.Where(x => x.MaNV == SelectedItem.MaNV);

                  if (displaylist != null && displaylist.Count() != 0)
                  {
                      return true;
                  }
                  return false;
              },
              (p) =>
              {
                  var nv = DataProvider.Ins.db.NhanViens.Where(x => x.MaNV == SelectedItem.MaNV).SingleOrDefault();
                  nv.HoVaTen = HoVaTen;
                  nv.CMND = CMND;
                  nv.DiaChi = DiaChi;
                  nv.SDT = SDT;
                  nv.NgaySinh = NgaySinh;
                  nv.MaLoaiNV = SelectedLoaiNV.MaLoaiNV;

                  DataProvider.Ins.db.SaveChanges();

                  SelectedItem.HoVaTen = HoVaTen;
                  SelectedItem.CMND = CMND;
                  SelectedItem.DiaChi = DiaChi;
                  SelectedItem.SDT = SDT;
                  SelectedItem.NgaySinh = NgaySinh;
                  SelectedItem.LoaiNV = SelectedLoaiNV;
              }
              );

            DeleteCommand = new RelayCommand<object>(
              (p) =>
              {
                  if (SelectedItem == null)
                      return false;

                  return true;
              },
              (p) =>
              {
                  var nv = DataProvider.Ins.db.NhanViens.Where(x => x.MaNV == SelectedItem.MaNV).SingleOrDefault();
                  nv.isDeleted = true;

                  DataProvider.Ins.db.SaveChanges();
                  List.Remove(nv);
              }
              );
        }
    }
}
