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
    [Activity(Label = "Lista zleceñ", Icon = "@drawable/lista_zlecen")]
    public class listaZlecen_Activity : Activity 
    {
        private ListView listaZlecen_ListView;
        private List<string> mItems;
        private List<string> wykonane_List;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.listaZlecen);

            listaZlecen_ListView = FindViewById<ListView>(Resource.Id.listView1);
            listaZlecen_ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e) { itemClick_Function(sender,e); };

            mItems = new List<string>();
            mItems.Add("test");
            mItems.Add("test1");
            mItems.Add("test2");
            mItems.Add("test3");
            mItems.Add("test4");
            mItems.Add("test5");
            mItems.Add("test6");
            mItems.Add("test7");
            mItems.Add("test8");
            mItems.Add("test9");
            mItems.Add("test10");

            wykonane_List = new List<string>();
            wykonane_List.Add("1");
            wykonane_List.Add("1");
            wykonane_List.Add("0");
            wykonane_List.Add("1");
            wykonane_List.Add("0");
            wykonane_List.Add("1");
            wykonane_List.Add("0");
            wykonane_List.Add("0");
            wykonane_List.Add("1");
            wykonane_List.Add("0");
            wykonane_List.Add("1");

            listaZlecen_ListViewAdapter adapter = new listaZlecen_ListViewAdapter(this, mItems, mItems, mItems, mItems, mItems, wykonane_List);

            listaZlecen_ListView.Adapter = adapter;
        }

        private void itemClick_Function(object sender, AdapterView.ItemClickEventArgs e)
        {
            ListView test = (ListView) sender;
            String test1 = test.GetItemAtPosition(Convert.ToInt32(e.Id)).ToString();
            Toast.MakeText(this, e.Id.ToString()+"\n"+ test1, ToastLength.Short).Show();
        }
    }
}