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
            mFragmentHolder.Add(new zakladkaPodpisListaZlecenSerwisowychSzczegoly());
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