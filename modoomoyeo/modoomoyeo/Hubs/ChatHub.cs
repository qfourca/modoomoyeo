using Microsoft.AspNetCore.SignalR;
using modoomoyeo.Database;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public Task JoinRoom(int roomId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        }

        public Task LeaveRoom(int roomId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }
        public async Task SendMessage(int userid, int targetid, string message)
        {
            Console.Write(userid); Console.WriteLine(targetid);
            ChatingQurey? db = Context.GetHttpContext().RequestServices.GetService(typeof(ChatingQurey)) as ChatingQurey;
            int permission = db.findPermission(userid, targetid);
            Console.WriteLine(permission);
            if (permission == -1)
            {
                db.insertPermission(userid, targetid);
                permission = db.findPermission(userid, targetid);
            }
            if (permission != 0)
                await JoinRoom(permission);
            if (message == "ALL")
            {
                List <Chatinglog> chatinglogs = db.findLog(DateTime.MinValue ,DateTime.Now, permission);
                foreach (Chatinglog chatinglog in chatinglogs)
                    await Clients.Caller.SendAsync("logMessage", chatinglog.ownerid, chatinglog.contents, chatinglog.time.ToString());
            }
            else
            {
                Chatinglog chatinglog = new Chatinglog(userid, message, permission);
                db.insertLog(chatinglog);
                if(permission == 0)
                    await Clients.Others.SendAsync("ReceiveMessage", userid, message);  
                else
                    await Clients.OthersInGroup(permission.ToString()).SendAsync("ReceiveMessage", userid, message);
            }
        }
    }
}