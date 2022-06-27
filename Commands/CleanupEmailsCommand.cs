using ImapCleanup.Configuration;
using MailKit;
using MailKit.Net.Imap;
using System;

namespace ImapCleanup.Commands
{
    internal class CleanupEmailsCommand : BaseEmailCleanupCommand
    {
        internal CleanupEmailsCommand(ImapConfiguration config, bool whatIf)
            : base(config, whatIf)
        { 
        }

        /// <summary>
        /// Run the cleanup emails command.
        /// </summary>
        /// <param name="numberOfMessagesToKeep">Number of messages to keep.</param>
        internal void Run(int numberOfMessagesToKeep)
        {
            LoginAndRunAction((imapClient) => RemoveEmails(imapClient, numberOfMessagesToKeep));
        }

        private void RemoveEmails(ImapClient client, int numberOfMessagesToKeep)
        {
            Console.WriteLine("Getting messages...");
            client.Inbox.Open(FolderAccess.ReadWrite);

            Console.WriteLine($"Found {client.Inbox.Count} messages.");
            var numberToDelete = client.Inbox.Count - numberOfMessagesToKeep;

            if (numberToDelete > 0)
            {
                // Messages are fetch from oldest to newest.
                var messages = client.Inbox.Fetch(0, numberToDelete - 1, MessageSummaryItems.All);

                foreach (var message in messages)
                {
                    Console.WriteLine($"Marking message for deletion: [{message.Index}] {message.Date} - {message.NormalizedSubject}");

                    if (!WhatIf)
                    {
                        client.Inbox.AddFlags(message.Index, MessageFlags.Deleted, true);
                    }
                }

                Console.Write("Deleting messages from folder...");

                if (!WhatIf)
                {
                    client.Inbox.Expunge();
                }

                Console.WriteLine("All gone.");
            }
            else
            {
                Console.WriteLine("Nothing to delete.");
            }
        }
    }
}
