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
    public class zakladkaUrzadzeniaListaZlecenSerwisowychSzczegoly : Android.Support.V4.App.Fragment
    {
        private ListView Urzadzenia_ListView;
        private Context kontekst;

        private string szn_ID;
        List<SrwZlcUrz> szuList;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.skladnikiListaZlecenSzczegoly, container, false);

            Urzadzenia_ListView = view.FindViewById<ListView>(Resource.Id.skladnikiListaZlecenSzczegolyListView);
            Urzadzenia_ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e) { itemClick_Function(sender, e); };

            kontekst = listaZlecenSzczegoly_Activity.GetContext();
            szn_ID = listaZlecenSzczegoly_Activity.szn_ID;

            listaZlecenSzczegolyUrzadzenia_ListViewAdapter adapter = przygotujAdapter();
            Urzadzenia_ListView.Adapter = adapter;

            return view;
        }

        private listaZlecenSzczegolyUrzadzenia_ListViewAdapter przygotujAdapter()
        {
            listaZlecenSzczegolyUrzadzenia_ListViewAdapter adapter = null;
            szuList = new List<SrwZlcUrz>();

            try
            {
                DBRepository dbr = new DBRepository();
                szuList = dbr.SrwZlcUrz_GetRecords(szn_ID);
            }
            catch(Exception)
            {

            }

            if(szuList.Count > 0)
            {
                adapter = new listaZlecenSzczegolyUrzadzenia_ListViewAdapter(kontekst, szuList);
            }

            return adapter;
        }

        private void itemClick_Function(object sender, AdapterView.ItemClickEventArgs e)
        {
            // throw new NotImplementedException();
        }
        
        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Urz¹dzenia";
        }
    }
}