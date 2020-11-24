using HtmlAgilityPack;
using System;
using System.Text.RegularExpressions;
using WebCrawler.Core.Contract;

namespace WebCrawler.Core.TagExtraction {
    public class ImageTagExtractor : IExtractor {
        private IProgress<string> _progress;
        private string _domain;

        public ImageTagExtractor(IProgress<string> progress, string domain) {
            _progress = progress;
            _domain = domain;
        }

        public bool CanExtract(string name) {
            return !string.IsNullOrWhiteSpace(name) && name.Contains("img");
        }

        public void Extract(HtmlNode htmlNode) {
            var regex = new Regex("^\\/+");
            var value = htmlNode.GetAttributeValue("src", "");
            var match = regex.Match(value);
            if (match.Success) {
                value = $"{_domain}/{value.Substring(match.Length)}";
            }
            else {
                value = $"{_domain}/{value}";
            }
            _progress.Report(value);
        }
    }
}
    