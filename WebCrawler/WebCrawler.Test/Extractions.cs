
using HtmlAgilityPack;
using NUnit.Framework;
using System.IO;
using System.Linq;
using WebCrawler.Core.TagExtraction;

namespace WebCrawler.Test {
    [TestFixture]
    public class Extractions {
        private HtmlDocument _htmlDoc;

        [SetUp]
        public void Initialize() {
            var html = File.ReadAllText("text.txt");
            _htmlDoc = new HtmlDocument();
            _htmlDoc.LoadHtml(html);
        }

        [Test]
        public void AnchorTagShouldReturnHyperLink() {
            // Arrange
            var anchorTag = _htmlDoc.DocumentNode.Descendants("a").ElementAt(1);
            var url = "/fr/neuf";
            
            // Act
            var anchorExtractor = new AnchorTagExtractor();
            var result = anchorExtractor.Extract(anchorTag);


            Assert.AreEqual(url, result);
        }

        //[Test]
        //public void ImageTagShouldReturnSource() {
        //    var src = "images/background.png";
        //    var outerHtml = $"";
        //}
    }
}
