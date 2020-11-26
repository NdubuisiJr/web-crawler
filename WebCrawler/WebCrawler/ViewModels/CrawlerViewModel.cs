using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WebCrawler.Command;
using WebCrawler.Core;

namespace WebCrawler.ViewModels {
    public class CrawlerViewModel : ViewModelBase {
        public CrawlerViewModel() {
            InitializeCollections();
            Craw = new ActionCommand<object>(CrawAction, x => _canCraw);
            Cancel = new ActionCommand<object>(CancelAction, x => _canCancel);
        }

        private void CancelAction(object obj) {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            _canCraw = true;
            _canCancel = false;
            Cancel.RaiseCanExecuteChanged();
            Craw.RaiseCanExecuteChanged();
        }

        private async void CrawAction(object obj) {
            InitializeCollections();
            _canCraw = false;
            _canCancel = true;
            Craw.RaiseCanExecuteChanged();
            Cancel.RaiseCanExecuteChanged();
            var textProgress = new Progress<string>(value => {
                if (TagCount == "Cancelled")
                    return;
                Texts.Insert(0, value);
            });
            var linkProgress = new Progress<string>(value => {
                if (TagCount == "Cancelled")
                    return;
                Links.Insert(0, value);
            });
            var imageProgress = new Progress<string>(value => {
                if (TagCount == "Cancelled")
                    return;
                Images.Insert(0, value);
            });
            var audioProgress = new Progress<string>(value => {
                if (TagCount == "Cancelled")
                    return;
                Audios.Insert(0, value);
            });
            var videoProgress = new Progress<string>(value => {
                if (TagCount == "Cancelled")
                    return;
                Videos.Insert(0, value);
            });
            var countProgress = new Progress<int>(value => {
                if (value == -888 & TagCount!="Cancelled") {
                    TagCount = "Completed";
                    _canCraw = true;
                    _canCancel = false;
                    Cancel.RaiseCanExecuteChanged();
                    Craw.RaiseCanExecuteChanged();
                }
                else {
                    TagCount = value == -999 || value == -888 ? "Cancelled" : $"Number of tags crawled = {value}";
                }
            });
            await Task.Run(() => Extract(
                textProgress,
                countProgress,
                linkProgress,
                imageProgress,
                audioProgress,
                videoProgress
            ));
        }

        private void Extract(
                IProgress<string> textProgress,
                IProgress<int> countProgress,
                IProgress<string> linkProgress,
                IProgress<string> imageProgress,
                IProgress<string> audioProgress,
                IProgress<string> videoProgress
            ) {

            if (string.IsNullOrWhiteSpace(URL)) {
                TagCount = "Error...\nInvalid or blank URL";
                _canCancel = false;
                _canCraw = true;
                Cancel.RaiseCanExecuteChanged();
                Craw.RaiseCanExecuteChanged();
                return;
            }
            _tokenSource = new CancellationTokenSource();
            var crawingBot = new Bot();
            crawingBot.Craw(
                URL,
                textProgress,
                countProgress,
                linkProgress,
                imageProgress,
                audioProgress,
                videoProgress,
                _tokenSource.Token,
                ErrorCallBack
             );
        }


        private void InitializeCollections() {
            Texts = new ObservableCollection<string>();
            Links = new ObservableCollection<string>();
            Images = new ObservableCollection<string>();
            Audios = new ObservableCollection<string>();
            Videos = new ObservableCollection<string>();
        }

        private void ErrorCallBack(string errorMessage) {
            MessageBox.Show(errorMessage);
            _canCancel = false;
            _canCraw = true;
            Cancel.RaiseCanExecuteChanged();
            Craw.RaiseCanExecuteChanged();
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

        private ObservableCollection<string> _text;
        public ObservableCollection<string> Texts {
            get => _text;
            set {
                _text = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<string> _links;
        public ObservableCollection<string> Links {
            get => _links;
            set {
                _links = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<string> _images;
        public ObservableCollection<string> Images {
            get => _images;
            set {
                _images = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<string> _audios;
        public ObservableCollection<string> Audios {
            get => _audios;
            set {
                _audios = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<string> _videos;
        public ObservableCollection<string> Videos {
            get => _videos;
            set {
                _videos = value;
                RaisePropertyChanged();
            }
        }

        public ActionCommand<object> Craw { get; set; }
        public ActionCommand<object> Cancel { get; set; }

        private CancellationTokenSource _tokenSource;
        private bool _canCraw = true;
        private bool _canCancel = false;
    }
}
