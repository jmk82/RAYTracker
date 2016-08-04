using System.Collections.Generic;
using System.IO;

namespace RAYTracker.Domain.Utils
{
    public class Reader
    {
        private readonly StreamReader _reader;

        public Reader(string filename)
        {
            _reader = new StreamReader(filename);
        }

        public Reader(StreamReader reader)
        {
            _reader = reader;
        }

        public IList<string> GetAllLinesAsStrings()
        {
            var lines = new List<string>();
            string line;

            while ((line = _reader.ReadLine()) != null)
            {
                lines.Add(line);
            }

            return lines;
        }
    }
}
