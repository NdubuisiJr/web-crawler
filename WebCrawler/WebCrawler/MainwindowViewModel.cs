using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.Command;
using WebCrawler.Core;

namespace WebCrawler {
    public class MainwindowViewModel : INotifyPropertyChanged {
        public MainwindowViewModel() {
            Texts = new ObservableCollection<string>();
            Links = new ObservableCollection<string>();
            Images = new ObservableCollection<string>();
            Audios = new ObservableCollection<string>();
            Videos = new ObservableCollection<string>();
            Craw = new ActionCommand<object>(CrawAction);
            Cancel = new ActionCommand<object>(CancelAction);
        }

        private void CancelAction(object obj) {
            _tokenSource.Cancel();
        }

        private async void CrawAction(object obj) {
            var textProgress = new Progress<string>(value => {
                Texts.Insert(0, value);
            });
            var linkProgress = new Progress<string>(value => {
                Links.Insert(0, value);
            });
            var imageProgress = new Progress<string>(value => {
                Images.Insert(0, value);
            });
            var audioProgress = new Progress<string>(value => {
                Audios.Insert(0, value);
            });
            var videoProgress = new Progress<string>(value => {
                Videos.Insert(0, value);
            });
            var countProgress = new Progress<int>(value => {
                TagCount = $"Number of tags crawled = {value}";
            });
            await Task.Run(() =>Extract(
                textProgress,
                countProgress,
                linkProgress,
                imageProgress,
                audioProgress,
                videoProgress
            ));
        }

        public void Extract(
                IProgress<string> textProgress,
                IProgress<int> countProgress,
                IProgress<string> linkProgress,
                IProgress<string> imageProgress,
                IProgress<string> audioProgress,
                IProgress<string> videoProgress
            ) {

            if (string.IsNullOrWhiteSpace(URL)) {
                TagCount = "Error...\nInvalid or blank URL";
                return;
            }
            _tokenSource = new CancellationTokenSource();
            try {
                var crawingBot = new Bot();
                crawingBot.Craw(
                    URL, 
                    textProgress,
                    countProgress,
                    linkProgress,
                    imageProgress,
                    audioProgress,
                    videoProgress,
                    _tokenSource.Token
                 );
            }
            catch (OperationCanceledException e) {
                TagCount = "Cancelled";
            }
            finally {
                _tokenSource.Dispose();
            }
        }

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

        public ObservableCollection<string> Texts { get; set; }
        public ObservableCollection<string> Links { get; set; }
        public ObservableCollection<string> Images { get; set; }
        public ObservableCollection<string> Audios { get; set; }
        public ObservableCollection<string> Videos { get; set; }

        private void RaisePropertyChanged([CallerMemberName] string propName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private CancellationTokenSource _tokenSource;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
