using System;

namespace ImapCleanup
{
    class Program
    {
        /// <summary>
        /// IMAP cleanup tool. Will delete emails in the inbox, only keeping a given number of emails.
        /// </summary>
        /// <param name="hostname">Host name of the IMAP server (required)</param>
        /// <param name="port">Port number (required)</param>
        /// <param name="username">Username for the IMAP account (required)</param>
        /// <param name="password">Password for the IMAP account (required)</param>
        /// <param name="count">Number of emails to keep.</param>
        static void Main(string hostname, int? port, string username, string password, int count = 1000)
        {
            if (string.IsNullOrEmpty(hostname) || port == null || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Invalid command line arguments. Use -? -h or --help for more info.\n");
                return;
            }

            new CleanupEmailsCommand(new ImapConfiguration
            {
                Hostname = hostname,
                Port = port.Value,
                UseSsl = true,
                Username = username,
                Password = password
            }).Run(count);
        }
    }
}
