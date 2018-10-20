using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using KJVSharp.ViewModels;
///sdcard/android/data/
namespace KJVSharp
{
    public partial class MainPage : ContentPage
    {MainPageViewModel mpvm = new MainPageViewModel();
     
        public MainPage()
        {
            InitializeComponent();
            var mpvm = new MainPageViewModel();
            BindingContext = mpvm;
            bPicker.ItemsSource =(ObservableCollection<Book>)   mpvm.Books;
            vList.ItemsSource = mpvm.Verses;
        }

        private void vList_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            if (e == null) return; // has been set to null, do not 'process' tapped event
            MessagingCenter.Instance.Send(sender, ((ListView)sender).SelectedItem.ToString());
            Debug.WriteLine("Tapped: " + e.Item);
            ((ListView)sender).SelectedItem = null; // de-select the row


        }

        private void bPick_SelectedIndexChanged(object sender, EventArgs e)
        {
            // List<Verse> ml = new List<Verse>();
            //// StaticHelperMethods.MakeListpre(ref   ml ,ref myBList);
            // foreach (var item in (((Picker)sender).Items))
            // {
            //     Debug.WriteLine($"{item}");
            // }
            // myVList=ml.Where (x=> (((Picker)sender).Items.ElementAt(((Picker)sender).SelectedIndex))==x.Book.BookName).ToList();
        }
    }
}
