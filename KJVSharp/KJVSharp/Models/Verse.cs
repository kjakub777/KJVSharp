///sdcard/android/data/
namespace KJVSharp
{
    public class Verse : Base
    {
        private string _text;
        private string _number;
        private string _chapter;
        private Book book;
        public string Text { get => _text; set => _text = value; }
        public string Number { get => _number; set => _number = value; }
        public string Chapter { get => _chapter; set => _number = value; }
        public Book Book { get => book; set => book = value; }

        public override string ToString()
        {
            return  string.Join(" ",(new string[] {Book.BookName, Chapter, Number, Text}));
        }
    }
}
