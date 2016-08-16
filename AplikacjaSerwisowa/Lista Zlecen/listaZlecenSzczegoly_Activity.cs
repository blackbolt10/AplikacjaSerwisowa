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
using Android.Support.V4;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace AplikacjaSerwisowa
{
    [Activity(Label = "Sczegó³y zlecenia serwisowego")]
    public class listaZlecenSzczegoly_Activity : FragmentActivity
    {
        private ViewPager mViewPager;
        private SlidingTabScrollView mScrollView;

        public static String szn_ID;
        public static Context kontekst;
        public static String knt_GidNumer { get; set; }
        public static String szn_AdWNumer { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.listaZlecenSzczegolyLayout);

            szn_ID = Intent.GetStringExtra("szn_ID") ?? "no data avalible";
            kontekst = this;

            // Create your application here

            mScrollView = FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabsListaZlecenSzczegoly);
            mViewPager = FindViewById<ViewPager>(Resource.Id.viewPagerListaZlecenSzczegoly);

            mViewPager.Adapter = new SamplePagerAdapterListaZlecenSerwisowych(SupportFragmentManager);
            mScrollView.ViewPager = mViewPager;
        }
        public static Context GetContext()
        {
            return kontekst;
        }
    }



    public class SamplePagerAdapterListaZlecenSerwisowych : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> mFragmentHolder;

        public SamplePagerAdapterListaZlecenSerwisowych(Android.Support.V4.App.FragmentManager fragManager) : base(fragManager)
        {
            mFragmentHolder = new List<Android.Support.V4.App.Fragment>();
            mFragmentHolder.Add(new zakladkaOgolneListaZlecenSerwisowychSzczegoly());
            mFragmentHolder.Add(new zakladkaKontrahentListaZlecenSerwisowychSzczegoly());
            mFragmentHolder.Add(new zakladkaCzynnosciListaZlecenSerwisowychSzczegoly());
            mFragmentHolder.Add(new zakladkaSkladnikiListaZlecenSerwisowychSzczegoly());
            mFragmentHolder.Add(new zakladkaUrzadzeniaListaZlecenSerwisowychSzczegoly());
        }

        public override int Count
        {
            get { return mFragmentHolder.Count; }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return mFragmentHolder[position];
        }
    }

    public class zakladkaOgolneListaZlecenSerwisowychSzczegoly : Android.Support.V4.App.Fragment
    {
        private string szn_ID;
        private Context kontekst;
        private TextView dataNumerTextView, kontrahentGlownyTextView, kontrahentDocelowyTextView, stanTextView;
        private LinearLayout linearLayout;
        private ImageView imageView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.ogolneListaZlecenSzczegoly, container, false);

            dataNumerTextView = view.FindViewById<TextView>(Resource.Id.dataNumerDokListaZlecenSzczegolyTextView);
            kontrahentGlownyTextView = view.FindViewById<TextView>(Resource.Id.nazwaGlownegoKontrahentaListaZlecenSzczegolyTextView);
            kontrahentDocelowyTextView = view.FindViewById<TextView>(Resource.Id.nazwaDocelowegoKontrahentaListaZlecenSzczegolyTextView);
            stanTextView = view.FindViewById<TextView>(Resource.Id.stanListaZlecenSzczegolyTextView);

            linearLayout = view.FindViewById<LinearLayout>(Resource.Id.ListaZlecenSzczegolyLinearLayout);
            imageView = view.FindViewById<ImageView>(Resource.Id.ListaZlecenSzczegolyLimageView);

            szn_ID = listaZlecenSzczegoly_Activity.szn_ID;
            kontekst = listaZlecenSzczegoly_Activity.GetContext();

            ustawDaneZlecenia();

            return view;
        }

        private void ustawDaneZlecenia()
        {
            SrwZlcNag szn = null;
            DBRepository dbr = new DBRepository();

            try
            {
                szn = dbr.SrwZlcNag_GetRecordGetRecord(szn_ID);
            }
            catch(Exception exc)
            {
                Toast.MakeText(kontekst, "B³¹d listaZlecenSzczegoly_Activity.ustawDaneZlecenia():\n" + exc.Message, ToastLength.Short);
            }

            if(szn != null)
            {
                dataNumerTextView.Text = szn.SZN_DataWystawienia.Split(' ')[0] + " - " + szn.SZN_Dokument;
                stanTextView.Text = szn.SZN_Stan;
                if(szn.SZN_KnANumer != -1)
                {
                    KntAdresyTable kntAdres = dbr.kntAdresy_GetRecord(szn.SZN_KnANumer.ToString());
                    if(kntAdres!=null)
                    {
                        if(kntAdres.Kna_nazwa1 != "")
                        {
                            kontrahentDocelowyTextView.Text = kntAdres.Kna_nazwa1;
                        }
                        else if(kntAdres.Kna_Akronim != "")
                        {
                            kontrahentDocelowyTextView.Text = kntAdres.Kna_Akronim;
                        }
                        else
                        {
                            kontrahentDocelowyTextView.Text = "{brak kontrahenta docelowego}";
                        }
                    }
                }
                else
                {
                    kontrahentDocelowyTextView.Text = "{brak kontrahenta docelowego}";
                }

                if(szn.SZN_KntNumer != -1)
                {
                    KntKartyTable kntKarta = dbr.kntKarty_GetRecord(szn.SZN_KntNumer.ToString());
                    if(kntKarta != null)
                    {
                        if(kntKarta.Knt_nazwa1 != "")
                        {
                            kontrahentGlownyTextView.Text = kntKarta.Knt_nazwa1;
                        }
                        else if(kntKarta.Knt_Akronim != "")
                        {
                            kontrahentGlownyTextView.Text = kntKarta.Knt_Akronim;
                        }
                        else
                        {
                            kontrahentGlownyTextView.Text = "{brak kontrahenta g³ównego}";
                        }
                    }
                }
                else
                {
                    kontrahentGlownyTextView.Text = "{brak kontrahenta g³ównego}";
                }
                listaZlecenSzczegoly_Activity.knt_GidNumer = szn.SZN_KntNumer.ToString();
                listaZlecenSzczegoly_Activity.szn_AdWNumer = szn.SZN_KnANumer.ToString();
            }
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Ogólne";
        }
    }

    public class zakladkaKontrahentListaZlecenSerwisowychSzczegoly : Android.Support.V4.App.Fragment
    {
        private string szn_ID;
        private Context kontekst;

        private TextView akronimGlownyTextView, nazwa1GlownyTextView, nazwa2GlownyTextView, nazwa3GlownyTextView,
            ulicaGlownyTextView, kodPMiastoGlownyTextView, telefon1GlownyTextView, telefon2GlownyTextView,
            nipGlownyTextView, emailGlownyTextView;
        private TextView nipNaglowekGlownyTextView, telefonNaglowekGlownyTextView, emailNaglowekGlownyTextView;

        private LinearLayout telefonGlownyLinearLayout;

        private TextView akronimDocelowyTextView, nazwa1DocelowyTextView, nazwa2DocelowyTextView, nazwa3DocelowyTextView,
            ulicaDocelowyTextView, kodPMiastoDocelowyTextView, telefon1DocelowyTextView, telefon2DocelowyTextView,
            nipDocelowyTextView, emailDocelowyTextView;
        private TextView nipNaglowekDocelowyTextView, telefonNaglowekDocelowyTextView, emailNaglowekDocelowyTextView;

        private LinearLayout telefonDocelowyLinearLayout;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.kontrahentListaZlecenSzczegolyLayout, container, false);
            
            akronimGlownyTextView = view.FindViewById<TextView>(Resource.Id.akronimGlownyKontrahentListaZlecenSczegolyTextView);
            nazwa1GlownyTextView = view.FindViewById<TextView>(Resource.Id.nazwa1GlownyKontrahentListaZlecenSczegolyTextView);
            nazwa2GlownyTextView = view.FindViewById<TextView>(Resource.Id.nazwa2GlownyKontrahentListaZlecenSczegolyTextView);
            nazwa3GlownyTextView = view.FindViewById<TextView>(Resource.Id.nazwa3GlownyKontrahentListaZlecenSczegolyTextView);
            ulicaGlownyTextView = view.FindViewById<TextView>(Resource.Id.ulicaGlownyKontrahentListaZlecenSczegolyTextView);
            kodPMiastoGlownyTextView = view.FindViewById<TextView>(Resource.Id.kodpMiastoGlownyKontrahentListaZlecenSczegolyTextView);
            telefon1GlownyTextView = view.FindViewById<TextView>(Resource.Id.telefon2GlownyKontrahentListaZlecenSczegolyTextView);
            telefon2GlownyTextView = view.FindViewById<TextView>(Resource.Id.telefon1GlownyKontrahentListaZlecenSczegolyTextView);
            nipGlownyTextView = view.FindViewById<TextView>(Resource.Id.nipGlownyKontrahentListaZlecenSczegolyTextView);
            emailGlownyTextView = view.FindViewById<TextView>(Resource.Id.emailGlownyKontrahentListaZlecenSczegolyTextView);
            telefonGlownyLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.telefonGlownyKontrahentListaZlecenSczegolyLinearLayout);

            nipNaglowekGlownyTextView = view.FindViewById<TextView>(Resource.Id.nipNaglowekGlownyKontrahentListaZlecenSczegolyTextView);
            telefonNaglowekGlownyTextView = view.FindViewById<TextView>(Resource.Id.telefonNaglowekGlownyKontrahentListaZlecenSczegolyTextView);
            emailNaglowekGlownyTextView = view.FindViewById<TextView>(Resource.Id.emailNaglowekGlownyKontrahentListaZlecenSczegolyTextView);

            akronimDocelowyTextView = view.FindViewById<TextView>(Resource.Id.akronimDocelowyKontrahentListaZlecenSczegolyTextView);
            nazwa1DocelowyTextView = view.FindViewById<TextView>(Resource.Id.nazwa1DocelowyKontrahentListaZlecenSczegolyTextView);
            nazwa2DocelowyTextView = view.FindViewById<TextView>(Resource.Id.nazwa2DocelowyKontrahentListaZlecenSczegolyTextView);
            nazwa3DocelowyTextView = view.FindViewById<TextView>(Resource.Id.nazwa3DocelowyKontrahentListaZlecenSczegolyTextView);
            ulicaDocelowyTextView = view.FindViewById<TextView>(Resource.Id.ulicaDocelowyKontrahentListaZlecenSczegolyTextView);
            kodPMiastoDocelowyTextView = view.FindViewById<TextView>(Resource.Id.kodpMiastoDocelowyKontrahentListaZlecenSczegolyTextView);
            telefon1DocelowyTextView = view.FindViewById<TextView>(Resource.Id.telefon2DocelowyKontrahentListaZlecenSczegolyTextView);
            telefon2DocelowyTextView = view.FindViewById<TextView>(Resource.Id.telefon1DocelowyKontrahentListaZlecenSczegolyTextView);
            nipDocelowyTextView = view.FindViewById<TextView>(Resource.Id.nipDocelowyKontrahentListaZlecenSczegolyTextView);
            emailDocelowyTextView = view.FindViewById<TextView>(Resource.Id.emailDocelowyKontrahentListaZlecenSczegolyTextView);
            telefonDocelowyLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.telefonDocelowyKontrahentListaZlecenSczegolyLinearLayout);

            nipNaglowekDocelowyTextView = view.FindViewById<TextView>(Resource.Id.nipNaglowekDocelowyKontrahentListaZlecenSczegolyTextView);
            telefonNaglowekDocelowyTextView = view.FindViewById<TextView>(Resource.Id.telefonNaglowekDocelowyKontrahentListaZlecenSczegolyTextView);
            emailNaglowekDocelowyTextView = view.FindViewById<TextView>(Resource.Id.emailNaglowekDocelowyKontrahentListaZlecenSczegolyTextView);

            szn_ID = listaZlecenSzczegoly_Activity.szn_ID;
            kontekst = listaZlecenSzczegoly_Activity.GetContext();

            ustawKontrahentaGlownego();
            ustawKontrahentaDocelowego();

            return view;
        }

        private void ustawKontrahentaGlownego()
        {
            KntKartyTable kntKarta = null;
            DBRepository dbr = new DBRepository();

            try
            {
                kntKarta = dbr.kntKarty_GetRecord(listaZlecenSzczegoly_Activity.knt_GidNumer);
            }
            catch(Exception exc)
            {
                Toast.MakeText(kontekst, "B³¹d zakladkaKontrahentListaZlecenSerwisowychSzczegoly.ustawKontrahentaGlownego():\n" + exc.Message, ToastLength.Short);
            }

            if(kntKarta != null)
            {
                akronimGlownyTextView.Text = "["+kntKarta.Knt_Akronim+"]";
                if(akronimGlownyTextView.Text == "[]")
                {
                    akronimGlownyTextView.Visibility = ViewStates.Gone;
                }

                nazwa1GlownyTextView.Text = kntKarta.Knt_nazwa1;
                if(nazwa1GlownyTextView.Text == "")
                {
                    nazwa1GlownyTextView.Visibility = ViewStates.Gone;
                }

                nazwa2GlownyTextView.Text = kntKarta.Knt_nazwa2;
                if(nazwa2GlownyTextView.Text == "")
                {
                    nazwa2GlownyTextView.Visibility = ViewStates.Gone;
                }

                nazwa3GlownyTextView.Text = kntKarta.Knt_nazwa3;
                if(nazwa3GlownyTextView.Text == "")
                {
                    nazwa3GlownyTextView.Visibility = ViewStates.Gone;
                }

                ulicaGlownyTextView.Text = kntKarta.Knt_ulica;
                if(ulicaGlownyTextView.Text == "")
                {
                    ulicaGlownyTextView.Visibility = ViewStates.Gone;
                }

                kodPMiastoGlownyTextView.Text = kntKarta.Knt_KodP+" "+kntKarta.Knt_miasto;
                if(kodPMiastoGlownyTextView.Text == "")
                {
                    kodPMiastoGlownyTextView.Visibility = ViewStates.Gone;
                }

                telefon1GlownyTextView.Text = kntKarta.Knt_telefon1;
                if(telefon1GlownyTextView.Text == "")
                {
                    telefon1GlownyTextView.Visibility = ViewStates.Gone;
                }

                telefon2GlownyTextView.Text = kntKarta.Knt_telefon2;
                if(telefon2GlownyTextView.Text == "")
                {
                    telefon2GlownyTextView.Visibility = ViewStates.Gone;
                }

                if(telefon1GlownyTextView.Text == ""&&telefon2GlownyTextView.Text=="")
                {
                    telefonNaglowekGlownyTextView.Visibility = ViewStates.Gone;
                }

                nipGlownyTextView.Text = kntKarta.Knt_nip;
                if(nipGlownyTextView.Text == "")
                {
                    nipGlownyTextView.Visibility = ViewStates.Gone;
                    nipNaglowekGlownyTextView.Visibility = ViewStates.Gone;
                }

                emailGlownyTextView.Text = kntKarta.Knt_email;
                if(emailGlownyTextView.Text == "")
                {
                    emailGlownyTextView.Visibility = ViewStates.Gone;
                    emailNaglowekGlownyTextView.Visibility = ViewStates.Gone;
                }

            }
        }
        private void ustawKontrahentaDocelowego()
        {
            KntAdresyTable kntAdres = null;
            DBRepository dbr = new DBRepository();

            try
            {
                kntAdres = dbr.kntAdresy_GetRecord(listaZlecenSzczegoly_Activity.szn_AdWNumer);
            }
            catch(Exception exc)
            {
                Toast.MakeText(kontekst, "B³¹d zakladkaKontrahentListaZlecenSerwisowychSzczegoly.ustawKontrahentaGlownego():\n" + exc.Message, ToastLength.Short);
            }

            if(kntAdres != null)
            {
                akronimDocelowyTextView.Text = "[" + kntAdres.Kna_Akronim + "]";
                if(akronimDocelowyTextView.Text == "[]")
                {
                    akronimDocelowyTextView.Visibility = ViewStates.Gone;
                }

                nazwa1DocelowyTextView.Text = kntAdres.Kna_nazwa1;
                if(nazwa1DocelowyTextView.Text == "")
                {
                    nazwa1DocelowyTextView.Visibility = ViewStates.Gone;
                }

                nazwa2DocelowyTextView.Text = kntAdres.Kna_nazwa2;
                if(nazwa2DocelowyTextView.Text == "")
                {
                    nazwa2DocelowyTextView.Visibility = ViewStates.Gone;
                }

                nazwa3DocelowyTextView.Text = kntAdres.Kna_nazwa3;
                if(nazwa3DocelowyTextView.Text == "")
                {
                    nazwa3DocelowyTextView.Visibility = ViewStates.Gone;
                }

                ulicaDocelowyTextView.Text = kntAdres.Kna_ulica;
                if(ulicaDocelowyTextView.Text == "")
                {
                    ulicaDocelowyTextView.Visibility = ViewStates.Gone;
                }

                kodPMiastoDocelowyTextView.Text = kntAdres.Kna_KodP + " " + kntAdres.Kna_miasto;
                if(kodPMiastoDocelowyTextView.Text == "")
                {
                    kodPMiastoDocelowyTextView.Visibility = ViewStates.Gone;
                }

                telefon1DocelowyTextView.Text = kntAdres.Kna_telefon1;
                if(telefon1DocelowyTextView.Text == "")
                {
                    telefon1DocelowyTextView.Visibility = ViewStates.Gone;
                }

                telefon2DocelowyTextView.Text = kntAdres.Kna_telefon2;
                if(telefon2DocelowyTextView.Text == "")
                {
                    telefon2DocelowyTextView.Visibility = ViewStates.Gone;
                }

                if(telefon1DocelowyTextView.Text == "" && telefon2DocelowyTextView.Text == "")
                {
                    telefonNaglowekDocelowyTextView.Visibility = ViewStates.Gone;
                }

                nipDocelowyTextView.Text = kntAdres.Kna_nip;
                if(nipDocelowyTextView.Text == "")
                {
                    nipDocelowyTextView.Visibility = ViewStates.Gone;
                    nipNaglowekDocelowyTextView.Visibility = ViewStates.Gone;
                }

                emailDocelowyTextView.Text = kntAdres.Kna_email;
                if(emailDocelowyTextView.Text == "")
                {
                    emailDocelowyTextView.Visibility = ViewStates.Gone;
                    emailNaglowekDocelowyTextView.Visibility = ViewStates.Gone;
                }

            }
            else
            {
                ukryjDocelowego();
            }
        }

        private void ukryjDocelowego()
        {
            akronimDocelowyTextView.Visibility = ViewStates.Gone;
            nazwa1DocelowyTextView.Text = "{Brak kontrahenta docelowego}";
            nazwa2DocelowyTextView.Visibility = ViewStates.Gone;
            nazwa3DocelowyTextView.Visibility = ViewStates.Gone;
            ulicaDocelowyTextView.Text = "{Brak adresu kontrahenta docelowego}";
            kodPMiastoDocelowyTextView.Visibility = ViewStates.Gone;
            telefon1DocelowyTextView.Visibility = ViewStates.Gone;
            telefon2DocelowyTextView.Visibility = ViewStates.Gone;
            nipDocelowyTextView.Visibility = ViewStates.Gone;
            emailDocelowyTextView.Visibility = ViewStates.Gone;
            telefonDocelowyLinearLayout.Visibility = ViewStates.Gone;
            nipNaglowekDocelowyTextView.Visibility = ViewStates.Gone;
            telefonNaglowekDocelowyTextView.Visibility = ViewStates.Gone;
            emailNaglowekDocelowyTextView.Visibility = ViewStates.Gone;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Kontrahent";
        }
    }

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

            if(szcList.Count>0)
            {
                adapter = new listaZlecenSzczegolyCzynnosci_ListViewAdapter(kontekst,szcList);
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

    public class zakladkaUrzadzeniaListaZlecenSerwisowychSzczegoly : Android.Support.V4.App.Fragment
    {
        private Button mButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Frag3Layout, container, false);

            mButton = view.FindViewById<Button>(Resource.Id.button1);
            return view;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Fragment 3";
        }
    }
}