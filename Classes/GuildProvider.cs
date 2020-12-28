using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.Addons.BotConstructor
{
    /// <summary>
    /// This class helps you to find some things by name, id and ect.
    /// </summary>
    public class GuildsProvider
    {
        /// <summary>
        /// Initilizes new <see cref="GuildsProvider"/>
        /// </summary>
        public GuildsProvider() { }        

        /// <summary>
        /// Searches users by nickname in guild
        /// </summary>
        /// <param name="guild">Guild where you want find users</param>
        /// <param name="nickname">Nickname</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        public IEnumerable<SocketGuildUser> SearchUsersByNickname(IGuild guild ,string nickname)
        {
            var socketGuild = guild as SocketGuild;
            foreach (var user in socketGuild.Users)
                if (user.Nickname == nickname)
                    yield return user;
        }

        /// <summary>
        /// Searches user by username in guild
        /// </summary>
        /// <param name="guild">Guild where you want find user</param>
        /// <param name="username">Username</param>
        /// <returns><see cref="SocketGuildUser"/></returns>
        public SocketGuildUser SearchUserByUsername(IGuild guild, string username)
        {
            var socketGuild = guild as SocketGuild;
            foreach (var user in socketGuild.Users)
                if (user.Username == username)
                    return user;
            return null;
        }

        /// <summary>
        /// Searches user by id
        /// </summary>
        /// <param name="guild">Guild where you want find user</param>
        /// <param name="id">ID</param>
        /// <returns><see cref="SocketGuildUser"/></returns>
        public async Task<SocketGuildUser> SearchUserByIdAsync(IGuild guild, ulong id) =>  await guild.GetUserAsync(id) as SocketGuildUser;

        /// <summary>
        /// Searches Channel by name.
        /// </summary>
        /// <param name="guild"><see cref="IGuild"/></param>
        /// <param name="name">Name</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        public IEnumerable<SocketGuildChannel> GetChannelByName(IGuild guild, string name)
        {
            var SocketGuild = guild as SocketGuild;
            var channels = SocketGuild.Channels;

            foreach (var channel in channels)
                if (channel.Name == name)
                    yield return channel;
        }

        /// <summary>
        /// Searches channel by ID
        /// </summary>
        /// <param name="guild"><see cref="IGuild"/></param>
        /// <param name="id">ID</param>
        /// <returns><see cref="SocketGuildUser"/></returns>
        public async Task<SocketGuildChannel> GetChannelById(IGuild guild, ulong id) => await guild.GetChannelAsync(id) as SocketGuildChannel;

        /// <summary>
        /// Gets channel by type like <see cref="SocketTextChannel"/>
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="guild"><see cref="IGuild"/></param>
        /// <param name="name">Name</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        public IEnumerable<SocketGuildChannel> GetChannelByName<T>(IGuild guild, string name)
        {
            foreach (var channel in GetChannelByName(guild, name))            
                if (channel is T)
                    yield return channel;            
        }
    }
}
