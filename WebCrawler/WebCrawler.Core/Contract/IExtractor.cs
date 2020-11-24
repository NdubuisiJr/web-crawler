using HtmlAgilityPack;

namespace WebCrawler.Core.Contract {
    public interface IExtractor {
        bool CanExtract(string name);
        void Extract(HtmlNode htmlNode);
    }
}
