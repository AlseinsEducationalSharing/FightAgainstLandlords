using System.Threading.Tasks;
using Alsein.Utilities.LifetimeAnnotations;

namespace FAL.Client.Services
{
    [Transient]
    public class FALClientService
    {
        private LocalPlayer _player;

        public async Task Start(LocalPlayer player)
        {
            _player = player;
            var op1 = await _player.ReceiveAsync();
            var op2 = await _player.ReceiveAsync();
            var op3 = await _player.ReceiveAsync();
        }
    }
}