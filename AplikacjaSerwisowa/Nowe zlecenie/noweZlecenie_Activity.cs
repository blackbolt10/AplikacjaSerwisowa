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
    [Activity(Label = "noweZlecenie_Activity")]
    public class noweZlecenie_Activity : FragmentActivity
    {
        private ViewPager mViewPager;
        private SlidingTabScrollView mScrollView;

        public static String szn_ID;
        private static Context kontekst;
        public static String knt_GidNumer { get; set; }
        public static String szn_AdWNumer { get; set; }

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
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.glowneOkno_Menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public static Context GetContext()
        {
            return kontekst;
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

    public class zakladkaOgolneNoweZlecenie : Android.Support.V4.App.Fragment
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

    public class zakladkaKontrahentNoweZlecenie : Android.Support.V4.App.Fragment
    {
        private static Button mGlownyButton, mDocelowyButton;
        private static LinearLayout mGlownyLinearLayout, mDocelowyLinearLayout;
        private static TextView mAkronimGlownyTextView, mNazwaGlownyTextView;
        private static TextView mAkronimDocelowyTextView, mNazwaDocelowyTextView;

        public static Int32 Knt_GIDNumer = -1;
        public static Int32 Kna_GIDNumer = -1;

        Context kontekst;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.noweZlecenieZakladkaKontrahentLayout, container, false);

            Kna_GIDNumer = -1;
            Knt_GIDNumer = -1;
            kontekst = noweZlecenie_Activity.GetContext();

            mGlownyButton = view.FindViewById<Button>(Resource.Id.dodajGlownyNoweZlecenieButton);
            mGlownyButton.Click += MGlownyButton_Click;

            mGlownyLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.GlownyNoweZlecenieLinearLayout);

            mAkronimGlownyTextView = view.FindViewById<TextView>(Resource.Id.akronimGlownyNoweZlecenieTextView);
            mAkronimGlownyTextView.Text = "";
            mNazwaGlownyTextView = view.FindViewById<TextView>(Resource.Id.nazwaGlownyNoweZlecenieTextView);
            mNazwaGlownyTextView.Text = "";

            mDocelowyButton = view.FindViewById<Button>(Resource.Id.dodajDocelowyNoweZlecenieButton);
            mDocelowyButton.Click += MDocelowyButton_Click;

            mDocelowyLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.DocelowyNoweZlecenieLinearLayout);

            mAkronimDocelowyTextView = view.FindViewById<TextView>(Resource.Id.akronimDocelowyNoweZlecenieTextView);
            mAkronimDocelowyTextView.Text = "";
            mNazwaDocelowyTextView = view.FindViewById<TextView>(Resource.Id.nazwaDocelowyNoweZlecenieTextView);
            mNazwaDocelowyTextView.Text = "";

            ustawWidocznosc();

            return view;
        }

        private void MGlownyButton_Click(object sender, EventArgs e)
        {
            Intent noweZlecenieDodajKontrahentGlowny = new Intent(kontekst, typeof(noweZlecenieDodajKontrahenta_Activity));
            noweZlecenieDodajKontrahentGlowny.PutExtra("glowny", "1");
            StartActivity(noweZlecenieDodajKontrahentGlowny);

            Kna_GIDNumer = -1;
            mAkronimDocelowyTextView.Text = "";
            mNazwaDocelowyTextView.Text = "";
        }
        private void MDocelowyButton_Click(object sender, EventArgs e)
        {
            Intent listaZlecenSzczegolyIntent = new Intent(kontekst, typeof(noweZlecenieDodajKontrahenta_Activity));
            listaZlecenSzczegolyIntent.PutExtra("glowny", "0");
            listaZlecenSzczegolyIntent.PutExtra("Knt_GIDNumer", Knt_GIDNumer.ToString());
            StartActivity(listaZlecenSzczegolyIntent);
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Kontrahent";
        }

        private static void ustawWidocznosc()
        {
            if(Knt_GIDNumer == -1)
            {
                mGlownyLinearLayout.Visibility = ViewStates.Gone;
                mDocelowyButton.Enabled = false;
            }
            else
            {
                mGlownyLinearLayout.Visibility = ViewStates.Visible;
                mDocelowyButton.Enabled = true;
            }

            if(Kna_GIDNumer == -1)
            {
                mDocelowyLinearLayout.Visibility = ViewStates.Gone;
            }
            else
            {
                mDocelowyLinearLayout.Visibility = ViewStates.Visible;
            }
        }

        public static void aktualizujKontrahentaGlownego(String akronim, String nazwa, Int32 kntGidNumer)
        {
            mAkronimGlownyTextView.Text = akronim;
            mNazwaGlownyTextView.Text = nazwa;
            Knt_GIDNumer = kntGidNumer;
            ustawWidocznosc();
        }

        public static void aktualizujKontrahentaDocelowego(String akronim, String nazwa, Int32 _Kna_GIDNumer)
        {
            mAkronimDocelowyTextView.Text = akronim;
            mNazwaDocelowyTextView.Text = nazwa;
            Kna_GIDNumer = _Kna_GIDNumer;
            ustawWidocznosc();
        }
    }

    public class zakladkaCzynnosciNoweZlecenie : Android.Support.V4.App.Fragment
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

    public class zakladkaSkladnikiNoweZlecenie : Android.Support.V4.App.Fragment
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