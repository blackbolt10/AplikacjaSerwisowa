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
    public class zakladkaKontrahentNoweZlecenie : Android.Support.V4.App.Fragment
    {
        private static Button mGlownyButton, mDocelowyButton;
        private static LinearLayout mGlownyLinearLayout, mDocelowyLinearLayout;
        private static TextView mAkronimGlownyTextView, mNazwaGlownyTextView;
        private static TextView mAkronimDocelowyTextView, mNazwaDocelowyTextView;


        Context kontekst;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.noweZlecenieZakladkaKontrahentLayout, container, false);

            kontekst = noweZlecenie_Activity.GetContext();

            mGlownyButton = view.FindViewById<Button>(Resource.Id.dodajGlownyNoweZlecenieButton);
            mGlownyButton.Click += MGlownyButton_Click;

            mGlownyLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.GlownyNoweZlecenieLinearLayout);

            mAkronimGlownyTextView = view.FindViewById<TextView>(Resource.Id.akronimGlownyNoweZlecenieTextView);
            mNazwaGlownyTextView = view.FindViewById<TextView>(Resource.Id.nazwaGlownyNoweZlecenieTextView);
            mAkronimGlownyTextView.Text = noweZlecenie_Activity.akronimGlowny;
            mNazwaGlownyTextView.Text = noweZlecenie_Activity.NazwaGlowny;

            mDocelowyButton = view.FindViewById<Button>(Resource.Id.dodajDocelowyNoweZlecenieButton);
            mDocelowyButton.Click += MDocelowyButton_Click;

            mDocelowyLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.DocelowyNoweZlecenieLinearLayout);

            mAkronimDocelowyTextView = view.FindViewById<TextView>(Resource.Id.akronimDocelowyNoweZlecenieTextView);
            mNazwaDocelowyTextView = view.FindViewById<TextView>(Resource.Id.nazwaDocelowyNoweZlecenieTextView);
            mAkronimDocelowyTextView.Text = noweZlecenie_Activity.akronimDocelowy;
            mNazwaDocelowyTextView.Text = noweZlecenie_Activity.NazwaDocelowy;

            ustawWidocznosc();

            return view;
        }

        private void MGlownyButton_Click(object sender, EventArgs e)
        {
            Intent noweZlecenieDodajKontrahentGlowny = new Intent(kontekst, typeof(noweZlecenieDodajKontrahenta_Activity));
            noweZlecenieDodajKontrahentGlowny.PutExtra("glowny", "1");
            StartActivity(noweZlecenieDodajKontrahentGlowny);

            noweZlecenie_Activity.Kna_GIDNumer = -1;
            noweZlecenie_Activity.akronimDocelowy = "";
            noweZlecenie_Activity.NazwaDocelowy = "";
        }
        private void MDocelowyButton_Click(object sender, EventArgs e)
        {
            Intent listaZlecenSzczegolyIntent = new Intent(kontekst, typeof(noweZlecenieDodajKontrahenta_Activity));
            listaZlecenSzczegolyIntent.PutExtra("glowny", "0");
            listaZlecenSzczegolyIntent.PutExtra("Knt_GIDNumer", noweZlecenie_Activity.Knt_GIDNumer.ToString());
            StartActivity(listaZlecenSzczegolyIntent);
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Kontrahent";
        }

        private static void ustawWidocznosc()
        {
            if(noweZlecenie_Activity.Knt_GIDNumer == -1)
            {
                mGlownyLinearLayout.Visibility = ViewStates.Gone;
                mDocelowyButton.Enabled = false;
            }
            else
            {
                mGlownyLinearLayout.Visibility = ViewStates.Visible;
                mDocelowyButton.Enabled = true;
                mAkronimGlownyTextView.Text = noweZlecenie_Activity.akronimGlowny;
                mNazwaGlownyTextView.Text = noweZlecenie_Activity.NazwaGlowny;
            }

            if(noweZlecenie_Activity.Kna_GIDNumer == -1)
            {
                mDocelowyLinearLayout.Visibility = ViewStates.Gone;
            }
            else
            {
                mDocelowyLinearLayout.Visibility = ViewStates.Visible;
                mAkronimDocelowyTextView.Text = noweZlecenie_Activity.akronimDocelowy;
                mNazwaDocelowyTextView.Text = noweZlecenie_Activity.NazwaDocelowy;
            }
       } 

        public static void aktualizujKontrahentaGlownego()
        {
            ustawWidocznosc();
        }

        public static void aktualizujKontrahentaDocelowego()
        {
            ustawWidocznosc();
        }

        public static List<Int32> pobierzKontrahentow()
        {
            List<Int32> lista = new List<int>();

            if(noweZlecenie_Activity.Knt_GIDNumer != -1 && noweZlecenie_Activity.Kna_GIDNumer != -1)
            {
                lista.Add(noweZlecenie_Activity.Knt_GIDNumer);
                lista.Add(noweZlecenie_Activity.Kna_GIDNumer);
            }
            else if(noweZlecenie_Activity.Knt_GIDNumer == -1 && noweZlecenie_Activity.Kna_GIDNumer == -1)
            {
                lista.Add(0); // 1 - brak obu knt
            }
            else if(noweZlecenie_Activity.Knt_GIDNumer != -1 && noweZlecenie_Activity.Kna_GIDNumer == -1)
            {
                lista.Add(2); // 2- brak noweZlecenie_Activity.Kna_GIDNumer
            }
            else if(noweZlecenie_Activity.Knt_GIDNumer == -1 && noweZlecenie_Activity.Kna_GIDNumer != -1)
            {
                lista.Add(1);// 1- brak noweZlecenie_Activity.Knt_GIDNumer
            }

            return lista;
        }
    }
}