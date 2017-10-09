using System.Collections.Generic;
using System.IO;

namespace GoldFmNoRepeatWeekdayFinder
{
    class RecordWriter
    {
        public List<string> ReadFileLines(string path)
        {
            if (!File.Exists(path))
            {
                return new List<string>();
            }
            string[] lines = File.ReadAllLines(path);
            return new List<string>(lines);
        }


        public void RecordLineToFile(string path, string line)
        {
            (new FileInfo(path)).Directory.Create();

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(line);
            }
        }        
    }
}
