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
    [Activity(Label = "Wybierz")]
    public class noweZlecenieDodajCzynnSklad_Activit : Activity
    {
        private string czynnosc;
        private EditText filtrEditText;
        private ListView listaCzynnSklad;
        private string filtr;

        private List<TwrKartyTable> twrKartyObjectLista;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.noweZlecenieDodawanieKontrahetnaLayout);
            
            czynnosc = Intent.GetStringExtra("czynnosc") ?? "0";
            filtr = Intent.GetStringExtra("filtr") ?? "";

            filtrEditText = FindViewById<EditText>(Resource.Id.filtrKontrahentNoweZlecenieTextView);
            filtrEditText.TextChanged += new EventHandler<TextChangedEventArgs>(FiltrEditText_TextChanged);
            listaCzynnSklad = FindViewById<ListView>(Resource.Id.listaKontrahentNoweZlecenieListView);
            listaCzynnSklad.ItemClick += ListaKontrahentowListView_ItemClick;

            if(czynnosc == "0")
            {
                pobierzCzynnosci();
            }
            else
            {
                pobierzSkladniki();
            }
        }

        private void ListaKontrahentowListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            TwrKartyTable twrKartyObject = twrKartyObjectLista[e.Position];

            if(czynnosc == "0")
            {
                zakladkaCzynnosciNoweZlecenie.dodajCzynnosc(twrKartyObject);
            }
            else
            {
                zakladkaSkladnikiNoweZlecenie.dodajSkladnik(twrKartyObject);
            }

            this.Finish();
        }

        private void FiltrEditText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(czynnosc == "0")
            {
                pobierzCzynnosci();
            }
            else
            {
                pobierzSkladniki();
            }
        }    

        private void pobierzCzynnosci()
        {
            DBRepository dbr = new DBRepository();

            twrKartyObjectLista = new List<TwrKartyTable>();
            twrKartyObjectLista = dbr.TwrKartyTable_GetFilteredRecords(filtrEditText.Text, filtr, true);

            if(twrKartyObjectLista.Count > 0)
            {
                listaCzynnSklad_ListViewAdapter adapter = new listaCzynnSklad_ListViewAdapter(this, twrKartyObjectLista, true, true);
                listaCzynnSklad.Adapter = adapter;
            }
            else
            {
                listaCzynnSklad.Adapter = null;
            }
        }

        private void pobierzSkladniki()
        {
            DBRepository dbr = new DBRepository();

            twrKartyObjectLista = new List<TwrKartyTable>();
            twrKartyObjectLista = dbr.TwrKartyTable_GetFilteredRecords(filtrEditText.Text, filtr, false);

            if(twrKartyObjectLista.Count > 0)
            {
                listaCzynnSklad_ListViewAdapter adapter = new listaCzynnSklad_ListViewAdapter(this, twrKartyObjectLista, true, false);
                listaCzynnSklad.Adapter = adapter;
            }
            else
            {
                listaCzynnSklad.Adapter = null;
            }
        }
    }



}