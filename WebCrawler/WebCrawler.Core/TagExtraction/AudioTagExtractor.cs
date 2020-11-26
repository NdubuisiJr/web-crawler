using HtmlAgilityPack;
using System;
using WebCrawler.Core.Contract;
using WebCrawler.Core.Utils;

namespace WebCrawler.Core.TagExtraction {
    public class AudioTagExtractor : IExtractor {
        private IProgress<string> _progress;
        private string _domain;

        public AudioTagExtractor(IProgress<string> progress, string domain) {
            _progress = progress;
            _domain = domain;
        }

        public bool CanExtract(string name) {
            return name.Contains("audio");
        }

        public void Extract(HtmlNode htmlNode) {
            ExtractionUtils.InnerExtractSource(htmlNode.InnerText,_progress,_domain);
        }
    }
}
