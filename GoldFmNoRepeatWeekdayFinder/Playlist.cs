using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldFmNoRepeatWeekdayFinder
{
    class Playlist
    {
        public List<Song> playlist = new List<Song>();
        Song nowPlayingSong;
        Song lastPlayingSong;
        DateTime now;
        DateTime then = new DateTime();
        public RecordWriter writer = new RecordWriter();

        public void checkCurrentTrack(Song newSong)
        {
            resetPlaylistNewDay();

            nowPlayingSong = newSong;
            
            if (lastPlayingSong == null || nowPlayingSong.ID != lastPlayingSong.ID)
            {            
                if (nowPlayingSong.type == "song")
                {
                    string output = "Now Playing @ " + now + ": " + nowPlayingSong.artist + " - " + nowPlayingSong.title;

                    Console.WriteLine(output);

                    string csvLine =
                        nowPlayingSong.playedDateTime.ToString() +
                        "," + nowPlayingSong.artist +
                        "," + nowPlayingSong.title +
                        "," + nowPlayingSong.number +
                        "," + nowPlayingSong.coverArtUrl +
                        "," + nowPlayingSong.ID +
                        "," + nowPlayingSong.songLength.ToString();

                    writer.RecordLineToFile(@"C:\\workspace\songAnalyser\" + DateTime.Now.ToString("yyyyMMdd") + ".csv", csvLine);

                    playlist.Add(nowPlayingSong);
                }
            }

            lastPlayingSong = nowPlayingSong;
        }

        private void resetPlaylistNewDay()
        {
            now = DateTime.Now;
            if (then.Date != DateTime.Now.Date)
            {
                playlist.Clear();

                const string csvLine = "PlayedDateTime,Artist,Title,Number,CoverArtUrl,ID,SongLength";
                string path = @"C:\\workspace\songAnalyser\" + DateTime.Now.ToString("yyyyMMdd") + ".csv";

                if (writer.ReadFileLines(path).FirstOrDefault() != csvLine)
                {                   
                    writer.RecordLineToFile(path, csvLine);
                }
                
            }
            then = now;
        }

        public int getCurrentSongPlayedCount(DateTime startTime, DateTime endTime)
        {
            var matchingSongs = playlist.Where(x => x == nowPlayingSong).
                Where(x => x.playedDateTime < endTime && x.playedDateTime > startTime);

            return matchingSongs.Count();            
        }
    }
}
