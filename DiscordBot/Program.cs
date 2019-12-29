using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient client;
        private CommandService service;
        private CommandHandler handler;
        public async Task MainAsync()
        {
            client = new DiscordSocketClient();
            service = new CommandService();
            handler = new CommandHandler(client, service);
            client.Log += Log;

            var token = "NDM3OTQ2NjU3MjU1MDYzNTUy.XeVOdA.ANI6MdxRVCAe_ebcGasPfcREVyM";


            await handler.InstallCommandsAsync();
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);

        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

    }


}
