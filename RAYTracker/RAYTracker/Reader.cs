﻿using System.Collections.Generic;
using System.IO;

namespace RAYTracker
{
    public class Reader
    {
        private readonly StreamReader _reader;

        public Reader(string filename)
        {
            _reader = new StreamReader(filename);
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
