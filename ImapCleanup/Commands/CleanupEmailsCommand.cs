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
        /// <param name="batchSize">Batch size to use for fetched emails.</param>
        internal void Run(int numberOfMessagesToKeep, int batchSize)
        {
            LoginAndRunAction((imapClient) => RemoveEmails(imapClient, numberOfMessagesToKeep, batchSize));
        }

        private void RemoveEmails(ImapClient client, int numberOfMessagesToKeep, int batchSize)
        {
            Console.WriteLine("Getting messages...");
            client.Inbox.Open(FolderAccess.ReadWrite);

            Console.WriteLine($"Found {client.Inbox.Count} messages.");
            var numberToDelete = client.Inbox.Count - numberOfMessagesToKeep;
            
            var startIndex = 0;

            if (numberToDelete > 0)
            {
                do {
                    var batchToDelete = numberToDelete > batchSize ? batchSize : numberToDelete;
                    var endIndex = startIndex + batchToDelete - 1;

                    Console.WriteLine($"Processing emails from index {startIndex} to {endIndex}");

                    // Messages are fetched from oldest to newest.
                    var messages = client.Inbox.Fetch(startIndex, endIndex, MessageSummaryItems.All);

                    foreach (var message in messages)
                    {
                        Console.WriteLine($"Marking message for deletion: [{message.Index}] {message.Date} - {message.NormalizedSubject}");

                        if (!WhatIf)
                        {
                            client.Inbox.AddFlags(message.Index, MessageFlags.Deleted, true);
                        }
                    }

                    numberToDelete -= batchToDelete;
                    startIndex = endIndex + 1;
                } while (numberToDelete > 0);

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
