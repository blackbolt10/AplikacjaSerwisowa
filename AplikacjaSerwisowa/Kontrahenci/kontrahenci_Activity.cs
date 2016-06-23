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
        ListView listaTowarow;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.kontrahenci);

            listaTowarow = FindViewById<ListView>(Resource.Id.kartyTowarowListView);
            listaTowarow.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e) { };// itemClick_Function(sender, e); };

            List<string> twr_kod_List = new List<string>();
            List<string> twr_gidnumer_List = new List<string>();
            List<string> twr_typ_List = new List<string>();
            List<string> twr_nazwa_List = new List<string>();

            // Create your application here

            try
            {
                String dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);
                var table = db.Table<KntKarty>();

                foreach (var item in table)
                {
                   /* twr_gidnumer_List.Add(item.TWR_GIDNumer.ToString());
                    twr_kod_List.Add(item.TWR_Kod);
                    twr_nazwa_List.Add(item.TWR_Nazwa);
                    twr_typ_List.Add(item.TWR_Typ.ToString());*/
                }
            }
            catch (Exception exc)
            {
                messagebox("B³¹d magazyn_Activity.OnCreate():\n" + exc.Message, "B³¹d", 0);
            }


            if (twr_gidnumer_List.Count > 0 && twr_nazwa_List.Count > 0 && twr_kod_List.Count > 0 && twr_typ_List.Count > 0)
            {
                kartyTowarow_ListViewAdapter adapter = new kartyTowarow_ListViewAdapter(this, twr_kod_List, twr_gidnumer_List, twr_typ_List, twr_nazwa_List, 0);
                listaTowarow.Adapter = adapter;
            }
            else
            {
                kartyTowarow_ListViewAdapter adapter = new kartyTowarow_ListViewAdapter(this, null, null, null, null, 0);
                listaTowarow.Adapter = adapter;
            }
        }
        private void messagebox(String tekst, String tytul = "", Int32 icon = 1)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            if (icon == 0)
            {
                alert.SetIconAttribute(Android.Resource.Attribute.AlertDialogIcon);
            }

            alert.SetTitle(tytul);
            alert.SetMessage(tekst);
            alert.SetPositiveButton("OK", (senderAlert, args) => { });

            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}