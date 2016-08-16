using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AplikacjaSerwisowa
{
    public class AdresyKontrahenta : Android.Support.V4.App.Fragment
    {
        private ListView mkontrahecniAdresyListView;
        private Context kontekstGlowny;
        private String mKNT_GIDNumer;
        private Int32 mUkrywanieGidNmer = 1;
        private List<KntAdresyTable> kntAdresyList;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Frag2Layout, container, false);

            mKNT_GIDNumer = kontrahenciInformacje.GetKnt_GidNumer();
            kontekstGlowny = kontrahenciInformacje.GetContext();

            mkontrahecniAdresyListView = view.FindViewById<ListView>(Resource.Id.kontrahecniAdresyFrag2ListView);
            mkontrahecniAdresyListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e) { itemClick_Function(sender, e); };

            kntAdresyList = new List<KntAdresyTable>();
            try
            {
                DBRepository dbr = new DBRepository();
                kntAdresyList = dbr.kntAdresy_GetRecords(mKNT_GIDNumer);
            }
            catch(Exception exc)
            {
                messagebox("B³¹d Adresy.PobierzAdresy():\n" + exc.Message, "B³¹d", 0);
            }

            kontrahenciAdresy_ListViewAdapter adapter;

            if(kntAdresyList.Count > 0)
            {
                adapter = new kontrahenciAdresy_ListViewAdapter(kontekstGlowny, kntAdresyList, mUkrywanieGidNmer);
            }
            else
            {
                adapter = null;
            }

            mkontrahecniAdresyListView.Adapter = adapter;

            return view;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Adresy";
        }

        private void itemClick_Function(object sender, AdapterView.ItemClickEventArgs e)
        {
            ListView test = (ListView)sender;
            String test1 = test.GetItemAtPosition(Convert.ToInt32(e.Id)).ToString();
        }

        private void messagebox(String tekst, String tytul = "", Int32 icon = 1)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(kontekstGlowny);

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
    }
}