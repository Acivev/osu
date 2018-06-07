using osu.Framework.Configuration;

namespace osu.Framework.Platform.Android
{
    public class AndroidGameWindow : GameWindow
    {
        public AndroidGameWindow(AndroidPlatformGameView gameView)
            : base(gameView.Width, gameView.Height)
        {
        }

        public override void SetupWindow(FrameworkConfigManager config)
        {
        }
    }
}
