using Android.App;
using Android.Widget;
using Android.OS;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Views;

namespace SignalR_Drag
{
    [Activity(Label = "SignalR_Drag", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private LinearLayout mRoot;

        HubConnection hubConnection;
        private IHubProxy mhubProxy;
        TextView textView;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource  
            SetContentView(Resource.Layout.Main);
            mRoot = FindViewById<LinearLayout>(Resource.Id.root);

            LinearLayout ll = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.WrapContent);

            TextView tv = new TextView(this) { Text = "Hello" };
            tv.LayoutParameters = lp;
            ll.AddView(tv);

            TextView tv2 = new TextView(this) { Text = "Hello" };
            tv2.LayoutParameters = lp;
            ll.AddView(tv2);

            var that = this;

            await ConnectToSignalRServer();

            mhubProxy.On<ContactMessageViewModel>("newMessage", (message) =>
            {
                var v = message.MessageContent;
            });
        }

        private async Task<bool> ConnectToSignalRServer()
        {
            bool connected = false;
            try
            {
                var querystringData = new Dictionary<string, string>
                {
                    { "access_token", "_Mo_T4X3v2kA5BESwSCDCnI0ZqMAppMEcGaMju6RrNX5WeKmnYn0UB1WdAt8kAaZ64AD8NTwrsJj3bgD64y0mr70sOfloEPJNb2AprHjNIsYvirKkBiZ0Bek1IjmTg0bcnE1u4Wt_gjMu2xaGAO2XYJoCCHEMUjUYCDEF25R7yV0EroNiRWftR2kxP0m_3oTliHnX50HnWFHfjJGbKoLuOLaJpl3WJs7k5Gcuxf51RSyP50Vum910NiCEYDBLfSL82u_gTqqE-LQETNStmTPq_kM2b44Huo9kdVckxrjbfbbMpQENFL0teS-s6Y4zSLn98q1IdvhUp7smQUMx_KDDKGWWJuCUo056h0w6R5--WOAK5FS6I34XV5q2NqVSD2n8dpx42APHDrhiGHO_TCUZ8t15XGgYfFGMwFFydzscvjdzuj8D3Ovx2RQVD4DFMX3vLCf2FjOCjUr0q-Ne6R0GSBZhkM8FGDEGQd1MGpKUPwN-OOE_uEelWUOUczHRVUeBjRgdmnftfXFp31vhAyQ-A" }
                };

                hubConnection = new HubConnection("http://192.168.0.8", querystringData);
                mhubProxy = hubConnection.CreateHubProxy("ChatHub");
                await hubConnection.Start();

                //See @Oran Dennison's comment on @KingOfHypocrites's answer
                if (hubConnection.State == ConnectionState.Connected)
                {
                    connected = true;
                    hubConnection.Closed += Connection_Closed;
                }                

                return connected;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                return false;
            }
        }

        private async void Connection_Closed()
        {
            // specify a retry duration
            TimeSpan retryDuration = TimeSpan.FromSeconds(30);

            while (DateTime.UtcNow < DateTime.UtcNow.Add(retryDuration))
            {
                bool connected = await ConnectToSignalRServer();
                if (connected)
                    return;
            }
            Console.WriteLine("Connection closed");
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

