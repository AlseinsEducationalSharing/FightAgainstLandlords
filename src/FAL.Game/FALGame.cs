using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alsein.Utilities;

namespace FAL.Game
{
    public class FALGame
    {
        private Player[] _players;

        private int _landlordIndex;

        private int _currentPlayer;

        public FALGame(Player[] players)
        {
            _players = players;
        }

        public async Task<(int, int, int)> Play()
        {
            var (playerCards, groundCards) = Deal();
            for (var i = 0; i < 3; i++)
            {
                await _players[i].ServerOperation(ServerOperationType.GameStart);
                await _players[i].ServerOperation(ServerOperationType.GameInformation, new GameInformation(_players.Select(p => (p.Name, p.Score)).ToArray()));
                _players[i].Cards = new List<int>(playerCards[i]);
                await _players[i].ServerOperation(ServerOperationType.UpdateCards, _players[i].Cards);
            }
            return default;
        }

        public static (int[][], int[]) Deal()
        {
            var cards = Mess(0.To(12).Plural(4).SelectMany(x => x).Concat(new[] { 13, 14 })).ToArray();
            var result = (players: new int[3][], ground: new int[3]);
            for (var i = 0; i < 3; i++)
            {
                result.players[i] = cards.Skip(i * 17).Take(17).OrderBy(x => x).ToArray();
            }
            result.ground = cards.Skip(51).OrderBy(x => x).ToArray();
            return result;
        }

        private static IEnumerable<T> Mess<T>(IEnumerable<T> source)
        {
            var arr = source.ToArray();
            var pickeds = false.Plural(arr.Length).ToArray();
            var num = 0;
            var rand = new Random();
            while (num < arr.Length)
            {
                var r = rand.Next(arr.Length - num);
                var index = r;
                for (var i = 0; i <= index; i++)
                {
                    while (pickeds[i])
                    {
                        index++;
                        i++;
                    }
                }
                pickeds[index] = true;
                yield return arr[index];
                num++;
            }
        }
    }
}