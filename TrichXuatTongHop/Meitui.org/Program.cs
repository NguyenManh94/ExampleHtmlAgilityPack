using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Meitui.org
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            LayLinkPageAnh();
            Console.ReadLine();
        }

        /*Link Home Page*/
        public static List<String> ListTrang(string xpath)
        {
            var link = "http://www.xpmeinv.com/";
            //htmlDocument.Encoding = Encoding.GetEncoding("gd2312");
            var htmlDocument = LoadWeb(link);

            var htmlNodeCll = htmlDocument.DocumentNode.SelectNodes(xpath);
            var sbd = new StringBuilder();
            var lstCategorie = new List<string>();
            foreach (var node in htmlNodeCll)
            {
                //sbd.Append(node.Attributes[0].Value + "\n");
                lstCategorie.Add(node.Attributes[0].Value);
            }
            //return sbd.ToString();
            return lstCategorie;
        }

        /*Download So trang va add vao List*/
        public static List<int> ListSoTrang()
        {
            var lstTrang = ListTrang("//div[@class='left']/div[@class='box']/div[@class='tit']/a");
            var lstSo = new List<int>();
            for (int i = 0; i < lstTrang.Count; i++)
            {
                lstSo.Add(Convert.ToInt32(LayTongSoPage("http://www.xpmeinv.com" + lstTrang[i])));
            }
            return lstSo;
        }

        public static string LayTongSoPage(string link)
        {
            //http://www.xpmeinv.com/html/riben/list_1_1.html
            var htmlDocument = LoadWeb(link);
            return htmlDocument.DocumentNode.SelectSingleNode("//div[@class='page']/li/span/strong[1]").InnerText;
        }

        /*Tạo danh sách Categories với 2 thuộc tính lưu trữ là Categories và số trang của Categories*/
        public static List<Categories> ListCategories()
        {
            var lstTrang = ListTrang("//div[@class='left']/div[@class='box']/div[@class='tit']/a");
            var lstCategories = new List<Categories>();
            var lstSoTrang = ListSoTrang();
            for (int i = 0; i < lstSoTrang.Count; i++)
            {
                lstCategories.Add(new Categories { LinkCategories = lstTrang[i], NumberPage = lstSoTrang[i] });
            }
            return lstCategories;
        }


        #region TestDownload Zo
        /*Có danh sách Page và tổng chi tiết của Page rồi giờ ta tiến hành lấy hết 1 loạt các link của ảnh cá nhân
          Thao tác này chưa vào chi tiết của ảnh
         */

        //public static void LayLinkPageAnh()
        //{
        //    var lstCategories = ListCategories();
        //    var sbd = new StringBuilder();
        //    for (int i = 0; i < lstCategories.Count; i++)
        //    {
        //        //Console.WriteLine("Link thể loại: {0}", "http://www.xpmeinv.com" + lstCategories[i].LinkCategories);
        //        sbd.AppendFormat("Link thể loại: {0}\r\n", "http://www.xpmeinv.com" + lstCategories[i].LinkCategories);
        //        for (int j = 0; j < lstCategories[i].NumberPage; j++)
        //        {

        //            sbd.AppendFormat(
        //                BocLink("http://www.xpmeinv.com" + lstCategories[i].LinkCategories
        //                 + "list_" + (i + 1) + "_" + (j + 1) + ".html", i + 1, j + 1)
        //            );
        //        }
        //        sbd.AppendFormat("================================================================\r\n");
        //    }
        //    File.WriteAllText(@"D:\Link.txt", sbd.ToString(), Encoding.UTF8);
        //}

        //public static string BocLink(string link, int pageTong, int pageCon)
        //{
        //    //var link = "http://www.xpmeinv.com/";
        //    var htmlDocument = LoadWeb(link);
        //    //Do các link giống nhau ở xpath lên ta sẽ xử lý luôn 1 chuỗi xpath duy nhất
        //    var htmlNodeCll = htmlDocument.DocumentNode.SelectNodes("//div[@class='piclist']//div[@class='item']/div/a");

        //    //int j = 0;
        //    //foreach (var node in htmlNodeCll)
        //    //{

        //    //    Console.WriteLine(node.Attributes[0].Value + (j++));
        //    //    Debug.WriteLine(node.Attributes[0].Value);
        //    //}
        //    var sbd = new StringBuilder();
        //    int j = 1;
        //    int x = 1;
        //    for (int i = 0; i < htmlNodeCll.Count; i++)
        //    {
        //        if (i % 2 == 0)
        //        {
        //            //Console.WriteLine("\tMa Anh: {0}.{1}-{2}" + htmlNodeCll[i].Attributes[0].Value, pageTong, pageCon, j++);
        //            Console.WriteLine("\tMa Anh: {0}.{1}-{2}"
        //                        + htmlNodeCll[i].Attributes[0].Value + "\r\n"
        //                        , pageTong, pageCon, x++);
        //            sbd.AppendFormat("  Ma Anh: {0}.{1}-{2,-3}: "
        //                        + htmlNodeCll[i].Attributes[0].Value + "\r\n"
        //                        , pageTong, pageCon, j++);
        //        }
        //    }
        //    return sbd.ToString();
        //}
        #endregion

        /*Có danh sách Page và tổng chi tiết của Page rồi giờ ta tiến hành lấy hết 1 loạt các link của ảnh cá nhân
          Thao tác này chưa vào chi tiết của ảnh
         */

        /*Bao gồm cả download*/
        public static void LayLinkPageAnh()
        {
            TaoThuMuc(@"D:\ImageCrawler");
            string[] arrChiTietAnh;


            var lstCategories = ListCategories();
            var sbd = new StringBuilder();
            for (int i = 0; i < lstCategories.Count; i++)
            {
                TaoThuMuc(string.Format(@"D:\ImageCrawler\TapAnh{0}", i));

                Console.WriteLine("Link thể loại: {0}", "http://www.xpmeinv.com" + lstCategories[i].LinkCategories);
                sbd.AppendFormat("Link thể loại: {0}\r\n", "http://www.xpmeinv.com" + lstCategories[i].LinkCategories);
                for (int j = 0; j < lstCategories[i].NumberPage; j++)
                {
                    TaoThuMuc(string.Format(@"D:\ImageCrawler\TapAnh{0}\Page{1}", i, j));
                    var htmlDocument = LoadWeb(string.Format("http://www.xpmeinv.com" + lstCategories[i].LinkCategories
                         + "list_" + (i + 1) + "_" + (j + 1) + ".html"));

                    //Do các link giống nhau ở xpath lên ta sẽ xử lý luôn 1 chuỗi xpath duy nhất
                    /*Chuỗi lấy link từng người mẫu*/
                    var htmlNodeCll = htmlDocument.DocumentNode.SelectNodes("//div[@class='piclist']//div[@class='item']/div/a");
                    int x = 0;
                    int test = 0;
                    for (int k = 0; k < htmlNodeCll.Count; k++)
                    {
                        /*do ta đặt tạo thư mục ở đây lên biến x chưa bị tăng -- ta phải tự tăng nó trong đây */
                        TaoThuMuc(string.Format(@"D:\ImageCrawler\TapAnh{0}\Page{1}\Girl{2}", i, j, x + 1));
                        if (k % 2 == 0)
                        {
                            /*Đoạn này Lấy được link của từng người mẫu -- thường có khoảng 20 người trong 1 trang*/
                            test = test + 1; x = x + 1;
                            sbd.AppendFormat("  Ma Anh: {0}.{1}-{2}:  http://www.xpmeinv.com"
                                        + htmlNodeCll[k].Attributes[0].Value + "\r\n"
                                        , i + 1, j + 1, x);
                            Console.WriteLine("  Ma Anh: {0}.{1}-{2}:  http://www.xpmeinv.com"/*-----------------------------------------*/
                                        + htmlNodeCll[k].Attributes[0].Value + "\r\n"
                                        , i + 1, j + 1, test);

                            arrChiTietAnh = LayChiTietAnh("http://www.xpmeinv.com" + htmlNodeCll[k].Attributes[0].Value);
                            sbd.AppendFormat(@"    Tiêu đề ảnh: {0} - Tổng số ảnh: {1}\r\n", arrChiTietAnh[0], arrChiTietAnh[1]);
                            Console.WriteLine(@"    Tiêu đề ảnh: {0} - Tổng số ảnh: {1}", arrChiTietAnh[0], arrChiTietAnh[1]);/*------------------------------*/

                            for (int h = 0; h < Int32.Parse(arrChiTietAnh[1]) - 1; h++)
                            {
                                /*chuỗi lấy link ảnh chi tiết của người mẫu*/
                                var htmlDocument2 = LoadWeb("http://www.xpmeinv.com"
                                + htmlNodeCll[k].Attributes[0].Value.Split('.')[0] + "_" + (h + 2) + ".html");
                                string nodeAnh = "";
                                /*Tại page 13 tiến trình bị block do mã java Script ta phải lấy attribute thông qua Attributes[0]*/
                                try
                                {
                                    nodeAnh = htmlDocument2.DocumentNode
                                        .SelectSingleNode(@"//div[@class='picture']/dl[2]/a/img 
                                            | //div[@class='picture']/dl[2]/img
                                            | //div[@class='picture']/a/img
                                            | //div[@class='picture']/img
                                            | //div[@class='picture']/dl[1]/a/img
                                            | //div[@class='picture']/dl[3]/a/img")
                                        .Attributes["src"].Value;
                                }
                                catch
                                {
                                    sbd.AppendFormat("\t - Phần mục không có ảnh để hiển thị");
                                    Console.WriteLine("\t - Phần mục không có ảnh để hiển thị");
                                    continue;
                                }



                                /*Load du lieu anh cua trang chu*/
                                var htmlAnhBanDau = LoadWeb("http://www.xpmeinv.com" + htmlNodeCll[k].Attributes[0].Value);
                                var nodeChu = htmlDocument2.DocumentNode
                                        .SelectSingleNode(@"//div[@class='picture']/dl[2]/a/img 
                                            | //div[@class='picture']/dl[2]/img
                                            | //div[@class='picture']/a/img
                                            | //div[@class='picture']/img
                                            | //div[@class='picture']/dl[1]/a/img
                                            | //div[@class='picture']/dl[3]/a/img")
                                        .Attributes["src"].Value;

                                /*Nếu ảnh không có ngoại lệ thì download và ghi vào file Text*/
                                sbd.AppendFormat("\t - Link ảnh: {0}\r\n", "http://www.xpmeinv.com" + nodeAnh);
                                Console.WriteLine("\t - Link ảnh: {0}\r\n", "http://www.xpmeinv.com" + nodeAnh);
                                //Tải về ảnh đầu tiên mà ta bỏ qua lúc này... (lúc này ở trên ta chọn bắt đầu từ ảnh 2)
                                //DownLoadAnh("http://www.xpmeinv.com" + nodeChu
                                //    , string.Format(@"D:\ImageCrawler\TapAnh{0}\Page{1}\Girl{2}\Photo0.jpg"
                                //    , i, j, x));

                                DownLoadAnh("http://www.xpmeinv.com" + nodeAnh
                                    , string.Format(@"D:\ImageCrawler\TapAnh{0}\Page{1}\Girl{2}\Photo{3}.jpg"
                                    , i, j, x, h + 1));
                            }
                        }
                        Console.WriteLine("Girl -{0} Tải xuống Thành Công======================", x);
                    }
                    Console.WriteLine("Page{0} Tải xuống Thành Công ===================================", j);
                }
                sbd.AppendFormat("========================= Kết thuc thể loại {0} =======================\r\n\n"
                    , lstCategories[i].LinkCategories.Split('/')[2].ToString());
                Console.WriteLine("TapAnh-DanhMuc {0} Tải xuống Thành Công ========================================", i);
            }
            File.WriteAllText(@"D:\Link.bad", sbd.ToString(), Encoding.UTF8);
        }

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

        /*Thử nghiệm lại ko liên quan tới Download Ảnh*/
        public static void LayLinkPageAnh2()
        {
            string[] arrChiTietAnh;


            var lstCategories = ListCategories();
            var sbd = new StringBuilder();
            for (int i = 0; i < lstCategories.Count; i++)
            {
                Console.WriteLine("Link thể loại: {0}", "http://www.xpmeinv.com" + lstCategories[i].LinkCategories);
                sbd.AppendFormat("Link thể loại: {0}\r\n", "http://www.xpmeinv.com" + lstCategories[i].LinkCategories);
                for (int j = 0; j < lstCategories[i].NumberPage; j++)
                {
                    var htmlDocument = LoadWeb(string.Format("http://www.xpmeinv.com" + lstCategories[i].LinkCategories
                         + "list_" + (i + 1) + "_" + (j + 1) + ".html"));
                    var htmlNodeCll = htmlDocument.DocumentNode.SelectNodes("//div[@class='piclist']//div[@class='item']/div/a");
                    int x = 0;
                    int test = 0;
                    for (int k = 0; k < htmlNodeCll.Count; k++)
                    {
                        if (k % 2 == 0)
                        {
                            /*Đoạn này Lấy được link của từng người mẫu -- thường có khoảng 20 người trong 1 trang*/
                            test = test + 1; x = x + 1;
                            sbd.AppendFormat("  Ma Anh: {0}.{1}-{2}:  http://www.xpmeinv.com"
                                        + htmlNodeCll[k].Attributes[0].Value + "\r\n"
                                        , i + 1, j + 1, x);
                            Console.WriteLine("  Ma Anh: {0}.{1}-{2}:  http://www.xpmeinv.com"/*-----------------------------------------*/
                                        + htmlNodeCll[k].Attributes[0].Value + "\r\n"
                                        , i + 1, j + 1, test);

                            arrChiTietAnh = LayChiTietAnh("http://www.xpmeinv.com" + htmlNodeCll[k].Attributes[0].Value);
                            sbd.AppendFormat("    Tiêu đề ảnh: {0} - Tổng số ảnh: {1} " + "\r\n", arrChiTietAnh[0], arrChiTietAnh[1]);
                            Console.WriteLine("    Tiêu đề ảnh: {0} - Tổng số ảnh: {1}", arrChiTietAnh[0], arrChiTietAnh[1]);/*------------------------------*/

                            for (int h = 0; h < Int32.Parse(arrChiTietAnh[1]) - 1; h++)
                            {
                                var htmlDocument2 = LoadWeb("http://www.xpmeinv.com"
                                + htmlNodeCll[k].Attributes[0].Value.Split('.')[0] + "_" + (h + 2) + ".html");
                                string nodeAnh = "";
                                /*Tại page 13 tiến trình bị block do mã java Script ta phải lấy attribute thông qua Attributes[0]*/
                                try
                                {
                                    nodeAnh = htmlDocument2.DocumentNode
                                        .SelectSingleNode(@"//div[@class='picture']/dl[2]/a/img 
                                            | //div[@class='picture']/dl[2]/img
                                            | //div[@class='picture']/a/img
                                            | //div[@class='picture']/img
                                            | //div[@class='picture']/dl[1]/a/img
                                            | //div[@class='picture']/dl[3]/a/img")
                                        .Attributes["src"].Value;
                                }
                                catch
                                {
                                    sbd.AppendFormat("\t - Phần mục không có ảnh để hiển thị");
                                    Console.WriteLine("\t - Phần mục không có ảnh để hiển thị");
                                    continue;
                                }

                                sbd.AppendFormat("\t - Link ảnh: {0}\r\n", "http://www.xpmeinv.com" + nodeAnh);
                                Console.WriteLine("\t - Link ảnh: {0}\r\n", "http://www.xpmeinv.com" + nodeAnh);
                            }
                            File.AppendAllText(@"D:\Link.txt", sbd.ToString(), Encoding.UTF8);
                            sbd.Clear();
                        }
                    }
                    Console.WriteLine("Page{0} Đã Được Lấy ===================================", j + 1);
                    sbd.AppendFormat("    Danh Muc {0} - Page {1} Đã Được Lấy =========================== \r\n", i + 1, j + 1);
                    File.AppendAllText(@"D:\Link.txt", sbd.ToString(), Encoding.UTF8);
                    sbd.Clear();
                }
                Console.WriteLine("========================= Kết thuc thể loại {0} =======================\r\n\n"
                    , lstCategories[i].LinkCategories.Split('/')[2].ToString());
                File.AppendAllText(@"D:\Link.txt", string.Format("========================= Kết thuc thể loại {0} =======================\r\n\n"
                    , lstCategories[i].LinkCategories.Split('/')[2].ToString()), Encoding.UTF8);
                sbd.Clear();
            }
            //File.AppendAllText(@"D:\Link.bad", sbd.ToString(), Encoding.UTF8);
            Console.WriteLine("Ghi tập tin thành công");
        }

        /* Link gốc ban đầu: http://www.xpmeinv.com/
         * Lấy số được page với số trang
         * ==> Từ đây lấy tiếp các LinkCategory ví dụ
         *  1 /html/riben/       4   http://www.xpmeinv.com/html/riben/list_1_1.html từ 1.1 --> 1.5
         *  2 /html/xinggan/     5   
         *  3 /html/qingchun/    5
         *  4 /html/mote/        5
         *  5 /html/siwa/        5
         *  ========================================================================================
         *  Chuối cộng cuối cùng để bóc được link chi tiết gái: http://www.xpmeinv.com + LinkCategory + "list_" + i + "_" + j + ".html"
         *   
         *   Nhiệm vụ A giờ: Lấy hết sạch link tại 5 chuyên mục này
         *   Nhiệm vụ B tiếp theo lấy link của từng cái ảnh gái chi tiết
         *   
         * Triển khai nhiệm vụ A
         *  for (int i=0; i< lstCategory.Count; i++)
         *  Begin
         *      for(int j=0; j< lstCategory[i].SoTrang; j++)
         *      Begin
         *          BocLink(http://www.xpmeinv.com + lstCategory[i] + "list_" + i + "_" + j + ".html")
         *      End
         *  End
         *  
         * 
         * Triển khai nhiệm vụ B
         *  Tại thời điểm boc link ta được các link chi tiết của ảnh rồi    
         */


        public static List<String> DownLoadLinkChiTiet()
        {
            var lstCategories = ListTrang("//div[@class='left']/div[@class='box']/div[@class='tit']/a");
            for (int i = 0; i < lstCategories.Count; i++)
            {
                LoadWeb("http://www.xpmeinv.com" + lstCategories[i]);
            }
            return lstCategories;
        }

        private static HtmlDocument LoadWeb(string link)
        {
            var htmlDocument = new HtmlDocument();
            var hw = new HtmlWeb();
            hw.OverrideEncoding = Encoding.GetEncoding("gb2312");
            htmlDocument = hw.Load(link);
            return htmlDocument;
        }

        /*Lấy chi tiết ảnh bao gồm lấy tiêu đề và tổng số ảnh*/
        public static string[] LayChiTietAnh(string link)
        {
            var htmlDocument = LoadWeb(link);
            var strTitle = htmlDocument.DocumentNode.SelectSingleNode("//h1").InnerText;
            var strSoAnh = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='page']/li[last()-1]/a").InnerText;
            string[] arr = { strTitle, strSoAnh };
            return arr;
        }

        /*Test Lưu Json Object và lưu dữ liệu ra file Text*/
        public static void SaveJsonObject()
        {
            string[] arrChiTietAnh;
            var root = new Root();
            var lstXpmeninv = new List<Xpmeinv>();

            var lstCategories = ListCategories();
            for (int i = 3; i < lstCategories.Count; i++)
            {
                Console.WriteLine("Link thể loại: {0}", "http://www.xpmeinv.com" + lstCategories[i].LinkCategories);
                var xpmeinv = new Xpmeinv();
                //Create Object Save Json
                xpmeinv.LinkTitle = "http://www.xpmeinv.com" + lstCategories[i].LinkCategories;
                var lstImage = new List<ImageGirl>();
                for (int j = 0; j < lstCategories[i].NumberPage; j++)
                {
                    var htmlDocument = LoadWeb(string.Format("http://www.xpmeinv.com" + lstCategories[i].LinkCategories
                         + "list_" + (i + 1) + "_" + (j + 1) + ".html"));
                    var htmlNodeCll = htmlDocument.DocumentNode.SelectNodes("//div[@class='piclist']//div[@class='item']/div/a");
                    int x = 0;
                    int test = 0;
                    for (int k = 0; k < htmlNodeCll.Count; k++)
                    {
                        if (k % 2 == 0)
                        {
                            /*Đoạn này Lấy được link của từng người mẫu -- thường có khoảng 20 người trong 1 trang*/
                            test = test + 1; x = x + 1;
                            Console.WriteLine("  Ma Anh: {0}.{1}-{2}:  http://www.xpmeinv.com"/*-----------------------------------------*/
                                        + htmlNodeCll[k].Attributes[0].Value + "\r\n"
                                        , i + 1, j + 1, test);

                            arrChiTietAnh = LayChiTietAnh("http://www.xpmeinv.com" + htmlNodeCll[k].Attributes[0].Value);
                            Console.WriteLine("    Tiêu đề ảnh: {0} - Tổng số ảnh: {1}", arrChiTietAnh[0], arrChiTietAnh[1]);/*------------------------------*/

                            //Tiếp tục json với Đối tượng con ImageGirl
                            ImageGirl ImageInfor = new ImageGirl();
                            ImageInfor.Title = arrChiTietAnh[0];
                            ImageInfor.N_Img = Convert.ToInt32(arrChiTietAnh[1]);
                            var lstString = new List<string>();

                            for (int h = 0; h < Int32.Parse(arrChiTietAnh[1]) - 1; h++)
                            {
                                var htmlDocument2 = LoadWeb("http://www.xpmeinv.com"
                                + htmlNodeCll[k].Attributes[0].Value.Split('.')[0] + "_" + (h + 2) + ".html");
                                string nodeAnh = "";
                                /*Tại page 13 tiến trình bị block do mã java Script ta phải lấy attribute thông qua Attributes[0]*/
                                try
                                {
                                    nodeAnh = htmlDocument2.DocumentNode
                                        .SelectSingleNode(@"//div[@class='picture']/dl[2]/a/img 
                                            | //div[@class='picture']/dl[2]/img
                                            | //div[@class='picture']/a/img
                                            | //div[@class='picture']/img
                                            | //div[@class='picture']/dl[1]/a/img
                                            | //div[@class='picture']/dl[3]/a/img")
                                        .Attributes["src"].Value;
                                }
                                catch
                                {
                                    Console.WriteLine("\t - Phần mục không có ảnh để hiển thị");
                                    continue;
                                }
                                Console.WriteLine("\t - Link ảnh: {0}\r\n", "http://www.xpmeinv.com" + nodeAnh);
                                lstString.Add("http://www.xpmeinv.com" + nodeAnh);
                            }
                            //Tiếp tục với Json
                            ImageInfor.LstImg = lstString;
                            //Gán tiếp luôn cho đầy đủ thông tin bên Xpmeinv
                            lstImage.Add(ImageInfor);
                            /*đủ hết thông tin rồi thì bh ta ghi Json Object ra file Text thôi :D*/
                        }
                    }
                    Console.WriteLine("Page{0} Đã Được Lấy ===================================", j + 1);
                }
                //Kinh nghiệm là khai báo trước vòng lặp thì gán ở cuối vòng lặp :D
                xpmeinv.ImageInfor = lstImage;
                lstXpmeninv.Add(xpmeinv);


                Console.WriteLine("========================= Kết thuc thể loại {0} =======================\r\n\n"
                    , lstCategories[i].LinkCategories.Split('/')[2].ToString());
            }
            root.Cate = lstXpmeninv;
            var strJson = JsonConvert.SerializeObject(root);
            File.AppendAllText(@"D:\Link1.txt", strJson, Encoding.UTF8);
            Console.WriteLine("Ghi tập tin thành công");
        }
    }
}
