using System;
using System.Linq ;
using System.Linq ;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace KJVSharp.ViewModels
{


    public class MainPageViewModel : ViewModelBase
    {
        private System.Collections.ObjectModel.ObservableCollection<Book> bs;
        private List<Verse> vs;
        public ObservableCollection<Book> Books
        {
            get
            {
                if (bs == null)
                {
                    bs = new ObservableCollection<Book>(StaticHelperMethods.GetBookList());
                }
                return bs;

            }
        }
        public IList<Verse> Verses
        {
            get
            {
                if (vs == null)
                {
                    vs = StaticHelperMethods.DataStoreList.ToList();
                }
                return vs;
            }
            private set
            {
                vs.Clear();
                vs.AddRange(value);
                OnPropertyChanged();
            }
        }


        Book selectedBook;
        public Book SelectedBook
        {
            get { return selectedBook; }
            set
            {
                if (selectedBook != value)
                {
                    selectedBook = value;
                    OnPropertyChanged();
                }
                Verses = StaticHelperMethods.DataStoreList.Where(x => x.Book.BookName == selectedBook.BookName).ToList();
            }
        }
    }
}