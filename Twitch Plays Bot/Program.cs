using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Twitch_Plays_Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> commandPairs = new Dictionary<string, string>();
            Dictionary<string, string> tSettingsPairs = new Dictionary<string, string>();

            Console.WriteLine("Welcome to Twitch Plays Bot.");
            Console.WriteLine("");

            Console.WriteLine("");
            Console.WriteLine("Loading Commands File...");
            string commandsFile = File.ReadAllText(@"commands.txt", Encoding.UTF8);

            Console.WriteLine("Commands File Loaded...");
            Console.WriteLine("");
            Console.WriteLine(commandsFile);
            Console.WriteLine("");

            string[] commandsList = commandsFile.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            //Console.WriteLine(commandsList[0]);

            foreach (string i in commandsList) {
                commandPairs.Add(i.Split(new string[] { ":" }, StringSplitOptions.None)[0].Replace(" ", string.Empty), i.Split(new string[] { ":" }, StringSplitOptions.None)[1].Replace(" ", string.Empty));
            }

            Console.WriteLine("Loading Twitch Settings File...");
            string tSettingsFile = File.ReadAllText(@"twitch_settings.txt", Encoding.UTF8);

            Console.WriteLine("Twitch Settings Loaded...");
            Console.WriteLine("");

            string[] tSettingsList = tSettingsFile.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            foreach (string i in tSettingsList)
            {
                tSettingsPairs.Add(i.Split(new string[] { ":" }, StringSplitOptions.None)[0].Replace(" ", string.Empty), i.Split(new string[] { ":" }, StringSplitOptions.None)[1].Replace(" ", string.Empty));
            }

            //Console.WriteLine(tSettingsPairs["bot_username"]);

            //Console.WriteLine(commandPairs["down"]);

            TcpClient tcpClient = new TcpClient("irc.twitch.tv", 6667);
            StreamReader reader = new StreamReader(tcpClient.GetStream());
            StreamWriter writer = new StreamWriter(tcpClient.GetStream());
            writer.AutoFlush = true;

            writer.WriteLine("PASS oauth:" + tSettingsPairs["oauth"] + Environment.NewLine
                + "NICK " + tSettingsPairs["bot_username"] + Environment.NewLine
                + "USER " + tSettingsPairs["bot_username"] + " 8 * :" + tSettingsPairs["bot_username"]);

            writer.WriteLine("JOIN #" + tSettingsPairs["channel"]);

            writer.WriteLine(":" + tSettingsPairs["bot_username"]
                + "!" + tSettingsPairs["bot_username"] + "@"
                + tSettingsPairs["bot_username"] + ".tmi.twitch.tv PRIVMSG #" + tSettingsPairs["channel"] + " :Kappa Twitch Plays Bot is now Online Kappa");

            while (true)
            {
                //:twitchpl!twitchpl@twitchpl.tmi.twitch.tv PRIVMSG #deeredman1991 :Kappa

                if (tcpClient.Available > 0)
                {
                    string message = reader.ReadLine();
                    try
                    {
                        if (commandPairs.ContainsKey(message.Split(new string[] { ":" }, StringSplitOptions.None)[2]))
                        {
                            System.Windows.Forms.SendKeys.SendWait(commandPairs[message.Split(new string[] { ":" }, StringSplitOptions.None)[2]]);
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }
                    Console.WriteLine($"\r\n{message}");
                }

                Thread.Sleep(100);
            }
            //System.Windows.Forms.SendKeys.SendWait(commandPairs["a"]);

            //Console.ReadKey();
        }
    }
}
