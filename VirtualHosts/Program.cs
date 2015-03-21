using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualHosts
{
    class Program
    {
        static string defaultHostDir = "public_html";

        static List<Host> hosts = new List<Host>();

        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            ReadHome();
            if (args.Length == 0 || args[0] == "generate")
            {
                GenerateVhosts();
                CleanHostsFile();
                GenerateHosts();
            }
            else if (args[0] == "restore" || args[0] == "clean")
            {
                CleanHostsFile();
            }

            Finish();
        }

        static void ReadHome()
        {
            DirectoryInfo homeDirectoryInfo = null;

            if (Directory.Exists(Config.HomeDir))
            {
                homeDirectoryInfo = new DirectoryInfo(Config.HomeDir);
            }
            else if (Directory.Exists(Config.homeDir))
            {
                homeDirectoryInfo = new DirectoryInfo(Config.homeDir);
            }
            else
            {
                Console.WriteLine(String.Format("\n\rFailed reading home directory! Make sure that '{0}' exists and is accessible.", Config.homeDir));
                Console.ReadKey();

                Environment.Exit(0);
            }

            if (!Directory.Exists(Config.XamppDir))
            {
                Console.WriteLine(String.Format("\n\rFailed reading XAMPP directory! Make sure that '{0}' exists and is accessible.", Config.XamppDir));
                Console.ReadKey();

                Environment.Exit(0);
            }

            if (homeDirectoryInfo != null)
            {
                var directories = homeDirectoryInfo.GetDirectories();

                foreach (var d in directories)
                {
                    foreach (var sd in d.GetDirectories())
                    {
                        if (sd.Name != Config.defaultHostDir)
                        {
                            var name = sd.Name + "." + d.Name;

                            hosts.Add(new Host { Name = name, Path = sd.FullName.Replace("\\", "/") });
                            //hosts.Add(new Host { Name = "www." + name, Path = sd.FullName });
                        }
                    }

                    hosts.Add(new Host { Name = d.Name, Path = d.FullName.Replace("\\", "/") + "/" + Config.defaultHostDir });
                    //hosts.Add(new Host { Name = "www." + d.Name, Path = d.FullName + "\\" + defaultHostDir });
                }
                Console.WriteLine(String.Format("  Found {0} sites!", hosts.Count));
            }
        }

        static void GenerateVhosts()
        {
            var template = (File.Exists("Template.txt"))
                ? File.ReadAllText("Template.txt").ToString()
                : Config.defaultTemplate;

            string vhostsContents = "";
            foreach (var host in hosts)
            {
                vhostsContents += String.Format(template, host.Name, host.Path) + "\n\r";
            }

            try
            {
                File.WriteAllText(Config.VhostsFile, vhostsContents);

                Console.WriteLine("  Added virtual hosts to Apache.");
            }
            catch
            {
                Console.WriteLine(String.Format("\n\rFailed writing '{0}'! Make sure the file exists and is writeable.", Config.VhostsFile));
                Console.ReadKey();

                Environment.Exit(0);
            }
        }

        static void GenerateHosts()
        {
            var hostItems = new List<string>();
            hostItems.Add("#==== Virtual hosts");
            foreach (var host in hosts)
            {
                hostItems.Add(String.Format("{0}\t{1}", "127.0.0.1", host.Name));
                hostItems.Add(String.Format("{0}\t{1}", "127.0.0.1", "www." + host.Name));
            }

            try
            {
                var hostsFile = System.IO.File.ReadAllText(Config.HostsFile);

                File.WriteAllText(Config.HostsFile, hostsFile + "\n" + String.Join("\n", hostItems));

                Console.WriteLine(String.Format("  Added {0} items to hosts file.", hostItems.Count() - 1));
            }
            catch
            {
                Console.WriteLine(String.Format("\n\rFailed accessing '{0}'! Make sure the file exists and is writeable.", Config.HostsFile));
                Console.ReadKey();

                Environment.Exit(0);
            }
        }

        static void CleanHostsFile()
        {
            var hostItems = new List<string>();
            hostItems.Add("#==== Virtual hosts");
            foreach (var host in hosts)
            {
                hostItems.Add(String.Format("{0}\t{1}", "127.0.0.1", host.Name));
                hostItems.Add(String.Format("{0}\t{1}", "127.0.0.1", "www." + host.Name));
            }

            try
            {
                var hostsFile = System.IO.File.ReadAllLines(Config.HostsFile);

                var newHostsFileContents = CleanHostsFile(hostsFile, hostItems);

                var difference = hostsFile.Count() - newHostsFileContents.Count();

                File.WriteAllText(Config.HostsFile, String.Join("\n", newHostsFileContents));

                Console.WriteLine(String.Format("  Removed {0} items from hosts file.", difference != 0 ? difference - 1 : 0));
            }
            catch
            {
                Console.WriteLine(String.Format("\n\rFailed writing '{0}'! Make sure the file exists and the application has access to it.", Config.HostsFile));
                Console.ReadKey();

                Environment.Exit(0);
            }
        }

        static List<string> CleanHostsFile(string[] hostsLines, List<string> hostItems)
        {
            return hostsLines.ToList().Except(hostItems).ToList();
        }

        static void Finish()
        {
            Thread.Sleep(3000);

            Environment.Exit(0);
        }
    }

    public class Host
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
