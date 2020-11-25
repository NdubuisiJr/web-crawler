using System.Windows.Controls;
using WebCrawler.ViewModels;

namespace WebCrawler.Views {
    /// <summary>
    /// Interaction logic for CrawlerView.xaml
    /// </summary>
    public partial class CrawlerView : UserControl {
        public CrawlerView() {
            InitializeComponent();
            DataContext = new CrawlerViewModel();
        }
    }
}
