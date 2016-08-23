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
    public class noweZlecenieDodajUrzadzenie_Activit : Activity
    {
        private string czynnosc;
        private EditText filtrEditText;
        private ListView listaUrzadzen;
        private CheckBox wszystkieCheckBox;
        private Button dodajButton;
        private string filtr;

        private List<SrwUrzadzenia> urzadzeniaObjectLista;
        private int KNT_GIDNumer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.noweZlecenieDodawanieUrzadzeniaLayout);

            KNT_GIDNumer = -1;
            KNT_GIDNumer = noweZlecenie_Activity.Knt_GIDNumer;
            filtr = Intent.GetStringExtra("filtr") ?? "";

            filtrEditText = FindViewById<EditText>(Resource.Id.filtrUrzadzenNoweZlecenieTextView);
            filtrEditText.TextChanged += new EventHandler<TextChangedEventArgs>(FiltrEditText_TextChanged);

            listaUrzadzen = FindViewById<ListView>(Resource.Id.listaUrzadzenNoweZlecenieListView);
            listaUrzadzen.ItemClick += ListaKontrahentowListView_ItemClick;

            wszystkieCheckBox = FindViewById<CheckBox>(Resource.Id.UrzadzenNoweZlecenieCheckBox);
            wszystkieCheckBox.CheckedChange += WszystkieCheckBox_CheckedChange;
            if(KNT_GIDNumer == -1)
            {
                wszystkieCheckBox.Visibility = ViewStates.Gone;
            }
            else
            {
                wszystkieCheckBox.Visibility = ViewStates.Visible;
            }

            dodajButton = FindViewById<Button>(Resource.Id.UrzadzenNoweZlecenieButton);
            dodajButton.Click += DodajButton_Click;

            pobierzUrzadzenia();
        }

        private void DodajButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void WszystkieCheckBox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            pobierzUrzadzenia();
        }

        private void ListaKontrahentowListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            SrwUrzadzenia urzadzenie = urzadzeniaObjectLista[e.Position];
            zakladkaUrzadzeniaNoweZlecenie.dodajUrzadzenie(urzadzenie);

            this.Finish();
        }

        private void FiltrEditText_TextChanged(object sender, TextChangedEventArgs e)
        {
            pobierzUrzadzenia();
        }    

        private void pobierzUrzadzenia()
        {
            DBRepository dbr = new DBRepository();

            urzadzeniaObjectLista = new List<SrwUrzadzenia>();
            urzadzeniaObjectLista = dbr.SrwUrzadzenia_GetFilteredRecords(filtrEditText.Text, filtr, wszystkieCheckBox.Checked, KNT_GIDNumer);

            if(urzadzeniaObjectLista.Count > 0)
            {
                listaUrzadzenia_ListViewAdapter adapter = new listaUrzadzenia_ListViewAdapter(this, urzadzeniaObjectLista, false);
                listaUrzadzen.Adapter = adapter;
            }
            else
            {
                listaUrzadzen.Adapter = null;
            }
        }
    }



}