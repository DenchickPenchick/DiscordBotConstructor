using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Discord.Commands;

namespace Discord.Addons.BotConstructor
{
    /// <summary>
    /// Main class. There you can edit things like bot name. Also you can run bot and add commands.
    /// </summary>
    public class Bot
    {
        /// <summary>
        /// Bot token. You can't run bot without it.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Discord Client
        /// </summary>
        public DiscordSocketClient Client { get; private set; }
        /// <summary>
        /// Get bot as <see cref="SocketUser"/>.
        /// </summary>
        public SocketUser GetBotAsUser { get => Client.CurrentUser; }
        /// <summary>
        /// Interactive service apply you to conduct dialogue (or something else) with user.
        /// </summary>
        public InteractiveService InteractiveService { get; private set; }
        /// <summary>
        /// This propery set time span of timeout when someone conducts dialogue with bot. If user won't answer question (or something else) span bot stop dialogue.
        /// </summary>
        public TimeSpan TimeSpanForInteractivity { get; set; }
        /// <summary>
        /// Commands service. If you want make your own commands you should use this method <see cref="InitilizeNewCommands(Type)"/>
        /// </summary>
        public CommandService CommandService { get; set; }
        /// <summary>
        /// Services. You can use to excute commands.
        /// </summary>
        public IServiceProvider Services { get; private set; }

        /// <summary>
        /// Initilizes new <see cref="Bot"/>
        /// </summary>
        /// <param name="token">There you should enter the bot token. You can get it on this website: https://discord.com/developers/applications/. You can enter it later, but you can't run bot without token. If token equals null <see cref="NoTockenException"/> will throw.</param>
        public Bot(string token = null) 
        {
            Token = token;           
        }

        /// <summary>
        /// Run bot. If <see cref="Bot.Token"/> equals null <see cref="NoTockenException"/> will throw.
        /// </summary>
        /// <returns><see cref="Task.CompletedTask"/></returns>
        public async Task RunBotAsync()
        {
            if (Token == null)
                throw new NoTockenException();

            Client = new DiscordSocketClient();
            InteractiveService = new InteractiveService(Client, new InteractiveServiceConfig
            {
                DefaultTimeout = TimeSpanForInteractivity
            });
            Services = new ServiceCollection().
                AddSingleton(Client)
                .AddSingleton(InteractiveService)
                .BuildServiceProvider();

            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();
            await Task.Delay(-1);
        }

        /// <summary>
        /// Initilize new commands by <see cref="Type"/>. Use typeof() to enter type.
        /// </summary>
        /// <param name="type">Enter type there by typeof()</param>
        /// <returns></returns>
        public async Task InitilizeNewCommands(Type type)
        {
            await CommandService.AddModuleAsync(type, Services);
        }

        /// <summary>
        /// This exception will throw if <see cref="Bot.Token"/> equals null.
        /// </summary>
        public class NoTockenException : Exception
        {
            public NoTockenException() : base("Token was null.") { }
        }
    }
}
