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
            CancellationToken token,
            Action<string> errorCallBack) {

            var domainRegex = new Regex("(http[s]*:\\/\\/|[w]{3}\\.)[a-zA-Z0-9]+\\.[a-zA-Z0-9]+[\\.a-zA-Z0-9]*");
            var match = domainRegex.Match(url);
            var domain = "";
            if (match.Success) {
                domain = match.Value;
                if(match.Groups[1].Value == "www.") {
                    var newDomain = domain.Replace("www.", "https://");
                    url = url.Replace(domain, newDomain);
                    domain = newDomain;
                }
            }

            _extractors = new List<IExtractor> {
                new AnchorTagExtractor(linkProgress, domain),
                new ImageTagExtractor(imageProgress, domain),
                new AudioTagExtractor(audioProgress,domain),
                new VideoTagExtractor(videoProgress,domain)
            };

            try {
                using (var httpClient = new HttpClient()) {
                    var html = "";
                    using (var response = await httpClient.GetAsync(url)) {
                        if (response.IsSuccessStatusCode &&
                            response.Content.Headers.ContentType.MediaType.Contains("html")) {
                            html = await response.Content.ReadAsStringAsync();
                        }
                        else {
                            errorCallBack.Invoke("Error, Content of url is not Html");
                            return;
                        }
                    }

                    //var html = await httpClient.GetStringAsync(url);
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
                        token,
                        errorCallBack
                    );
                    countProgress.Report(-888);
                }
            }
            catch{
                countProgress.Report(-999);
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
            CancellationToken token,
            Action<string> callback) {

            try {
                foreach (var lowerTag in tag.Descendants()) {
                    if (token.IsCancellationRequested)
                        break;
                    ExtractData(
                        lowerTag,
                        textProgress,
                        countProgress,
                        linkProgress,
                        imageProgress,
                        audioProgress,
                        videoProgress,
                        token,
                        callback
                  );
                }

                if (token.IsCancellationRequested) {
                    token.ThrowIfCancellationRequested();
                    return;
                }

                var extractor = _extractors.FirstOrDefault(x => x.CanExtract(tag.Name));
                if (extractor != null)
                    extractor.Extract(tag);

                Thread.Sleep(15);
                if (!string.IsNullOrWhiteSpace(tag.InnerText))
                    textProgress.Report($"{tag.InnerText.Replace('\n', ' ')}");
                countProgress.Report(count++);
            }
            catch (OperationCanceledException) {
                countProgress.Report(-999);
            }
        }

        private int count;
        private List<IExtractor> _extractors;
    }
}
