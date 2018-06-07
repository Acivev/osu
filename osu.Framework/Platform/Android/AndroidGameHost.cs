using System.Collections.Generic;
using osu.Framework.Input;
using osu.Framework.Input.Handlers;
using osu.Framework.Platform.Android.Input;
using osu.Framework.Platform.Linux;

namespace osu.Framework.Platform.Android
{
    public class AndroidGameHost : GameHost
    {
        private readonly AndroidPlatformGameView gameView;

        public AndroidGameHost(AndroidPlatformGameView gameView)
        {
            this.gameView = gameView;
            Window = new AndroidGameWindow(gameView);
        }

        public override ITextInputSource GetTextInput() => null;

        protected override IEnumerable<InputHandler> CreateAvailableInputHandlers()
        {
            yield return new AndroidTouchHandler(gameView);
        }

        protected override Storage GetStorage(string baseName) => new LinuxStorage(baseName);
    }
}
