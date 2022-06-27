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

        internal void Run(TimeSpan fromHour, TimeSpan toHour)
        {
            LoginAndRunAction((imapClient) => RemoveEmails(imapClient, fromHour, toHour));
        }

        private void RemoveEmails(ImapClient client, TimeSpan fromHour, TimeSpan toHour)
        {
            Console.WriteLine("Getting messages...");
            client.Inbox.Open(FolderAccess.ReadWrite);
            var messages = client.Inbox.Fetch(0, client.Inbox.Count - 1, MessageSummaryItems.All);

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

            Console.Write("Deleting messages from folder...");

            if (!WhatIf)
            {
                client.Inbox.Expunge();
            }

            Console.WriteLine("All gone.");
        }
    }
}