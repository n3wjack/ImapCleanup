using ImapCleanup.Commands;
using ImapCleanup.Configuration;
using System;
using System.Collections.Generic;
using System.CommandLine;

namespace ImapCleanup
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostnameOption = new Option<string>("--hostname", "Host name of the IMAP server") { IsRequired = true };
            var usernameOption = new Option<string>("--username", "Username for the IMAP account") { IsRequired = true };
            var passwordOption = new Option<string>("--password", "Password for the IMAP account") { IsRequired = true };
            var portOption = new Option<int>("--port", "Port number") { IsRequired = true };
            var whatIfOption = new Option<bool>(new string[] { "--whatif" }, "Simulate the action, listing emails that would be deleted.");

            var imapConfigurationBinder = new ImapConfigurationBinder(portOption, hostnameOption, usernameOption, passwordOption);
            var imapConfigurationOptions = new List<Option> { hostnameOption, usernameOption, passwordOption, portOption };
            
            CreateRootCommand(whatIfOption, imapConfigurationOptions, 
                new List<Command> 
                {
                    CreateCountCommand(whatIfOption, imapConfigurationBinder),
                    CreateTimeCommand(whatIfOption, imapConfigurationBinder)
                }).Invoke(args);
        }

        private static RootCommand CreateRootCommand(Option<bool> whatIfOption, List<Option> imapConfigurationOptions, List<Command> commands)
        {
            var rootCommand = new RootCommand();

            rootCommand.Description = "IMAP cleanup tool. Will delete emails in the inbox, only keeping a given number of emails.";
            imapConfigurationOptions.ForEach(o => rootCommand.AddGlobalOption(o));
            rootCommand.AddGlobalOption(whatIfOption);
            commands.ForEach(command => rootCommand.Add(command));

            return rootCommand;
        }

        private static Command CreateCountCommand(Option<bool> whatIfOption, ImapConfigurationBinder imapConfigurationBinder)
        {
            var emailsToKeepOption = new Option<int>("--keep", "Number of emails to keep.") { IsRequired = true };
            var batchSize = new Option<int>("--batchsize", "Batch size of emails to fetch (optional). Default is 200.") { IsRequired = false };
            var countCommand = new Command("count");

            countCommand.SetHandler(CountCommandHandler, imapConfigurationBinder, emailsToKeepOption, batchSize, whatIfOption);
            countCommand.Description = "Delete emails over the given number of emails to keep.";
            countCommand.AddOption(emailsToKeepOption);
            countCommand.AddOption(batchSize);

            return countCommand;
        }

        private static Command CreateTimeCommand(Option<bool> whatIfOption, ImapConfigurationBinder imapConfigurationBinder)
        {
            var fromHourOption = new Option<TimeSpan>(new string[] { "--fromhour", "--from" }, "The hour to delete from.") { IsRequired = true };
            var toHourOption = new Option<TimeSpan>(new string[] { "--tohour", "--to" }, "The hour to delete to.") { IsRequired = true };
            var batchSize = new Option<int>("--batchsize", "Batch size of emails to fetch (optional). Default is 200.") { IsRequired = false };
            var timeCommand = new Command("time");

            timeCommand.SetHandler(TimeCommandHandler, imapConfigurationBinder, fromHourOption, toHourOption, batchSize, whatIfOption);
            timeCommand.Description = "Delete emails in a given time frame.";
            timeCommand.AddOption(fromHourOption);
            timeCommand.AddOption(toHourOption);

            return timeCommand;
        }

        static void CountCommandHandler(ImapConfiguration configuration, int emailsToKeep, int batchSize, bool whatIf)
        {
            new CleanupEmailsCommand(configuration, whatIf).Run(emailsToKeep, batchSize <= 0 ? 200 : batchSize);
        }

        static void TimeCommandHandler(ImapConfiguration configuration, TimeSpan fromHour, TimeSpan toHour, int batchSize, bool whatIf)
        {
            new DeleteEmailsByHourCommand(configuration, whatIf).Run(fromHour, toHour, batchSize <= 0 ? 200 : batchSize); 
        }
    }
}
