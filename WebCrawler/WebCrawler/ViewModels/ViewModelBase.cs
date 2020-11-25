using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WebCrawler.ViewModels {
    public class ViewModelBase : INotifyPropertyChanged {

        protected void RaisePropertyChanged([CallerMemberName] string propName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
