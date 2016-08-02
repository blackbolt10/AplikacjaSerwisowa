using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AplikacjaSerwisowa
{
    [Activity(Label = "Aplikacja serwisowa", MainLauncher = true, Icon = "@drawable/app_icon")]
    public class MainActivity : Activity
    {
        public String mailLabelString = "test";
        Button zaloguj_Button, wszyscy_Button, uzuplnijtest_Button;
        EditText login_EditText, haslo_EditText;
        RelativeLayout loadingAnimation_RelativeLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            // Create your application here
            zaloguj_Button = FindViewById<Button>(Resource.Id.zaloguj_Button);
            zaloguj_Button.Click += delegate { zaloguj_Button_Click(); };

            wszyscy_Button = FindViewById<Button>(Resource.Id.wszyscyButton);
            wszyscy_Button.Click += delegate { wszyscy_Button_Click(); };

            uzuplnijtest_Button = FindViewById<Button>(Resource.Id.uzupelnijTestButton);
            uzuplnijtest_Button.Click += delegate { uzuplnijtest_Button_Click(); };

            login_EditText = FindViewById<EditText>(Resource.Id.login_EditText);
            haslo_EditText = FindViewById<EditText>(Resource.Id.haslo_EditText);

            loadingAnimation_RelativeLayout = FindViewById<RelativeLayout>(Resource.Id.loadingPanel);
            loadingAnimation_RelativeLayout.Visibility = ViewStates.Invisible;

            OdczytDanychZPamieciUrzadzenia();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.mainActivity_Menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        private void zapisDanychDoPamieciUrzadzenia()
        {
            var prefs = Application.Context.GetSharedPreferences(ApplicationInfo.LoadLabel(PackageManager), FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString("loginMainActivity", login_EditText.Text);
            prefEditor.Commit();
        }
        private void OdczytDanychZPamieciUrzadzenia()
        {
            var prefs = Application.Context.GetSharedPreferences(ApplicationInfo.LoadLabel(PackageManager), FileCreationMode.Private);

            login_EditText.Text = prefs.GetString("loginMainActivity", "");
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.ustawienia_mainActivityMenu:
                    ustawienia_mainActivityMenu();
                    return true;
                case Resource.Id.zamknij_mainActivityMenu:
                    Finish();
                    return true;
                default:
                    return false;
            }
        }

        private void ustawienia_mainActivityMenu()
        {
            StartActivity(typeof(ustawienia_Activity));
        }

        private void zaloguj_Button_Click()
        {
            loadingAnimation_RelativeLayout.Visibility = ViewStates.Visible;

            zapisDanychDoPamieciUrzadzenia();

            DBRepository dbr = new DBRepository();
            String result = dbr.Login(login_EditText.Text, haslo_EditText.Text);
            if (result != "1")
            {
                Toast.MakeText(this, result, ToastLength.Short).Show();
            }
            else
            {
                StartActivity(typeof(glowneOkno_Activity));
                haslo_EditText.Text = "";
            }

            loadingAnimation_RelativeLayout.Visibility = ViewStates.Invisible;
        }
        private void wszyscy_Button_Click()
        {
            loadingAnimation_RelativeLayout.Visibility = ViewStates.Visible;

            DBRepository dbr = new DBRepository();
            String result = dbr.OperatorzyTable_GetAllRecords();
            Toast.MakeText(this, result, ToastLength.Long).Show();

            loadingAnimation_RelativeLayout.Visibility = ViewStates.Invisible;
        }

        private void uzuplnijtest_Button_Click()
        {
            login_EditText.Text = "test";
            haslo_EditText.Text = "test";
            zaloguj_Button_Click();
        }
    }
}

