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
            await ReplyAsync(AsyncTasks.showPlayer(query).Result);
        }

        [Command("current",RunMode = RunMode.Async)]
        [Summary("Gives details if player is playing")]
        public async Task Current([Remainder] string query)
        {
            await ReplyAsync(AsyncTasks.showCurrent(query).Result);
        }
    }
}
