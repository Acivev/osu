extern alias Android;
using System.Collections.Generic;
using Android::Android.Views;
using osu.Framework.Input;
using osu.Framework.Input.Handlers;
using OpenTK;
using OpenTK.Input;


namespace osu.Framework.Platform.Android.Input
{
    public class AndroidTouchHandler : InputHandler
    {
        private List<int> pointerIds = new List<int>();

        public AndroidTouchHandler(AndroidPlatformGameView gameView)
        {
            gameView.Touch += handleTouches;
        }

        private void handleTouches(object sender, View.TouchEventArgs e)
        {
            var basicState = new Framework.Input.MouseState
            {
                Position = new Vector2(e.Event.GetX(), e.Event.GetY())
            };

            switch (e.Event.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.Move:
                    basicState.SetPressed(MouseButton.Left, true);
                    PendingStates.Enqueue(new InputState { Mouse = basicState });
                    break;
                case MotionEventActions.Up:
                    PendingStates.Enqueue(new InputState { Mouse = basicState });
                    break;
            }

            /* Multi-touch?

            var pointerIndex = e.Event.ActionIndex;

            var pointerId = e.Event.GetPointerId(pointerIndex);

            switch (e.Event.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:

                    break;
                case MotionEventActions.Up:
                case MotionEventActions.PointerUp:

                    break;
            }*/
        }

        public override bool Initialize(GameHost host)
        {
            return true;
        }

        public override bool IsActive => true;
        public override int Priority => 0;
    }
}
