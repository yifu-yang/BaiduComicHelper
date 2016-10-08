using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BaiduComicHelperLibrary;

namespace ComicDownloadUnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void testUrl()
        {
            DownloadHelper dh = new DownloadHelper();
            var url = dh.BuildUrl("288329771", 1, true);
            Assert.AreEqual(url, "http://tieba.baidu.com/p/288329771?see_lz=1&pn=1");
        }

        [TestMethod]
        public void testGetPages()
        {
            DownloadHelper dh = new DownloadHelper();
            var url = dh.BuildUrl("288329771", 1, true);
            var html = dh.GetPages(url);
            Assert.IsNotNull(html);
        }

        [TestMethod]
        public void testGetPics()
        {
            DownloadHelper dh = new DownloadHelper();
            var pics = dh.GetPicsUrls("288329771", 1, true).Count;
            Assert.AreEqual(pics, 30);
        }

        [TestMethod]
        public void testSavePhoto()
        {
            DownloadHelper dh = new DownloadHelper();
            var pics = dh.GetPicsUrls("288329771", 1, true);
            foreach (var pic in pics)
            {
                dh.SavePic(pic, Guid.NewGuid()+"testimg.jpg");
            }
        }
    }
}
