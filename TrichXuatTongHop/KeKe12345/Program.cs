using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace KeKe12345
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            //DownLoadAnh();
            LayLinkTatCa();
            Console.ReadLine();
        }

        public static void TestDownLoad()
        {
            var wc = new WebClient { Encoding = Encoding.UTF8 };
            string result = wc.DownloadString("http://www.keke12345.net/");
            Console.WriteLine(result);
        }

        /*Lấy số trang trong mỗi page*/
        public static int[] LaySoTrang()
        {
            string[] lstTheLoai = { "http://www.keke12345.net/gaoqing/", "http://www.keke12345.net/gaoqing/cn/"
                                      , "http://www.keke12345.net/gaoqing/rihan/" };
            int[] lstSoTrang = new int[3];
            for (int i = 0; i < lstTheLoai.Length; i++)
            {
                var htmlDocument = LoadWeb(lstTheLoai[i]);
                var node = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='pages']/a[last()-2]").InnerText;
                lstSoTrang[i] = Convert.ToInt32(node);
            }
            return lstSoTrang;
        }

        /*Lấy ra danh mục*/
        public static List<DanhMuc> LstDanhMuc()
        {
            var listTrang = LaySoTrang();
            var lstDanhMuc = new List<DanhMuc>
            {    
                new DanhMuc{TheLoai="http://www.keke12345.net/gaoqing/"+"list_5_",SoTrang=listTrang[0]},
                new DanhMuc{TheLoai="http://www.keke12345.net/gaoqing/cn/"+"list_",SoTrang=listTrang[1]},
                new DanhMuc{TheLoai="http://www.keke12345.net/gaoqing/rihan/"+"list_",SoTrang=listTrang[2]}
            };
            return lstDanhMuc;
        }

        public static HtmlDocument LoadWeb1(string url)
        {
            var htmlDocument = new HtmlWeb
            {
                UserAgent = @"Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.132 Safari/537.36"
            }.Load(url);

            return htmlDocument;
        }

        public static HtmlDocument LoadWeb(string url)
        {
            var htmlDocument = new HtmlDocument();
            var hw = new HtmlWeb();
            htmlDocument = hw.Load(url);
            hw.OverrideEncoding = Encoding.GetEncoding("gb2312");
            return htmlDocument;
        }

        /*Hàm sử dụng để lấy số trang ảnh của 1 Girl*/
        public static int SoAnhGirl(string url)
        {
            try
            {
                var htmlDocument = LoadWeb(url);
                var node = htmlDocument.DocumentNode.SelectSingleNode(@"//div[@class='page']/a[last()-1]").InnerText;
                return Convert.ToInt32(node); //1-4-13 die
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /*Lấy tất cả nội dung của link ảnh trên trang web*/
        public static void LayLinkTatCa()
        {
            var lstDanhMuc = LstDanhMuc();
            var sbd = new StringBuilder();
            /*Duyet qua danh muc: o day la 3*/
            for (int i = 0; i < lstDanhMuc.Count; i++)
            {
                Console.WriteLine("Danh Mục Thứ {0}", i + 1);
                sbd.AppendFormat("Danh Mục Thứ {0} \r\n", i + 1);
                /*Duyet qua tung page cua trang, voi tong so page = lstDanhMuc[i].SoTrang*/
                for (int j = 4; j < lstDanhMuc[i].SoTrang; j++) //==========================================
                {
                    Console.WriteLine("  DM{0} - Trang Thứ {1}", i + 1, j + 1);
                    sbd.AppendFormat("  DM{0} - Trang Thứ {1} \r\n", i + 1, j + 1);
                    /*Tại đây tiếp tục lấy danh sách link chi tiết ảnh của các Girl -cấu trúc: http://www.keke12345.net/gaoqing/list_5_1.html */
                    var htmlDocument1 = LoadWeb(lstDanhMuc[i].TheLoai + (j + 1) + ".html");
                    var nodeCll = htmlDocument1.DocumentNode.SelectNodes(@"//div[@class='t']/a");
                    /*Khi có ảnh chi tiết girl dùng vòng for để lấy tổng số page lưu ảnh của girl*/
                    for (int h = 0; h < nodeCll.Count; h++)  //==========================================
                    {
                        Console.WriteLine("    - Link Photo Girl Thứ {0}", h + 1);
                        sbd.AppendFormat("    - Link Photo Girl Thứ {0} \r\n", h + 1);
                        /*Chỗ này vào được trang đầu tiên của ảnh Girl rồi này -- */
                        int soPageGirl;
                        try
                        {
                            soPageGirl = SoAnhGirl("http://www.keke12345.net" + nodeCll[h].Attributes["href"].Value);
                        }
                        catch
                        {
                            Console.WriteLine("Page không tồn tại --- 1 Ngoại lệ được phát sinh");
                            continue;
                        }
                        /*Sử dụng vòng for này để lấy hết từng link ảnh của girl tại từng page 1*/
                        for (int k = 0; k < soPageGirl; k++)
                        {
                            Console.WriteLine("     Ảnh của Girl tại trang thứ {0}", k + 1);
                            sbd.AppendFormat("     Ảnh của Girl tại trang thứ {0} \r\n", k + 1);
                            HtmlDocument htmlDocument2;
                            if (k.Equals(0))
                            {
                                /*Lấy dữ liệu page đầu tiên của girl*/
                                htmlDocument2 = LoadWeb("http://www.keke12345.net"
                                                   + nodeCll[h].Attributes["href"].Value.Split('.')[0] + ".html");
                            }
                            else
                            {
                                /*Lấy dữ liệu tiếp theo page của Girl*/
                                htmlDocument2 = LoadWeb("http://www.keke12345.net"
                                                   + nodeCll[h].Attributes["href"].Value.Split('.')[0] + "_" + (k + 1) + ".html");
                            }
                            var nodeCll2 = htmlDocument2.DocumentNode.SelectNodes("//div[@class='page-list']/p/img");
                            for (int l = 0; l < nodeCll2.Count; l++)
                            {
                                Console.WriteLine("      Link: " + nodeCll2[l].Attributes["src"].Value);
                                sbd.AppendFormat("      Link: " + nodeCll2[l].Attributes["src"].Value + "\r\n");
                            }
                        }
                        Console.WriteLine("      ===============================Photo Girl ============================================");
                        sbd.AppendFormat("      ===============================Photo Girl ============================================ \r\n");
                        File.AppendAllText(@"D:\LinkKeke.txt", sbd.ToString(), Encoding.UTF8);
                        sbd.Clear();
                    }
                }
            }
        }

        /*Download Ảnh Riêng dựa trên link lấy*/
        public static void DownLoadAnh()
        {
            var lstDanhMuc = LstDanhMuc();
            TaoThuMuc(@"D:\ImageWebHeHe");
            /*Duyet qua danh muc: o day la 3*/
            for (int i = 0; i < lstDanhMuc.Count; i++)
            {
                Console.WriteLine("Danh Mục Thứ {0}", i + 1);
                TaoThuMuc(string.Format(@"D:\ImageWebHeHe\DanhMuc{0}", i + 1));
                /*Duyet qua tung page cua trang, voi tong so page = lstDanhMuc[i].SoTrang*/
                for (int j = 0; j < lstDanhMuc[i].SoTrang; j++)
                {
                    TaoThuMuc(string.Format(@"D:\ImageWebHeHe\DanhMuc{0}\TrangThu{1}", i + 1, j + 1));
                    Console.WriteLine("  DM{0} - Trang Thứ {1}", i + 1, j + 1);

                    /*Tại đây tiếp tục lấy danh sách link chi tiết ảnh của các Girl -cấu trúc: http://www.keke12345.net/gaoqing/list_5_1.html */
                    var htmlDocument1 = LoadWeb(lstDanhMuc[i].TheLoai + (j + 1) + ".html");
                    var nodeCll = htmlDocument1.DocumentNode.SelectNodes(@"//div[@class='t']/a");
                    /*Khi có ảnh chi tiết girl dùng vòng for để lấy tổng số page lưu ảnh của girl*/
                    for (int h = 0; h < nodeCll.Count; h++)
                    {
                        Console.WriteLine("    - Link Photo Girl Thứ {0}", h + 1);
                        TaoThuMuc(string.Format(@"D:\ImageWebHeHe\DanhMuc{0}\TrangThu{1}\PhotoGirl{2}", i + 1, j + 1, h + 1));
                        /*Chỗ này vào được trang đầu tiên của ảnh Girl rồi này -- */
                        int soPageGirl;
                        try
                        {
                            soPageGirl = SoAnhGirl("http://www.keke12345.net" + nodeCll[h].Attributes["href"].Value);
                        }
                        catch
                        {
                            Console.WriteLine("Page không tồn tại --- 1 Ngoại lệ được phát sinh");
                            continue;
                        }
                        /*Sử dụng vòng for này để lấy hết từng link ảnh của girl tại từng page 1*/

                        for (int k = 0; k < soPageGirl; k++)
                        {
                            Console.WriteLine("     Ảnh của Girl tại trang thứ {0}", k + 1);
                            TaoThuMuc(string.Format(@"D:\ImageWebHeHe\DanhMuc{0}\TrangThu{1}\PhotoGirl{2}\Page{3}"
                                , i + 1, j + 1, h + 1, k + 1));

                            HtmlDocument htmlDocument2;
                            if (k.Equals(0))
                            {
                                /*Lấy dữ liệu page đầu tiên của girl*/
                                htmlDocument2 = LoadWeb("http://www.keke12345.net"
                                                   + nodeCll[h].Attributes["href"].Value.Split('.')[0] + ".html");
                            }
                            else
                            {
                                /*Lấy dữ liệu tiếp theo page của Girl*/
                                htmlDocument2 = LoadWeb("http://www.keke12345.net"
                                                   + nodeCll[h].Attributes["href"].Value.Split('.')[0] + "_" + (k + 1) + ".html");
                            }
                            var nodeCll2 = htmlDocument2.DocumentNode.SelectNodes("//div[@class='page-list']/p/img");
                            for (int l = 0; l < nodeCll2.Count; l++)
                            {
                                Console.WriteLine("      Link: " + nodeCll2[l].Attributes["src"].Value);
                                DownLoadAnh(nodeCll2[l].Attributes["src"].Value,
                                string.Format(@"D:\ImageWebHeHe\DanhMuc{0}\TrangThu{1}\PhotoGirl{2}\Page{3}\PhotoGirl{4}.jpg"
                                , i + 1, j + 1, h + 1, k + 1, l + 1));
                            }
                        }
                        Console.WriteLine("      =================== Photo Girl {0} tai xuong thanh cong================================", h);
                        //File.AppendAllText(@"D:\LinkKeke.txt", "", Encoding.UTF8);
                    }
                }
            }
        }

        #region Phân tích trang web
        /* Trang web keke12345 chia ra làm 3 danh mục cần lấy ảnh chính
         *    1DM: Tại mỗi danh mục sẽ có tổng cộng N-Page
         *        1Page: Tại 1 trang lại có M- Ảnh của Girl
         *             1 Girl: Khi kích vào 1 girl riêng lại có L trang liên quan tới Girl này
         *                    Tại mỗi trang ta tiến hành bóc tách được ảnh của Girl
         *                    
         * ================= Triển khai bóc tách ===================================
         * For i=0; i< lstDanhMuc.count; i++
         * Begin
         *      For j=0; j< lstDanhMuc.SoTrang; j++
         *      Begin
         *          For k=0; k < lstAnhPage ;k++
         *          Begin
         *              Lấy được các link ảnh trong page này
         *          End
         *      End
         * End
         * 
         */
        #endregion

        public static void TaoThuMuc(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void DownLoadAnh(string url, string path)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var wc = new WebClient();
            //wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36");
            wc.DownloadFile(url, path);
        }
    }
}
