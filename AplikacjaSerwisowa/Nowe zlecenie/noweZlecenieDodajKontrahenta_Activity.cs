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
using Android.Views;
using Android.Widget;
using Android.Text;

namespace AplikacjaSerwisowa
{
    [Activity(Label = "Wybierz kontrahenta")]
    public class noweZlecenieDodajKontrahenta_Activity : Activity
    {
        private string Kna_GIDNumer;
        private string Knt_GIDNumer;
        private string glowny;
        private EditText filtrEditText;
        private ListView listaKontrahentowListView;
        private List<KntAdresyTable> kntAdresyList;
        private List<KntKartyTable> kntKartyList;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.noweZlecenieDodawanieKontrahetnaLayout);

            Kna_GIDNumer = Intent.GetStringExtra("Kna_GIDNumer") ?? "-1";
            Knt_GIDNumer = Intent.GetStringExtra("Knt_GIDNumer") ?? "-1";
            glowny = Intent.GetStringExtra("glowny") ?? "0";

            filtrEditText = FindViewById<EditText>(Resource.Id.filtrKontrahentNoweZlecenieTextView);
            filtrEditText.TextChanged += new EventHandler<TextChangedEventArgs>(FiltrEditText_TextChanged);
            listaKontrahentowListView = FindViewById<ListView>(Resource.Id.listaKontrahentNoweZlecenieListView);
            listaKontrahentowListView.ItemClick += ListaKontrahentowListView_ItemClick;

            if(glowny == "1")
            {
                pobierzKntKarty();
            }
            else
            {
                pobierzKntAdresy();
            }
        }

        private void ListaKontrahentowListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if(glowny == "1")
            {
                noweZlecenie_Activity.aktualizujKontrahentaGlownego("[" + kntKartyList[e.Position].Knt_Akronim + "]", kntKartyList[e.Position].Knt_nazwa1, kntKartyList[e.Position].Knt_GIDNumer);
            }
            else
            {
                noweZlecenie_Activity.aktualizujKontrahentaDocelowego("[" + kntAdresyList[e.Position].Kna_Akronim + "]", kntAdresyList[e.Position].Kna_nazwa1, kntAdresyList[e.Position].Kna_GIDNumer);
            }

            this.Finish();
        }

        private void FiltrEditText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(glowny == "1")
            {
                pobierzKntKarty();
            }
            else
            {
                pobierzKntAdresy();
            }
        }    

        private void pobierzKntKarty()
        {
            DBRepository dbr = new DBRepository();

            kntKartyList = new List<KntKartyTable>();
            kntKartyList = dbr.kntKarty_GetFilteredRecords(filtrEditText.Text);

            if(kntKartyList.Count > 0)
            {
                listaKontrahentow_ListViewAdapter adapter = new listaKontrahentow_ListViewAdapter(this, kntKartyList, null);
                listaKontrahentowListView.Adapter = adapter;
            }
            else
            {
                listaKontrahentowListView.Adapter = null;
            }
        }

        private void pobierzKntAdresy()
        {
            DBRepository dbr = new DBRepository();

            kntAdresyList = new List<KntAdresyTable>();
            kntAdresyList = dbr.kntAdresy_GetFilteredRecords(filtrEditText.Text, Knt_GIDNumer);

            if(kntAdresyList.Count > 0)
            {
                listaKontrahentow_ListViewAdapter adapter = new listaKontrahentow_ListViewAdapter(this, null, kntAdresyList);
                listaKontrahentowListView.Adapter = adapter;
            }
            else
            {
                listaKontrahentowListView.Adapter = null;
            }
        }
    }



}