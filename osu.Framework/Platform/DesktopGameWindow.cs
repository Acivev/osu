﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

extern alias Android;

using System;
using osu.Framework.Configuration;
using osu.Framework.Input;
using OpenTK;
using Size = Android::System.Drawing.Size;
using Point = Android::System.Drawing.Point;

namespace osu.Framework.Platform
{
    public class DesktopGameWindow : GameWindow
    {
        private const int default_width = 1366;
        private const int default_height = 768;

        private readonly BindableInt widthFullscreen = new BindableInt();
        private readonly BindableInt heightFullscreen = new BindableInt();
        private readonly BindableInt width = new BindableInt();
        private readonly BindableInt height = new BindableInt();

        private readonly BindableDouble windowPositionX = new BindableDouble();
        private readonly BindableDouble windowPositionY = new BindableDouble();

        public readonly Bindable<WindowMode> WindowMode = new Bindable<WindowMode>();

        public readonly Bindable<ConfineMouseMode> ConfineMouseMode = new Bindable<ConfineMouseMode>();

        protected new OpenTK.GameWindow Implementation => (OpenTK.GameWindow)base.Implementation;

        public DesktopGameWindow()
            : base(default_width, default_height)
        {
            Implementation.Resize += OnResize;
            Implementation.Move += OnMove;
        }

        public override void SetupWindow(FrameworkConfigManager config)
        {
            config.BindWith(FrameworkSetting.WidthFullscreen, widthFullscreen);
            config.BindWith(FrameworkSetting.HeightFullscreen, heightFullscreen);

            config.BindWith(FrameworkSetting.Width, width);
            config.BindWith(FrameworkSetting.Height, height);

            config.BindWith(FrameworkSetting.WindowedPositionX, windowPositionX);
            config.BindWith(FrameworkSetting.WindowedPositionY, windowPositionY);

            config.BindWith(FrameworkSetting.ConfineMouseMode, ConfineMouseMode);

            ConfineMouseMode.ValueChanged += confineMouseMode_ValueChanged;
            ConfineMouseMode.TriggerChange();

            config.BindWith(FrameworkSetting.WindowMode, WindowMode);

            WindowMode.ValueChanged += windowMode_ValueChanged;
            WindowMode.TriggerChange();

            Exited += onExit;
        }

        protected void OnResize(object sender, EventArgs e)
        {
            if (Implementation.ClientSize.IsEmpty) return;

            switch (WindowMode.Value)
            {
                case Configuration.WindowMode.Windowed:
                    width.Value = Implementation.ClientSize.Width;
                    height.Value = Implementation.ClientSize.Height;
                    break;
            }
        }

        protected void OnMove(object sender, EventArgs e)
        {
            // The game is windowed and the whole window is on the screen (it is not minimized or moved outside of the screen)
            if (WindowMode.Value == Configuration.WindowMode.Windowed
                && Position.X > 0 && Position.X < 1
                && Position.Y > 0 && Position.Y < 1)
            {
                windowPositionX.Value = Position.X;
                windowPositionY.Value = Position.Y;
            }
        }

        private void confineMouseMode_ValueChanged(ConfineMouseMode newValue)
        {
            bool confine = false;

            switch (newValue)
            {
                case Input.ConfineMouseMode.Fullscreen:
                    confine = WindowMode.Value != Configuration.WindowMode.Windowed;
                    break;
                case Input.ConfineMouseMode.Always:
                    confine = true;
                    break;
            }

            if (confine)
                CursorState |= CursorState.Confined;
            else
                CursorState &= ~CursorState.Confined;
        }

        private void windowMode_ValueChanged(WindowMode newMode)
        {
            switch (newMode)
            {
                case Configuration.WindowMode.Fullscreen:
                    DisplayResolution newResolution = DisplayDevice.Default.SelectResolution(widthFullscreen, heightFullscreen, 0, DisplayDevice.Default.RefreshRate);
                    DisplayDevice.Default.ChangeResolution(newResolution);

                    Implementation.WindowState = WindowState.Fullscreen;
                    break;
                case Configuration.WindowMode.Borderless:
                    DisplayDevice.Default.RestoreResolution();

                    Implementation.WindowState = WindowState.Maximized;
                    Implementation.WindowBorder = WindowBorder.Hidden;

                    //must add 1 to enter borderless
                    Implementation.ClientSize = new Size(DisplayDevice.Default.Bounds.Width + 1, DisplayDevice.Default.Bounds.Height + 1);
                    Position = Vector2.Zero;
                    break;
                default:
                    DisplayDevice.Default.RestoreResolution();

                    Implementation.WindowState = WindowState.Normal;
                    Implementation.WindowBorder = WindowBorder.Resizable;

                    Implementation.ClientSize = new Size(width, height);
                    Position = new Vector2((float)windowPositionX, (float)windowPositionY);
                    break;
            }

            ConfineMouseMode.TriggerChange();
        }

        private void onExit()
        {
            switch (WindowMode.Value)
            {
                case Configuration.WindowMode.Fullscreen:
                    widthFullscreen.Value = Implementation.ClientSize.Width;
                    heightFullscreen.Value = Implementation.ClientSize.Height;
                    break;
            }

            DisplayDevice.Default.RestoreResolution();
        }

        public override Vector2 Position
        {
            get
            {
                return new Vector2((float)Implementation.Location.X / (DisplayDevice.Default.Width - Implementation.Size.Width),
                                   (float)Implementation.Location.Y / (DisplayDevice.Default.Height - Implementation.Size.Height));
            }

            set
            {
                Implementation.Location = new Point(
                    (int)Math.Round((DisplayDevice.Default.Width - Implementation.Size.Width) * value.X),
                    (int)Math.Round((DisplayDevice.Default.Height - Implementation.Size.Height) * value.Y));
            }
        }

        public override void CycleMode()
        {
            switch (WindowMode.Value)
            {
                case Configuration.WindowMode.Windowed:
                    WindowMode.Value = Configuration.WindowMode.Borderless;
                    break;
                case Configuration.WindowMode.Borderless:
                    WindowMode.Value = Configuration.WindowMode.Fullscreen;
                    break;
                default:
                    WindowMode.Value = Configuration.WindowMode.Windowed;
                    break;
            }
        }
    }
}
