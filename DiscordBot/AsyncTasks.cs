using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class AsyncTasks
    {
        public static async Task<string> showPlayer(string query)
        {
            var LoLSummonerModel = await AsyncCalls.LoLSummonerAsync(query);
            var LoLLeagueEntry = await AsyncCalls.LoLSummonerEntryAsync(LoLSummonerModel.id);
            var LoLTftEntry = await AsyncCalls.LoLTftEntryAsync(LoLSummonerModel.id);

            string Level = string.Format("Level: {0}", LoLSummonerModel.summonerLevel);

            if (LoLSummonerModel == null)
            {
                return("```No such player```");
            }
            else if (LoLLeagueEntry.Count == 0)
            {
                return($"```{Level} with no ranked history```");
            }
            else
            {
                string SoloRank = string.Format("Solo Rank: {0} {1}", LoLLeagueEntry[1].tier, LoLLeagueEntry[1].rank);
                string FlexRank = string.Format("Flex Rank: {0} {1}", LoLLeagueEntry[0].tier, LoLLeagueEntry[0].rank);
                string TftRank = string.Format("Tft Rank: {0} {1}", LoLTftEntry[0].tier, LoLTftEntry[0].rank);

                return($"```{Level}\n{SoloRank}\n{FlexRank}\n{TftRank}```");
            }
        }

        public static async Task<string> showCurrent(string query)
        {
            var LoLSummonerModel = await AsyncCalls.LoLSummonerAsync(query);
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
    }
}
