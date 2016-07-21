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
    public class zakladkaSkladnikiNoweZlecenie : Android.Support.V4.App.Fragment
    {
        private static Button mDodajButton, mUsunButton;
        private static ListView mListaListView;

        private static List<TwrKartyTable> skladnikiList;

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

            skladnikiList = new List<TwrKartyTable>();
            aktualizujListeSkladnikow();

            return view;
        }

        private static void aktualizujListeSkladnikow()
        {
            if(skladnikiList.Count > 0)
            {
                listaCzynnSklad_ListViewAdapter adapter = new listaCzynnSklad_ListViewAdapter(kontekst, skladnikiList, false, false);
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
            for(int i = skladnikiList.Count - 1; i >= 0; i--)
            {
                if(skladnikiList[i].zaznaczone == true)
                {
                    skladnikiList.RemoveAt(i);
                }
            }
            aktualizujListeSkladnikow();
        }

        private void MDodajButton_Click(object sender, EventArgs e)
        {
            String filtr = ustawFiltr();

            Intent noweZlecenieCzynnosciIntent = new Intent(kontekst, typeof(noweZlecenieDodajCzynnSklad_Activit));
            noweZlecenieCzynnosciIntent.PutExtra("czynnosc", "1");
            noweZlecenieCzynnosciIntent.PutExtra("filtr", filtr);
            StartActivity(noweZlecenieCzynnosciIntent);
        }
        private string ustawFiltr()
        {
            string filtr = "-1";

            for(int i = 0; i < skladnikiList.Count; i++)
            {
                filtr += ", " + skladnikiList[i].Twr_GIDNumer;
            }

            return filtr;
        }

        public static void dodajSkladnik(TwrKartyTable skladnik)
        {
            skladnikiList.Add(skladnik);
            aktualizujListeSkladnikow();
        }

        public static void aktualizujChecBox(Int32 position)
        {
            skladnikiList[position].zaznaczone = !skladnikiList[position].zaznaczone;

            sprawdzCzyGotoweDoUsuniecia();
        }

        private static void sprawdzCzyGotoweDoUsuniecia()
        {
            Boolean zaznaczony = false;
            for(int i = 0; i < skladnikiList.Count; i++)
            {
                if(skladnikiList[i].zaznaczone == true)
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

        public static void ustawIlosc(double ilosc)
        {
            skladnikiList[skladnikiList.Count - 1].Ilosc = ilosc;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Składniki";
        }
    }
}