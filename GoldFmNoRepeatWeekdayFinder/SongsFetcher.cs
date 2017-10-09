using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Text;

namespace GoldFmNoRepeatWeekdayFinder
{
    class SongsFetcher
    {
        public Song GetNowPlayingSong()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://media.arn.com.au/XML-JSON.aspx?source=www.gold1043.com.au&feedUrl=xml/gold1043_now.xml");

            request.ContentType = "application/json; charset=utf-8";
            request.PreAuthenticate = true;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            string json;

            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                json = reader.ReadToEnd();
            }

            try
            {
                JToken now_playing_token = JObject.Parse(json)["on_air"]["now_playing"]["audio"];
                return SongSerializer.SerializeJSON(now_playing_token);
            }
            catch
            {
                return null;
            }
            
        }
    }
}
