using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using WebCrawler.Core.Models;

namespace WebCrawler.Core.Utils {
    public class DataIO {
        private XmlSerializer _serializer;
        private string _path;

        public DataIO() {
            _serializer = new XmlSerializer(typeof(Site));
            _path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/.webcrawler";
            var dirInfo = new DirectoryInfo(_path);
            if (!dirInfo.Exists)
                dirInfo.Create();
        }

        public void Save(Site data) {
            var filePath = Path.Combine(_path, $"{data.SiteName}.xml");
            var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            _serializer.Serialize(fileStream, data);
        }

        public Site Open(string siteName) {
            var path = Path.Combine(_path, $"{siteName}.xml");
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return (Site)_serializer.Deserialize(stream);
        }

        public bool Delete(string siteName) {
            var path = Path.Combine(_path, $"{siteName}.xml");
            if (File.Exists(path))
                File.Delete(path);
            else
                return false;
            return true;
        }

        public IEnumerable<string> GetSaved() {
            var files = new DirectoryInfo(_path).GetFiles();
            foreach (var file in files)
                yield return file.Name.Replace(".xml","");
        }
    }
}
