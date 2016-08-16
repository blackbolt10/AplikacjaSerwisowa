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
    [Activity(Label = "Kontrahenci", Icon = "@drawable/kontrahenci")]
    public class kontrahenci_Activity : Activity
    {
        private ListView listaKontrahentow;
        private Button usunButton;
        private EditText filtrEditText;

        private List<KntKartyTable> kntKartyList;

        private Int32 mUkrywanieGidNmer = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.kontrahenci);

            listaKontrahentow = FindViewById<ListView>(Resource.Id.kontrahenciListView);
            listaKontrahentow.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e) { listaKontrahentowItemClick_Function(sender, e); };

            usunButton = FindViewById<Button>(Resource.Id.filtrKontrahenciButton);
            usunButton.Click += UsunButton_Click;

            filtrEditText = FindViewById<EditText>(Resource.Id.filtrKontrahenciTextView);
            filtrEditText.TextChanged += new EventHandler<TextChangedEventArgs>(FiltrEditText_TextChanged);

            // Create your application here

            generujListViewAdapter();
        }

        private void generujListViewAdapter()
        {
            kntKartyList = new List<KntKartyTable>();
            try
            {
                DBRepository dbr = new DBRepository();
                kntKartyList = dbr.kntKarty_GetFilteredRecords(filtrEditText.Text);
            }
            catch(Exception exc)
            {
                messagebox("B³¹d kontrahenci_Activity.generujListViewAdapter():\n" + exc.Message, "B³¹d", 0);
            }

            kntKarty_ListViewAdapter adapter;

            if(kntKartyList.Count >0)
            {
                adapter = new kntKarty_ListViewAdapter(this, kntKartyList, mUkrywanieGidNmer);
            }
            else
            {
                adapter = null;
            }

            listaKontrahentow.Adapter = adapter;
        }

        private void FiltrEditText_TextChanged(object sender, TextChangedEventArgs e)
        {
            generujListViewAdapter();
        }

        private void UsunButton_Click(object sender, EventArgs e)
        {
            filtrEditText.Text = "";
        }

        private void messagebox(String tekst, String tytul = "", Int32 icon = 1)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            if(icon == 0)
            {
                alert.SetIconAttribute(Android.Resource.Attribute.AlertDialogIcon);
            }

            alert.SetTitle(tytul);
            alert.SetMessage(tekst);
            alert.SetPositiveButton("OK", (senderAlert, args) => { });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void listaKontrahentowItemClick_Function(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent kontrahentInformacjeActivity= new Intent(this, typeof(kontrahenciInformacje));
            kontrahentInformacjeActivity.PutExtra("kna_gidnumer", kntKartyList[e.Position].Knt_GIDNumer.ToString());
            StartActivity(kontrahentInformacjeActivity);
        }
    }
}