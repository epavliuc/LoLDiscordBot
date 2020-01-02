using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class AsyncTasks
    {
        public static async Task<string> ShowPlayer(string query)
        {
            var LoLSummonerModel = await AsyncCalls.LoLSummonerAsync(query);
            if (LoLSummonerModel == null)
            {
                return("```No such player```");
            }

            string Level = string.Format("Level: {0}", LoLSummonerModel.summonerLevel);
            var LoLLeagueEntry = await AsyncCalls.LoLSummonerEntryAsync(LoLSummonerModel.id);
            var LoLTftEntry = await AsyncCalls.LoLTftEntryAsync(LoLSummonerModel.id);

            if (LoLLeagueEntry.Count == 0)
            {
                if (LoLTftEntry.Count == 0)
                {
                    return $"```{Level}\n With no ranked or tft history```";
                }
                string TftRank = string.Format("Tft Rank: {0} {1} {2}% Win Rate", LoLTftEntry[0].tier, LoLTftEntry[0].rank, WinRate(LoLTftEntry[0].wins, LoLTftEntry[0].losses));
                return $"```{Level}\n{TftRank}\nWith no ranked history```";
            }else if(LoLLeagueEntry.Count == 1)
            {
                if (LoLTftEntry.Count == 0)
                {
                    if (LoLLeagueEntry[0].queueType == "RANKED_SOLO_5x5")
                    {
                        string SoloRank = string.Format("Solo Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[0].tier, LoLLeagueEntry[0].rank, WinRate(LoLLeagueEntry[0].wins, LoLLeagueEntry[0].losses));
                        return $"```{Level}\n{SoloRank}\nNo Flex history\nNo Tft history```";
                    }
                    else if(LoLLeagueEntry[0].queueType != "RANKED_SOLO_5x5")
                    {
                        string FlexRank = string.Format("Solo Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[0].tier, LoLLeagueEntry[0].rank, WinRate(LoLLeagueEntry[0].wins, LoLLeagueEntry[0].losses));
                        return $"```{Level}\n{FlexRank}\nNo Solo history\nNo Tft history```";
                    }
                    return $"```{Level}\n With no ranked or tft history```";
                }
                return "";
            }
            else
            {
                string SoloRank = string.Format("Solo Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[1].tier, LoLLeagueEntry[1].rank, WinRate(LoLLeagueEntry[1].wins, LoLLeagueEntry[1].losses));
                string FlexRank = string.Format("Flex Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[0].tier, LoLLeagueEntry[0].rank, WinRate(LoLLeagueEntry[0].wins, LoLLeagueEntry[0].losses));  
                if (LoLTftEntry.Count == 0)
                {
                    return $"```{Level}\n{SoloRank}\n{FlexRank}\nWith no tft history```";
                }
                string TftRank = string.Format("Tft Rank: {0} {1} {2}% Win Rate", LoLTftEntry[0].tier, LoLTftEntry[0].rank, WinRate(LoLTftEntry[0].wins, LoLTftEntry[0].losses));
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
                return("```Player not in a game```");
            }
            else
            {
                TimeSpan gametime = TimeSpan.FromSeconds(LoLCurrentGame.gameLength);
                string str = gametime.ToString(@"mm\:ss");
                string Name = string.Format("{0} is in a {1} game", LoLSummonerModel.name, LoLCurrentGame.gameMode);
                string GameTime = string.Format("In game for: {0}", str);
                return($"```{Name}\n{GameTime}```");
            }
        }

        public static double WinRate(double Wins, double Losses)
        {
            double result = (Wins / (Wins + Losses))*100;
            return  Math.Round(result,1);
        }
    }
}
