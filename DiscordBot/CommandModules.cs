using System;
using System.Threading.Tasks;
using Discord.Commands;
using LoLApiDLL.DataModels;

namespace DiscordBot
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("blet")]
        [Summary("Echoes a message.")]
        public async Task Blet()
        {

            await ReplyAsync("idi nahui");
        }


        [Command("player",RunMode = RunMode.Async)]
        [Summary("Gives details of LoL player")]
        public async Task Player([Remainder]string query)
        {         
            var LoLSummonerModel = await AsyncCalls.LoLSummonerAsync(query);
            var LoLLeagueEntry = await AsyncCalls.LoLSummonerEntryAsync(LoLSummonerModel.id);
            var LoLTftEntry = await AsyncCalls.LoLTftEntryAsync(LoLSummonerModel.id);

            string Level = string.Format("Level: {0}", LoLSummonerModel.summonerLevel); 

            if (LoLSummonerModel == null)
            {
                await ReplyAsync("```No such player```");
            }else if(LoLLeagueEntry.Count==0)
            {
                await ReplyAsync($"```{Level} with no ranked history```");
            }
            else
            {
                string SoloRank = string.Format("Solo Rank: {0} {1}", LoLLeagueEntry[1].tier, LoLLeagueEntry[1].rank);
                string FlexRank = string.Format("Flex Rank: {0} {1}", LoLLeagueEntry[0].tier, LoLLeagueEntry[0].rank);
                string TftRank = string.Format("Tft Rank: {0} {1}", LoLTftEntry[0].tier, LoLTftEntry[0].rank);

                await ReplyAsync($"```{Level}\n{SoloRank}\n{FlexRank}\n{TftRank}```");
            }
        }

        [Command("current",RunMode = RunMode.Async)]
        [Summary("Gives details if player is playing")]
        public async Task Current([Remainder] string query)
        {
            var LoLSummonerModel = await AsyncCalls.LoLSummonerAsync(query);
            var LoLCurrentGame = await AsyncCalls.LoLCurrentGameAsync(LoLSummonerModel.id);
         
            if (LoLCurrentGame == null)
            {
                await ReplyAsync("```Player not in a game```");
            }
            else
            {
                TimeSpan gametime = TimeSpan.FromSeconds(LoLCurrentGame.gameLength);
                string str = gametime.ToString(@"mm\:ss");
                string Name = string.Format("{0} is in a {1} game", LoLSummonerModel.name, LoLCurrentGame.gameMode);
                string GameTime = string.Format("In game for: {0}", str);
                await ReplyAsync($"```{Name}\n{GameTime}```");
            }
        }
    }




}
