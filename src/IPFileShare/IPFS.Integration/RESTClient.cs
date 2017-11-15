using System;
using System.Collections.Generic;
using System.Linq;
using IPFS.Integration.Messages;
using IPFS.Results;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using IPFS.Integration.Errors;
using System.Net;
using System.Net.Http.Headers;
namespace IPFS.Integration
{
    public class RESTClient
    {
        public Uri GatewayUrl {get;}
        
        public RESTClient(string gatewayUrl)
        {
            var url = new Uri(gatewayUrl);
            this.GatewayUrl = new Uri(url, "api/v0/");
            
            Console.WriteLine(this.GatewayUrl.ToString());
        }
        
        public T Message<T>() where T : IApiMessage, new()
        {
            var request = new T();
            request.Client = this;
            return request; 
        }
        
        public async Task<Result<string>> UploadAsync(Uri url, Stream body, string fileparam="file", string filename="")
        {
            var result = new Result<string>();
            
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(body);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            if(string.IsNullOrWhiteSpace(filename))
            {
                content.Add(streamContent, fileparam);
            }
            else
            {
                content.Add(streamContent, fileparam, filename);
            }
            using (var response = await Client.PostAsync(url, content))
            {
                var validation = await ValidateResponseAsync(response);
                
                if(!validation.Success)
                {
                    result.AddErrors(validation.Errors);
                    return result;
                }
                
                var json = await response.Content.ReadAsStringAsync();
                
                result.SetValue(json);
                return result;
            }
        }
        
        public async Task<Result<Stream>> DownloadAsync(Uri url)
        {
            var result = new Result<Stream>();
             
            var response = await Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            var validation = await ValidateResponseAsync(response);
            
            if(!validation.Success)
            {
                result.AddErrors(validation.Errors);
                return result;
            }
            
            var stream = await response.Content.ReadAsStreamAsync();
            result.SetValue(stream);
            return result;
        }
        
        public async Task<Result<string>> PostAsync(Uri url)
        {
            var result = new Result<string>();
            
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            
            using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                var validation = await ValidateResponseAsync(response);
                
                if(!validation.Success)
                {
                    result.AddErrors(validation.Errors);
                    return result;
                }
                
                var json = await response.Content.ReadAsStringAsync();
                
                result.SetValue(json);
                return result;
            }
        }
        
        public async Task<Result<string>> GetAsync(Uri url)
        {
            var result = new Result<string>();
            
            using (var response = await Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                var validation = await ValidateResponseAsync(response);
                
                if(!validation.Success)
                {
                    result.AddErrors(validation.Errors);
                    return result;
                }
                
                var json = await response.Content.ReadAsStringAsync();
                
                result.SetValue(json);
                return result;
            }
        }
        
        private HttpClient Client 
        {
            get
            {
                var client = new HttpClient();
                return client;
            }
        }
        
        private async Task<VoidResult> ValidateResponseAsync(HttpResponseMessage response)
        {
            var result = new VoidResult();
            
            if (response.IsSuccessStatusCode)
                return result;
                
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                result.AddErrors(new InvalidIPFSCommandError("Invalid IPFS command"));
                return result;
            }
            
            var body = await response.Content.ReadAsStringAsync();
            
            var message = (string)JsonConvert.DeserializeObject<dynamic>(body).Message;
            
            result.AddErrors(new HttpRequestError(message));
            return result;
        }
    }
}