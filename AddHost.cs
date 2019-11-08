using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace wdt
{
    class AddHost
    {
        public static void RunAddHosts(string[] args)
        {
            try
            {
                Helpers.RequireAdministrator();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }


            if (args.Length == 0)
            {
                DisplayHelp();
                return;
            }

            switch (args[0])
            {
                case "help":
                case "--help":
                    DisplayHelp();
                    break;
                default:
                    AddLocalHosts(args.ToList());
                    break;
            }
            return;
        }

        static void DisplayHelp()
        {
            Console.WriteLine("usage: wdt addhost [<host> <host> ...]");
            Console.WriteLine();
        }

        static void AddLocalHosts(List<string> hosts)
        {
            string hostsFilePath = Environment.GetEnvironmentVariable("windir") + @"\System32\drivers\etc\hosts";

            // 1. Read existing hosts file
            var lines = File.ReadAllLines(hostsFilePath);
            Dictionary<int, string> existingHosts = new Dictionary<int, string>();

            // 2. Check if host is already in file
            for (int i = 0; i < hosts.Count(); i++)
            {
                string host = hosts[i];
                string lineFound = lines
                .FirstOrDefault(l => Regex.IsMatch(l, @"\b" + Regex.Escape(host) + @"\b"));
                if (lineFound != null)
                {
                    hosts.RemoveAt(i);
                    i--;
                    Console.WriteLine($"host {host} already exists in hosts file as:");
                    Console.WriteLine($"\t{lineFound}");
                }
            }

            // 3. Return if no new hosts need to be added
            if (hosts.Count() < 1)
            {
                Console.WriteLine("Finished. No new hosts were added.");
                return;
            }

            // 4. Create backup
            File.Copy(hostsFilePath, $"hosts-{DateTime.Now:yyyy-MM-dd_hh-mm-ss}.backup");

            // 5. Add new hosts
            Console.WriteLine("Appending the following hosts:");
            foreach (string host in hosts)
            {
                Console.WriteLine($"\t{host}");
            }

            List<string> newLines = new List<string>();
            // 5.1 Detect if a new line needs to be added or not
            var allText = File.ReadAllText(hostsFilePath);
            string last4String = new string(allText.TakeLast(4).ToArray());
            if (!(last4String.Contains("\r") || last4String.Contains("\n")))
                newLines.Add("");

            newLines.AddRange(hosts.Select(h => $"127.0.0.1\t\t\t{h}").ToList());
            File.AppendAllLines(hostsFilePath, newLines);

            // 6. Display finishing statement and return
            Console.WriteLine($"Finished adding {hosts.Count()} host(s) to the hosts file.");
            return;
        }
    }
}