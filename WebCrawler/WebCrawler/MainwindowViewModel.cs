using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.Command;
using WebCrawler.Core.Contract;
using WebCrawler.Core.TagExtraction;

namespace WebCrawler {
    public class MainwindowViewModel : INotifyPropertyChanged {
        public MainwindowViewModel() {
            Data = new ObservableCollection<string>();
            _anchorExtractor = new AnchorTagExtractor();
            Craw = new ActionCommand<object>(CrawAction);
            Cancel = new ActionCommand<object>(CancelAction);
        }

        private void CancelAction(object obj) {
            _tokenSource.Cancel();
        }

        private async void CrawAction(object obj) {
            var progress = new Progress<string>(value => {
                Data.Add(value);
            });
            var countProgress = new Progress<int>(value => {
                TagCount = $"Number of tags crawled = {value}";
            });
            await Task.Run(() => Extract(progress, countProgress));

        }

        public async void Extract(IProgress<string> progress, IProgress<int> countProgress) {
            if (string.IsNullOrWhiteSpace(URL)) {
                TagCount = "Error...\nInvalid or blank URL";
                return;
            }
            _tokenSource = new CancellationTokenSource();
            try {
                using (var httpClient = new HttpClient()) {
                    var html = await httpClient.GetStringAsync(URL);
                    var htmlDoc = new HtmlDocument();

                    htmlDoc.LoadHtml(html);
                    var body = htmlDoc.DocumentNode.Descendants("body").First();
                    ExtractData(body, progress, countProgress, _tokenSource.Token);
                }
            }
            catch (OperationCanceledException e) {
                TagCount = "Cancelled";
            }
            finally {
                _tokenSource.Dispose();
            }
        }

        private void ExtractData(HtmlNode tag, IProgress<string> progress, IProgress<int> countProgress, CancellationToken token) {

            if (token.IsCancellationRequested)
                token.ThrowIfCancellationRequested();

            foreach (var lowerTag in tag.Descendants()) {
                ExtractData(lowerTag, progress, countProgress, token);
            }

            if (tag.Name.Contains("a")) {
                progress.Report("EXTRACTOR = " + _anchorExtractor.Extract(tag.OuterHtml));
            };

            Thread.Sleep(100);

            progress.Report($"{tag.InnerText.Replace('\n', ' ')}");
            countProgress.Report(count++);
        }

        private int count;

        private string _tagCount;
        public string TagCount {
            get => _tagCount;
            set {
                _tagCount = value;
                RaisePropertyChanged();
            }
        }

        private string _url;
        public string URL {
            get => _url;
            set {
                _url = value;
                RaisePropertyChanged();
            }
        }

        public ActionCommand<object> Craw { get; set; }
        public ActionCommand<object> Cancel { get; set; }

        public ObservableCollection<string> Data { get; protected set; }

        private void RaisePropertyChanged([CallerMemberName] string propName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private IExtractor _anchorExtractor;
        private CancellationTokenSource _tokenSource;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
