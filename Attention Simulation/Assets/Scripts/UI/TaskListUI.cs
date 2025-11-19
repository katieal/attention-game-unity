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
    public class TaskListUI : MonoBehaviour
    {
        [Title("References")]
        public UIDocument TaskDocument;
        public SubjectColorDataSource ColorData;

        [TitleGroup("Events")]
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private LocationInfoEventSO _locationChangedEvent;

        // ui elements
        private VisualElement _scheduleElement;
        // final vars
        private Color _highlightColor;
        private Color _defaultColor;

        private void Awake()
        {
            ColorData = new SubjectColorDataSource();

            _highlightColor = new Color(255, 234, 0, 172);
            _defaultColor = new Color(0, 0, 0, 0);

            _scheduleElement = TaskDocument.rootVisualElement.Q("class-schedule");
            _scheduleElement.dataSource = ColorData;
        }

        private void OnEnable()
        {
            // subscribe to events
            _locationChangedEvent.OnInvokeEvent += OnLocationChanged;
        }
        private void OnDisable()
        {
            // unsubscribe from events
            _locationChangedEvent.OnInvokeEvent -= OnLocationChanged;
        }

        private void OnLocationChanged(LocationInfo info)
        {
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

        #region Class Schedule
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
