using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;

namespace AplikacjaSerwisowa
{
    public class SlidingTabScrollView : HorizontalScrollView
    {
        private const int TITLE_OFFSET_DIPS = 24;
        private const int TAB_VIEW_PADDING_DIPS = 16;
            private const int TAB_VIEW_TEXT_SIZE_SP = 12;

        private int mTitleOffset;

        private int mTabViewLayoutID;
        private int mTabViewTextViewID;

        private ViewPager mViewPager;
        private ViewPager.IOnPageChangeListener mViewPageChangeListener;

        private static SlidingTabStrip mtabStrip;

        private int mScrollState;

        public interface TabColorizer
        {
            int GetIndicatorColor(int position);
            int GetDividerColor(int position);
        }

        public SlidingTabScrollView(Context context) : this(context, null) { }

        public SlidingTabScrollView(Context context, IAttributeSet attrs) : this(context, attrs, 0) { }

        public SlidingTabScrollView(Context context, IAttributeSet attrs, int defaultStyle) : base(context, attrs, defaultStyle)
        {
            float density = 1.5f;

            //disable the scroll bar
            HorizontalScrollBarEnabled = false;

            //make sure the tab strip fill the view
            FillViewport = true;
            this.SetBackgroundColor(Android.Graphics.Color.Rgb(0xE5, 0xE5, 0xE5)); //grey color

            mTitleOffset = (int)(TITLE_OFFSET_DIPS* density);

            mtabStrip = new SlidingTabStrip(context);
            this.AddView(mtabStrip, LayoutParams.MatchParent, LayoutParams.MatchParent);
        }

        public TabColorizer CustomTabColorizer
        {
            set { mtabStrip.CustomTabColorizer = value; }
        }

        public int [] SelectedIndicatorColor
        {
            set { mtabStrip.SelectedIndicatorColors = value; }
        }

        public int [] DividerColors
        {
            set { mtabStrip.DividerColors = value;}
        }

        public ViewPager.IOnPageChangeListener OnPageListener
        {
            set { mViewPageChangeListener = value; }
        }

        public ViewPager ViewPager
        {
            set
            {
                mtabStrip.RemoveAllViews();

                mViewPager = value;
                if(value != null)
                {
                    value.PageSelected += value_PageSelected;
                    value.PageScrollStateChanged += value_PageScrollStateChange;
                    value.PageScrolled += value_PageScrolled;
                }
            }
        }

        private void value_PageScrolled(object sender, ViewPager.PageScrolledEventArgs e)
        {
            int tabCount = mtabStrip.ChildCount;

            if(tabCount == 0 || e.Position < 0 || e.Position >= tabCount)
            {
                return;
            }

            mtabStrip.OnViewPagerPageChange(e.Position, e.PositionOffset);

            View selectedTitle = mtabStrip.GetChildAt(e.Position);
            int extraOffset = (selectedTitle != null ? (e.Position * selectedTitle.Width) : 0);
            scrollToTab(e.Position, extraOffset);

            if(mViewPageChangeListener != null)
            {
                mViewPageChangeListener.OnPageScrolled(e.Position,e.PositionOffset,e.PositionOffsetPixels);
            }
        }

        private void value_PageScrollStateChange(object sender, ViewPager.PageScrollStateChangedEventArgs e)
        {
            mScrollState = e.State;

            if(mViewPageChangeListener != null)
            {
                mViewPageChangeListener.OnPageScrollStateChanged(e.State);
            }
        }

        void value_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if(mScrollState == ViewPager.ScrollStateIdle)
            {
                mtabStrip.OnViewPagerPageChange(e.Position, 0f);
                scrollToTab(e.Position, 0);
            }

            if(mViewPageChangeListener != null)
            {
                mViewPageChangeListener.OnPageSelected(e.Position);
            }
        }

        private void PopulateTabStrip()
        {
            PagerAdapter adapter = mViewPager.Adapter;
            for(int i=0;i<adapter.Count;i++)
            {
                TextView tabView = CreateDefaultTabView(Context);
                tabView.Text = i.ToString();
                tabView.SetTextColor(Android.Graphics.Color.Black);
                tabView.Tag = i;
                tabView.Click += tabView_Click;
                mtabStrip.AddView(tabView);
            }
        }

        private void tabView_Click(object sender, EventArgs e)
        {
            TextView clickTab = (TextView)sender;
            int pageToScrollTo = (int)clickTab.Tag;
            mViewPager.CurrentItem = pageToScrollTo;
        }

        private TextView CreateDefaultTabView(Context context)
        {
            float density = 1.5f;

            TextView textView = new TextView(context);
            textView.Gravity = GravityFlags.Center;
            textView.SetTextSize(ComplexUnitType.Sp, TAB_VIEW_TEXT_SIZE_SP);
            textView.Typeface = Android.Graphics.Typeface.DefaultBold;

            if(Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Honeycomb)
            {
                TypedValue outValue = new TypedValue();
                Context.Theme.ResolveAttribute(Android.Resource.Attribute.SelectableItemBackground, outValue, false);
                textView.SetBackgroundResource(outValue.ResourceId);
            }

            if(Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.IceCreamSandwich)
            {
                textView.SetAllCaps(true);
            }

            int padding = (int)(TAB_VIEW_PADDING_DIPS * density);
            textView.SetPadding(padding, padding, padding, padding);

            return textView;
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            if(mViewPager != null)
            {
                scrollToTab(mViewPager.CurrentItem, 0);
            }
        }

        private void scrollToTab(int tabIndex, int extraOffset)
        {
            int tabCount = mtabStrip.ChildCount;
            if(tabCount == 0|| tabIndex<0 || tabIndex>=tabCount)
            {
                // no need to go
                return;
            }


            View selectedChild = mtabStrip.GetChildAt(tabCount);
            if(selectedChild != null)
            {
                int scrollAmountX = selectedChild.Left + extraOffset;

                if(tabCount>0|| extraOffset > 0)
                {
                    scrollAmountX -= mTitleOffset;
                }

                this.ScrollTo(scrollAmountX, 0);
            }
        }
    }
}