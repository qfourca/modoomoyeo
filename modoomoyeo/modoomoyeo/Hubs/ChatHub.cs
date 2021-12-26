using Microsoft.AspNetCore.SignalR;
using modoomoyeo.Database;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
    public async Task SendMessage(string user, string message)
        {
            ChatingQurey db = Context.GetHttpContext().RequestServices.GetService(typeof(ChatingQurey)) as ChatingQurey;
            if (user == "logrequest")
            {
                Console.WriteLine(DateTime.MinValue.ToString());
                List <Chatinglog> chatinglogs= db.findLog(DateTime.MinValue ,DateTime.Now, 0);
                foreach (Chatinglog chatinglog in chatinglogs)
                    Console.WriteLine(chatinglog.contents);
            }
            else
            {
           
                Chatinglog chatinglog = new Chatinglog(1, message, 0);
                db.insertLog(chatinglog);
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
        }
    }
}
