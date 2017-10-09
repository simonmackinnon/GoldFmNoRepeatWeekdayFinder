using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace GoldFmNoRepeatWeekdayFinder
{
    class SongSerializer
    {
        public static Song SerializeJSON(JToken token)
        {
            JToken artist = token["artist"]["value"];
            JToken title = token["title"]["value"];
            JToken coverarturl = token["coverart"]["value"];
            JToken id = token["ID"];
            JToken songlenth = token["length"]["value"];
            JToken playeddatetime = token["played_datetime"]["value"];
            JToken number = token["number"]["value"];

            JToken type = token["type"];
                        
            var song = new Song();
            song.artist = csvEncodeString(artist.ToString());
            song.title = csvEncodeString(title.ToString());
            song.coverArtUrl = WebUtility.UrlEncode(coverarturl.ToString());
            song.ID = csvEncodeString(id.ToString());
            song.number = csvEncodeString(number.ToString());
            song.playedDateTime = DateTime.Parse(playeddatetime.ToString());
            song.songLength = TimeSpan.Parse(songlenth.ToString());
            song.type = type.ToString();
            return song;
        }

        public static string csvEncodeString(string str)
        {
            var charsToRemove = new string[] { "@", ",", ".", ";", "'" };
            foreach (var c in charsToRemove)
            {
                str = str.Replace(c, string.Empty);
            }

            return str;
        }
    }
}
