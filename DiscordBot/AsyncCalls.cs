﻿using System;
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
    public static class AsyncCalls
    {
        public static async Task<LoLSummonerModel> LoLSummonerAsync(string name)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Riot-Token", ApiConfig.ApiKey);
                try
                {
                    var content = await client.GetStringAsync(ApiConfig.SummonerApiUrl + name);
                    return JsonConvert.DeserializeObject<LoLSummonerModel>(content);
                }
                catch
                {
                    return await Task.FromResult<LoLSummonerModel>(null);
                }
                
            }
        }

        public static async Task<List<LoLLeagueEntryModel>> LoLSummonerEntryAsync(string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Riot-Token", ApiConfig.ApiKey);
                try
                {
                    var content = await client.GetStringAsync(ApiConfig.LeagueEntryApiUrl + id);
                    return JsonConvert.DeserializeObject<List<LoLLeagueEntryModel>>(content);
                }
                catch
                {
                    return await Task.FromResult<List<LoLLeagueEntryModel>>(null);
                }
                
            }
        }

        public static async Task<List<LoLTftModel>> LoLTftEntryAsync(string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Riot-Token", ApiConfig.ApiKey);
                try
                {
                    var content = await client.GetStringAsync(ApiConfig.TftApiUrl + id);
                    return JsonConvert.DeserializeObject<List<LoLTftModel>>(content);
                }
                catch
                {
                    return await Task.FromResult<List<LoLTftModel>>(null);
                }

            }
        }

        public static async Task<LoLCurrentGameModel> LoLCurrentGameAsync(string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Riot-Token", ApiConfig.ApiKey);
                try
                {
                    var content = await client.GetStringAsync(ApiConfig.CurrentGameApiUrl + id);
                    return JsonConvert.DeserializeObject<LoLCurrentGameModel>(content);
                }
                catch
                {
                    return await Task.FromResult<LoLCurrentGameModel>(null);
                }
            }
        }

        public static async Task<List<LoLMasteryModel>> LoLMasteryAsync(string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Riot-Token", ApiConfig.ApiKey);
                try
                {
                    var content = await client.GetStringAsync(ApiConfig.MasteryApiUrl + id);
                    return JsonConvert.DeserializeObject<List<LoLMasteryModel>>(content);
                }
                catch
                {
                    return await Task.FromResult<List<LoLMasteryModel>>(null);
                }
            }
        }
        public static async Task<LoLChampionModel> LoLChampionAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var content = await client.GetStringAsync("http://ddragon.leagueoflegends.com/cdn/9.24.2/data/en_US/champion.json");
                    return JsonConvert.DeserializeObject<LoLChampionModel>(content);
                }
                catch
                {
                    return await Task.FromResult<LoLChampionModel>(null);
                }
            }
        }
    }
}
