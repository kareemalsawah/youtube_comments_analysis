using Microcharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;
using SkiaSharp;

namespace Analyzo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Charts_page : ContentPage
	{
		public Charts_page (float pos_ratio)
		{
			InitializeComponent ();
            float neg_ratio = 100 - pos_ratio;

            // Variable that is used to draw the pie chart
            List<Entry> entries = new List<Entry>
            {
                new Entry(pos_ratio)
                {
                    Color=SKColor.Parse("#819999"),
                    Label ="Positive",
                    ValueLabel = pos_ratio+""

                },
                new Entry(neg_ratio)
                {
                    Color = SKColor.Parse("#d22f32"),
                    Label = "Negative",
                    ValueLabel = neg_ratio + ""
                },
             };

            // Add data to the chart and make it visible
            Chart1.Chart = new DonutChart() { Entries = entries };
            Chart1.IsVisible = true;

            //Set the text of the labels that show the ratios between pos and neg as numbers
            positive_ratio.Text =Math.Floor(pos_ratio * 100)/100.0 + "%";
            negative_ratio.Text = 100-(Math.Floor(pos_ratio * 100) / 100.0 )+ "%";
        }
	}
}