using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoeShopApp.Model;

namespace ShoeShopApp.ViewModel
{
    public class MainViewModel: BaseViewModel
    {
        private string _textContent;
        public string textContent { get => _textContent; set { _textContent = value; OnPropertyChanged(); } }

        public MainViewModel()
        {
            List<NhaCungCap> nhaCungCaps = new List<NhaCungCap>();
            nhaCungCaps = DataProvider.Ins.db.NhaCungCaps.ToList();
            textContent ="Hello World!!! This text is loaded from database (table NhaCungCap): <"+nhaCungCaps[0].TenNCC+"> " +
                "Test connect successfully";
        }

    }
}
