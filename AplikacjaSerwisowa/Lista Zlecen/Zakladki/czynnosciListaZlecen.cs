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
    public class zakladkaCzynnosciListaZlecenSerwisowychSzczegoly : Android.Support.V4.App.Fragment
    {
        private ListView czynnosciListView;
        private Context kontekst;

        private string szn_ID;
        List<SrwZlcCzynnosci> szcList;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.czynnosciListaZlecenSzczegoly, container, false);

            czynnosciListView = view.FindViewById<ListView>(Resource.Id.czynnosciListaZlecenSzczegolyListView);
            czynnosciListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e) { itemClick_Function(sender, e); };

            kontekst = listaZlecenSzczegoly_Activity.GetContext();
            szn_ID = listaZlecenSzczegoly_Activity.szn_ID;

            listaZlecenSzczegolyCzynnosci_ListViewAdapter adapter = przygotujAdapter();
            czynnosciListView.Adapter = adapter;

            return view;
        }

        private listaZlecenSzczegolyCzynnosci_ListViewAdapter przygotujAdapter()
        {
            listaZlecenSzczegolyCzynnosci_ListViewAdapter adapter = null;
            szcList = new List<SrwZlcCzynnosci>();


            try
            {
                DBRepository dbr = new DBRepository();
                szcList = dbr.SrwZlcCzynnosci_GetRecords(szn_ID);
            }
            catch(Exception)
            {

            }

            if(szcList.Count > 0)
            {
                adapter = new listaZlecenSzczegolyCzynnosci_ListViewAdapter(kontekst, szcList);
            }

            return adapter;
        }

        private void itemClick_Function(object sender, AdapterView.ItemClickEventArgs e)
        {
            // throw new NotImplementedException();
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Czynnoœci";
        }
    }
}