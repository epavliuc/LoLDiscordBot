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
            
            
            var LoLSummonerModel = await Methods.LoLSummonerAsync(query);
            var LoLLeagueEntry = await Methods.LoLSummonerEntryAsync(LoLSummonerModel.id);
            
            Console.WriteLine("testing");
            string Level = string.Format("Level: {0}", LoLSummonerModel.summonerLevel);
            string SoloRank = string.Format("Solo Rank: {0} {1}", LoLLeagueEntry[1].rank, LoLLeagueEntry[1].tier);
            Console.WriteLine("testing2");
            await ReplyAsync("\n"+Level+"\n"+SoloRank);
        }
    }




}
