using MailKit.Net.Imap;
using System;

namespace ImapCleanup
{
    internal class CleanupEmailsCommand
    {
        private ImapConfiguration _config;

        internal CleanupEmailsCommand(ImapConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Run the cleanup emails command.
        /// </summary>
        /// <param name="numberOfMessagesToKeep">Number of messages to keep.</param>
        internal void Run(int numberOfMessagesToKeep)
        {
            Console.Write("Connecting to IMAP server... ");
            using (var client = new ImapClient())
            {
                client.Connect(_config.Hostname, _config.Port, _config.UseSsl);
                Console.WriteLine("Connected!");
                Console.Write("Logging in... ");
                client.Authenticate(_config.Username, _config.Password);
                Console.WriteLine("Logged in!");

                RemoveEmails(client, numberOfMessagesToKeep);

                Console.Write("Logging out... ");
                client.Disconnect(true);
                Console.WriteLine("Logged out!");
            }
        }

        private void RemoveEmails(ImapClient client, int numberOfMessagesToKeep)
        {
            Console.WriteLine("Getting messages...");
            client.Inbox.Open(MailKit.FolderAccess.ReadWrite);

            Console.WriteLine($"Found {client.Inbox.Count} messages.");
            var numberToDelete = client.Inbox.Count - numberOfMessagesToKeep;

            if (numberToDelete > 0)
            {                
                // Messages are fetch from oldest to newest.
                var messages = client.Inbox.Fetch(0, numberToDelete - 1, MailKit.MessageSummaryItems.All);

                foreach (var message in messages)
                {
                    Console.WriteLine($"Marking message for deletion: [{message.Index}] {message.Date} - {message.NormalizedSubject}");
                    client.Inbox.AddFlags(message.Index, MailKit.MessageFlags.Deleted, true);
                }

                Console.Write("Deleting messages from folder...");
                client.Inbox.Expunge();
                Console.WriteLine("All gone.");
            }
            else
            {
                Console.WriteLine("Nothing to delete.");
            }
        }
    }
}
