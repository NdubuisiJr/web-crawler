using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using WebCrawler.Core.Contract;
using WebCrawler.Core.TagExtraction;

namespace WebCrawler.Core {
    public class Bot : IBot{
        public async void Craw(
            string url, 
            IProgress<string> textProgress, 
            IProgress<int> countProgress,
            IProgress<string> linkProgress,
            IProgress<string> imageProgress,
            IProgress<string> audioProgress,
            IProgress<string> videoProgress,
            CancellationToken token) {

            var domainRegex = new Regex("(http[s]*:\\/\\/|)([w]{3}\\.|)[a-zA-Z0-9]+\\.[a-zA-Z0-9]+[\\.a-zA-Z0-9]*");
            var match = domainRegex.Match(url);
            var domain = "";
            if (match.Success)
                domain = match.Value;

            _extractors = new List<IExtractor> {
                new AnchorTagExtractor(linkProgress, domain),
                new ImageTagExtractor(imageProgress, domain)
            };

            using (var httpClient = new HttpClient()) {
                var html = await httpClient.GetStringAsync(url);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var body = htmlDoc.DocumentNode.Descendants("body").First();
                ExtractData(
                    body,
                    textProgress,
                    countProgress,
                    linkProgress,
                    imageProgress,
                    audioProgress,
                    videoProgress,
                    token
                );
            }
        }

        private void ExtractData(
            HtmlNode tag,
            IProgress<string> textProgress,
            IProgress<int> countProgress,
            IProgress<string> linkProgress,
            IProgress<string> imageProgress,
            IProgress<string> audioProgress,
            IProgress<string> videoProgress,
            CancellationToken token) {

            if (token.IsCancellationRequested)
                token.ThrowIfCancellationRequested();

            foreach (var lowerTag in tag.Descendants()) {
                ExtractData(
                    lowerTag,
                    textProgress,
                    countProgress,
                    linkProgress,
                    imageProgress,
                    audioProgress,
                    videoProgress,
                    token
              );
            }

            var extractor = _extractors.FirstOrDefault(x => x.CanExtract(tag.Name));
            if (extractor != null)
                extractor.Extract(tag);

            Thread.Sleep(10);
            if (!string.IsNullOrWhiteSpace(tag.InnerText))
                textProgress.Report($"{tag.InnerText.Replace('\n', ' ')}");
            countProgress.Report(count++);
        }

        private int count;
        private List<IExtractor> _extractors;
    }
}
