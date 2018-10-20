using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KJVSharp
{
    public enum Testament { Old, New }
    public static class DataRetreiver
    {
       
        public static void MakeList(ref List<Verse> lst)
        {
            lst = new List<Verse>();
            //return  Task.Run<int>(() =>
            //{
            using (CsvFileReader reader = new CsvFileReader("/sdcard/Android/data/com.companyname.KJVSharp/files/_MyV.csv"))
            {
                Testament testament = Testament.Old;
                int i = 0;
                try
                {
                    CsvRow row = new CsvRow();
                    Book curBook = new Book() { BookName = "", };
                    //   Chapter curChap = new Chapter() { Number = "", /*Testament = testament, BibleIndex = context.Books.Count() + 1,*/ Oid = new Guid(GetRandomOid()) };
                    var curVer = new Verse() { Text = "", };
                    while (reader.ReadRow(row))
                    {
                        string[] segments = row.GetSegments();
                        Console.WriteLine($"{i}  {row }  {row }");
                        string verse =  segments[1].Replace("<i>", "").Replace("</i>", "");  //should be Book,  <i>was</i> ! ;
                        string cell = segments[0];
                        string book; string vnum; string ch;

                        try
                        {
                            if (GetBookChapterVerseNum(cell, out book, out ch, out vnum))
                            { //Book booktoadd = ;
                                StaticHelperMethods.WriteOut($"Book {book} {ch}:{vnum} {verse}");
                                testament = testament == Testament.Old && book.Contains("Matthew") ? Testament.New : testament;

                                if (curBook.BookName == book)
                                    StaticHelperMethods.WriteOut($"SAMWBOOK");
                                else
                                {

                                    curBook = new Book() { BookName = book, };
                                    //    curBook = new Book() { BookName = book, /*Testament = testament, BibleIndex = context.Books.Count() + 1,*/ Oid = new Guid(GetRandomOid())  };
                                }



                                curVer = new Verse()
                                {
                                    Book = curBook,
                                    Number = vnum,
                                    Chapter = ch,
                                    Text = verse,
                                    //    Length = verse.Length,
                                    //    NumWords = verse.Where(x => string.IsNullOrWhiteSpace(x.ToString())).Count()
                                };

                                lst.Add(curVer);

                            }
                        }
                        catch (Exception ex)
                        {
                            StaticHelperMethods.WriteOut($"Ex {ex}");
                            System.Diagnostics.Debug.WriteLineIf(true, $"{ex}");
                            //         return MasterbkList;
                        }
                    }// context.SaveChanges();
                     //   return ret;// context.SaveChanges();
                }
                catch (Exception ex)
                {
                    StaticHelperMethods.WriteOut($"Ex {ex}");
                    System.Diagnostics.Debug.WriteLineIf(true, $"{ex}");
                    //       return MasterbkList;
                }
            }
            var books = lst.Select(x => x.Book).Distinct();
            foreach (var item in books)
            {
                item.Verses.AddRange(lst.Where(x => x.Book == item));
            }
            return;
        }

        public static bool GetBookChapterVerseNum(string cell, out string book, out string ch, out string vnum)
        {
            bool beforeletters = true;
            book = ""; string chap_verse = "";
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
                    return false;
                }
            }
            catch (Exception ex)
            {
                ch = vnum = "0";
                System.Diagnostics.Debug.WriteLineIf(true, $"{ex}");
                return false;
            }
            return true;
        }

    }
}
