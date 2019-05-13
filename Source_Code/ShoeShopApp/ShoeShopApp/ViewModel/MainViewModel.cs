using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeShopApp.ViewModel
{
    public class MainViewModel: BaseViewModel
    {
        private string _textContent;
        public string textContent { get => _textContent; set { _textContent = value; OnPropertyChanged(); } }

        public MainViewModel()
        {
            textContent = "Hello World";
        }

    }
}
