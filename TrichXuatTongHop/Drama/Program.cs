using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace DramaImage
{
    class Program
    {

        /// <summary>
        /// Describe: Extract Information to Web http://dep.drama.vn
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.BufferHeight = 5000;
            DownLoadStringUrl();
            Console.ReadLine();
        }

        /*Load Web*/
        public static HtmlDocument ResultWeb(string url)
        {
            var hw = new HtmlWeb
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36",
                OverrideEncoding = Encoding.UTF8
            };
            return hw.Load(url);
        }

        /*Select Infor Categories and Title */
        public static List<Categories> ListCategories()
        {
            var htmlDocument = ResultWeb("http://dep.drama.vn/");
            var htmlNodeListCate = htmlDocument.DocumentNode.SelectNodes("//div[@id='categories-2']/ul/li/a");
            var lstCate = new List<Categories>();
            foreach (var node in htmlNodeListCate)
            {
                lstCate.Add(new Categories { TitleCate = node.InnerText, UrlCate = node.Attributes["href"].Value });
            }
            return lstCate;
        }

        /*Select NodeList vs Xpath*/
        public static HtmlNodeCollection SelectNodes(string url, string xpath)
        {
            var htmlDocument = ResultWeb(url);
            return htmlDocument.DocumentNode.SelectNodes(xpath);
        }

        /*Select Infor All url and Categorie - Title in Web Page*/
        public static void DownLoadStringUrl()
        {
            var lstCategories = ListCategories();
            /*JsonObject*/
            var drama = new Drama();
            var lstCate = new List<Cate>();

            /*Browsing from Categories*/
            for (int i = 0; i < 6; i++)
            {
                /*JsonObject*/
                var cate = new Cate { CateId = i, TitleCate = lstCategories[i].TitleCate, UrlCate = lstCategories[i].UrlCate };
                var lstItem = new List<Item>();

                Console.WriteLine("Thể loại :==== {0}", lstCategories[i].TitleCate);
                /*Do trang web này load theo Ajax lên ta sẽ dùng 1 con số cực lớn để duyệt trang*/
                for (int j = 0; j < 1000; j++)
                {
                    var htmlNodeListLink = SelectNodes(lstCategories[i].UrlCate + "/page/" + (j + 1)
                                                , @"//div/article/div/div/h2/a");
                    if (htmlNodeListLink == null)
                    {
                        Console.WriteLine("Số Page lấy được là {0}", j);
                        break;
                    }
                    /*Browsing continue List Image - Lấy các link của anh trong 1 trang*/
                    for (int k = 0; k < htmlNodeListLink.Count; k++)
                    {
                        /*Json Object*/
                        var item = new Item { TitleItem = htmlNodeListLink[k].InnerText };
                        var lstUrl = new List<string>();

                        Console.WriteLine("\tTitle: {0,-40}", htmlNodeListLink[k].InnerText);
                        HtmlNodeCollection htmlNLLImg;
                        htmlNLLImg = SelectNodes(htmlNodeListLink[k].Attributes["href"].Value
                                            , @"//div[@class='post-content']/p/a/img 
                                                | //div[@class='post-content']/h1/a/img
                                                | //div[@class='post-content']/div/a/img");
                        for (int h = 0; h < htmlNLLImg.Count; h++)
                        {
                            lstUrl.Add(htmlNLLImg[h].Attributes["src"].Value);
                            Console.WriteLine("\tLink: {0}", htmlNLLImg[h].Attributes["src"].Value);
                        }
                        item.UrlImage = lstUrl;
                        lstItem.Add(item);
                    }
                    Console.WriteLine("===================== Kết thúc Page {0} =============================", j + 1);
                }
                cate.ItemInfor = lstItem;
                lstCate.Add(cate);
            }
            drama.LstCate = lstCate;
            var s = JsonConvert.SerializeObject(drama);
            File.WriteAllText(@"D:\DramaImage.json", s, Encoding.UTF8);
            Console.WriteLine("Hoan tat");
        }
    }

    /* Phân tích trang web:
     * Ban đầu: Danh Mục Thể Loại - Categories
     *  Tại 1 chuyên mục: Ajax Load Image: Lấy theo đúng thẻ nội dung
     *      Tại 1 Image hiển thị ra 1 trang và lấy ảnh theo đúng cấu trúc
     *      
     */
}
