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
    class kartyTowarow_ListViewAdapter : BaseAdapter<string>
    {
        private List<string> mtwr_kod_List;
        private List<string> mtwr_gidnumer_List;
        private List<string> mtwr_typ_List;
        private List<string> mtwr_nazwa_List;
        private Int32 mukrywanie;

        private Context mContext;
        private kontrahenci_Activity kontrahenci_Activity;
        private object p1;
        private object p2;
        private object p3;
        private object p4;
        private object p5;
        private object p6;
        private object p7;
        private object p8;
        private object p9;
        private object p10;
        private object p11;
        private object p12;
        private object p13;
        private object p14;
        private object p15;
        private object p16;
        private int v;

        public kartyTowarow_ListViewAdapter(Context context, List<string> twr_kod_List, List<string> twr_gidnumer_List, List<string> twr_typ_List, List<string> twr_nazwa_List, Int32 ukrywanie)
        {
            mtwr_gidnumer_List = twr_gidnumer_List;
            mtwr_kod_List = twr_kod_List;
            mtwr_nazwa_List = twr_nazwa_List;
            mtwr_typ_List = twr_typ_List;
            mukrywanie = ukrywanie;

            mContext = context;
        }

        public kartyTowarow_ListViewAdapter(kontrahenci_Activity kontrahenci_Activity, object p1, object p2, object p3, object p4, object p5, object p6, object p7, object p8, object p9, object p10, object p11, object p12, object p13, object p14, object p15, object p16, int v)
        {
            this.kontrahenci_Activity = kontrahenci_Activity;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
            this.p5 = p5;
            this.p6 = p6;
            this.p7 = p7;
            this.p8 = p8;
            this.p9 = p9;
            this.p10 = p10;
            this.p11 = p11;
            this.p12 = p12;
            this.p13 = p13;
            this.p14 = p14;
            this.p15 = p15;
            this.p16 = p16;
            this.v = v;
        }

        public override int Count
        {
            get
            {
                if(mtwr_kod_List != null)
                {
                    return mtwr_kod_List.Count;
                }
                else
                {
                    return 0;
                }
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.kartyTowarow_row, null, false);
            }

            TextView kod_TextView = row.FindViewById<TextView>(Resource.Id.akronimKartyTowaroweRowTextView);
            TextView gidnumer_TextView = row.FindViewById<TextView>(Resource.Id.gidNumerKartyTowaroweRowTextView);
            TextView typ_TextView = row.FindViewById<TextView>(Resource.Id.typKartyTowaroweRowsTextView);
            TextView nazwa_TextView = row.FindViewById<TextView>(Resource.Id.nazwaKartyTowaroweRowTextView);
            LinearLayout param_LinearLayout = row.FindViewById<LinearLayout>(Resource.Id.parametryKartyTowarowLinearLayout);

            kod_TextView.Text = mtwr_kod_List[position];
            gidnumer_TextView.Text = mtwr_gidnumer_List[position];
            typ_TextView.Text = mtwr_typ_List[position];
            nazwa_TextView.Text = mtwr_nazwa_List[position];

            if(mukrywanie ==1)
            {
                param_LinearLayout.Visibility = ViewStates.Visible;
            }
            else
            {
                param_LinearLayout.Visibility = ViewStates.Gone;
            }

            return row;
        }
        public override string this[int position]
        {
            get { return mtwr_kod_List[position]; }
        }
    }
}