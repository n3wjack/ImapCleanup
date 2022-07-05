using ImapCleanup.Configuration;
using MailKit.Net.Imap;
using System;

namespace ImapCleanup.Commands
{
    internal class BaseEmailCleanupCommand
    {
        private ImapConfiguration _config;
        protected bool WhatIf { get; }

        internal BaseEmailCleanupCommand(ImapConfiguration config, bool whatIf)
        {
            _config = config;
            WhatIf = whatIf;
        }

        /// <summary>
        /// Log in to the IMAP server and perform the given action. 
        /// </summary>
        /// <param name="action">Action to perform.</param>
        protected void LoginAndRunAction(Action<ImapClient> action)
        {
            Console.Write("Connecting to IMAP server... ");
            using (var client = new ImapClient())
            {
                client.Connect(_config.Hostname, _config.Port, _config.UseSsl);
                Console.WriteLine("Connected!");
                Console.Write("Logging in... ");
                client.Authenticate(_config.Username, _config.Password);
                Console.WriteLine("Logged in!");

                action(client);

                Console.Write("Logging out... ");
                client.Disconnect(true);
                Console.WriteLine("Logged out!");
            }
        }
    }
}
