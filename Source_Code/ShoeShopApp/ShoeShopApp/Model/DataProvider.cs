using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoeShopApp.Model;

namespace ShoeShopApp.Model
{
    public class DataProvider
    {
        private static DataProvider _ins;

        public static DataProvider Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new DataProvider();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        public ShoeShopEntities db { get; set; }
        public object DB { get; internal set; }

        private DataProvider()
        {
            db = new ShoeShopEntities();
        }
    }
}
