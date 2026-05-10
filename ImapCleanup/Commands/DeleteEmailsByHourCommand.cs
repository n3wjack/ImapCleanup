using ImapCleanup.Configuration;
using ImapCleanup.Extensions;
using MailKit;
using MailKit.Net.Imap;
using System;

namespace ImapCleanup.Commands
{
    internal class DeleteEmailsByHourCommand : BaseEmailCleanupCommand
    {
        public DeleteEmailsByHourCommand(ImapConfiguration configuration, bool whatIf)
            : base(configuration, whatIf)
        {
        }

        internal void Run(TimeSpan fromHour, TimeSpan toHour, int batchSize)
        {
            LoginAndRunAction((imapClient) => RemoveEmails(imapClient, fromHour, toHour, batchSize));
        }

        private void RemoveEmails(ImapClient client, TimeSpan fromHour, TimeSpan toHour, int batchSize)
        {
            Console.WriteLine("Getting messages...");

            client.Inbox.Open(FolderAccess.ReadWrite);

            var emailCount = client.Inbox.Count;
            var startIndex = 0;

            Console.WriteLine($"Number of emails: {emailCount}");

            do
            {
                var endIndex = startIndex + batchSize - 1;
                endIndex = endIndex > emailCount - 1 ? emailCount - 1 : endIndex;

                Console.WriteLine($"Processing emails from index {startIndex} to {endIndex}");

                var messages = client.Inbox.Fetch(startIndex, endIndex, MessageSummaryItems.All);

                foreach (var message in messages)
                {
                    if (!message.Date.TimeOfDay.IsInTimeFrame(fromHour, toHour))
                    {
                        continue;
                    }

                    Console.WriteLine($"Marking message for deletion: [{message.Index}] {message.Date} - {message.NormalizedSubject}");

                    if (!WhatIf)
                    {
                        client.Inbox.AddFlags(message.Index, MessageFlags.Deleted, true);
                    }
                }

                startIndex = endIndex + 1;

            } while (startIndex < emailCount - 1);

            Console.Write("Deleting messages from folder...");

            if (!WhatIf)
            {
                client.Inbox.Expunge();
            }

            Console.WriteLine("All gone.");
        }
    }
}