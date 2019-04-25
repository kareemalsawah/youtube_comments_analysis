using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Analyzo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Start_Clicked(object sender, EventArgs e)
        {
            //Navigate to the search page
            Navigation.PushModalAsync(new SearchPage());
        }
    }
}
