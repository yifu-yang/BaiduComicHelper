using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiduComicHelperLibrary
{
    public class ComicHelper
    {
        public List<string> chapterIds { get; set; }

        private DownloadHelper _helper;

        public DownloadHelper helper
        {
            set
            {
                _helper = value;
            }
            get
            {
                if (_helper == null)
                {
                    _helper = new DownloadHelper();
                }
                return _helper;
            }
        }

        public int DownloadComic(string dir, bool isLZ)
        {
            helper.rootPath = dir;
            foreach (var id in chapterIds)
            {
                Console.WriteLine("now download id :"+id);
                helper.DownloadComic(id, isLZ);
                Console.WriteLine(" download id :" + id+" finished");
            }
            return 0;
        }

    }
}
