using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    [Activity(Label = "Nowe zlecenie")]
    public class noweZlecenie_Activity : FragmentActivity
    {
        private ViewPager mViewPager;
        private SlidingTabScrollView mScrollView;

        public static String szn_ID;
        private static Context kontekst;

        public static List<TwrKartyTable> skladnikiList;
        public static List<TwrKartyTable> czynnosciList;

        public static Int32 Knt_GIDNumer;
        public static String akronimGlowny;
        public static String NazwaGlowny;

        public static Int32 Kna_GIDNumer;
        public static String akronimDocelowy;
        public static String NazwaDocelowy;

        public static String opisSrwZlcNag;
        public static String DataWystawienia;
        public static String DataRealizacji;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.noweZlecenie);

            kontekst = this;
            // Create your application here

            mScrollView = FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabsNoweZlecenie);
            mViewPager = FindViewById<ViewPager>(Resource.Id.viewPagerNoweZlecenie);

            mViewPager.Adapter = new SamplePagerAdapterNoweZlecenie(SupportFragmentManager);
            mScrollView.ViewPager = mViewPager;

            String KNT_GIDNumer = Intent.GetStringExtra("KNT_GIDNumer") ?? "no data avalible";
            if(KNT_GIDNumer != "no data avalible")
            {
                Knt_GIDNumer = Convert.ToInt32(KNT_GIDNumer);
                pobierzKntKarte();
            }
            else
            {
                parametryStartowe();
            }

            skladnikiList = new List<TwrKartyTable>();
            czynnosciList = new List<TwrKartyTable>();

            opisSrwZlcNag = "";
            ustawDaty();
        }

        private void pobierzKntKarte()
        {
            DBRepository dbr = new DBRepository();
            KntKartyTable kntKarta =  dbr.kntKarty_GetRecord(Knt_GIDNumer.ToString());
            akronimGlowny = kntKarta.Knt_Akronim;
            NazwaGlowny = kntKarta.Knt_nazwa1;
        }

        private void ustawDaty()
        {
            DataWystawienia = DateTime.Today.Year.ToString() + "-";
            DataRealizacji = DateTime.Today.Year.ToString() + "-";

            if(DateTime.Today.Month.ToString().Length == 1)
            {
                DataWystawienia += "0" + DateTime.Today.Month.ToString() + "-";
                DataRealizacji += "0" + DateTime.Today.Month.ToString() + "-";
            }
            else
            {
                DataWystawienia += DateTime.Today.Month.ToString() + "-";
                DataRealizacji += DateTime.Today.Month.ToString() + "-";
            }

            if(DateTime.Today.Day.ToString().Length == 1)
            {
                DataWystawienia += "0" + DateTime.Today.Day.ToString();
                DataRealizacji += "0" + DateTime.Today.Day.ToString();
            }
            else
            {
                DataWystawienia += DateTime.Today.Day.ToString();
                DataRealizacji += DateTime.Today.Day.ToString();
            }
        }

        private void parametryStartowe()
        {
            Knt_GIDNumer = -1;
            akronimGlowny = "";
            NazwaGlowny = "";

            Kna_GIDNumer = -1;
            akronimDocelowy = "";
            NazwaDocelowy = "";            
        }

        public static Context GetContext()
        {
            return kontekst;
        }

        public static void ustawIloscSkladnikow(double ilosc)
        {
            skladnikiList[skladnikiList.Count - 1].Ilosc = ilosc;
        }

        public static void ustawIloscCzynnosci(double ilosc)
        {
            czynnosciList[czynnosciList.Count - 1].Ilosc = ilosc;
        }

        public static void aktualizujKontrahentaGlownego(String akronim, String nazwa, Int32 kntGidNumer)
        {
            Knt_GIDNumer = kntGidNumer;
            akronimGlowny = akronim;
            NazwaGlowny = nazwa;

            zakladkaKontrahentNoweZlecenie.aktualizujKontrahentaGlownego();
        }

        public static void aktualizujKontrahentaDocelowego(String akronim, String nazwa, Int32 _Kna_GIDNumer)
        {
            Kna_GIDNumer = _Kna_GIDNumer;
            akronimDocelowy = akronim;
            NazwaDocelowy = nazwa;
            zakladkaKontrahentNoweZlecenie.aktualizujKontrahentaDocelowego();
        }
    }

    public class SamplePagerAdapterNoweZlecenie : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> mFragmentHolder;

        public SamplePagerAdapterNoweZlecenie(Android.Support.V4.App.FragmentManager fragManager) : base(fragManager)
        {
            mFragmentHolder = new List<Android.Support.V4.App.Fragment>();
            mFragmentHolder.Add(new zakladkaOgolneNoweZlecenie());
            mFragmentHolder.Add(new zakladkaKontrahentNoweZlecenie());
            mFragmentHolder.Add(new zakladkaCzynnosciNoweZlecenie());
            mFragmentHolder.Add(new zakladkaSkladnikiNoweZlecenie());
            mFragmentHolder.Add(new zakladkaPodpisNoweZlecenie());
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
}