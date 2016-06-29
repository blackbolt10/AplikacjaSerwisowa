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
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace AplikacjaSerwisowa
{
    [Activity(Label = "kontrahenciInformacje")]
    public class kontrahenciInformacje : FragmentActivity
    {
        private ViewPager mViewPager;
        private SlidingTabScrollView mScrollView;

        public static String kna_gidnumer;
        public static Context kontekst;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.kontrahenciInformacje);

            // Create your application here
            kna_gidnumer = Intent.GetStringExtra("kna_gidnumer") ?? "no data avalible";
            kontekst = this;

            mScrollView = FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
            mViewPager = FindViewById<ViewPager>(Resource.Id.viewPager);

            mViewPager.Adapter = new SamplePagerAdapter(SupportFragmentManager);
            mScrollView.ViewPager = mViewPager;
        }

        public static string GetKnt_GidNumer()
        {
            return kna_gidnumer;
        }
        public static Context GetContext()
        {
            return kontekst;
        }
    }

    public class SamplePagerAdapter : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> mFragmentHolder;
    
        public SamplePagerAdapter(Android.Support.V4.App.FragmentManager fragManager) : base(fragManager)
        {
           string test = kontrahenciInformacje.GetKnt_GidNumer();

            mFragmentHolder = new List<Android.Support.V4.App.Fragment>();
            mFragmentHolder.Add(new Fragment1());
            mFragmentHolder.Add(new Fragment2());
            mFragmentHolder.Add(new Fragment3());
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

    public class Fragment1 : Android.Support.V4.App.Fragment
    {
        private TextView mAkronimTextView, mNazwaTextView, mNipTextView;
        private TextView mTelefon1TextView, mTelefon2TextView, mTelefon3TextView;
        private TextView mTelexTextView, mEmailTextView, mUrlTextView, mFaxTextView;
        private TextView mGidNumerTextView, mKodPMiastoTextView, mUlicaTextView;

        private TextView mTelefonNazwaTextView, mFaxNazwaTextView, mEmailNazwaTextView, mUrlNazwaTextView;
        private String mGidNumerKontrahenta;
        private Context kontekstGlowny;

        private Boolean mUkrywanie = true;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Frag1Layout, container, false);

            mGidNumerKontrahenta = kontrahenciInformacje.GetKnt_GidNumer();
            kontekstGlowny = kontrahenciInformacje.GetContext();

            mAkronimTextView = view.FindViewById<TextView>(Resource.Id.akronimFrag1TextView);
            mNazwaTextView = view.FindViewById<TextView>(Resource.Id.nazwaFrag1TextView);
            mNipTextView = view.FindViewById<TextView>(Resource.Id.nipFrag1TextView);
            mTelefon1TextView = view.FindViewById<TextView>(Resource.Id.telefon1Frag1TextView);
            mTelefon2TextView = view.FindViewById<TextView>(Resource.Id.telefon2Frag1TextView);
            mTelefon3TextView = view.FindViewById<TextView>(Resource.Id.telefon3Frag1TextView);
            mTelexTextView = view.FindViewById<TextView>(Resource.Id.telexFrag1TextView);
            mFaxTextView = view.FindViewById<TextView>(Resource.Id.faxFrag1TextView);
            mUlicaTextView = view.FindViewById<TextView>(Resource.Id.ulicaFrag1TextView);
            mKodPMiastoTextView = view.FindViewById<TextView>(Resource.Id.kodPMiastoFrag1TextView);
            mGidNumerTextView = view.FindViewById<TextView>(Resource.Id.gidNumerFrag1TextView);
            mEmailTextView = view.FindViewById<TextView>(Resource.Id.emailFrag1TextView);
            mUrlTextView = view.FindViewById<TextView>(Resource.Id.urlFrag1TextView);

            mTelefonNazwaTextView = view.FindViewById<TextView>(Resource.Id.telefonNazwaFrag1TextView);
            mFaxNazwaTextView = view.FindViewById<TextView>(Resource.Id.faxNazwaFrag1TextView);
            mEmailNazwaTextView = view.FindViewById<TextView>(Resource.Id.emailNazwaFrag1TextView);
            mUrlNazwaTextView = view.FindViewById<TextView>(Resource.Id.urlNazwaFrag1TextView);

            pobierzDaneKontrahenta();

            return view;
        }

        private void pobierzDaneKontrahenta()
        {
            try
            {
                DBRepository dbr = new DBRepository();
                List<String> result = dbr.kntKarty_GetRecord(mGidNumerKontrahenta);

                mGidNumerTextView.Text = result[0];
                if(mUkrywanie)
                {
                    mGidNumerTextView.Visibility = ViewStates.Gone;
                }

                mAkronimTextView.Text = result[1];
                mNazwaTextView.Text = result[2];

                if(result[3]!= "")
                {
                    mNazwaTextView.Text += "\n" + result[3];
                }

                if(result[4] != "")
                {
                    mNazwaTextView.Text += "\n" + result[4];
                }
                
                mKodPMiastoTextView.Text = result[5] + result[6];
                mUlicaTextView.Text = result[7];
                mNipTextView.Text = result[9];

                if(mNipTextView.Text == "")
                {
                    mNipTextView.Visibility = ViewStates.Gone;
                }

                mTelefon1TextView.Text = result[10];
                if(mTelefon1TextView.Text == "")
                {
                    mTelefon1TextView.Visibility = ViewStates.Gone;
                }

                mTelefon2TextView.Text = result[11];
                if(mTelefon2TextView.Text == "")
                {
                    mTelefon2TextView.Visibility = ViewStates.Gone;
                }

                mTelefon3TextView.Text = result[12];
                if(mTelefon3TextView.Text == "")
                {
                    mTelefon3TextView.Visibility = ViewStates.Gone;
                }

                mTelexTextView.Text = result[13];
                if(mTelexTextView.Text == "")
                {
                    mTelexTextView.Visibility = ViewStates.Gone;
                }

                if(mTelefon1TextView.Text == ""&& mTelefon2TextView.Text == ""&& mTelefon3TextView.Text == ""&& mTelexTextView.Text == "")
                {
                    mTelefonNazwaTextView.Visibility = ViewStates.Gone;
                }

                mFaxTextView.Text = result[14];
                if(mFaxTextView.Text == "")
                {
                    mFaxTextView.Visibility = ViewStates.Gone;
                    mFaxNazwaTextView.Visibility = ViewStates.Gone;
                }

                mEmailTextView.Text = result[15];
                if(mEmailTextView.Text == "")
                {
                    mEmailTextView.Visibility = ViewStates.Gone;
                    mEmailNazwaTextView.Visibility = ViewStates.Gone; ;
                }

                mUrlTextView.Text = result[16];
                if(mUrlTextView.Text == "")
                {
                    mUrlTextView.Visibility = ViewStates.Gone;
                    mUrlNazwaTextView.Visibility = ViewStates.Gone;
                }
            }
            catch(Exception exc)
            {
                Toast.MakeText(kontekstGlowny, "B³¹d DaneKontrahenta.pobierzDaneKontrahenta():\n" + exc.Message, ToastLength.Short);
            }
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Dane kontrahenta";
        }
    }

    public class Fragment2 : Android.Support.V4.App.Fragment
    {
        private EditText mTxt;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Frag2Layout, container, false);

            mTxt = view.FindViewById<EditText>(Resource.Id.editText1);
            mTxt.Text = "Fragment 2 Class :)";
            return view;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Adresy";
        }
    }

    public class Fragment3 : Android.Support.V4.App.Fragment
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