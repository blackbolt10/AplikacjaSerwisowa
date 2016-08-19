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
    [Activity(Label = "Wybór daty")]
    public class noweZlecenieDatePickerActivity : Activity
    {
        private CalendarView calendarView;
        private Button zapiszButton;
        private TextView dataTextView;
        private string czyWystawienia;
        private long minDate;
        private string wybranaData;
        private string DataWystawieniaString;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.noweZlecenieDataPickerLayout);

            // Create your application here
            calendarView = FindViewById<CalendarView>(Resource.Id.noweZlecenieDateCalendarView);
            calendarView.DateChange += CalendarView_DateChange;

            zapiszButton = FindViewById<Button>(Resource.Id.NoweZlecenieDatePickerZapiszButton);
            zapiszButton.Click += ZapiszButton_Click;
            dataTextView = FindViewById<TextView>(Resource.Id.NoweZlecenieDatePickerDataTextView);

            wybranaData = "";

            czyWystawienia = Intent.GetStringExtra("czyWystawienia") ?? "1";
            DataWystawieniaString = Intent.GetStringExtra("dataWystawienia") ?? "";

            if(czyWystawienia != "1" && DataWystawieniaString != "")
            {
                aktualizujMinDateRealizacji(DataWystawieniaString);
            }

            ustawDateWybrana();        
        }

        private void ustawDateWybrana()
        {
            TimeSpan time = TimeSpan.FromMilliseconds(calendarView.Date);
            DateTime dataWybrana = new DateTime(1970, 1, 1) + time;

            String dzien = dataWybrana.Day.ToString();
            if(dzien.Length == 1)
            {
                dzien = "0" + dzien;
            }

            String miesiac = dataWybrana.Month.ToString();
            if(miesiac.Length == 1)
            {
                miesiac = "0" + miesiac;
            }

            wybranaData = dataWybrana.Year.ToString() + "-" + miesiac + "-" + dzien;
            dataTextView.Text = "Wybrano datę: " + wybranaData;
        }

        private void aktualizujMinDateRealizacji(string dataWystawieniaString)
        {
            String[] dataWystawienia = dataWystawieniaString.Split('-',' ');
            DateTime data = new DateTime(Convert.ToInt32(dataWystawienia[0]), Convert.ToInt32(dataWystawienia[1]), Convert.ToInt32(dataWystawienia[2]), 0,0,0);
            DateTime dataFirst = new DateTime(1970, 01, 01, 00, 00, 00);

            minDate = (long)((data- dataFirst).TotalMilliseconds);
        }

        private void CalendarView_DateChange(object sender, CalendarView.DateChangeEventArgs e)
        {
            String dzien = e.DayOfMonth.ToString();
            if(dzien.Length == 1)
            {
                dzien = "0" + dzien;
            }

            String miesiac = (e.Month + 1).ToString();
            if(miesiac.Length == 1)
            {
                miesiac = "0" + miesiac;
            }

            wybranaData = e.Year.ToString() + "-" + miesiac + "-" + dzien;

            dataTextView.Text = "Wybrano datę: " + wybranaData;            
        }

        private void ZapiszButton_Click(object sender, EventArgs e)
        {
            if(calendarView.Date >= minDate)
            {
                if(czyWystawienia == "1")
                {
                    zakladkaOgolneNoweZlecenie.aktualizujDate(1, wybranaData);
                }
                else
                {
                    zakladkaOgolneNoweZlecenie.aktualizujDate(0, wybranaData);
                }
                this.Finish();
            }
            else
            {
                Toast.MakeText(this, "Data realizacji nie może być wcześniej niż data wystawienia ("+ DataWystawieniaString + ")", ToastLength.Short).Show();
            }
        }
    }
}