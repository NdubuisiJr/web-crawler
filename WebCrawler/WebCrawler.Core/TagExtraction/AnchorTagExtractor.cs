using System.Text.RegularExpressions;
using WebCrawler.Core.Contract;

namespace WebCrawler.Core.TagExtraction {
    public class AnchorTagExtractor : IExtractor {
        public string Extract(string outHtml) {
            var linkRegex = new Regex("<a.+href=[\'\"](.*)[\'\"].*>");
            var link = "";
            var matches = linkRegex.Match(outHtml);
            if (matches != null) 
                link = matches.Groups[1].Value;
            return link;
        }
    }
}
