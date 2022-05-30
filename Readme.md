# IMAP Cleanup

IMAP Cleanup is a small .NET Core applicaton to delete old emails from an IMAP inbox and leave only a number of most recent messages behind.
You can thus also use this to delete all emails from an IMAP inbox.

It uses SSL by default to connect to the IMAP server. 

## Requirements

To run the application you can download the Windows x64 binary to run it without dependencies on a Windows 64-bit operating system.
If you are on another platform, you need to install the [.NET Core framework](https://dotnet.microsoft.com/download) to be able to run the general framework dependent release.

## Installing

- Download the installer zip for your system. See Releases below. If you are running on a Windows x64 system (a recent machine running Windows 10 for example) you'll want to get the x64 self-contained zip. If in another case, get the framework dependent version. You will need to [install .NET core](https://dotnet.microsoft.com/download) for this.
- Extract the zip file in a new folder somewhere.

For the self-contained version, the zip only contains a single executable. You can copy this file anywhere, and run it from there.
For the framework version, it contains a number of other files, so it's better to keep this in it's own folder, and run the program from there.

## Usage

Once built or installed, you can call the executable using command line arguments to point it to your IMAP inbox, like this:

	.\ImapCleanup.exe --hostname imap.mailserver.com --port 993 --username jack@foobar.com --password horsestaplebattery --count 500

This will delete all messages, except the 500 most recent ones.

For help, use `.\ImapCleanup.exe /?`

## Releases

See [releases](https://github.com/n3wjack/ImapCleanup/releases)

- ImapCleanup-1.0.zip: framework dependent version. You need to have the .NET Core 3.1 framework installed to run this. This runs on any platform.
- ImapCleanup-1.0-Windowsx64-self-contained.zip: a self-contained release for Windows x64 only. This runs without any dependencies.

## Building

You need to have the .NET Core 3.1 SDK installed to build this. You can use Visual Studio Community edition to build it.

There are 3 way to build the project.

1. Use Visual Studio.
2. Run the `build.cmd` script to build a Windows 64-bit self-contained executable.
3. Use the `dotnet` tool from the command line: `dotnet build ImapCleanup.sln`

## Disclaimer

This tool deletes emails. **USE AT YOUR OWN RISK.**

This little tool works fine for me. It deletes the oldest messages, leaves the right ones and that's it.
I haven't tested this thoroughly on every possible email server and IMAP configuration in the world. So if this ends up deleting vital emails you're on your own I'm afraid.
Before you point this to anything critical, test it on a dummy inbox with some test emails, just to be sure. 

