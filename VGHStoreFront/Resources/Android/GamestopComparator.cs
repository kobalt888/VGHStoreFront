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
using Android.Hardware;
using Android.Graphics;
using Android.Util;
using PriceCheckerVGH;
using System.Threading;
using VGHStoreFront;
using Android.Views.InputMethods;

namespace VGHTools
{
    [Activity(Theme = "@android:style/Theme.Black", Label = "Gamestop Price Viewer")]
    public class GamestopComparator : Activity
    {
        Core lookupCore = new Core();
        int upcModifier = 0;
        protected override void OnCreate(Bundle bundle)
        {
            
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Gamestop);
            TextView UPCBox = FindViewById<TextView>(Resource.Id.upcBox);
            RadioButton standardUpc = FindViewById<RadioButton>(Resource.Id.radioButton1);
            RadioButton vghUpc = FindViewById<RadioButton>(Resource.Id.radioButton2);
            ImageView logo = FindViewById<ImageView>(Resource.Id.imageView1);
            logo.Click += logoClick;
            standardUpc.Click += RadioButtonClick;
            vghUpc.Click += RadioButtonClick;
            UPCBox.KeyPress += upcEvent;
            Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);
        }

        private InputMethodManager getSystemService(object iNPUT_METHOD_SERVICE)
        {
            throw new NotImplementedException();
        }

        private void logoClick(object sender, EventArgs e)
        {
            if(lookupCore.gameData!=null)
            {
                Toast.MakeText(this, "Game Added", ToastLength.Short).Show();
                lookupCore.writeGame();
            }
            

        }

        private void RadioButtonClick(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            Toast.MakeText(this, rb.Text, ToastLength.Short).Show();
            if(rb.Text=="VGH UPC")
            {
                upcModifier = 2;
            }
            else
            {
                upcModifier = 0;
            }
        }



        private void upcEvent(object sender, View.KeyEventArgs e)
        {
            if(e.Event.Action==KeyEventActions.Down && e.KeyCode == Keycode.Enter)
            {
                e.Handled = true;
                TextView UPCBox = FindViewById<TextView>(Resource.Id.upcBox);
                string foundUpc = UPCBox.Text;
                if (foundUpc.Length > 11)
                {
                    if(upcModifier==2)
                    {
                        string upc = UPCBox.Text.Substring(0, UPCBox.Text.Length - 2);
                        ThreadPool.QueueUserWorkItem(o => getGame(upc));
                    }
                    else
                    {
                        ThreadPool.QueueUserWorkItem(o => getGame(foundUpc));
                    }
                }
                else
                {
                    Toast.MakeText((this), Resource.String.FailedUpc, ToastLength.Long).Show();
                    lookupCore.result = "Fail";
                    updateUI();
                }
            }     
            else
            {
                e.Handled = false;
            }
        }


        private void getGame(string upc)
        {         
           lookupCore.getGame(upc).Wait();
           updateUI();
        }

        private void updateUI()
        {
            try
            {
                TextView gameTitleView = FindViewById<TextView>(Resource.Id.GameTitleTextView);
                TextView consoleView = FindViewById<TextView>(Resource.Id.ConsoleTextView);
                TextView priceView = FindViewById<TextView>(Resource.Id.PriceTextView);
                TextView UPCBox = FindViewById<TextView>(Resource.Id.upcBox);

                RunOnUiThread(() => UPCBox.Text = "");


                if (lookupCore.result=="OK")
                {
                    
                    RunOnUiThread(() => gameTitleView.Text = lookupCore.gameData.title);
                    RunOnUiThread(() => consoleView.Text = lookupCore.gameData.console);
                    RunOnUiThread(() => priceView.Text = lookupCore.gameData.price);
                }
                else
                {
                    //failed to get a new game...
                    Toast.MakeText((this), Resource.String.FailedUpc, ToastLength.Long).Show();
                    RunOnUiThread(() => gameTitleView.Text = "No game found with UPC");
                    RunOnUiThread(() => consoleView.Text = "No game found with UPC");
                    RunOnUiThread(() => priceView.Text = "No game found with UPC");
                }

            }

            catch (Exception ex)
            {
                Log.Error("VGH", ex.Message);
            }
        }

        
    }
}