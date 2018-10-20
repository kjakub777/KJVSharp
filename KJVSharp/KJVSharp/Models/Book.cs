using System.Collections.Generic;
///sdcard/android/data/
namespace KJVSharp
{
    public class Book : Base
    {
        List<Verse> _Verses;
        public override string ToString()
        {
            return string.Format("{0}", BookName);
        }
        public string BookName { get; set; }
        public List<Verse> Verses { get => _Verses!=null?_Verses:(_Verses=new List<Verse>()); set => _Verses = value; }


    }
}
