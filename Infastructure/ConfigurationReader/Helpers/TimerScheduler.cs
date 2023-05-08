using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationReaderLib.Helpers
{
    internal class TimerScheduler : IDisposable
    {
        private List<System.Timers.Timer> _timerList = new List<System.Timers.Timer>();

        internal System.Timers.Timer ScheduleTimer(Action method, int timeInterval)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            var timer = new System.Timers.Timer();
            timer.Interval = timeInterval;
            timer.Elapsed += (Object source, System.Timers.ElapsedEventArgs e) =>
            {
                method.Invoke();
            };
            timer.Start();
            _timerList.Add(timer);
            return timer;
        }

        internal void CloseTimer(System.Timers.Timer timer)
        {
            if (timer == null)
            {
                throw new ArgumentNullException("timer");
            }

            var isTimerInTheList = _timerList.Contains(timer);

            if (!isTimerInTheList)
            {
                throw new Exception("Timer Not Scheduled Yet But Tried To Be Closed");
            }

            timer.Close();
            _timerList.Remove(timer);
        }

        public void Dispose()
        {
            foreach (var timer in _timerList)
            {
                timer.Dispose();
            }
        }
    }
}
