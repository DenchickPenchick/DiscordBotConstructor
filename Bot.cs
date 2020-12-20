using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;
using DiscordBotConstructor;

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
        public DiscordSocketClient Client { get; private set; } = new DiscordSocketClient();
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
        /// Initilizes new <see cref="Rooms"/>
        /// </summary>
        public Rooms Rooms { get => new Rooms(this); }

        /// <summary>
        /// Initilizes new <see cref="PagedEmbed"/>
        /// </summary>
        public PagedEmbed PagedEmbed { get => new PagedEmbed(this); }

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
        /// Set bot status
        /// </summary>
        /// <returns><see cref="Task.CompletedTask"/></returns>
        public async Task SetStatusAsync(UserStatus userStatus)
        {            
            await Client.SetStatusAsync(userStatus);
        }

        /// <summary>
        /// Set bot game
        /// </summary>
        /// <param name="name">Name of game</param>
        /// <param name="streamUrl">Stream url</param>
        /// <returns><see cref="Task.CompletedTask"/></returns>
        public async Task SetGameAsync(string name, string streamUrl = null)
        {
            await Client.SetGameAsync(name, streamUrl);
        }

        /// <summary>
        /// This exception will throw if <see cref="Bot.Token"/> equals null.
        /// </summary>
        public class NoTockenException : Exception
        {
            /// <summary>
            /// Initalize new exception
            /// </summary>
            public NoTockenException() : base("Token was null.") { }
        }
    }
}
