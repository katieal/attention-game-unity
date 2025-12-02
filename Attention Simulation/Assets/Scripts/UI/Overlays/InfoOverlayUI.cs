using Emyra.FocusGame.EventChannel;
using Emyra.FocusGame.Locations;
using Emyra.FocusGame.GameData;
using Emyra.FocusGame.Managers;
using Emyra.FocusGame.School;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using RoomInfo = Emyra.FocusGame.Locations.RoomInfo;

namespace Emyra.FocusGame.UI
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
        [SerializeField] private LocationInfoEventSO _locationChangedEvent;

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
        }

        private void OnDisable()
        {
            _weekChangedEvent.OnInvokeEvent -= OnWeekChanged;
            _changeDayEvent.OnResultEvent -= OnDayChanged;
            _timeChangedEvent.OnInvokeEvent -= OnTimeChanged;
            _locationChangedEvent.OnInvokeEvent -= OnLocationChanged;
        }

        private void Start()
        {
            // todo: init overlaydata vars with save data 
            OverlayData.Week = DayManager.Instance.Week;
            OverlayData.Day = GetDayString(DayManager.Instance.Day);
            OverlayData.Time = GetTimeString(DayManager.Instance.Time);
            OverlayData.Place = "Home";
            OverlayData.Room = "History";
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
        private void OnLocationChanged(RoomInfo info)
        {
            OverlayData.Place = info.Place.ToString();

            // if player is in a classroom, room label displays current subject instead of room name
            if (info.Room == RoomName.Classroom)
            {
                OverlayData.Room = info.SubjectName;
            }
            else
            {
                OverlayData.Room = info.Room.ToString();
            }
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

        // convert subject enum into display string
        private string GetSubjectString(SubjectType subject)
        {
            string name = subject.ToString();

            // excluding the first character, insert a space before each capital letter
            for (int i = 1; i < name.Length; i++)
            {
                if (Char.IsUpper(name[i]))
                {
                    name = name.Insert(i, " ");
                    i++;
                }
            }

            return name;
        }
    }

    public class OverlayDataSource : INotifyBindablePropertyChanged
    {
        private int _week;
        private string _day;
        private string _time;
        private string _place;
        private string _room;

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
        public string Place
        {
            get => _place;
            set
            {
                if (_place == value) return;
                _place = value;
                Notify();
            }
        }

        [CreateProperty]
        public string Room
        {
            get => _room;
            set
            {
                if (_room == value) return;
                _room = value;
                Notify();
            }
        }

        void Notify([CallerMemberName] string property = "")
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property));
        }
    }
}
