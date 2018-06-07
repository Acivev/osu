extern alias Android;

using Android::Android.Content;
using OpenTK.Platform.Android;

namespace osu.Framework.Platform.Android
{
    public class AndroidPlatformGameView : AndroidGameView
    {
        public AndroidPlatformGameView(Context context)
            : base(context)
        {
        }
    }
}
