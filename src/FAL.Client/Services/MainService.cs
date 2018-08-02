using System;
using System.Threading.Tasks;
using Alsein.Utilities.LifetimeAnnotations;
using Autofac;
using Microsoft.AspNetCore.SignalR.Client;

namespace FAL.Client.Services
{
    [Singleton]
    public class MainService
    {
        public HubConnection HubConnection { get; set; }

        public IContainer Container { get; set; }

        private Resulter StopResulter { get; } = new Resulter();

        private string _nickName;

        public async Task Main(string[] args)
        {
            await HubConnection.StartAsync();
            var (playerCount, roomCount) = await HubConnection.InvokeAsync<(int, int)>("Hello");
            Console.WriteLine($"Welcome to FAL game, there are {playerCount} players online and {roomCount} rooms open.");
            Console.Write("Input your nickname: ");
            login:
            if (!await HubConnection.InvokeAsync<bool>("Login", _nickName = Console.ReadLine()))
            {
                Console.WriteLine($"The name \"{_nickName}\" has already been taken, please try another:");
                goto login;
            }
            Console.WriteLine("Login succeed!");
            Console.Write("Input a room number to join: ");
            join:
            if (int.TryParse(Console.ReadLine(), out var num))
            {
                if (!await HubConnection.InvokeAsync<bool>("Join", num))
                {
                    Console.WriteLine($"The room {num} is full! Try again:");
                    goto join;
                }
            }
            else
            {
                Console.WriteLine("The room number must be a numeral input! Try again:");
                goto join;
            }
            var player = new LocalPlayer(HubConnection);

            HubConnection.On<Operation<ServerOperationType>>("GameOperation", x => player.SendAsync(x));
            player.Receive += async obj => await HubConnection.SendAsync("GameOperation", obj.Result);

            var client = Container.Resolve<FALClientService>();
            await client.Start(player);
        }
    }
}