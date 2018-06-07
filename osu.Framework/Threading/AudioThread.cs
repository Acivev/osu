﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using osu.Framework.Statistics;
using System;
using System.Collections.Generic;

namespace osu.Framework.Threading
{
    public class AudioThread : GameThread
    {
        internal const string THREAD_NAME = "Framework.Audio.Thread";

        public AudioThread(Action onNewFrame)
            : base(onNewFrame, THREAD_NAME)
        {
        }

        internal override IEnumerable<StatisticsCounterType> StatisticsCounters => new[]
        {
            StatisticsCounterType.TasksRun,
            StatisticsCounterType.Tracks,
            StatisticsCounterType.Samples,
            StatisticsCounterType.SChannels,
            StatisticsCounterType.Components,
        };

        public override void Exit()
        {
            base.Exit();

            ManagedBass.Bass.Free();
        }
    }
}
