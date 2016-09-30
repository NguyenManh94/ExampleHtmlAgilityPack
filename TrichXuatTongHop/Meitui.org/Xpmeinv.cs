using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meitui.org
{
    public class Root
    {
        public List<Xpmeinv> Cate { get; set; }
    }

    public class Xpmeinv
    {
        public string LinkTitle { get; set; }
        public List<ImageGirl> ImageInfor { get; set; }
    }

    public class ImageGirl
    {
        public string Title { get; set; }
        public int N_Img { get; set; }
        public List<string> LstImg { get; set; }
    }
}
