using System;
using System.Linq;
using System.Collections.Generic;
///sdcard/android/data/
namespace KJVSharp
{
    public class CsvRow
    {
        public override string ToString()
        {
            return string.Format("LineText: {0}", LineText);
        }
        List<char> contents;
        public List<char> Row { get => contents; }
        public string[] GetSegments()
        {
            List<string> myList = new List<string>();
            string segment = ""; bool inQuotes = false; bool off = false;

            for (int i = 0; i < contents.Count; i++)
            {
                if (!off)
                {
                    switch (contents[i])
                    {
                        case '<':
                            off = true; continue;
                        case ',':
                            if (!inQuotes)
                            {
                                myList.Add(segment); segment = "";
                                continue;
                            }
                            break;
                        case '"':
                            inQuotes = !inQuotes;
                            break;
                    }
                }
                else if (contents[i] == '>')
                {
                    off = false;
                    continue;
                }
                segment += contents[i];
                if (i + 1 == contents.Count) myList.Add(segment);
            }//for
            return myList.ToArray();
        }
        public string LineText { get => string.Join("", contents); set { contents = (value.ToList<char>()); } }
    }

}
