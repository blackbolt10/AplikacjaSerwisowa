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

namespace AplikacjaSerwisowa
{
    [Activity(Label = "Lista zleceñ", Icon = "@drawable/lista_zlecen")]
    public class listaZlecen_Activity : Activity 
    {
        private ListView listaZlecen_ListView;
        private List<String> kontrahenci_List;
        private List<String> stan_List;
        private List<String> data_list;
        private List<String> naglowki_list;
        private List<String> wykonanie_list;
        private List<String> szn_ID_list;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.listaZlecen);

            listaZlecen_ListView = FindViewById<ListView>(Resource.Id.listView1);
            listaZlecen_ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e) { itemClick_Function(sender,e); };

            kontrahenci_List = new List<string>();
            stan_List = new List<string>();
            data_list = new List<string>();
            naglowki_list = new List<string>();
            wykonanie_list = new List<string>();
            szn_ID_list = new List<string>();
            
            try
            {
                String dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);
                var table = db.Table<SrwZlcNagTable>();

                foreach(var item in table)
                {
                    string wynik = "";
                    string kntKartaNazwa = pobierzInformacjeOKntKarta(item.SZN_KntNumer);
                    string kntAdresNazwa = pobierzInformacjeOKntKarta(item.SZN_KnDNumer);

                    if(kntKartaNazwa!="" && kntAdresNazwa!="")
                    {
                        if(kntKartaNazwa == kntAdresNazwa)
                        {
                            wynik = kntKartaNazwa;
                        }
                        else
                        {
                            wynik = kntKartaNazwa + "\n" + kntAdresNazwa;
                        }
                    }
                    else if(kntKartaNazwa != "" && kntAdresNazwa == "")
                    {
                        wynik = kntKartaNazwa;
                    }
                    else if(kntKartaNazwa == "" && kntAdresNazwa != "")
                    {
                        wynik = kntAdresNazwa;
                    }
                    else 
                    {
                        wynik = "{brak nazwy}";
                    }

                    kontrahenci_List.Add(wynik);
                    data_list.Add(item.SZN_DataWystawienia.Split(' ')[0]);
                    stan_List.Add(item.SZN_Stan);
                    naglowki_list.Add(item.Dokument);
                    szn_ID_list.Add(item.SZN_Id.ToString());

                    Random test = new Random();
                    test.Next(0, 1);
                    wykonanie_list.Add(test.Next(0, 1).ToString());
                }
            }
            catch(Exception exc)
            {
                messagebox("B³¹d listaZlecen_Activity.OnCreate():\n" + exc.Message, "B³¹d", 0);
            }

            listaZlecen_ListViewAdapter adapter;

            if(kontrahenci_List.Count > 0 && stan_List.Count > 0)
            {
                adapter = new listaZlecen_ListViewAdapter(this, kontrahenci_List, stan_List, naglowki_list, data_list, wykonanie_list);
            }
            else
            {
                adapter = new listaZlecen_ListViewAdapter(this, null, null, null, null, wykonanie_list);
            }

            listaZlecen_ListView.Adapter = adapter;
        }

        private String pobierzInformacjeOKntKarta(int sZN_KnANumer)
        {
            KntKartyTable kntKarta = new KntKartyTable();

            try
            {
                DBRepository dbr = new DBRepository();
                kntKarta = dbr.kntKarty_GetRecord(sZN_KnANumer.ToString());
            }
            catch(Exception exc)
            {
                Toast.MakeText(this, "B³¹d listaZlecen_Activity.pobierzInformacjeOKntKarta():\n" + exc.Message, ToastLength.Short);
            }
            return kntKarta.Knt_nazwa1;
        }

        private String pobierzInformacjeOKntAdres(int sZN_AdWNumer)
        {
            KntAdresyTable kntAdres = new KntAdresyTable();

            try
            {
                DBRepository dbr = new DBRepository();
                kntAdres = dbr.kntAdresy_GetRecord(sZN_AdWNumer.ToString());
            }
            catch(Exception exc)
            {
                Toast.MakeText(this, "B³¹d listaZlecen_Activity.pobierzInformacjeOKntAdres():\n" + exc.Message, ToastLength.Short);
            }
           
            return kntAdres.Kna_nazwa1;
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

        private void itemClick_Function(object sender, AdapterView.ItemClickEventArgs e)
        {
            /*ListView test = (ListView) sender;
            String test1 = test.GetItemAtPosition(Convert.ToInt32(e.Id)).ToString();
            String gidnumer = szn_ID_list[Convert.ToInt32(e.Id)];
            Toast.MakeText(this, e.Id.ToString()+"\n"+ test1+"\n"+ gidnumer, ToastLength.Short).Show();
            */

            Intent listaZlecenSzczegolyIntent = new Intent(this, typeof(listaZlecenSzczegoly_Activity));
            listaZlecenSzczegolyIntent.PutExtra("szn_ID", szn_ID_list[e.Position]);
            StartActivity(listaZlecenSzczegolyIntent);
        }
    }
}