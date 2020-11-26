using HtmlAgilityPack;
using System;
using System.Text.RegularExpressions;

namespace WebCrawler.Core.Utils {
    public static class ExtractionUtils {
        public static string ExtractSrc(HtmlNode node, string domain) {
            var regex = new Regex(@"^\/+");
            var value = node.GetAttributeValue("src", "");
            var match = regex.Match(value);
            if (match.Success) {
                value = $"{domain}/{value.Substring(match.Length)}";
            }
            return value;
        }

        public static string ExtractSrc(string value, string domain) {
            var regex = new Regex(@"^\/+");
            var match = regex.Match(value);
            if (match.Success) {
                value = $"{domain}/{value.Substring(match.Length)}";
            }
            return value;
        }

        public static void InnerExtractSource(string innerText, IProgress<string> progress, string domain) {
            var regex = new Regex("<\\s*source[\\s\\w/.=\"\']*src\\s*=\\s*(\' | \")([\\w.]*)(\'|\")[\\s\\w /.= \"\']*>");
            var matches = regex.Matches(innerText);
            foreach (Match match in matches) {
                progress.Report(ExtractSrc(match.Groups[2].Value, domain));
            }
        }
    }
}
