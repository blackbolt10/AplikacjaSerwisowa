using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AplikacjaSerwisowa
{
    [Activity(Label = "noweZlecenieDatePickerActivity")]
    public class noweZlecenieDatePickerActivity : Activity
    {
        private CalendarView calendarView;
        private string czyWystawienia;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.noweZlecenieDataPickerLayout);

            // Create your application here
            calendarView = FindViewById<CalendarView>(Resource.Id.noweZlecenieDateCalendarView);
            calendarView.DateChange += CalendarView_DateChange;

            czyWystawienia = Intent.GetStringExtra("DataWystawienia") ?? "1";
        }

        private void CalendarView_DateChange(object sender, CalendarView.DateChangeEventArgs e)
        {
            String dzien = e.DayOfMonth.ToString();
            if(dzien.Length == 1)
            {
                dzien = "0" + dzien;
            }

            String miesiac = (e.Month+1).ToString();
            if(miesiac.Length == 1)
            {
                miesiac = "0" + miesiac;
            }

            String data = e.Year.ToString()+"-"+ miesiac + "-"+dzien;

            if(czyWystawienia == "1")
            {
                zakladkaOgolneNoweZlecenie.aktualizujDate(1, data);
            }
            else
            {
                zakladkaOgolneNoweZlecenie.aktualizujDate(0, data);
            }
            this.Finish();
        }
    }
}