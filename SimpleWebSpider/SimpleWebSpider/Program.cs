using HtmlAgilityPack;
using HttpCode.Core;
using System;
using System.Threading;

namespace SimpleWebSpider
{
    class Program
    {
        //Reference Link http://www.cnblogs.com/stulzq/p/7474334.html

        static void Main(string[] args)
        {
            int pageIndex = 1;//页数
            int maxPageIndex = 10;//最大页数
            HttpHelpers httpHelpers = new HttpHelpers();

            for (int i = 0; i < maxPageIndex; i++)
            {
                HttpItems items = new HttpItems();
                items.Url = "https://www.cnblogs.com/mvc/AggSite/PostList.aspx";//请求地址
                items.Method = "Post";//请求方式 post
                items.Postdata = "{\"CategoryType\":\"SiteHome\"," +
                                    "\"ParentCategoryId\":0," +
                                    "\"CategoryId\":808," +
                                    "\"PageIndex\":" + (i + 1) + "," + //因为i从0开始 所以此处我们要加1
                                    "\"TotalPostCount\":4000," +
                                    "\"ItemListActionName\":\"PostList\"}";//请求数据
                HttpResults hr = httpHelpers.GetHtml(items);

                //解析数据
                HtmlDocument doc = new HtmlDocument();
                //加载html
                doc.LoadHtml(hr.Html);

                //获取 class=post_item_body 的div列表
                HtmlNodeCollection itemNodes = doc.DocumentNode.SelectNodes("div[@class='post_item']/div[@class='post_item_body']");

                Console.WriteLine($"第{i + 1}页数据：");

                //循环根据每个div解析我们想要的数据
                foreach (var item in itemNodes)
                {
                    //获取包含博文标题和地址的 a 标签
                    var nodeA = item.SelectSingleNode("h3/a");
                    //获取博文标题
                    string title = nodeA.InnerText;
                    //获取博文地址 a标签的 href 属性
                    string url = nodeA.GetAttributeValue("href", "");

                    //获取包含作者名字的 a 标签
                    var nodeAuthor = item.SelectSingleNode("div[@class='post_item_foot']/a[@class='lightblue']");
                    string author = nodeAuthor.InnerText;
                    //输出数据
                    Console.WriteLine($"标题：{title} | 作者：{author} | 地址：{url}");
                }

                //每抓取一页数据 暂停三秒
                Thread.Sleep(3000);
            }

            Console.ReadKey();
        }
    }
}
