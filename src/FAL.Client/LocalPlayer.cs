using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace FAL.Client
{
    public class LocalPlayer : Player
    {
        private HubConnection _hubConnection;

        public LocalPlayer(HubConnection hubConnection)
        {
            _hubConnection = hubConnection;
        }
    }
}