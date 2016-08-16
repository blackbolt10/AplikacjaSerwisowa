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
    [Activity(Label = "Lista zleceñ", Icon = "@drawable/lista_zlecen")]
    public class listaZlecen_Activity : Activity 
    {
        private ListView listaZlecen_ListView;
        private EditText filtrEditText;
        Button filtrButton;
        private List<SrwZlcNag> srwZlcNagList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.listaZlecen);

            listaZlecen_ListView = FindViewById<ListView>(Resource.Id.listView1);
            listaZlecen_ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e) { itemClick_Function(sender,e); };

            filtrButton = FindViewById<Button>(Resource.Id.filtrListaZlecenButton);
            filtrButton.Click += FiltrButton_Click;

            filtrEditText = FindViewById<EditText>(Resource.Id.filtrListaZlecenTextView);
            filtrEditText.TextChanged += new EventHandler<TextChangedEventArgs>(FiltrEditText_TextChanged);

            generujListeSrwZlcNag();
        }

        private void FiltrButton_Click(object sender, EventArgs e)
        {
            filtrEditText.Text = "";
        }

        private void FiltrEditText_TextChanged(object sender, TextChangedEventArgs e)
        {
            generujListeSrwZlcNag();
        }

        private void generujListeSrwZlcNag()
        {
            srwZlcNagList = new List<SrwZlcNag>();

            try
            {
                DBRepository dbr = new DBRepository();
                srwZlcNagList = dbr.pobierzListeSrwZlcNag(filtrEditText.Text);
            }
            catch(Exception exc)
            {
                messagebox("B³¹d listaZlecen_Activity.OnCreate():\n" + exc.Message, "B³¹d", 0);
            }

            listaZlecen_ListViewAdapter adapter;

            if(srwZlcNagList.Count > 0)
            {
                adapter = new listaZlecen_ListViewAdapter(this, srwZlcNagList);
            }
            else
            {
                adapter = null;
            }
            listaZlecen_ListView.Adapter = adapter;
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
            Intent listaZlecenSzczegolyIntent = new Intent(this, typeof(listaZlecenSzczegoly_Activity));
            listaZlecenSzczegolyIntent.PutExtra("szn_ID", srwZlcNagList[e.Position].SZN_Id.ToString());
            StartActivity(listaZlecenSzczegolyIntent);
        }
    }
}