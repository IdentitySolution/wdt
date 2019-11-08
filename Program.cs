using System;
using System.Linq;
using System.Reflection;

namespace wdt
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                DisplayHelp();
                return;
            }

            switch (args[0])
            {
                case "-v":
                case "--version":
                    DisplayVersion();
                    break;
                case "-h":
                    DisplayH();
                    break;
                case "help":
                case "--help":
                    DisplayHelp();
                    break;
                case "addhosts":
                    AddHost.RunAddHosts(args.Skip(1).ToArray());
                    break;
                default:
                    Console.WriteLine($"wdt: '{args[0]}' is not a wdt command. See 'wdt --help'");
                    break;
            }

            return;
        }

        static void DisplayH()
        {
            Console.WriteLine("usage: wdt [--version] [--help]");
            Console.WriteLine("           <command> [<args>]");
            Console.WriteLine();
        }
        static void DisplayHelp()
        {
            DisplayH();
            Console.WriteLine("There are a list of availabale commands:");
            Console.WriteLine();
            Console.WriteLine("\taddhosts\t\tAdd a host or hosts pointing to 127.0.0.1 to hosts file");
            Console.WriteLine();
        }

        static void DisplayVersion()
        {
            var versionString = Assembly.GetEntryAssembly()
                        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                        .InformationalVersion
                        .ToString();
            Console.WriteLine($"wdt version {versionString}");
        }
    }
}
