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
    public class DaneKontrahenta : Android.Support.V4.App.Fragment
    {
        private TextView mAkronimTextView, mNazwaTextView, mNipTextView;
        private TextView mTelefon1TextView, mTelefon2TextView;
        private TextView mTelexTextView, mEmailTextView, mUrlTextView, mFaxTextView;
        private TextView mGidNumerTextView, mKodPMiastoTextView, mUlicaTextView;

        private TextView mTelefonNazwaTextView, mFaxNazwaTextView, mEmailNazwaTextView, mUrlNazwaTextView;
        private String mKNT_GIDNumer;
        private Context kontekstGlowny;

        private Boolean mUkrywanie = true;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Frag1Layout, container, false);

            mKNT_GIDNumer = kontrahenciInformacje.GetKnt_GidNumer();
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
                KntKartyTable result = dbr.kntKarty_GetRecord(mKNT_GIDNumer);

                mGidNumerTextView.Text = result.Knt_GIDNumer.ToString();
                if(mUkrywanie)
                {
                    mGidNumerTextView.Visibility = ViewStates.Gone;
                }

                mAkronimTextView.Text = result.Knt_Akronim;
                mNazwaTextView.Text = result.Knt_nazwa1;

                if(result.Knt_nazwa2 != "")
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

                if(mTelefon1TextView.Text == "" && mTelefon2TextView.Text == "" && mTelexTextView.Text == "")
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
                    mEmailNazwaTextView.Visibility = ViewStates.Gone;
                    ;
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
}