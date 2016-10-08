using BaiduComicHelperLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiduComicConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
                Console.WriteLine("lost args");
            string rootpath = args[0];
            List<string> chapterId = new List<string>();
            if (args.Length < 2)
                Console.WriteLine("no chapter ids");
            for (int i = 1; i < args.Length; i++)
            {
                chapterId.Add(args[i]);
            }
            ComicHelper helper = new ComicHelper();
            helper.chapterIds = chapterId;
            helper.DownloadComic(rootpath, false);
            Console.WriteLine("download finished!");
            Console.ReadLine();
        }
    }
}
