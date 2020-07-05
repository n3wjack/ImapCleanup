namespace ImapCleanup
{
    internal class ImapConfiguration
    {
        public bool UseSsl { get; set; }
        public int Port { get; set; }
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Ass { get; internal set; }
    }
}