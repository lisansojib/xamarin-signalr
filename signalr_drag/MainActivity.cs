using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;

namespace SignalR_Drag
{
    [Activity(Label = "SignalR_Drag", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private FrameLayout mRoot;
        private View draggableView;
        private IHubProxy mhubProxy;
        private float XInit = 0;
        private float YInit = 0;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource  
            SetContentView(Resource.Layout.Main);
            mRoot = FindViewById<FrameLayout>(Resource.Id.root);
            draggableView = FindViewById(Resource.Id.draggableView);
            draggableView.Touch += DraggableView_Touch;
            var querystringData = new Dictionary<string, string>
            {
                { "access_token", "B58m183YH2g7CY5bzj_yFwiL9e8Z7It1V_dA53ZWc4h84Te-wAPyKCgRvjF9NLO3QHD2B-t4LzBVBJPHLtLaxZw4UuOTshH3bgEdt4vZSsUCNwdYcLJcUEqE_5uRrsLOGg3Di2m7kGyJKp5p5Edymyaig7SfXyH85UkAFKhdzs1Zra35KsfAq_WQXDnUjz9K2VMWmxLE71dHHzvgLds-yvNb7ITXlE_TmQR4JQSYV0zL7ypJfmUEADb0r3PxqUzyz31eDfFsavq5fBY7k7Trzkkw5notp02hRrCMPtwoFB0vdct6fF9k9CCkep8h8mEEXCX0kWNQXv3oRilJX9jyc9CgAh5Z0IMeM6QKhVo5R7gI0Tc69itpjBscGfXuN6ljPvmcGxC7oGtEElMWRL3cxhrA-Hwmq-ga53RmkUlNs8TzvtZ9tyoH5d6ETCRsQfOeLBvD0HG3PAJZ76z0PIxzjQ4Z_k0dWv1Xb1Awp-Vf_TTyJDywpgTsUp3-Pl2x8FtM28MOHxunRqCLXGhSFWNJhQ" }
            };
            HubConnection hubConnection = new HubConnection("http://192.168.43.62", querystringData);
            mhubProxy = hubConnection.CreateHubProxy("ChatHub");
            try
            {
                await hubConnection.Start();
            }
            catch (Exception ex)
            {
                //Catch handle Errors.   
            }

            mhubProxy.On<ContactMessageViewModel>("newMessage", (message) =>
            {
                var v = message;
            });
        }
        private async void DraggableView_Touch(object sender, View.TouchEventArgs e)
        {
            float x = e.Event.RawX;
            float y = e.Event.RawY;
            View touchedView = sender as View;
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    XInit = touchedView.GetX() - x;
                    YInit = touchedView.GetY() - y;
                    break;
                case MotionEventActions.Move:
                    touchedView.Animate().X(e.Event.RawX + XInit).Y(e.Event.RawY + YInit).SetDuration(0).Start();
                    break;
                default:
                    break;
            }
            mRoot.Invalidate();
            //await mhubProxy.Invoke("DragView", new object[] {
            //    e.Event.RawX, XInit, e.Event.RawY, YInit
            //});
        }
    }

    /// <summary>
    /// ContactMessageViewModel
    /// </summary>
    public class ContactMessageViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// ContactUserID
        /// </summary>
        public string ContactUserID { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// MessageType is either "File/Text"
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// MessageContent
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// Discriminator
        /// </summary>
        public string Discriminator { get; set; }

        /// <summary>
        /// CreateOn
        /// </summary>
        public DateTime CreateOn { get; set; }

        /// <summary>
        /// CreateOnStr
        /// </summary>
        public string CreateOnStr { get; set; }

        /// <summary>
        /// MessageStatus
        /// </summary>
        public string MessageStatus { get; set; }

        /// <summary>
        /// HasBeenReadStatus
        /// </summary>
        public string HasBeenReadStatus { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Full file Path
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// ContentType
        /// </summary>
        public string ContentType { get; set; }
    }
}

