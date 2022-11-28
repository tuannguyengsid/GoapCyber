﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Data.Mongodb.FrameworkCore
{   
    /// <summary>
    /// purpose of this class is to get timings of executions of certain code. Need to have this for recording purposes in the furture.
    /// </summary>
    public class GSIDTimer
    {
        Stopwatch sw;
        GSIDTimeUnit _timeUnit;

        public GSIDTimer()
        {
            _timeUnit = GSIDTimeUnit.Millisecond;

            sw = new Stopwatch();
            sw.Start();
        }

        public GSIDTimer(GSIDTimeUnit timeUnit)
        {
            _timeUnit = timeUnit;

            sw = new Stopwatch();
            sw.Start();
        }

        public string Finish() 
        {
            sw.Stop();

            if (_timeUnit == GSIDTimeUnit.Millisecond)
                return sw.ElapsedMilliseconds.ToString("0.##") + " ms";
            else if (_timeUnit == GSIDTimeUnit.Second)
                return sw.Elapsed.TotalSeconds.ToString("0.##") + " s";
            else if (_timeUnit == GSIDTimeUnit.Minute)
                return sw.Elapsed.TotalMinutes.ToString("0.##") + " mins";
            else if (_timeUnit == GSIDTimeUnit.Hour)
                return sw.Elapsed.TotalHours.ToString("0.##") + " hrs";
            else if (_timeUnit == GSIDTimeUnit.Day)
                return sw.Elapsed.TotalDays.ToString("0.##") + " days";
            else
                return "NOT SUPPORTED";
            
        }
    }

    public enum GSIDTimeUnit
    {
        Millisecond,
        Second,
        Minute,
        Hour,
        Day
    }
}
