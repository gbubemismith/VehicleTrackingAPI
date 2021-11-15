using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Core.Dtos;
using Core.Interfaces.services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class HttpServices : IHttpServices
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        public HttpServices(IConfiguration config, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _config = config;

        }
        public async Task<string> LocalityName(decimal latitude, decimal longitude)
        {
            /** using geocode api to return formatted addres and best practice 
            for returning locality as recommended by google 
            https://developers.google.com/maps/documentation/geocoding/web-service-best-practices#Parsing 
            **/

            var client = _clientFactory.CreateClient("google");
            client.DefaultRequestHeaders.Accept.Clear();

            var builder = new UriBuilder(_config["Api:Url"]);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["latlng"] = $"{latitude},{longitude}";
            query["key"] = _config["Api:Key"];
            builder.Query = query.ToString();

            var url = builder.ToString();

            using (var response = await client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var resp = JsonConvert.DeserializeObject<LocalityResponseDto>(response.Content.ReadAsStringAsync().Result);

                    return resp.results[0].formatted_address;

                }
            }

            return null;
        }
    }
}