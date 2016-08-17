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
    public class zakladkaSkladnikiListaZlecenSerwisowychSzczegoly : Android.Support.V4.App.Fragment
    {
        private ListView SkladnikiListView;
        private Context kontekst;

        private string szn_ID;
        List<SrwZlcSkladniki> szcList;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.skladnikiListaZlecenSzczegoly, container, false);

            SkladnikiListView = view.FindViewById<ListView>(Resource.Id.skladnikiListaZlecenSzczegolyListView);
            SkladnikiListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e) { itemClick_Function(sender, e); };

            kontekst = listaZlecenSzczegoly_Activity.GetContext();
            szn_ID = listaZlecenSzczegoly_Activity.szn_ID;

            listaZlecenSzczegolySkladniki_ListViewAdapter adapter = przygotujAdapter();
            SkladnikiListView.Adapter = adapter;

            return view;
        }

        private listaZlecenSzczegolySkladniki_ListViewAdapter przygotujAdapter()
        {
            listaZlecenSzczegolySkladniki_ListViewAdapter adapter = null;
            szcList = new List<SrwZlcSkladniki>();

            try
            {
                DBRepository dbr = new DBRepository();
                szcList = dbr.SrwZlcSkladniki_GetRecords(szn_ID);
            }
            catch(Exception)
            {

            }

            if(szcList.Count > 0)
            {
                adapter = new listaZlecenSzczegolySkladniki_ListViewAdapter(kontekst, szcList);
            }

            return adapter;
        }

        private void itemClick_Function(object sender, AdapterView.ItemClickEventArgs e)
        {
            // throw new NotImplementedException();
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Sk³adniki";
        }
    }
}