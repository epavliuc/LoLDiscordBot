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
            //if player has no ranked history
            if (LoLLeagueEntry.Count == 0)
            {
                //and no tft ranked history
                if (LoLTftEntry.Count == 0)
                {
                    return $"```{Level}\n With no ranked or tft history```";
                }
                TftRank = string.Format("Tft Rank: {0} {1} {2}% Win Rate", LoLTftEntry[0].tier, LoLTftEntry[0].rank, WinRate(LoLTftEntry[0].wins, LoLTftEntry[0].losses));
                return $"```{Level}\n{TftRank}\nWith no ranked history```";
            }
            //if player only have one ranked queue history
            else if (LoLLeagueEntry.Count == 1)
            {
                //if player has no tft ranked history
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
                    //if player have only one type of ranked history, solo or flex
                    if (LoLLeagueEntry[0].queueType == "RANKED_SOLO_5x5")
                    {
                        SoloRank = string.Format("Solo Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[0].tier, LoLLeagueEntry[0].rank, WinRate(LoLLeagueEntry[0].wins, LoLLeagueEntry[0].losses));
                        TftRank = string.Format("Tft Rank: {0} {1} {2}% Win Rate", LoLTftEntry[0].tier, LoLTftEntry[0].rank, WinRate(LoLTftEntry[0].wins, LoLTftEntry[0].losses));
                        return $"```{Level}\n{SoloRank}\nNo Flex history\n{TftRank}```";
                    }
                    else
                    {
                        FlexRank = string.Format("Solo Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[0].tier, LoLLeagueEntry[0].rank, WinRate(LoLLeagueEntry[0].wins, LoLLeagueEntry[0].losses));
                        TftRank = string.Format("Tft Rank: {0} {1} {2}% Win Rate", LoLTftEntry[0].tier, LoLTftEntry[0].rank, WinRate(LoLTftEntry[0].wins, LoLTftEntry[0].losses));
                        return $"```{Level}\n{FlexRank}\nNo Solo history\n{TftRank}```";
                    }
                }
            }
            else
            //if player has all details present arange them based on queue type.
            {
                if (LoLLeagueEntry[0].queueType != "RANKED_SOLO_5x5")
                {
                    SoloRank = string.Format("Solo Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[1].tier, LoLLeagueEntry[1].rank, WinRate(LoLLeagueEntry[1].wins, LoLLeagueEntry[1].losses));
                    FlexRank = string.Format("Flex Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[0].tier, LoLLeagueEntry[0].rank, WinRate(LoLLeagueEntry[0].wins, LoLLeagueEntry[0].losses));
                }
                else
                {
                    SoloRank = string.Format("Solo Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[0].tier, LoLLeagueEntry[0].rank, WinRate(LoLLeagueEntry[0].wins, LoLLeagueEntry[0].losses));
                    FlexRank = string.Format("Flex Rank: {0} {1} {2}% Win Rate", LoLLeagueEntry[1].tier, LoLLeagueEntry[1].rank, WinRate(LoLLeagueEntry[1].wins, LoLLeagueEntry[1].losses));
                }
               
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
            List<string> endResult = new List<string>();
            //if input number is between 1-10
            if (nrOfChamps >= 1 && nrOfChamps <= 10)
            {
                var LoLSummonerModel = await AsyncCalls.LoLSummonerAsync(querylist[1]);
                if (LoLSummonerModel == null)
                {
                    endResult.Add("No such player");
                    return endResult;
                }
                var lolMasteryModel = await AsyncCalls.LoLMasteryAsync(LoLSummonerModel.id);
                var lolChampionModel = await AsyncCalls.LoLChampionAsync();


                lolMasteryModel.OrderBy(o => o.championPoints).ToList();
                Dictionary<string, string> champId = new Dictionary<string, string>();
                Dictionary<int, string> pointsNames = new Dictionary<int, string>();

                //add champion id and the total mastery points to dictionary
                for (int i = 0; nrOfChamps - 1 >= i; i++)
                {
                    champId.Add(lolMasteryModel[i].championId.ToString(), lolMasteryModel[i].championPoints.ToString());
                }

                //to dictionary add the total mastery points and champion names(instead of ID)
                foreach (var champion in lolChampionModel.data.Values)
                {
                    if (champId.ContainsKey(champion.key))
                    {
                        pointsNames.Add(Int32.Parse(champId[champion.key]), champion.name);
                    }
                }
                //sort dictionary
                SortedDictionary<int, string> sortedEndResult = new SortedDictionary<int, string>(pointsNames);
                
                foreach (var item in sortedEndResult)
                {
                    endResult.Add($"Name:{item.Value} Points:{String.Format("{0:##,#}", item.Key)}");
                }
                return endResult;
            }
            else
            {
                endResult.Add("Max 10 champs plz");
                return endResult;
            }
        }

        public static async Task<string>ShowChampion(string query)
        {
            string input = UppercaseFirst(query);
            var lolChampionModel = await AsyncCalls.LoLChampionAsync();
            Dictionary<string, Champion> championDictionary = lolChampionModel.data;
            var sC = new Champion();
            try
            {
                sC = championDictionary[input];
            }
            catch
            {
                return "Wrong input, sorry";
            }

            string result;

            result = 
                $"Name:{sC.id}\n" +
                $"{UppercaseFirst(sC.title)}\n" +
                $"{sC.tags[0]},{sC.tags[1]}\n" +
                $"Stats\n" +
                $"HP = {sC.stats.hp}";
            
            return result;
        }

        //gives you a functional op.gg link
        public static string OpGG(string query)
        {
            return string.Format("<https://euw.op.gg/summoner/userName={0}>",query);
        }

        //calculate win percentage
        static double WinRate(double Wins, double Losses)
        {
            double result = (Wins / (Wins + Losses)) * 100;
            return Math.Round(result, 1);
        }

        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
