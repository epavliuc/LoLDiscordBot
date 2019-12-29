using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LolApiDll.ApiCall;
using LolApiDll.DataModels;
using LoLApiDLL.DataModels;
using Newtonsoft.Json;

namespace DiscordBot
{
    public static class Methods
    {
        public static async Task<LoLSummonerModel> LoLSummonerAsync(string name)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Riot-Token", ApiConfig.ApiKey);
                var content = await client.GetStringAsync(ApiConfig.SummonerApiUrl + name);
                return JsonConvert.DeserializeObject<LoLSummonerModel>(content);
            }
        }

        public static async Task<List<LoLLeagueEntryModel>> LoLSummonerEntryAsync(string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Riot-Token", ApiConfig.ApiKey);
                var content = await client.GetStringAsync(ApiConfig.LeagueEntryApiUrl + id);
                return JsonConvert.DeserializeObject<List<LoLLeagueEntryModel>>(content);
            }
        }
    }
}
