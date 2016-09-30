using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace TestDownloadImage
{
    //1.1.13
    //2.1.11
    //3.1.10
    //4.1.11
    class Program
    {
        static void Main(string[] args)
        {
            //SoAnhGirl("http://www.keke12345.net/gaoqing/cn/Ugirls/2015/0908/11039.html");
            DownLoad();
        }


        public static void TaoFile(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static int SoAnhGirl(string url)
        {
            var htmlDocument = LoadWeb(url);
            var node = htmlDocument.DocumentNode.SelectSingleNode(@"//div[@class='page']/a[last()-1]").InnerText;
            return Convert.ToInt32(node);
        }




        public static void DownloadTemp()
        {
            var htmlDocument = LoadWeb("http://www.keke12345.net/gaoqing/cn/Ugirls/2015/0915/11076.html");
            //var node = htmlDocument.DocumentNode.SelectNodes("//div[@class='t']/a");
            var node = htmlDocument.DocumentNode.SelectNodes("//div[@class='page-list']/p/img");
            Console.WriteLine();

        }

        public static void DownLoad()
        {
            Console.OutputEncoding = Encoding.UTF8;
            var wc = new WebClient();
            //wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36");
            var test = wc.DownloadString("http://www.xpmeinv.com/uploads/allimg/c140809/140K3232611A0-IL29.jpg");
            wc.DownloadFile("http://www.xpmeinv.com/uploads/allimg/c140809/140K3232611A0-IL29.jpg", @"D:\a.jpg");
            Console.WriteLine("Success");
            Console.ReadLine();
        }

        private static HtmlDocument LoadWeb(string link)
        {
            var htmlDocument = new HtmlWeb
            {
                UserAgent =
                    "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.132 Safari/537.36"
            }.Load(link);
            return htmlDocument;
        }

        /*Lấy chi tiết ảnh bao gồm lấy tiêu đề và tổng số ảnh*/
        public static string[] LayChiTietAnh(string link)
        {
            var htmlDocument = LoadWeb(link);
            var strTitle = htmlDocument.DocumentNode.SelectSingleNode("//h1").InnerText;
            var strSoAnh = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='page']/li[last()-1]/a").InnerText;
            string[] arr = { strTitle, strSoAnh };
            File.WriteAllText(@"D:\test.txt", strTitle + "   " + strSoAnh);
            return arr;
        }
    }
}
