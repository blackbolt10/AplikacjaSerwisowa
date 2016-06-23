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

        public kartyTowarow_ListViewAdapter(Context context, List<string> twr_kod_List, List<string> twr_gidnumer_List, List<string> twr_typ_List, List<string> twr_nazwa_List, Int32 ukrywanie)
        {
            mtwr_gidnumer_List = twr_gidnumer_List;
            mtwr_kod_List = twr_kod_List;
            mtwr_nazwa_List = twr_nazwa_List;
            mtwr_typ_List = twr_typ_List;
            mukrywanie = ukrywanie;

            mContext = context;
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