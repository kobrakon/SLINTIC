using System;
using System.Timers;

namespace SLINTIC
{
    public class Interval
    {
        public System.Timers.Timer timer;
        public float interval;
        public Action method;

        /// <summary>
        /// Invokes the target method in a loop according to a set interval
        /// </summary>
        public Interval(Action method, float interval)
        {
            this.method = method;
            this.interval = interval;
            this.timer = new System.Timers.Timer(interval);
            this.timer.AutoReset = true;
            this.timer.Elapsed += (sender, e) => 
            {
                this.method();
            };
        }

        /// <summary>
        /// Starts the interval loop
        /// </summary>
        public void Start()
        {
            this.timer.Start();
        }

        /// <summary>
        /// Suspends the interval loop
        /// </summary>
        public void Suspend()
        {
            this.timer.Stop();
        }
    }
}