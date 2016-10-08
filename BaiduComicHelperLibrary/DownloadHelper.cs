using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaiduComicHelperLibrary
{
    public class DownloadHelper
    {
        public string rootPath { get; set; }

        public void DownloadComic(string pageId, bool isLZ)
        {
            string chapterName = string.Empty;
            int pageCount = GetPageCountAndName(pageId, isLZ, out chapterName);
            List<string> picSet = new List<string>();
            for (int pageindex = 1; pageindex <= pageCount; pageindex++)
            {
                picSet.AddRange(GetPicsUrls(pageId, pageindex, isLZ));
            }
            Console.WriteLine(chapterName + " total pics : " + picSet.Count);
            if (!Directory.Exists(chapterName))
                Directory.CreateDirectory(Path.Combine(rootPath, chapterName));
            for (int imgIndex = 1; imgIndex < picSet.Count + 1; imgIndex++)
            {
                SavePic(picSet[imgIndex - 1], Path.Combine(rootPath, chapterName, imgIndex + ".jpg"));
            }
        }

        public void SavePic(string url, string fileName)
        {
            if (File.Exists(fileName))
                return;
            bool flag = false;
            while (!flag)
            {
                try
                {
                    var req = HttpWebRequest.CreateHttp(url);
                    req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36";
                    req.Host = "hiphotos.baidu.com";
                    using (var rep = req.GetResponse())
                    {
                        using (var stream = new BinaryReader(rep.GetResponseStream()))
                        {
                            File.WriteAllBytes(fileName, stream.ReadBytes(1024 * 1024 * 10 * 1));
                            flag = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("exception :" + e.ToString()+" file name :"+fileName);
                }
            }
        }

        public int GetPageCountAndName(string pageId, bool isLZ, out string name)
        {
            string url = BuildUrl(pageId, 1, isLZ);
            var htmlContent = GetPages(url);
            string nameRegex = "<title>([^/]+)</title>";
            name = Regex.Match(htmlContent, nameRegex).Groups[1].Value;
            File.WriteAllText( name + ".html",htmlContent);
            string pageCountRegex = "<span\\sclass=\"red\">(\\d+)</span>";
            var match = Regex.Match(htmlContent, pageCountRegex).Groups[1].Value;
            int pageCount;
            if (int.TryParse(match, out pageCount))
            {
                Console.WriteLine("chapter :" + name + " total page:" + pageCount);
                return pageCount;
            }
            else
            {
                return -1;
            }
        }

        public List<string> GetPicsUrls(string pageId, int pageNum, bool isLZ)
        {
            List<string> Result = new List<string>();
            string url = BuildUrl(pageId, pageNum, isLZ);
            var htmlTxt = GetPages(url);
            string regex = "http://hiphotos\\.baidu\\.com/[^/]+/pic/item/[^\\.]+\\.jpg|http://imgsrc\\.baidu\\.com/forum/w%3D580/sign=[^/]+/[^\\.]+\\.jpg";
            var matches = Regex.Matches(htmlTxt, regex);
            foreach (Match match in matches)
            {
                Result.Add(match.Groups[0].Value);
            }
            return Result;
        }

        public string GetPages(string url)
        {
            string htmlText = string.Empty;
            var req = HttpWebRequest.Create(url);
            using (var rep = req.GetResponse())
            {
                try
                {
                    var repStream = rep.GetResponseStream();
                    htmlText = new StreamReader(repStream).ReadToEnd();
                }
                catch
                {
                    //Undo: exception process
                }
                finally
                {
                    rep.Dispose();
                }
            }
            return htmlText;
        }

        public string BuildUrl(string pageId, int pageNum, bool isLZ)
        {
            string template = @"http://tieba.baidu.com/p/";

            if (!string.IsNullOrWhiteSpace(pageId))
            {
                template += pageId;
            }

            if (isLZ)
            {
                template += "?see_lz=1";
            }
            else
            {
                template += "?see_lz=0";
            }

            if (pageNum > 0)
            {
                template += "&pn=" + pageNum;
            }
            return template;
        }
    }
}
