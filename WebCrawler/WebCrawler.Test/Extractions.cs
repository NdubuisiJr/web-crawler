
using NUnit.Framework;
using WebCrawler.Core.TagExtraction;

namespace WebCrawler.Test {
    public class Extractions {
        [Test]
        public void AnchorTagShouldReturnHyperLink() {
            // Arrange
            var url = "www.google.com";
            var outerHtml = $"<a href=\"{url}\">Go to google</a>";
            var outerHtml1 = $"<a href=\'{url}\'>Go to google</a>";

            // Act
            var anchorExtractor = new AnchorTagExtractor();
            var result = anchorExtractor.Extract(outerHtml);
            var result1 = anchorExtractor.Extract(outerHtml1);

            Assert.AreEqual(url, result);
            Assert.AreEqual(url, result1);
        }

        //[Test]
        //public void ImageTagShouldReturnSource() {
        //    var src = "images/background.png";
        //    var outerHtml = $"";
        //}
    }
}
