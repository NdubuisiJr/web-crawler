using WebCrawler.Navigation;
using WebCrawler.Views;

namespace WebCrawler {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow {
        public MainWindow() {
            InitializeComponent();
            DataContext = Navigator.INSTANCE
                         .SetNavigatorContext(new MainwindowViewModel());
            Navigator.INSTANCE.RequestNavigate(new WelcomeView());
        }
    }
}
