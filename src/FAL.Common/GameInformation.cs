namespace FAL
{
    public class GameInformation
    {
        public (string, int)[] Players { get; }

        public GameInformation(params (string, int)[] players) => Players = players;
    }
}