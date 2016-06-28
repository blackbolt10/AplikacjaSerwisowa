using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SQLite;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AplikacjaSerwisowa
{
    [Activity(Label = "Kontrahenci", Icon = "@drawable/kontrahenci")]
    public class kontrahenci_Activity : Activity
    {
        private ListView listaKontrahentow;
        private Int32 mUkrywanieGidNmer = 1;

        private List<string> kna_gidnumer_List = new List<string>();
        private List<string> kna_akronim_List = new List<string>();
        private List<string> kna_nazwa1_List = new List<string>();
        private List<string> kna_nazwa2_List = new List<string>();
        private List<string> kna_nazwa3_List = new List<string>();
        private List<string> kna_kodp_List = new List<string>();
        private List<string> kna_miasto_List = new List<string>();
        private List<string> kna_ulica_List = new List<string>();
        private List<string> kna_adresy_List = new List<string>();
        private List<string> kna_nip_List = new List<string>();
        private List<string> kna_telefon1_List = new List<string>();
        private List<string> kna_telefon2_List = new List<string>();
        private List<string> kna_telefon3_List = new List<string>();
        private List<string> kna_telex_List = new List<string>();
        private List<string> kna_fax_List = new List<string>();
        private List<string> kna_email_List = new List<string>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.kontrahenci);

            listaKontrahentow = FindViewById<ListView>(Resource.Id.kontrahenciListView);
            listaKontrahentow.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e) { listaKontrahentowItemClick_Function(sender, e); };


            // Create your application here

            try
            {
                String dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);
                var table = db.Table<KntKartyTable>();

                foreach (var item in table)
                {
                    kna_gidnumer_List.Add(item.Knt_GIDNumer.ToString());
                    kna_akronim_List.Add(item.Knt_Akrnonim);
                    kna_nazwa1_List.Add(item.Knt_nazwa1);
                    kna_nazwa2_List.Add(item.Knt_nazwa2);
                    kna_nazwa3_List.Add(item.Knt_nazwa3);
                    kna_kodp_List.Add(item.Knt_KodP);
                    kna_miasto_List.Add(item.Knt_miasto);
                    kna_ulica_List.Add(item.Knt_ulica);
                    kna_adresy_List.Add(item.Knt_Adres);
                    kna_nip_List.Add(item.Knt_nip);
                    kna_telefon1_List.Add(item.Knt_telefon1);
                    kna_telefon2_List.Add(item.Knt_telefon2);
                    kna_telefon3_List.Add(item.Knt_telefon3);
                    kna_telex_List.Add(item.Knt_telex);
                    kna_fax_List.Add(item.Knt_fax);
                    kna_email_List.Add(item.Knt_email);
                }
            }
            catch (Exception exc)
            {
                messagebox("B³¹d kontrahenci_Activity.OnCreate():\n" + exc.Message, "B³¹d", 0);
            }            

            kntKarty_ListViewAdapter adapter;

            if (kna_gidnumer_List.Count > 0 && kna_akronim_List.Count > 0 )
            {
                adapter = new kntKarty_ListViewAdapter(this, kna_gidnumer_List, kna_akronim_List, kna_nazwa1_List, kna_nazwa2_List, kna_nazwa3_List, kna_kodp_List, kna_miasto_List, kna_ulica_List, kna_adresy_List, kna_nip_List, kna_telefon1_List, kna_telefon2_List, kna_telefon3_List, kna_telex_List, kna_fax_List, kna_email_List, mUkrywanieGidNmer);
            }
            else
            {
                adapter = new kntKarty_ListViewAdapter(this, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, mUkrywanieGidNmer);
            }
            listaKontrahentow.Adapter = adapter;
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
            kontrahentInformacjeActivity.PutExtra("kna_gidnumer", kna_gidnumer_List[e.Position]);
            StartActivity(kontrahentInformacjeActivity);
        }
    }
}