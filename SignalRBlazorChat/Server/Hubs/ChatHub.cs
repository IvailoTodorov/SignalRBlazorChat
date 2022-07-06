namespace SignalRBlazorChat.Server.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System;
    using System.Threading.Tasks;

    public class ChatHub : Hub
    {
        private static Dictionary<string, string> Users = new Dictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext().Request.Query["username"];
            Users.Add(Context.ConnectionId, username);
            await SendMessage(string.Empty, $"{username} connected!");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var username = Users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            await SendMessage(string.Empty, $"{username} left!");
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("GotchaMessage", user, message);
        }
    }
}
