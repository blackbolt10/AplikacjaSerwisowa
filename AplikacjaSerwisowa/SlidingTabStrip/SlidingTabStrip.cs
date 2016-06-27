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
using Android.Graphics;

namespace AplikacjaSerwisowa
{
    public class SlidingTabStrip :LinearLayout
    {
        private const int DEFAULT_BOTTOM_BORDER_THICKNESS_DIPS = 2;
        private const byte DEFAULT_BOTTOM_BORDER_COLOR_ALPHA = 0x26;
        private const int SELECTED_INDICATOR_THICKNESS_DIPS = 8;
        private int[] INDICATOR_COLORS = { 0x19A319, 0x0000FC };
        private int[] DIVIDER_COLORS = { 0xC5C5C5};


        private const int DEFAULT_DIVIDER_THICKNESS_DIPS = 2;
        private const float DEFAULT_DIVIDER_HEIGHT = 0.5f;

        //BottomBorder
        private int mBottomBorderThickness;
        private Paint mBottomBorderPaint;
        private int mDefaultBottomBorderColor;

        //Indicator
        private int mSelectedIndicatorThickness;
        private Paint mSelectedIndicatorPaint;

        //Divider
        private Paint mDividerPaint;
        private float mDividerHeight;

        //Selected position and offset
        private int mSelectedPosition;
        private float mSelectionOffset;

        //Tab colorizer
        private SlidingTabScrollView.TabColorizer mCustomTabColorizer;
        private SimplyTabColorizer mDefaultTabColorizer;


        public SlidingTabStrip(Context context): this(context,null)
        { }

        public SlidingTabStrip(Context context , IAttributeSet attrs): base(context,attrs)
        {
            SetWillNotDraw(false);

            float density = 1.5f;

            TypedValue outValue = new TypedValue();
            context.Theme.ResolveAttribute(Android.Resource.Attribute.ColorForeground, outValue, true);
            int themeForeGround = outValue.Data;
            mDefaultBottomBorderColor = SetColorAlpha(themeForeGround, DEFAULT_BOTTOM_BORDER_COLOR_ALPHA);

            mDefaultTabColorizer = new SimplyTabColorizer();
            mDefaultTabColorizer.IndicatorColors = INDICATOR_COLORS;
            mDefaultTabColorizer.DividerColors = DIVIDER_COLORS;

            mBottomBorderThickness = (int)(DEFAULT_BOTTOM_BORDER_THICKNESS_DIPS * density);
            mBottomBorderPaint = new Paint();
            mBottomBorderPaint.Color = GetColorFromInteger(0xC5C5C5);

            mSelectedIndicatorThickness = (int)(SELECTED_INDICATOR_THICKNESS_DIPS * density);
            mSelectedIndicatorPaint = new Paint();

            mDividerHeight = DEFAULT_DIVIDER_HEIGHT;
            mDividerPaint = new Paint();
            mDividerPaint.StrokeWidth = (int)(DEFAULT_DIVIDER_THICKNESS_DIPS * density);
        }

        public SlidingTabScrollView.TabColorizer CustomTabColorizer
        {
            set
            {
                mCustomTabColorizer = value;
                this.Invalidate();
            }
        }

        public int [] SelectedIndicatorColors
        {
            set
            {
                mCustomTabColorizer = null;
                mDefaultTabColorizer.IndicatorColors = value;
                this.Invalidate();
            }
        }

        public int [] DividerColors
        {
            set
            {
                mDefaultTabColorizer = null;
                mDefaultTabColorizer.DividerColors = null;
                this.Invalidate();
            }
        }

        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetBlueComponent(color), Color.GetGreenComponent(color));
        }

        private int SetColorAlpha(int color, byte alpha)
        {
            return Color.Argb(alpha, Color.GetRedComponent(color), Color.GetBlueComponent(color), Color.GetGreenComponent(color));
        }

        public void OnViewPagerPageChange(int position, float positionOffset)
        {
            mSelectedPosition = position;
            mSelectionOffset = positionOffset;
            this.Invalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            int height = Height;
            int tabCount = ChildCount;
            int dividerheightPx = (int)(Math.Min(Math.Max(0f, mDividerHeight), 1f) * height);
            SlidingTabScrollView.TabColorizer tabColorizer = mCustomTabColorizer != null ? mCustomTabColorizer : mDefaultTabColorizer;


            if(tabCount > 0)
            {
                View selectedTitle = GetChildAt(mSelectedPosition);
                int left = selectedTitle.Left;
                int right = selectedTitle.Right;
                int color = tabColorizer.GetIndicatorColor(mSelectedPosition);

                if(mSelectionOffset>0f&& mSelectedPosition<(tabCount-1))
                {
                    int nextColor = tabColorizer.GetIndicatorColor(mSelectedPosition + 1);
                    if(color != nextColor)
                    {
                        color = blendColor(nextColor, color, mSelectionOffset);
                    }

                    View nextTitle = GetChildAt(mSelectedPosition + 1);
                    left = (int)(mSelectionOffset + nextTitle.Left + (1.0f - mSelectionOffset) * left);
                    right = (int)(mSelectionOffset + nextTitle.Right + (1.0f - mSelectionOffset) * right);
                }

                mSelectedIndicatorPaint.Color = GetColorFromInteger(color);

                canvas.DrawRect(left, height - mSelectedIndicatorThickness, right, height,mSelectedIndicatorPaint);

                //Creates vertivle dividers beetween tabs
                int separatorTop = (height - dividerheightPx) / 2;
                for(int i = 0; i<ChildCount;i++)
                {
                    View child = GetChildAt(i);
                    mDividerPaint.Color = GetColorFromInteger(tabColorizer.GetDividerColor(i));
                    canvas.DrawLine(child.Right, separatorTop, child.Right, separatorTop + dividerheightPx, mDividerPaint);
                }

                canvas.DrawRect(0, height - mBottomBorderThickness, Width, height, mBottomBorderPaint);
            }
        }

        private int blendColor(int color1, int color2, float ratio)
        {
            float inverRation = 1f - ratio;
            float r = (Color.GetRedComponent(color1) * ratio) + (Color.GetRedComponent(color2) * inverRation);
            float b = (Color.GetBlueComponent(color1) * ratio) + (Color.GetBlueComponent(color2) * inverRation);
            float g = (Color.GetGreenComponent(color1) * ratio) + (Color.GetGreenComponent(color2) * inverRation);

            return Color.Rgb((int)r, (int)g, (int)b);
        }

        private class SimplyTabColorizer: SlidingTabScrollView.TabColorizer
        {
            private int[] mIndicatorColors;
            private int[] mDividerColors;

            public int GetIndicatorColor(int position)
            {
                return mIndicatorColors[position % mIndicatorColors.Length];
            }

            public int GetDividerColor(int position)
            {
                return mDividerColors[position % mDividerColors.Length];
            }

            public int[] IndicatorColors
            {
                set { mIndicatorColors = value; }
            }
            public int[] DividerColors
            {
                set { mDividerColors = value; }
            }

        }
    }
}