using Emyra.Simulator.EventChannel;
using Emyra.Simulator.GameData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emyra.Simulator.Managers
{

    public enum DayString { Monday = 1, Tuesday = 2, Wednesday, Thursday, Friday, Saturday, Sunday }

    public class DayManager : MonoBehaviour
    {
        [field: Title("Current Game Info")]
        [field: SerializeField] public int Week { get; private set; } = 0;
        [field: SerializeField] public int Day { get; private set; } = 1;
        [field: SerializeField] [field: TimeSelector] public int Time { get; private set; } = 0;

        public int Hour { get { return Time / 60; } }
        public int Minute { get { return Time % 60; } }

        [Tooltip("Default time character wakes up at after sleeping")]
        [TimeSelector] public int _dayStartTime;

        [TitleGroup("Events")]
        //[FoldoutGroup("Events/Listening to Events")]
        //[SerializeField] private IntEventSO _setTimeEvent; // will add if needed

        //[FoldoutGroup("Events/Listening to Events")]
        //[Tooltip("Set current time to the default day start time.")]
        //[SerializeField] private VoidEventSO _resetTimeEvent;  // not sure if this one is necessary
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private IntEventSO _addTimeEvent;
        [FoldoutGroup("Events/Listening to Events")]
        // invoke to sleep until start of next/current morning
        [SerializeField] private VoidEventSO _sleepEvent; 

        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private IntEventSO _weekChangedEvent;
        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private IntEventSO _timeChangedEvent;

        [FoldoutGroup("Events/Responding to Events")]
        [SerializeField] private VoidIntRequestEventSO _changeDayEvent;

        // number of minutes in a day
        private int _maxTime = (60 * 24);


        #region Singleton
        // singleton reference
        private static DayManager _instance;
        public static DayManager Instance { get { return _instance; } }

        private void Awake()
        {
            // singleton pattern
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            Time = _dayStartTime;
        }
        #endregion



        private void OnEnable()
        {
            _changeDayEvent.OnRequestEvent += OnChangeDayRequest;
            // time
            //_setTimeEvent.OnInvokeEvent += OnSetTimeEvent;
            //_resetTimeEvent.OnInvokeEvent += OnResetTimeEvent;
            _addTimeEvent.OnInvokeEvent += OnAddTimeEvent;
            // game events
            _sleepEvent.OnInvokeEvent += OnSleepEvent;
        }
        private void OnDisable()
        {
            _changeDayEvent.OnRequestEvent -= OnChangeDayRequest;
            // time
            //_setTimeEvent.OnInvokeEvent -= OnSetTimeEvent;
            //_resetTimeEvent.OnInvokeEvent -= OnResetTimeEvent;
            _addTimeEvent.OnInvokeEvent -= OnAddTimeEvent;
            // game events
            _sleepEvent.OnInvokeEvent -= OnSleepEvent;
        }

        #region Utility
        public string GetDayString()
        {
            return ((DayString)Day).ToString();
        }

        public string GetTimeString()
        {
            int hours = Time / 60;
            int minutes = Time % 60;
            return string.Format("{0:00}:{1:00}", hours, minutes);
        }
        #endregion

        #region Callbacks
        private void OnChangeDayRequest() { ChangeDay(); }
       // private void OnSetTimeEvent(int newTime) { SetTime(newTime); }
        //private void OnResetTimeEvent() { ResetTime(); }
        private void OnAddTimeEvent(int minutes) { AddTime(minutes); }
        #endregion

        // called when player sleeps for thte night
        private void OnSleepEvent()
        {
            // todo: some sort of screen transition effect

            // change to next day if player sleeps after day starts and before midnight
            if (Time > _dayStartTime) // note: must check before time is reset
            {
                ChangeDay();
            }

            // reset time to default day start time
            ResetTime();
        }

        #region Day
        /// <summary>
        /// Change current day to next day in the week.
        /// Does not change time.
        /// </summary>
        private void ChangeDay()
        {
            // if current day is Sunday
            if (Day == 7)
            {
                // start new week
                Week += 1;
                _weekChangedEvent.InvokeEvent(Week);
                // reset day to Monday
                Day = 1;
            }
            else
            {
                // move to next day
                Day += 1;
            }

            // broadcast request result
            _changeDayEvent.SendResult(Day);
        }
        #endregion


        #region Time 
        private void SetTime(int newTime)
        {
            // set time to new time
            Time = newTime;
            _timeChangedEvent.InvokeEvent(Time);
        }

        private void ResetTime()
        {
            Time = _dayStartTime;
            _timeChangedEvent.InvokeEvent(Time);
        }

        /// <summary>
        /// Add minutes to current time, changing to next day if necessary.
        /// </summary>
        /// <param name="minutes"></param>
        private void AddTime(int minutes)
        {
            // if time goes past 24 hours
            if (Time + minutes >= _maxTime)
            {
                // calculate new time
                Time = (Time + minutes) - _maxTime;
                // broadcast new time
                _timeChangedEvent.InvokeEvent(Time);
                // change day
                ChangeDay();
            }
            else
            {
                // add minutes and broadcast change
                Time += minutes;
                _timeChangedEvent.InvokeEvent(Time);
            }
        }
        #endregion
    }
}
