using System;
using System.Net;
using System.Text;

namespace IPFS.Integration
{
    public static class UrlResolver
    {
        public static Uri GetAddUrl(Uri baseUrl, params string[] options) => GetUrl(baseUrl, "add", options);

        public static Uri GetFileInfoUrl(Uri baseUrl, params string[] options) => GetUrl(baseUrl, "file/ls", options);

        public static Uri GetNewObjectUrl(Uri baseUrl, params string[] options) => GetUrl(baseUrl, "object/new", options);

        public static Uri GetObjectUrl(Uri baseUrl, params string[] options) => GetUrl(baseUrl, "object/get", options);

        public static Uri GetLocalObjectListUrl(Uri baseUrl, params string[] options) => GetUrl(baseUrl, "pin/ls", options);

        public static Uri GetUpdateObjectUrl(Uri baseUrl, params string[] options) => GetUrl(baseUrl, "object/put", options);

        public static Uri GetStoreFolderUrl(Uri baseUrl, params string[] options) => GetUrl(baseUrl, "pin/add", options);

        public static Uri GetPublishUrl(Uri baseUrl, params string[] options) => GetUrl(baseUrl, "name/publish", options);

        public static Uri GetResolveUrl(Uri baseUrl, params string[] options) => GetUrl(baseUrl, "name/resolve", options);

        public static Uri GetDownloadUrl(Uri baseUrl, params string[] options) => GetUrl(baseUrl, "cat", options);

        public static Uri GetPeerInfoUrl(Uri baseUrl, params string[] options) => GetUrl(baseUrl, "id", options);

        public static Uri GetPeersUrl(Uri baseUrl, params string[] options) => GetUrl(baseUrl, "swarm/peers", options);

        private static Uri GetUrl(Uri baseUrl, string command, params string[] options)
        {
            var q = new StringBuilder();
            
            foreach (var option in options)
            {
                q.Append('&');
                var i = option.IndexOf('=');
                if (i < 0)
                {
                    q.Append(option);
                }
                else
                {
                    q.Append(option.Substring(0, i));
                    q.Append('=');
                    q.Append(WebUtility.UrlEncode(option.Substring(i + 1)));
                }
            }
            
            var url = new Uri(baseUrl, command);
            
            if (q.Length > 0)
            {
                q[0] = '?';
                url = new Uri(url, q.ToString());
            }
            
            return url;
        }
    }
}