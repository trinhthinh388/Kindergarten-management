using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KindergatenManagement.Model
{
    class DataProvider
    {
        private static DataProvider _ins;
        public static DataProvider Ins
        {
            get
            {
                if (_ins == null)
                {
                    return _ins = new DataProvider();
                }
                return _ins;
            }

            set
            {
                _ins = value;
            }
        }

        public KindergartenManagementEntities db { get; set; }

        private DataProvider()
        {
            db = new KindergartenManagementEntities();
        }
    }
}
