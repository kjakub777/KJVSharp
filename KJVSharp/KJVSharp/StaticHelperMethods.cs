using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
///sdcard/android/data/
namespace KJVSharp
{
    public class StaticHelperMethods
    {
        private static bool logOutput = true;
        private static Int64 MAX_LOG_SIZE = 20000000; //bytes
                                                      //******************
                                                      //just for printing output for debug use
        private static int outputs = 0;
        private static IEnumerable<Verse> s_dataStore;

        public static string dir
        {
            get
            {
                //  StaticHelperMethods.WriteOut($"{Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData)}");
                return
             Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "logg.txt");

            }
        }
        private static string filename { get { return Path.Combine(dir, @"klog"); } }

        public static void WriteOut(string msg, bool nl = true)

        {
            try
            {
                if (logOutput)
                {
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    msg = nl ? $"{msg}{Environment.NewLine}" : msg;
                    if (outputs++ == 0)
                    {
                        //make sure file not too big
                        Int64 fileSizeInBytes = new FileInfo(filename).Length;

                        if (fileSizeInBytes > MAX_LOG_SIZE)
                            File.WriteAllText(filename, $"\n\n" +
                                $"*************************************************************************" +
                                $"\n LOG {DateTime.Now}\n" +
                                $"*************************************************************************\n");
                        else File.AppendAllText(filename, $"\n\n" +
                             $"*************************************************************************" +
                             $"\n LOG {DateTime.Now}\n" +
                             $"*************************************************************************\n");
                        File.AppendAllText(filename, $"{msg}");
                    }
                    else
                    {
                        File.AppendAllText(filename, $"{msg}");
                    }
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                File.WriteAllText(filename, $"New CMS LOG {DateTime.Now}\n");
            }
            catch (Exception)
            {

            }
        }
        //*****************
        public static string[] GetBookChapterVerseNumpre(string line)
        {
            string cell = (new CsvRow() { LineText = line }/*, CsvFileReader.Reader*/).GetSegments()[0];

            bool beforeletters = true;
            string book = "", ch = "", vnum = ""; string chap_verse = "";
            try
            {
                foreach (char c in cell.Trim())
                {
                    if (beforeletters)
                    {
                        beforeletters = !char.IsLetter(c);
                        book += c;
                        continue;
                    }
                    if (char.IsLetter(c))
                    {
                        book += c;
                    }
                    else
                    {
                        //split into chapter and verse
                        chap_verse += c;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLineIf(true, $"{ex}");
            }
            try
            {
                var c_v = chap_verse.Trim().Split(':');
                if (c_v.Length == 2)
                {
                    ch = (c_v[0]);
                    vnum = (c_v[1]);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLineIf(true, $"BAD cell -->> {string.Join("", cell)}");
                    ch = vnum = "0";
                    return new string[] { book, ch, vnum };
                }
            }
            catch (Exception ex)
            {
                ch = vnum = "0";
                System.Diagnostics.Debug.WriteLine($"{ex}");
                return new string[] { book, ch, vnum };
            }
            try
            {
                Console.WriteLine($"Cell vals: {book}|{ch}|{vnum}");
                WriteOut($"Cell vals: {book}|{ch}|{vnum}");
            }
            catch (Exception x)
            {
                Console.WriteLine("DAMN" + x);
            }
            return new string[] { book, ch, vnum };
        }
        public static Guid GetRandomOid()
        {
            return Guid.NewGuid();
        }
        public static IEnumerable<Verse> DataStoreList { get => s_dataStore == null ? GetMakeListpre() : s_dataStore; private set => s_dataStore = value; }
        public static IEnumerable<Verse> GetMakeListpre()
        {
            List<Verse> lst = new List<Verse>();
            string line = "";
            //line = ""; string[] cur = GetBookChapterVerseNumpre(line); lst.Add(new Verse() { Book = new Book() { BookName = cur[0] }, Chapter = cur[1], Number = cur[2], Text = line.Substring(line.IndexOf(',') + 1) });
            string[] cur;
            /// line = "Exodus 15:1,\"Then sang Moses and the children of Israel this song unto the LORD, and spake, saying, I will sing unto the LORD, for he hath triumphed gloriously: the horse and his rider hath he thrown into the sea.\"";
            var myLines = DataStore.GetData();
            List<Book> books = new List<Book>();
            string curbook = ""; Book b = null;
            for (int i = 0; i < myLines.Length; i++)
            {
                line = myLines[i].Replace("_qt_", "\"");
                var first = line.Substring(0, line.IndexOf(','));
                //  line.ToList().Where(x=>char.Is)
                var sec = line.Substring(line.IndexOf(',') + 1);
                string bookname = GetBookName(first);
                if (bookname != curbook)
                {
                    if (i != 0)  // we dont want empty book!!
                    {
                         StaticHelperMethods.WriteOut($"Book {bookname} {GetChapter(first)}:{GetVnum(first)}  ");
                        b.Verses.AddRange(lst.Where(vrs => vrs.Book.BookName == curbook));
                        books.Add(b);
                        Console.WriteLine($"{curbook}Book Oid {b.Oid.ToString()}");
                    }
                    b = new Book()
                    {
                        BookName = bookname
                    };
                    curbook = bookname;
                }
                var strch = GetChapter(first);
                var strnum = GetVnum(first);
                var v = new Verse()
                {
                    Book = /*bookchanged ? books.Last() :*/ b,
                    Chapter = strch,
                    Number = strnum,
                    Text = sec
                };
                Console.WriteLine(v);
                lst.Add(v);
            }

            return lst;
        }

        public static string GetChapter(string firstCell)
        {

            string num = "";
            bool start = false;
            int i = 0;
            foreach (char c in firstCell)
            {
                if (i++ > 0 && !start && char.IsDigit(c))
                {
                    start = true;
                    if (c == ':') return num;
                    else num += c;
                }

            }
            return num;
        }
        public static string GetVnum(string firstCell)
        {
            try
            {
                return firstCell.Substring(firstCell.IndexOf(':') + 1);
            }
            catch
            {
                return "";
            }
        }
        public static string GetBookName(string firstCell)
        {
            try
            {
                bool started = false;
                string num = "";
                foreach (char c in firstCell)
                {
                    if (char.IsLetter(c))
                    {
                        started = true;
                        num += c;
                    }
                    else if (!started)
                    {
                        num += c;
                    }
                }
                return num;
            }
            catch { return ""; }
        }
        //using (CsvFileReader reader = new CsvFileReader("/sdcard/Android/data/com.companyname.KJVSharp/files/_MyV.csv"))
        //{
        //    Testament testament = Testament.Old;
        //    int i = 0;
        //    try
        //    {
        //        CsvRow row = new CsvRow();
        //        Book curBook = new Book() { BookName = "", };
        //        //   Chapter curChap = new Chapter() { Number = "", /*Testament = testament, BibleIndex = context.Books.Count() + 1,*/ Oid = new Guid(GetRandomOid()) };
        //        var curVer = new Verse() { Text = "", };
        //        while (reader.ReadRow(row))
        //        {
        //            Console.WriteLine($"{i}  {row[0]}  {row[1]}");
        //            string verse = row[1].Replace("<i>", "i").Replace("</i>", "i");  //should be Book,  <i>was</i> ! ;
        //            string cell = row[0];
        //            string book; string vnum; string ch;

        //            try
        //            {
        //                if (GetBookChapterVerseNum(cell, out book, out ch, out vnum))
        //                { //Book booktoadd = ;
        //                    StaticHelperMethods.WriteOut($"Book {book} {ch}:{vnum} {verse}");
        //                    testament = testament == Testament.Old && book.Contains("Matthew") ? Testament.New : testament;

        //                    if (curBook.BookName == book)
        //                        StaticHelperMethods.WriteOut($"SAMWBOOK");
        //                    else
        //                    {

        //                        curBook = new Book() { BookName = book, };
        //                        //    curBook = new Book() { BookName = book, /*Testament = testament, BibleIndex = context.Books.Count() + 1,*/ Oid = new Guid(GetRandomOid())  };
        //                    }



        //                    curVer = new Verse()
        //                    {
        //                        Book = curBook,
        //                        Number = vnum,
        //                        Chapter = ch,
        //                        Text = verse,
        //                        //    Length = verse.Length,
        //                        //    NumWords = verse.Where(x => string.IsNullOrWhiteSpace(x.ToString())).Count()
        //                    };

        //                    lst.Add(curVer);

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                StaticHelperMethods.WriteOut($"Ex {ex}");
        //                System.Diagnostics.Debug.WriteLineIf(true, $"{ex}");
        //                //         return MasterbkList;
        //            }
        //        }// context.SaveChanges();
        //         //   return ret;// context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        StaticHelperMethods.WriteOut($"Ex {ex}");
        //        System.Diagnostics.Debug.WriteLineIf(true, $"{ex}");
        //        //       return MasterbkList;
        //    }
        //}





        public static IEnumerable<Book> GetBookList()
        {
            var books = DataStoreList.Select(x => x.Book).Distinct();
            return books;
        }


    }

}
