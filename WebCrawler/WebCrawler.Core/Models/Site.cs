using System.Collections.Generic;

namespace WebCrawler.Core.Models {
    public class Site {
        public Site() {
            Texts = new List<string>();
            Links = new List<string>();
            Images = new List<string>();
            Audios = new List<string>();
            Videos = new List<string>();
        }
        public string SiteName { get; set; }
        public string Url { get; set; }
        public string TagCount { get; set; }
        public List<string> Texts { get; set; }
        public List<string> Links { get; set; }
        public List<string> Images { get; set; }
        public List<string> Audios { get; set; }
        public List<string> Videos { get; set; }
    }
}
