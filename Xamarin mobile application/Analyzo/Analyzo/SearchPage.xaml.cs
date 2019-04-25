using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Refit;
using Microcharts;
using Microcharts.Forms;
using Entry = Microcharts.Entry;
using SkiaSharp;

namespace Analyzo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchPage : ContentPage
	{

        

        public SearchPage ()
		{
			InitializeComponent ();

        }

        private async void Search_Clicked(object sender, EventArgs e)
        {
            loading_label.IsVisible = true;

            //Get the video id from the pasted url
            string videoURL = text_entry.Text,sID=""; int st = 0; 
            for (int i = 0; i < videoURL.Length; i++)
            {
                if ((videoURL[i] == '?' && videoURL[i + 1] == 'v' )|| (videoURL[i] == '&' && videoURL[i + 1] == 'v'))
                {
                    i+=3;
                    st = 1;
                }
                if (st==1)
                {
                    sID += videoURL[i]; 
                }
                if (videoURL[i] == '&') break;

            }
            
            string VideoID = sID; //Contains the video id to send to the server

            IAnalysisApi analysis_api = ApiService.getApiService();

            // data is the dictionary to send to the server in the post request
            var data = new Dictionary<string, object> {
                 {"num_of_comments", 200}
             };
            data.Add("video_id", VideoID);

            //Send the request to the server
            await analysis_api.getAnalysis(data).ContinueWith(post =>
            {
                if (post.IsCompleted && post.Status == TaskStatus.RanToCompletion)
                {
                    // Get result and update any UI here.
                    var post_res = post.Result;
                    float pos_ratio = (float) post_res.answer * 100;

                    Navigation.PushModalAsync(new Charts_page(pos_ratio)); //Switch to the page with the pie chart
                    loading_label.IsVisible = false; // Hide loading label
                }
                else if (post.IsFaulted)
                {
                    // If any error occurred exception throws.
                    this.DisplayAlert("Error", "post fault, please contact the developers and inform them of this error.", "Ok");
                }
                else if (post.IsCanceled)
                {
                    // Task cancelled
                    this.DisplayAlert("Error", "post cancelled, please contact the developers and inform them of this error.", "Ok");
                }
            }, TaskScheduler.FromCurrentSynchronizationContext())// execute in main/UI thread.
             .ConfigureAwait(false);
        }
    }
}