namespace FAL.Server
{
    public class UserInfo
    {
        public string Name { get; set; }

        public string ConnectionId { get; set; }

        public int Score { get; set; }

        public ClientPlayer CurrentPlayer { get; set; }
    }
}