using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SQLite;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace AplikacjaSerwisowa
{
    public class zakladkaCzynnosciNoweZlecenie : Android.Support.V4.App.Fragment
    {
        private static Button mDodajButton, mUsunButton;
        private static ListView mListaListView;

        private static Context kontekst;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.noweZlecenieZakladkaCzynnSkladLayout, container, false);

            kontekst = noweZlecenie_Activity.GetContext();

            mDodajButton = view.FindViewById<Button>(Resource.Id.noweZlecenieZakladkaCzynnSkladDodajButton);
            mDodajButton.Click += MDodajButton_Click;

            mUsunButton = view.FindViewById<Button>(Resource.Id.noweZlecenieZakladkaCzynnSkladUsunButton);
            mUsunButton.Click += MUsunButton_Click;

            mListaListView = view.FindViewById<ListView>(Resource.Id.noweZlecenieZakladkaCzynnSkladListView);

            aktualizujListeCzynnosci();

            return view;
        }

        public static void aktualizujListeCzynnosci()
        {
            if(noweZlecenie_Activity.czynnosciList.Count > 0)
            {
                listaCzynnSklad_ListViewAdapter adapter = new listaCzynnSklad_ListViewAdapter(kontekst, noweZlecenie_Activity.czynnosciList, false, true);
                mListaListView.Adapter = adapter;
                mUsunButton.Enabled = true;
            }
            else
            {
                mListaListView.Adapter = null;
                mUsunButton.Enabled = false;
            }
        }

        private void MUsunButton_Click(object sender, EventArgs e)
        {
            for(int i = noweZlecenie_Activity.czynnosciList.Count - 1; i >= 0; i--)
            {
                if(noweZlecenie_Activity.czynnosciList[i].zaznaczone == true)
                {
                    noweZlecenie_Activity.czynnosciList.RemoveAt(i);
                }
            }
            aktualizujListeCzynnosci();
        }

        private void MDodajButton_Click(object sender, EventArgs e)
        {
            String filtr = ustawFiltr();

            Intent noweZlecenieCzynnosciIntent = new Intent(kontekst, typeof(noweZlecenieDodajCzynnSklad_Activit));
            noweZlecenieCzynnosciIntent.PutExtra("czynnosc", "0");
            noweZlecenieCzynnosciIntent.PutExtra("filtr", filtr);
            StartActivity(noweZlecenieCzynnosciIntent);
        }

        private string ustawFiltr()
        {
            string filtr = "-1";

            for(int i = 0;i<noweZlecenie_Activity.czynnosciList.Count;i++)
            {
                filtr += ", "+noweZlecenie_Activity.czynnosciList[i].Twr_GIDNumer;
            }

            return filtr;
        }

        public static void dodajCzynnosc(TwrKartyTable czynnosc)
        {
            noweZlecenie_Activity.czynnosciList.Add(czynnosc);
            aktualizujListeCzynnosci();
        }

        public static void aktualizujChecBox(Int32 position)
        {
            noweZlecenie_Activity.czynnosciList[position].zaznaczone = !noweZlecenie_Activity.czynnosciList[position].zaznaczone;

            sprawdzCzyGotoweDoUsuniecia();
        }

        private static void sprawdzCzyGotoweDoUsuniecia()
        {
            Boolean zaznaczony = false;
            for(int i = 0; i < noweZlecenie_Activity.czynnosciList.Count; i++)
            {
                if(noweZlecenie_Activity.czynnosciList[i].zaznaczone == true)
                {
                    zaznaczony = true;
                    break;
                }
            }

            if(zaznaczony)
            {
                mUsunButton.Enabled = true;
            }
            else
            {
                mUsunButton.Enabled = false;
            }
        }

        public static List<TwrKartyTable> pobierzListeCzynnosci()
        {
            return noweZlecenie_Activity.czynnosciList;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Czynności";
        }
    }
}