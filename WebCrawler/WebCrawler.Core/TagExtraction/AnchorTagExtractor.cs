using HtmlAgilityPack;
using WebCrawler.Core.Contract;

namespace WebCrawler.Core.TagExtraction {
    public class AnchorTagExtractor : IExtractor {

        public string Extract(HtmlNode htmlNode) {
            var value= htmlNode.GetAttributeValue("href", "");
            return value;
        }
    }
}
