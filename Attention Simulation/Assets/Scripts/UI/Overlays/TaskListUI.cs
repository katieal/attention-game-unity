using Emyra.FocusGame.EventChannel;
using Emyra.FocusGame.Locations;
using Emyra.FocusGame.School;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using RoomInfo = Emyra.FocusGame.Locations.RoomInfo;

namespace Emyra.FocusGame.UI
{
    public class TaskListUI : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private UIDocument _taskDocument;


        [TitleGroup("Events")]
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private StringListEventSO _schoolScheduleEvent;
        [SerializeField] private LocationInfoEventSO _locationChangedEvent;

        public SchoolScheduleDataSource ScheduleData;
        //private List<string> _subjectNames;
        // ui elements
        private VisualElement _scheduleElement;

        // current position
        private int _scheduleIndex;

        // final vars
        private int _subjectCount = 7;
        private Color _defaultColor;
        private Color _highlightColor;
        private Color _disabledColor;

        private void Awake()
        {
            ScheduleData = new SchoolScheduleDataSource();
            ScheduleData.SubjectDataList = new SchoolSubjectDataSource[_subjectCount];

            // index starts at -1
            _scheduleIndex = -1;

            _defaultColor = new Color(0, 0, 0, 0);
            _highlightColor = new Color(255, 234, 0, 0.67f);
            _disabledColor = new Color(0, 0, 0, 0.5f);

            _scheduleElement = _taskDocument.rootVisualElement.Q("class-schedule");
            _scheduleElement.dataSource = ScheduleData;
        }

        private void OnEnable()
        {
            // subscribe to events
            _schoolScheduleEvent.OnInvokeEvent += OnSchoolScheduleEvent;
            _locationChangedEvent.OnInvokeEvent += OnLocationChanged;
        }
        private void OnDisable()
        {
            // unsubscribe from events
            _schoolScheduleEvent.OnInvokeEvent -= OnSchoolScheduleEvent;
            _locationChangedEvent.OnInvokeEvent -= OnLocationChanged;
        }

        private void OnSchoolScheduleEvent(List<string> subjectIds)
        {
            // set subject name labels in UI
            for (int i = 0; i < subjectIds.Count; i++)
            {
                string subjectName = GameData.SchoolSubjectDatabase.Instance.GetSubjectName(subjectIds[i]);
                ScheduleData.SubjectDataList[i] = new SchoolSubjectDataSource();
                ScheduleData.SubjectDataList[i].Color = _defaultColor;
                ScheduleData.SubjectDataList[i].Name = subjectName;
            }
            _scheduleIndex = -1;
        }

        private void OnLocationChanged(RoomInfo info)
        {
            // if in school and location is the next one in school schedule
            if ((info.Place == Place.School) && (info.SubjectName == ScheduleData.SubjectDataList[_scheduleIndex + 1].Name))
            {
                // ensure school schedule is visible
                _scheduleElement.visible = true;

                // grey out previous label if necessary
                if (_scheduleIndex >= 0 && _scheduleIndex < ScheduleData.SubjectDataList.Length)
                {
                    // subjects that have been completed are set to disabled color
                    ScheduleData.SubjectDataList[_scheduleIndex].Color = _disabledColor;
                }

                // highlight current label
                _scheduleIndex++;
                ScheduleData.SubjectDataList[_scheduleIndex].Color = _highlightColor;
            }
            else
            {
                // if not in classroom, hide schedule overlay and reset label colors
                _scheduleElement.visible = false;
                ResetColors();
            }
        }

        [Button]
        private void ResetColors()
        {
            for (int i = 0; i < ScheduleData.SubjectDataList.Length; i++)
            {
                ScheduleData.SubjectDataList[i].Color = _defaultColor;
            }
        }

        [Button]
        public void SetColor(float r, float g, float b, float a)
        {
            ScheduleData.SubjectDataList[0].Color = new Color (r, g, b, a);
        }

    }

    public class SchoolScheduleDataSource 
    {
        public SchoolSubjectDataSource[] SubjectDataList; 
    }

    public class SchoolSubjectDataSource : INotifyBindablePropertyChanged
    {
        private Color _color;
        private string _name;

        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

        [CreateProperty]
        public Color Color
        {
            get => _color; 
            set
            {
                if (_color == value) return;
                _color = value;
                Notify();
            }
        }

        [CreateProperty]
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                Notify();
            }
        }

        void Notify([CallerMemberName] string property = "")
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property));
        }
    }
}
