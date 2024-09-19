
using Microsoft.AspNetCore.SignalR;
using SignalR_group.Helpers;
using System.Collections.Concurrent;

namespace SignalR_group.Hubs
{
    public class MessageHub : Hub
    {
        private static ConcurrentDictionary<string, int> roomUserCounts = new ConcurrentDictionary<string, int>();

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveConnectInfo", "User Connected");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.Others.SendAsync("DisconnectInfo", "User disconnected");
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message + "'s Offer is ", FileHelper.Read());
        }

        public async Task JoinRoom(string room, string user)
        {

            if (roomUserCounts.ContainsKey(room) && roomUserCounts[room] >= 3)
            {
                await Clients.Caller.SendAsync("RoomFull", "Room is full. You cannot join.");
            }
            else
            {
                // Otaga giren useru=in sayini artirir
                roomUserCounts.AddOrUpdate(room, 1, (key, count) => count + 1);

                await Groups.AddToGroupAsync(Context.ConnectionId, room);
                await Clients.OthersInGroup(room).SendAsync("ReceiveJoinInfo", user);
             } 
        }

        //leave room
        public async Task LeaveRoom(string room, string user)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
            await Clients.OthersInGroup(room).SendAsync("ReceiveLeaveInfo", user);
        }




        public async Task SendMessageRoom(string room, string user)
        {
            await Clients.OthersInGroup(room).SendAsync("ReceiveInfoRoom", user, FileHelper.Read(room));
        }

        public async Task SendWinnerMessageRoom(string room, string message)
        {
            await Clients.Group(room).SendAsync("ReceiveWinInfoRoom", message, FileHelper.Read(room));
        }
    }
}
