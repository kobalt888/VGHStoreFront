using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Util;
using System;
using VGHStoreFront;
using VGHStoreFront.Resources.Android;

namespace VGHTools
{
    [Activity(Theme = "@android:style/Theme.Black", Label = "VGH Tools", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Menu);
            Button GamestopMenuButton = FindViewById<Button>(Resource.Id.GamestopMenuButton);
            Button GamestopViewer = FindViewById<Button>(Resource.Id.GamestopLogButton);
            GamestopMenuButton.Click += myCustomClick;
            GamestopViewer.Click += logViewClick;

        }

        private void myCustomClick(object sender, EventArgs e)
        {
            StartActivity(typeof(GamestopComparator));
        }

        private void logViewClick(object sender, EventArgs e)
        {
            StartActivity(typeof(GamestopLogViewer));
        }


    }
}

