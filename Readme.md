# IMAP Cleanup

IMAP Cleanup is a small .NET Core application to cleanup an IMAP inbox and leave only a number of most recent messages behind. 

It uses SSL by default to connect to the IMAP server. 

## Requirements

You need to have the .NET Core 3.1 SDK installed to build this. You can use Visual Studio Community edition to build it.

## Building

There are 3 way to build the project.

1. Use Visual Studio.
2. Run the `build.cmd` script to build a Windows 64-bit self-contained executable.
3. Use the `dotnet` tool from the command line: `dotnet build ImapCleanup.sln`

## Usage

Once built, you can call the executable using command line arguments to point it to your IMAP inbox, like this:

	.\ImapCleanup.exe --hostname imap.mailserver.com --port 993 --username jack@foobar.com --password horsestaplebattery --count 500

This will delete all messages, except the 500 most recent ones.

For help, use `.\ImapCleanup.exe /?`

## Disclaimer

This tool deletes emails. **USE AT YOUR OWN RISK.**

This little tool works fine for me. It deletes the oldest messages, leaves the right ones and that's it.
I haven't tested this thoroughly on every possible email server and IMAP configuration in the world. So if this ends up deleting vital emails you're on your own I'm afraid.
Before you point this to anything critical, test it on a dummy inbox with some test emails, just to be sure. 

