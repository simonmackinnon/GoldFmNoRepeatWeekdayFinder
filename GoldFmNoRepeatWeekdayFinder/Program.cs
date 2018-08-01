using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading;

namespace GoldFmNoRepeatWeekdayFinder
{
    class Program
    {
        const int startTimeOffsetHours = 6;
        const int endTimeOffsethours = 18;

        public static SecureString GetPassword()
        {
            var pwd = new SecureString();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Length > 0)
                    {
                        pwd.RemoveAt(pwd.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    pwd.AppendChar(i.KeyChar);
                    Console.Write("*");
                }
            }
            return pwd;
        }


        static void Main(string[] args)
        {
            bool showPassword = false;

            //Console.WriteLine("Enter sender email address: ");
            //string email = Console.ReadLine();

            Console.WriteLine("Sender email address: norepeatweekdayanalyser@gmail.com");
            string email = @"norepeatweekdayanalyser@gmail.com";

            Console.WriteLine("Enter your email address password: ");
            SecureString password = GetPassword();

            if (showPassword)
            {
                Console.WriteLine("Password is: ");
                string shwPsswrd = new System.Net.NetworkCredential(string.Empty, password).Password;
            }

            Console.WriteLine();
            Console.WriteLine("Enter recipient email address: ");
            string toEmail = Console.ReadLine();

            SongsFetcher fetcher = new SongsFetcher();
            Playlist playlist = new Playlist();
            MailSender sender = new MailSender(email, password, toEmail);

            sender.sendStartEmail();

            string path = @"C:\\workspace\songAnalyser\" + DateTime.Now.ToString("yyyyMMdd") + ".csv";

            if (File.Exists(path))
            {
                List<string> lines = playlist.writer.ReadFileLines(path);
                lines.Sort();
                lines.RemoveAt(0);

                foreach (string str in lines)
                {
                    try
                    {
                        string[] tokens = str.Split(',');

                        Song song =
                            new Song()
                            {
                                playedDateTime = Convert.ToDateTime(tokens[0]),
                                artist = tokens[1],
                                title = tokens[2],
                                number = tokens[3],
                                coverArtUrl = tokens[4],
                                ID = tokens[5],
                                songLength = TimeSpan.Parse(tokens[6])
                            };

                        Song nowPlayingSong = fetcher.GetNowPlayingSong();

                        if (song.artist == nowPlayingSong.artist
                            && song.title == nowPlayingSong.title
                            && song.type == nowPlayingSong.type
                            && song.playedDateTime == nowPlayingSong.playedDateTime)
                        {
                            continue;
                        }
                        else
                        {
                            playlist.playlist.Add(song);
                        }
                    }
                    catch { }                             
                }
            }

            while (true)
            {
                try
                {
                    Song nowPlayingSong = fetcher.GetNowPlayingSong();

                    if (nowPlayingSong == null)
                        continue;

                    playlist.checkCurrentTrack(nowPlayingSong);

                    int numTimesPlayedToday = playlist.getCurrentSongPlayedCount(
                        DateTime.Today.AddHours(startTimeOffsetHours),
                        DateTime.Today.AddHours(endTimeOffsethours));

                    if (nowPlayingSong.type == "song" && numTimesPlayedToday > 1)
                    {
                        sender.sendEmailAlert(nowPlayingSong, playlist);
                    }

                    Thread.Sleep(20000);
                }
                catch { }

            }
                                        
        }

     }
}
