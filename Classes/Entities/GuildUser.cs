using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Discord.Addons.BotConstructor.Entities
{
    /// <summary>
    /// Class where you can take user data also you can modify it.
    /// </summary>
    public class GuildUser : IUser
    {
        private readonly IUser _user;

        public SocketGuildUser SocketGuildUser { get => _user as SocketGuildUser; }

        /// <summary>
        /// User's nickname
        /// </summary>
        public string Nickname { get => SocketGuildUser.Nickname; set => SocketGuildUser.ModifyAsync(x => x.Nickname = value).GetAwaiter(); }

        /// <summary>
        /// User's role. Atention! Please see wiki. 
        /// </summary>
        public object Roles { get => SocketGuildUser.Roles; set => SocketGuildUser.ModifyAsync(x => x.Roles = (Optional<IEnumerable<IRole>>)value).GetAwaiter(); }        
        
        /// <summary>
        /// Initilizes new <see cref="GuildUser"/>
        /// </summary>
        /// <param name="user">User</param>
        public GuildUser(IUser user)
        {
            _user = user;
        }

        /// <summary>
        /// Id of user's avatar
        /// </summary>
        public string AvatarId => _user.AvatarId;

        /// <summary>
        /// Discriminator
        /// </summary>
        public string Discriminator => _user.Discriminator;

        /// <summary>
        /// Discriminator value
        /// </summary>
        public ushort DiscriminatorValue => _user.DiscriminatorValue;

        /// <summary>
        /// If it's true the user is bot.
        /// </summary>
        public bool IsBot => _user.IsBot;

        /// <summary>
        /// If it's true the user is webhook.
        /// </summary>
        public bool IsWebhook => _user.IsWebhook;

        /// <summary>
        /// Username
        /// </summary>
        public string Username => _user.Username;

        /// <summary>
        /// Data of registration user on Discord
        /// </summary>
        public DateTimeOffset CreatedAt => _user.CreatedAt;

        /// <summary>
        /// User id
        /// </summary>
        public ulong Id => _user.Id;

        /// <summary>
        /// Mention of user
        /// </summary>
        public string Mention => _user.Mention;

        /// <summary>
        /// User's activity
        /// </summary>
        public IActivity Activity => _user.Activity;

        /// <summary>
        /// User's status
        /// </summary>
        public UserStatus Status => _user.Status;

        /// <summary>
        /// Active Clients
        /// </summary>
        public IImmutableSet<ClientType> ActiveClients => _user.ActiveClients;

        /// <summary>
        /// Gets url of avatar
        /// </summary>
        /// <param name="format">Format of image</param>
        /// <param name="size">Width or height of avatar</param>
        /// <returns></returns>
        public string GetAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128) => _user.GetAvatarUrl();

        /// <summary>
        /// Gets url of avatar
        /// </summary>
        /// <returns></returns>
        public string GetDefaultAvatarUrl() => _user.GetDefaultAvatarUrl();

        /// <summary>
        /// Gets or create Discord channel with user.
        /// </summary>
        /// <param name="options">Options</param>
        /// <returns><see cref="IDMChannel"/></returns>
        public Task<IDMChannel> GetOrCreateDMChannelAsync(RequestOptions options = null) => _user.GetOrCreateDMChannelAsync();        
    }
}
