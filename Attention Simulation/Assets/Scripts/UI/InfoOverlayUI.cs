using Emyra.Simulator.EventChannel;
using Emyra.Simulator.GameData;
using Emyra.Simulator.Managers;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Emyra.Simulator.UI
{
    public class InfoOverlayUI : MonoBehaviour
    {
        public OverlayDataSource OverlayData;
        public UIDocument UIDocument;

        [TitleGroup("Events")]
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private IntEventSO _weekChangedEvent;
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private VoidIntRequestEventSO _changeDayEvent;
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private IntEventSO _timeChangedEvent;
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private LocationEventSO _locationChangedEvent;
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private VoidActivityEventSO _activityChangedEvent;

        private void Awake()
        {
            OverlayData = new OverlayDataSource();
            UIDocument.rootVisualElement.dataSource = OverlayData;
        }

        private void OnEnable()
        {
            _weekChangedEvent.OnInvokeEvent += OnWeekChanged;
            _changeDayEvent.OnResultEvent += OnDayChanged;
            _timeChangedEvent.OnInvokeEvent += OnTimeChanged;
            _locationChangedEvent.OnInvokeEvent += OnLocationChanged;
            _activityChangedEvent.OnResultEvent += OnActivityChanged;
        }
        private void OnDisable()
        {
            _weekChangedEvent.OnInvokeEvent -= OnWeekChanged;
            _changeDayEvent.OnResultEvent -= OnDayChanged;
            _timeChangedEvent.OnInvokeEvent -= OnTimeChanged;
            _locationChangedEvent.OnInvokeEvent -= OnLocationChanged;
            _activityChangedEvent.OnResultEvent -= OnActivityChanged;
        }

        private void Start()
        {
            // todo: init overlaydata vars with save data 
            OverlayData.Week = DayManager.Instance.Week;
            OverlayData.Day = GetDayString(DayManager.Instance.Day);
            OverlayData.Time = GetTimeString(DayManager.Instance.Time);
            OverlayData.Location = "Home";
            OverlayData.Subject = "History";
        }

        #region Callbacks
        private void OnWeekChanged(int week)
        {
            OverlayData.Week = week;
        }
        private void OnDayChanged(int day)
        {
            OverlayData.Day = GetDayString(day);
        }
        private void OnTimeChanged(int time)
        {
            OverlayData.Time = GetTimeString(time);
        }
        private void OnLocationChanged(Location location)
        {
            OverlayData.Location = location.ToString();
        }
        private void OnActivityChanged(ActivityName activity)
        {
            // not filtering out classes for now
            OverlayData.Subject = activity.ToString();


            //// if switching from no subject to subject or vice versa
            //if (OverlayData.Subject.IsNullOrWhitespace() ^ subject.IsNullOrWhitespace())
            //{
            //    // get subject label
            //    VisualElement ele = UIDocument.rootVisualElement.Q("ClassNameElement");

            //    // hide subject label if leaving school
            //    if (subject.IsNullOrWhitespace()) { ele.visible = false; }
            //    // show subject label if entering school
            //    else {  ele.visible = true; }
            //}

            //OverlayData.Subject = subject;
        }
        #endregion


        // not sure if these methods are staying here
        private string GetDayString(int day)
        {
            return ((DayString)day).ToString();
        }

        private string GetTimeString(int time)
        {
            int hours = time / 60;
            int minutes = time % 60;
            return string.Format("{0:00}:{1:00}", hours, minutes);
        }
    }

    public class OverlayDataSource : INotifyBindablePropertyChanged
    {
        private int _week;
        private string _day;
        private string _time;
        private string _location;
        private string _subject;

        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

        [CreateProperty]
        public int Week
        {
            get => _week;
            set
            {
                if (_week == value) return;
                _week = value;
                Notify();
            }
        }

        [CreateProperty]
        public string Day
        {
            get => _day;
            set
            {
                if (_day == value) return;
                _day = value;
                Notify();
            }
        }

        [CreateProperty]
        public string Time
        {
            get => _time;
            set
            {
                if (_time == value) return;
                _time = value;
                Notify();
            }
        }

        [CreateProperty]
        public string Location
        {
            get => _location;
            set
            {
                if (_location == value) return;
                _location = value;
                Notify();
            }
        }

        [CreateProperty]
        public string Subject
        {
            get => _subject;
            set
            {
                if (_subject == value) return;
                _subject = value;
                Notify();
            }
        }

        void Notify([CallerMemberName] string property = "")
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property));
        }
    }
}
