using WebCrawler.Core.Models;

namespace WebCrawler.Interfaces {
    public interface ISaveOpen {
        Site Save();
        void Open(Site data);
    }
}
