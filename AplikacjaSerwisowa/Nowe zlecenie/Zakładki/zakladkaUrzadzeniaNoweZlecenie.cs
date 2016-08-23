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
    public class zakladkaUrzadzeniaNoweZlecenie : Android.Support.V4.App.Fragment
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

            aktualizujListeUrzadzen();

            return view;
        }

        public static void aktualizujListeUrzadzen()
        {
            if(noweZlecenie_Activity.urzadzeniaList.Count > 0)
            {
                listaUrzadzenia_ListViewAdapter adapter = new listaUrzadzenia_ListViewAdapter(kontekst, noweZlecenie_Activity.urzadzeniaList, true);
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
            for(int i = noweZlecenie_Activity.urzadzeniaList.Count - 1; i >= 0; i--)
            {
                if(noweZlecenie_Activity.urzadzeniaList[i].zaznaczone == true)
                {
                    noweZlecenie_Activity.urzadzeniaList.RemoveAt(i);
                }
            }
            aktualizujListeUrzadzen();
        }

        private void MDodajButton_Click(object sender, EventArgs e)
        {
            String filtr = ustawFiltr();

            Intent noweZlecenieUrzadzeniaIntent = new Intent(kontekst, typeof(noweZlecenieDodajUrzadzenie_Activit));
            noweZlecenieUrzadzeniaIntent.PutExtra("filtr", filtr);
            StartActivity(noweZlecenieUrzadzeniaIntent);
        }

        private string ustawFiltr()
        {
            string filtr = "-1";

            for(int i = 0;i<noweZlecenie_Activity.urzadzeniaList.Count;i++)
            {
                filtr += ", "+noweZlecenie_Activity.urzadzeniaList[i].SrU_Id;
            }

            return filtr;
        }

        public static void dodajUrzadzenie(SrwUrzadzenia urzadzenie)
        {
            noweZlecenie_Activity.urzadzeniaList.Add(urzadzenie);
            aktualizujListeUrzadzen();
        }

        public static void aktualizujChecBox(Int32 position)
        {
            noweZlecenie_Activity.urzadzeniaList[position].zaznaczone = !noweZlecenie_Activity.urzadzeniaList[position].zaznaczone;

            sprawdzCzyGotoweDoUsuniecia();
        }

        private static void sprawdzCzyGotoweDoUsuniecia()
        {
            Boolean zaznaczony = false;
            for(int i = 0; i < noweZlecenie_Activity.urzadzeniaList.Count; i++)
            {
                if(noweZlecenie_Activity.urzadzeniaList[i].zaznaczone == true)
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

        public static List<SrwUrzadzenia> pobierzListeUrzadzen()
        {
            return noweZlecenie_Activity.urzadzeniaList;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Urządzenia";
        }
    }
}