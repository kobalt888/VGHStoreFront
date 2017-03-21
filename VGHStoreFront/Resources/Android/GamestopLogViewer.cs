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
using Android.Util;

namespace VGHStoreFront.Resources.Android
{
    [Activity(Theme = "@android:style/Theme.Black", Label = "GamestopLogViewer")]
    public class GamestopLogViewer : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LogView);
            string[] files= { "1", "2" };
            //files = System.IO.Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
            List<string> fileList = new List<string>(files);

            try
            {
               
            }
            catch(Exception ex)
            {
                Log.Error("VGH", ex.Message + ex.InnerException.Message + ex.Source);
            }
        }
    }
}