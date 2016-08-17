using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AplikacjaSerwisowa
{
    public class zakladkaPodpisListaZlecenSerwisowychSzczegoly : Android.Support.V4.App.Fragment
    {
        private string szn_ID;
        private Context kontekst;
        private ImageView imageView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.podpisListaZlecenLayout, container, false);

            szn_ID = listaZlecenSzczegoly_Activity.szn_ID;
            kontekst = listaZlecenSzczegoly_Activity.GetContext();

            imageView  = view.FindViewById<ImageView>(Resource.Id.ListaZlecenPodpisImageView);
            imageView.SetImageBitmap(pobierzBitmap());

            return view;
        }

        private Bitmap pobierzBitmap()
        {
            DBRepository dbr = new DBRepository();
            byte[] byteArray =  dbr.pobierzPodpis(szn_ID);
            Bitmap bitmapa = ByteArrayToImage(byteArray);

            return bitmapa;
        }
        private Bitmap ByteArrayToImage(byte[] byteArray)
        {
            Bitmap test = BitmapFactory.DecodeByteArray(byteArray, 0, byteArray.Length);
            return test;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Podpis";
        }
    }
}