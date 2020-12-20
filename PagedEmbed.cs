using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.BotConstructor;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBotConstructor
{
    /// <summary>
    /// This class can help you to create paginated embed. For example: if you have a lot of commands and you want make "Help" for this, but in regular embed the number of characters is limited so you can use paged embed
    /// </summary>
    public class PagedEmbed
    {
        private readonly Bot Bot;

        /// <summary>
        /// Initilizes new <see cref="PagedEmbed"/>
        /// </summary>
        /// <param name="bot"><see cref="Discord.Addons.BotConstructor.Bot"/></param>
        public PagedEmbed(Bot bot)
        {
            Bot = bot;
        }

        /// <summary>
        /// Send paginated reply.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="paginatedMessage">Paginated reply that you want to send</param>
        /// <returns><see cref="IUserMessage"/></returns>
        public async Task<IUserMessage> SendPaginatedEmbed(IMessage message, PaginatedMessage paginatedMessage) => 
            await Bot.InteractiveService.SendPaginatedMessageAsync(new SocketCommandContext(Bot.Client, message as SocketUserMessage), paginatedMessage);        

        /// <summary>
        /// Send paginated help.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="colorOfEmbed">Color of help</param>
        /// <param name="embedAuthorBuilder">If you want to write author you should initilize new <see cref="EmbedAuthorBuilder"/></param>
        /// <param name="title">Title of help</param>
        /// <returns></returns>
        public async Task<IUserMessage> SendPaginatedHelp(IMessage message, Color colorOfEmbed, EmbedAuthorBuilder embedAuthorBuilder = null, string title = null)
        {
            int pos = 0;
            int posit = 1;
            List<string> pages = new List<string>
            { 
                null
            };

            foreach (var command in Bot.CommandService.Commands)            
                if ((pages[pos] + $"\n{posit + 1}. Command {command.Name} {command.Summary}").Length <= 2048)
                    pages[pos] += $"\n{posit++}. Command {command.Name} {command.Summary}";
                else
                {
                    pages.Add($"\n{posit++}. Command {command.Name} {command.Summary}");
                    pos++;
                }    
            
            return await Bot.InteractiveService.SendPaginatedMessageAsync(new SocketCommandContext(Bot.Client, message as SocketUserMessage), new PaginatedMessage
            { 
                Title = title ?? $"{Bot.Client.CurrentUser.Username} help",
                Pages = pages,
                Color = colorOfEmbed,
                Author = embedAuthorBuilder
            });
        }
    }
}
