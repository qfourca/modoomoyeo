using Microsoft.AspNetCore.SignalR;
using modoomoyeo.Database;

namespace SignalRChat.Hubs
{
    public class CanvasHub : Hub
    {
        public Task JoinRoom(int roomId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        }

        public Task LeaveRoom(int roomId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }
        public async Task SendMessage(string type, int width, int x, int y)
        {
            await Clients.Others.SendAsync("ReceiveMessage", type, width, x, y);
            Console.Write(x); Console.Write(' '); Console.WriteLine(y);
        }
    }
}