namespace ImapCleanup.Configuration
{
    public class ImapConfiguration
    {
        public bool UseSsl { get; set; } = true;
        public int Port { get; set; }
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}