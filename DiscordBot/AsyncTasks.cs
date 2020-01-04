using LolApiDll.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class AsyncTasks
    {
        public static string Level;
        public static string TftRank;
        public static string SoloRank;
        public static string FlexRank;
        public static async Task<string> ShowPlayer(string query)
        {
            var LoLSummonerModel = await AsyncCalls.LoLSummonerAsync(query);
            if (LoLSummonerModel == null)
            {
                return ("```No such player```");
            }

            Level = string.Format("Level: {0}", LoLSummonerModel.summonerLevel);
            var LoLLeagueEntry = await AsyncCalls.LoLSummonerEntryAsync(LoLSummonerModel.id);
            var LoLTftEntry = await AsyncCalls.LoLTftEntryAsync(LoLSummonerModel.id);

            if (LoLLeagueEntry.Count == 0)
            {
                if (LoLTftEntry.Count == 0)
                {
                    return $"```{Level}\n With no ranked or tft history```";
                }
                TftRank = string.Format("Tft Rank: {0} {1} {2}% Win Rate", LoLTftEntry[0].tier, LoLTftEntry[0].rank, WinRate(LoLTftEntry[0].wins, LoLTftEntry[0].losses));
                return $"```{Level}\n{TftRank}\nWith no ranked history```";
            }
            else if (LoLLeagueEntry.Count == 1)
            {
                if (LoLTftEntry.Count == 0)
                {
                    if (LoLLeagueEntry[0].queueType == "RANKED_SOLO_5x5")
                    {
                        SoloRank = string.Format("Solo Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[0].tier, LoLLeagueEntry[0].rank, WinRate(LoLLeagueEntry[0].wins, LoLLeagueEntry[0].losses));
                        return $"```{Level}\n{SoloRank}\nNo Flex history\nNo Tft history```";
                    }
                    else if (LoLLeagueEntry[0].queueType != "RANKED_SOLO_5x5")
                    {
                        FlexRank = string.Format("Solo Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[0].tier, LoLLeagueEntry[0].rank, WinRate(LoLLeagueEntry[0].wins, LoLLeagueEntry[0].losses));
                        return $"```{Level}\n{FlexRank}\nNo Solo history\nNo Tft history```";
                    }
                    return $"```{Level}\n With no ranked or tft history```";
                }
                else
                {
                    if (LoLLeagueEntry[0].queueType == "RANKED_SOLO_5x5")
                    {
                        SoloRank = string.Format("Solo Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[0].tier, LoLLeagueEntry[0].rank, WinRate(LoLLeagueEntry[0].wins, LoLLeagueEntry[0].losses));
                        TftRank = string.Format("Tft Rank: {0} {1} {2}% Win Rate", LoLTftEntry[0].tier, LoLTftEntry[0].rank, WinRate(LoLTftEntry[0].wins, LoLTftEntry[0].losses));
                        return $"```{Level}\n{SoloRank}\nNo Flex history\n{TftRank}```";
                    }
                    else if (LoLLeagueEntry[0].queueType != "RANKED_SOLO_5x5")
                    {
                        FlexRank = string.Format("Solo Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[0].tier, LoLLeagueEntry[0].rank, WinRate(LoLLeagueEntry[0].wins, LoLLeagueEntry[0].losses));
                        TftRank = string.Format("Tft Rank: {0} {1} {2}% Win Rate", LoLTftEntry[0].tier, LoLTftEntry[0].rank, WinRate(LoLTftEntry[0].wins, LoLTftEntry[0].losses));
                        return $"```{Level}\n{FlexRank}\nNo Solo history\n{TftRank}```";
                    }
                    return $"```{Level}\n With no ranked or tft history```";
                }
            }
            else
            {
                SoloRank = string.Format("Solo Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[1].tier, LoLLeagueEntry[1].rank, WinRate(LoLLeagueEntry[1].wins, LoLLeagueEntry[1].losses));
                FlexRank = string.Format("Flex Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[0].tier, LoLLeagueEntry[0].rank, WinRate(LoLLeagueEntry[0].wins, LoLLeagueEntry[0].losses));
                if (LoLTftEntry.Count == 0)
                {
                    return $"```{Level}\n{SoloRank}\n{FlexRank}\nWith no tft history```";
                }
                TftRank = string.Format("Tft Rank: {0} {1} {2}% Win Rate", LoLTftEntry[0].tier, LoLTftEntry[0].rank, WinRate(LoLTftEntry[0].wins, LoLTftEntry[0].losses));
                return ($"```{Level}\n{SoloRank}\n{FlexRank}\n{TftRank}```");
            }
        }

        public static async Task<string> ShowCurrent(string query)
        {
            var LoLSummonerModel = await AsyncCalls.LoLSummonerAsync(query);
            if (LoLSummonerModel == null)
            {
                return ("```No such player```");
            }

            var LoLCurrentGame = await AsyncCalls.LoLCurrentGameAsync(LoLSummonerModel.id);
            if (LoLCurrentGame == null)
            {
                return ("```Player not in a game```");
            }
            else
            {
                TimeSpan gametime = TimeSpan.FromSeconds(LoLCurrentGame.gameLength);
                string str = gametime.ToString(@"mm\:ss");
                string Name = string.Format("{0} is in a {1} game", LoLSummonerModel.name, LoLCurrentGame.gameMode);
                string GameTime = string.Format("In game for: {0}", str);
                return ($"```{Name}\n{GameTime}```");
            }
        }

        public static async Task<List<string>> TopChamps(string query)
        {
            string[] querylist = query.Split(' ');
            int nrOfChamps = Int32.Parse(querylist[0]);
            var LoLSummonerModel = await AsyncCalls.LoLSummonerAsync(querylist[1]);
            var lolMasteryModel = await AsyncCalls.LoLMasteryAsync(LoLSummonerModel.id);
            var lolChampionModel = await AsyncCalls.LoLChampionAsync();

            
            lolMasteryModel.OrderBy(o => o.championPoints).ToList();

            List<string> champId= new List<string>();
            List<string> champName = new List<string>();


            for (int i = nrOfChamps-1; i >= 0; i--)
            {
                champId.Add(lolMasteryModel[i].championId.ToString());
            }

            foreach (var champion in lolChampionModel.data.Values)
            {
                if (champId.Contains(champion.key))
                {
                    champName.Add($"Name:{champion.name}");
                }
            }
            return champName;

        }

        public static string OpGG(string query)
        {
            return string.Format("<https://euw.op.gg/summoner/userName={0}>",query);
        }

        public static double WinRate(double Wins, double Losses)
        {
            double result = (Wins / (Wins + Losses)) * 100;
            return Math.Round(result, 1);
        }
    }
}
