using System;
using System.Collections.Generic;
using System.Linq;
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

        [Command("player", RunMode = RunMode.Async)]
        [Summary("Gives details of LoL player")]
        public async Task Player([Remainder]string query)
        {
            await ReplyAsync(AsyncTasks.ShowPlayer(query).Result + AsyncTasks.OpGG(query));
        }

        [Command("top", RunMode = RunMode.Async)]
        [Summary("Gives details of top played champs(by mastery)")]
        public async Task Top([Remainder] string query)
        {
            List<string> result = new List<string>();
            result = AsyncTasks.TopChamps(query).Result;
            result.Reverse();
            string joinedResult = result.Aggregate((a, b) => a + "\n" + b);
            await ReplyAsync($"```{joinedResult} ```");
        }

        [Command("current", RunMode = RunMode.Async)]
        [Summary("Gives details if player is playing")]
        public async Task Current([Remainder] string query)
        {
            await ReplyAsync(AsyncTasks.ShowCurrent(query).Result);
        }

        [Command("help")]
        [Summary("Displays the commands")]
        public async Task Help()
        {
            string commands = $"```?blet\n?player [name]\n?current [name]\n?top [nr] [name]```";
            await ReplyAsync(commands);
        }
    }
}
