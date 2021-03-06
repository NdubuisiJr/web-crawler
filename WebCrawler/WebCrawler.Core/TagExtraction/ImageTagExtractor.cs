﻿using HtmlAgilityPack;
using System;
using WebCrawler.Core.Contract;
using WebCrawler.Core.Utils;

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
            _progress.Report(ExtractionUtils.ExtractSrc(htmlNode, _domain));
        }
    }
}
    