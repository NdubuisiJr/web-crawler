using HtmlAgilityPack;

namespace WebCrawler.Core.Contract {
    public interface IExtractor {
        string Extract(HtmlNode htmlNode);
    }
}
