using HtmlAgilityPack;
using System;
using WebCrawler.Core.Contract;

namespace WebCrawler.Core.TagExtraction {
    public class AnchorTagExtractor : IExtractor {

        public AnchorTagExtractor(IProgress<string> progress, string domain) {
            _progress = progress;
            _domain = domain;
        }

        public bool CanExtract(string name) {
            return !string.IsNullOrWhiteSpace(name) && name == "a";
        }

        public void Extract(HtmlNode htmlNode) {
            var value= htmlNode.GetAttributeValue("href", "");
            if (value.StartsWith("/")) {
                value = $"{_domain}{value}";
            }
            _progress.Report(value);
        }

        private IProgress<string> _progress;
        private string _domain;
    }
}
