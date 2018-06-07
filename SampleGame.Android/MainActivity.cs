using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using osu.Framework.Platform.Android;

namespace SampleGame.Android
{
    [Activity(Label = "SampleGame.Android", MainLauncher = true, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Activity
    {
        private AndroidPlatformGameView gameView;
        private AndroidGameHost host;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn | WindowManagerFlags.Fullscreen);

            gameView = new AndroidPlatformGameView(ApplicationContext);
            SetContentView(gameView);
        }

        protected override void OnStart()
        {
            base.OnStart();
            
            //gameView.Run(30.0);

            /*
            host = new AndroidGameHost(gameView);
            host.Run(new SampleGame());
            */
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
        }
    }
}

