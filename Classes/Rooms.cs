using Discord.Addons.BotConstructor;
using Discord.WebSocket;
using System.Collections;
using System.Threading.Tasks;

namespace Discord.Addons.BotConstructor
{
    /// <summary>
    /// This module can help you create rooms system. For example: on Discord server a lot of voice channels and it terrifired. You can create and delete rooms dynamically!
    /// </summary>
    public class Rooms
    {
        /// <summary>
        /// Hashtable of guilds. Where guild (<see cref="SocketGuild"/>) is key and CreateRoomChannel is value (<see cref="SocketVoiceChannel"/>).
        /// </summary>
        public Hashtable GuildsCreateRoomsChannel { get; set; } = new Hashtable();

        private DiscordSocketClient Client { get; }

        /// <summary>
        /// Initilizes new <see cref="Rooms"/>
        /// </summary>
        /// <param name="bot"><see cref="Bot"/></param>
        public Rooms(Bot bot)
        {
            Client = bot.Client;
            Client.UserVoiceStateUpdated += Client_UserVoiceStateUpdated;

            foreach (var guild in Client.Guilds)            
                GuildsCreateRoomsChannel.Add(guild, null);            
        }

        private async Task Client_UserVoiceStateUpdated(SocketUser arg1, SocketVoiceState arg2, SocketVoiceState arg3)
        {
            var socketGuildUser = arg1 as SocketGuildUser;
            if (GuildsCreateRoomsChannel[socketGuildUser.Guild] != null)
            {
                SocketCategoryChannel roomsCategory = socketGuildUser.Guild.GetCategoryChannel((ulong)((SocketVoiceChannel)GuildsCreateRoomsChannel[socketGuildUser.Guild]).CategoryId);
                var createRoomChannel = (SocketVoiceChannel)GuildsCreateRoomsChannel[socketGuildUser.Guild];
                var channel = arg3.VoiceChannel;
                var prevchannel = arg2.VoiceChannel;
                if (channel != null && prevchannel != null && channel != prevchannel)
                {
                    if (channel == createRoomChannel)
                    {                                                
                        if (prevchannel.Name.Contains(socketGuildUser.Nickname) || prevchannel.Name.Contains(socketGuildUser.Username))
                            await socketGuildUser.ModifyAsync(x => x.Channel = prevchannel);
                        else if (prevchannel.Users.Count == 0 && prevchannel.Category == roomsCategory)
                            await prevchannel.DeleteAsync();
                        else
                            await CreateRoom(socketGuildUser);                        
                    }
                    else if (prevchannel != createRoomChannel && prevchannel.Category == roomsCategory && prevchannel.Users.Count == 0)
                        await prevchannel.DeleteAsync();
                }
                else if (channel != null)//User connected
                {
                    if (channel == createRoomChannel)
                        await CreateRoom(socketGuildUser);
                }
                else if (prevchannel != null)//User disconnected
                    if (prevchannel.Users.Count == 0 && prevchannel.Category == roomsCategory && prevchannel.Name != createRoomChannel.Name)
                        await prevchannel.DeleteAsync();
            }
        }

        private async Task<SocketVoiceChannel> CreateRoom(SocketGuildUser user)
        {
            bool haveChannel = false;
            SocketCategoryChannel roomsCategory = user.Guild.GetCategoryChannel((ulong)((SocketVoiceChannel)GuildsCreateRoomsChannel[user.Guild]).CategoryId);
            SocketVoiceChannel socketVoiceChannel = null;
            var guild = user.Guild;
            var voiceChannels = guild.VoiceChannels;            

            foreach (var oneofchannel in voiceChannels)
                if (user.Nickname != null)
                {
                    if (oneofchannel.Name.Contains(user.Nickname)) { socketVoiceChannel = oneofchannel; haveChannel = true; }
                    continue;
                }
                else
                {
                    if (oneofchannel.Name.Contains(user.Username)) { socketVoiceChannel = oneofchannel; haveChannel = true; }
                    continue;
                }

            if (!haveChannel)
            {
                var newChannel = await guild.CreateVoiceChannelAsync($"{user.Nickname ?? user.Username}'s room", x => x.CategoryId = roomsCategory.Id);
                await user.ModifyAsync(x =>
                {
                    x.Channel = newChannel;
                });
                return guild.GetVoiceChannel(newChannel.Id);
            }
            else
            {
                await user.ModifyAsync(x => { x.Channel = socketVoiceChannel; });
                return socketVoiceChannel;
            }
        }
    }
}
