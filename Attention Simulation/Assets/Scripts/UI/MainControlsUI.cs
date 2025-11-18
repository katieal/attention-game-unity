using Emyra.FocusGame.EventChannel;
using Emyra.FocusGame.GameData;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using LocationInfo = Emyra.FocusGame.GameData.LocationInfo;

namespace Emyra.FocusGame.UI
{
    public class MainControlsUI : MonoBehaviour
    {
        public UIDocument ControlsDocument;
        public SubjectColorDataSource ColorData;

        [TitleGroup("Events")]
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private LocationInfoEventSO _locationChangedEvent;
        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private VoidEventSO _startGameEvent; // temp (debug only)
        [SerializeField] private ActivityTypeEventSO _activitySelectedEvent;

        private VisualElement _scheduleElement;
        private Dictionary<ActivityType, Button> _buttonDict;

        private Color _highlightColor;
        private Color _defaultColor;

        // DEBUG (TEMP)
        private Button _startButton;

        private void Awake()
        {
            #region Debug
            _startButton = ControlsDocument.rootVisualElement.Q("start-button") as Button;
            #endregion
            ColorData = new SubjectColorDataSource();
            _buttonDict = new Dictionary<ActivityType, Button>();
            _highlightColor = new Color(255, 234, 0, 172);
            _defaultColor = new Color(0, 0, 0, 0);

            _scheduleElement = ControlsDocument.rootVisualElement.Q("class-schedule");
            _scheduleElement.dataSource = ColorData;
        }

        private void OnEnable()
        {
            #region Debug
            _startButton.RegisterCallback<ClickEvent>(StartGame);
            #endregion

            InitButtons();

            _locationChangedEvent.OnInvokeEvent += OnLocationChanged;
        }
        private void OnDisable()
        {
            #region Debug
            _startButton.UnregisterCallback<ClickEvent>(StartGame);
            #endregion

            foreach(ActivityType activity in _buttonDict.Keys)
            {
                _buttonDict[activity].UnregisterCallback<ClickEvent, ActivityType>(OnActivityClicked);
            }

            _locationChangedEvent.OnInvokeEvent -= OnLocationChanged;
        }

        private void InitButtons()
        {
            _buttonDict.Clear();

            // temp var for readability
            ActivityDatabase db = GameData.ActivityDatabase.Instance;

            // get all the buttons for each activity type and store them in a dictionary
            foreach (ActivityType activity in Enum.GetValues(typeof(ActivityType)))
            {
                string name = db.GetButtonName(activity);
                // skip if activity does not have matching button name
                if (name == null) { continue; }

                var ele = ControlsDocument.rootVisualElement.Q(db.GetButtonName(activity));
                if (ele != null)
                {
                    Button button = ele as Button;
                    button.RegisterCallback<ClickEvent, ActivityType>(OnActivityClicked, activity);
                    _buttonDict.Add(activity, button);
                }
            }
        }

        #region Debug
        private void StartGame(ClickEvent evt)
        {
            Debug.Log("starting game!");
            _startGameEvent.InvokeEvent();
            _startButton.style.display = DisplayStyle.None;
            _startButton.SetEnabled(false);
        }
        #endregion


        private void OnLocationChanged(LocationInfo info)
        {
            // update activity buttons shown
            foreach (ActivityType activity in _buttonDict.Keys)
            {
                if ((info.Activities & activity) == activity)
                {
                    _buttonDict[activity].visible = true;
                    _buttonDict[activity].SetEnabled(true);
                }
                else
                {
                    _buttonDict[activity].visible = false;
                    _buttonDict[activity].SetEnabled(false);
                }
            }

            // if in school, highlight corresponding subject in class schedule overlay
            if (info.Room == Room.Classroom) 
            { 
                _scheduleElement.visible = true;
                UpdateScheduleColors(info.Subject); 
            }
            else
            {
                // if not in classroom, hide schedule overlay
                _scheduleElement.visible = false;
            }
        }

        #region Activity Buttons

        private void OnActivityClicked(ClickEvent evt, ActivityType activity)
        {
            Debug.Log(activity.ToString());

            _activitySelectedEvent.InvokeEvent(activity);
        }

        #endregion

        #region Schedule Colors
        private void UpdateScheduleColors(Subject subject)
        {
            ResetColors();

            if (subject == Subject.Math) { ColorData.Subject1 = _highlightColor; }
            else if (subject == Subject.Science) { ColorData.Subject2 = _highlightColor; }
            else if (subject == Subject.LanguageArts) { ColorData.Subject3 = _highlightColor; }
            else if (subject == Subject.History) { ColorData.Subject4 = _highlightColor; }
        }

        private void ResetColors()
        {
            ColorData.Subject1 = _defaultColor;
            ColorData.Subject2 = _defaultColor;
            ColorData.Subject3 = _defaultColor;
            ColorData.Subject4 = _defaultColor;
        }
        #endregion
    }


    public class SubjectColorDataSource : INotifyBindablePropertyChanged
    {
        private Color _subject1;
        private Color _subject2;
        private Color _subject3;
        private Color _subject4;

        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

        [CreateProperty]
        public Color Subject1
        {
            get => _subject1;
            set
            {
                if (_subject1 == value) return;
                _subject1 = value;
                Notify();
            }
        }

        [CreateProperty]
        public Color Subject2
        {
            get => _subject2;
            set
            {
                if (_subject2 == value) return;
                _subject2 = value;
                Notify();
            }
        }

        [CreateProperty]
        public Color Subject3
        {
            get => _subject3;
            set
            {
                if (_subject3 == value) return;
                _subject3 = value;
                Notify();
            }
        }

        [CreateProperty]
        public Color Subject4
        {
            get => _subject4;
            set
            {
                if (_subject4 == value) return;
                _subject4 = value;
                Notify();
            }
        }

        void Notify([CallerMemberName] string property = "")
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property));
        }
    }
}
