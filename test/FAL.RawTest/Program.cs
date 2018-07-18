using System;
using System.Linq;
using Alsein.Utilities;

namespace FAL.RawTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var (players, ground) = FAL.Game.FALGame.Deal();
            players.ForAll(x => Console.WriteLine(x.Select(n => $"{n}").Join(",")));
            Console.WriteLine(ground.Select(n => $"{n}").Join(","));
        }
    }
}
