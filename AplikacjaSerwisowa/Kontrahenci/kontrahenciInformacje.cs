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
    [Activity(Label = "Informacje o kontrahencie")]
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
        private TextView mTelefon1TextView, mTelefon2TextView;
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
                KntKartyTable result = dbr.kntKarty_GetRecord(mGidNumerKontrahenta);

                mGidNumerTextView.Text = result.Knt_GIDNumer.ToString();
                if(mUkrywanie)
                {
                    mGidNumerTextView.Visibility = ViewStates.Gone;
                }

                mAkronimTextView.Text = result.Knt_Akronim;
                mNazwaTextView.Text = result.Knt_nazwa1;

                if(result.Knt_nazwa2!= "")
                {
                    mNazwaTextView.Text += "\n" + result.Knt_nazwa2;
                }

                if(result.Knt_nazwa3 != "")
                {
                    mNazwaTextView.Text += "\n" + result.Knt_nazwa2;
                }
                
                mKodPMiastoTextView.Text = result.Knt_KodP + result.Knt_miasto;
                mUlicaTextView.Text = result.Knt_ulica;
                mNipTextView.Text = result.Knt_nip;

                if(mNipTextView.Text == "")
                {
                    mNipTextView.Visibility = ViewStates.Gone;
                }

                mTelefon1TextView.Text = result.Knt_telefon1;
                if(mTelefon1TextView.Text == "")
                {
                    mTelefon1TextView.Visibility = ViewStates.Gone;
                }

                mTelefon2TextView.Text = result.Knt_telefon2;
                if(mTelefon2TextView.Text == "")
                {
                    mTelefon2TextView.Visibility = ViewStates.Gone;
                }

                mTelexTextView.Text = result.Knt_telex;
                if(mTelexTextView.Text == "")
                {
                    mTelexTextView.Visibility = ViewStates.Gone;
                }

                if(mTelefon1TextView.Text == ""&& mTelefon2TextView.Text == ""&& mTelexTextView.Text == "")
                {
                    mTelefonNazwaTextView.Visibility = ViewStates.Gone;
                }

                mFaxTextView.Text = result.Knt_fax;
                if(mFaxTextView.Text == "")
                {
                    mFaxTextView.Visibility = ViewStates.Gone;
                    mFaxNazwaTextView.Visibility = ViewStates.Gone;
                }

                mEmailTextView.Text = result.Knt_email;
                if(mEmailTextView.Text == "")
                {
                    mEmailTextView.Visibility = ViewStates.Gone;
                    mEmailNazwaTextView.Visibility = ViewStates.Gone; ;
                }

                mUrlTextView.Text = result.Knt_url;
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
        private ListView mkontrahecniAdresyListView;    
        private Context kontekstGlowny;
        private String mGidNumerKontrahenta;
        private Int32 mUkrywanieGidNmer = 1;

        private List<string> kna_gidnumer_List = new List<string>();
        private List<string> kna_kntnumer = new List<string>();
        private List<string> kna_akronim_List = new List<string>();
        private List<string> kna_nazwa1_List = new List<string>();
        private List<string> kna_nazwa2_List = new List<string>();
        private List<string> kna_nazwa3_List = new List<string>();
        private List<string> kna_kodp_List = new List<string>();
        private List<string> kna_miasto_List = new List<string>();
        private List<string> kna_ulica_List = new List<string>();
        private List<string> kna_adresy_List = new List<string>();
        private List<string> kna_nip_List = new List<string>();
        private List<string> kna_telefon1_List = new List<string>();
        private List<string> kna_telefon2_List = new List<string>();
        private List<string> kna_telex_List = new List<string>();
        private List<string> kna_fax_List = new List<string>();
        private List<string> kna_email_List = new List<string>();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Frag2Layout, container, false);

            mGidNumerKontrahenta = kontrahenciInformacje.GetKnt_GidNumer();
            kontekstGlowny = kontrahenciInformacje.GetContext();

            mkontrahecniAdresyListView = view.FindViewById<ListView>(Resource.Id.kontrahecniAdresyFrag2ListView);
            mkontrahecniAdresyListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e) { itemClick_Function(sender, e); };

            try
            {
                String dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);
                var result = db.Query<KntAdresyTable>("select * from KntAdresyTable where Kna_KntNumer = " + mGidNumerKontrahenta);
                
                foreach(var item in result)
                {
                    kna_gidnumer_List.Add(item.Kna_GIDNumer.ToString());
                    kna_kntnumer.Add(item.Kna_KntNumer.ToString());
                    kna_akronim_List.Add(item.Kna_Akronim);
                    kna_nazwa1_List.Add(item.Kna_nazwa1);
                    kna_nazwa2_List.Add(item.Kna_nazwa2);
                    kna_nazwa3_List.Add(item.Kna_nazwa3);
                    kna_kodp_List.Add(item.Kna_KodP);
                    kna_miasto_List.Add(item.Kna_miasto);
                    kna_ulica_List.Add(item.Kna_ulica);
                    kna_adresy_List.Add(item.Kna_Adres);
                    kna_nip_List.Add(item.Kna_nip);
                    kna_telefon1_List.Add(item.Kna_telefon1);
                    kna_telefon2_List.Add(item.Kna_telefon2);
                    kna_telex_List.Add(item.Kna_telex);
                    kna_fax_List.Add(item.Kna_fax);
                    kna_email_List.Add(item.Kna_email);
                }
            }
            catch(Exception exc)
            {
                messagebox("B³¹d Adresy.PobierzAdresy():\n" + exc.Message, "B³¹d", 0);
            }

            kontrahenciAdresy_ListViewAdapter adapter = new kontrahenciAdresy_ListViewAdapter(kontekstGlowny, kna_gidnumer_List, kna_kntnumer, kna_akronim_List, kna_nazwa1_List, kna_nazwa2_List, kna_nazwa3_List, kna_kodp_List, kna_miasto_List, kna_ulica_List, kna_adresy_List, kna_nip_List, kna_telefon1_List, kna_telefon2_List, kna_telex_List, kna_fax_List, kna_email_List, mUkrywanieGidNmer);
            mkontrahecniAdresyListView.Adapter = adapter;

            return view;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Adresy";
        }

        private void itemClick_Function(object sender, AdapterView.ItemClickEventArgs e)
        {
            ListView test = (ListView)sender;
            String test1 = test.GetItemAtPosition(Convert.ToInt32(e.Id)).ToString();
        }

        private void messagebox(String tekst, String tytul = "", Int32 icon = 1)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(kontekstGlowny);

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