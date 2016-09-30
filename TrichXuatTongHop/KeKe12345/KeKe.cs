using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeKe12345
{
    public class KeKe
    {
        public List<KeKe.Categories> cateries;
        public KeKe()
        {
            cateries = new List<Categories>();
        }

        public class Categories
        {
            public string TitleCate { get; set; }
            public List<Girl> girl;
            public Categories()
            {
                girl = new List<Girl>();
            }

            public class Girl
            {
                public string Name { get; set; }
                public int MyProperty { get; set; }
                public Girl()
                {
                    //link = new List<string>();
                }

               
            }
        }
    }
}
