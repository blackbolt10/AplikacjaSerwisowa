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
            mFragmentHolder.Add(new zakladkaPodpiszNoweZlecenie());
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
}