using Emyra.FocusGame.EventChannel;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Emyra.FocusGame.UI
{
    public class SubjectMenuUI : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private UIDocument _subjectDocument;

        [TitleGroup("Events")]
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private VoidEventSO _subjectMenuInputEvent;

        // ui components
        private ToggleButtonGroup _buttonGroup;

        public SubjectViewDataSource SubjectDataSource;
        private List<string> _subjectNames;
        private List<SubjectViewData> _subjectData;
        private bool _isMenuVisible;

        private void Awake()
        {
            _subjectNames = new List<string>()
            {
                "Art Fundamentals", "Geometry", "Chemistry", "US History", "World History", "AP Literature", "Spanish II", "Band", "Psychology"
            };


            // assign data sources
            _subjectData = new List<SubjectViewData>(7);
            SubjectDataSource = new SubjectViewDataSource();
            _subjectDocument.rootVisualElement.Q("subject-content-element").dataSource = SubjectDataSource;
            (_subjectDocument.rootVisualElement.Q("assignments-list") as MultiColumnListView).itemsSource = SubjectDataSource.Assignments;

            // register toggle button group callback
            _buttonGroup = _subjectDocument.rootVisualElement.Q("subject-toggle-group") as ToggleButtonGroup;
            _buttonGroup.RegisterValueChangedCallback(OnSelectedSubjectChanged);
        }

        private void OnEnable()
        {
            // menu should be hidden by default
            HideMenu();
            _isMenuVisible = false;

            _subjectMenuInputEvent.OnInvokeEvent += ToggleMenu;
        }
        private void OnDisable()
        {
            _subjectMenuInputEvent.OnInvokeEvent -= ToggleMenu;
            _buttonGroup.UnregisterValueChangedCallback(OnSelectedSubjectChanged);
        }

        private void Start()
        {
            
        }
        public void SetSubjectData(List<SubjectViewData> dataList)
        {
            _subjectData = dataList;
            UpdateButtonNames();
        }

        #region Callbacks
        private void ToggleMenu()
        {
            if (_isMenuVisible)
            {
                HideMenu();
                _isMenuVisible = false;
            }
            else
            {
                ShowMenu();
                _isMenuVisible = true;
            }
        }

        [Button]
        public void ShowMenu()
        {
            _subjectDocument.rootVisualElement.SetEnabled(true);
            _subjectDocument.rootVisualElement.visible = true;
            _buttonGroup.RegisterValueChangedCallback(OnSelectedSubjectChanged);
        }
        [Button]
        public void HideMenu()
        {
            _subjectDocument.rootVisualElement.visible = false;
            _buttonGroup.UnregisterValueChangedCallback(OnSelectedSubjectChanged);
            _subjectDocument.rootVisualElement.SetEnabled(false);
        }

        private void OnSelectedSubjectChanged(ChangeEvent<ToggleButtonGroupState> evt)
        {
            // get new selected value
            var options = evt.newValue.GetActiveOptions(stackalloc int[evt.newValue.length]);
            foreach (int option in options)
            {
                // there should only be one int in options
                SetSubjectView(option);
            }
        }
        #endregion

        private void UpdateButtonNames()
        {
            List<string> _newNames = new List<string>();
            for (int i = 0; i < _subjectData.Count; i++)
            {
                _newNames.Add(_subjectData[i].SubjectName);
            }

            if (!_subjectNames.Equals(_newNames))
            {
                _subjectNames = _newNames;

                for (int i = 0; i < _buttonGroup.childCount; i++)
                {
                    if (i < _subjectNames.Count)
                    {
                        (_buttonGroup[i] as Button).text = _subjectNames[i];
                    }
                }
            }
        }

        private void SetSubjectView(int index)
        {
#if UNITY_EDITOR
                // ensure index is in range
                Debug.Assert(index < _subjectData.Count, "subject view data index out of range");
#endif
            // temp var for readability
            SubjectViewData subjectData = _subjectData[index];

            // set subject stats
            SubjectDataSource.SubjectStats = new SubjectStatsViewDataSource()
            {
                SubjectName = subjectData.SubjectName,
                Knowledge = subjectData.Knowledge,
                Points = subjectData.Points,
                Grade = subjectData.Grade
            };

            // set assignment data
            SubjectDataSource.Assignments.Clear();
            foreach (AssignmentViewData data in subjectData.Assignments)
            {
                SubjectDataSource.Assignments.Add(new AssignmentViewDataSource()
                {
                    AssignmentName = data.AssignmentName,
                    PointsScore = data.PointsScore,
                    Grade = data.Grade
                });
            }
        }
    }

    public struct SubjectViewData
    {
        public string SubjectName;
        public string Knowledge;
        public string Points;
        public string Grade;
        public List<AssignmentViewData> Assignments;
    }

    public struct AssignmentViewData
    {
        public string AssignmentName;
        public string DueDate;
        public string Progress;
        public int PointValue;
        public string PointsScore;
        public string Grade;
    }

    public class SubjectViewDataSource
    {
        public SubjectStatsViewDataSource SubjectStats;
        public List<AssignmentViewDataSource> Assignments;

        public SubjectViewDataSource()
        {
            SubjectStats = new SubjectStatsViewDataSource();
            Assignments = new List<AssignmentViewDataSource>();
        }
    }

    public class SubjectStatsViewDataSource : INotifyBindablePropertyChanged
    {
        private string _subjectName;
        private string _knowledge;
        private string _points;
        private string _grade;

        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

        [CreateProperty]
        public string SubjectName
        {
            get => _subjectName;
            set
            {
                if (_subjectName == value) return;
                _subjectName = value;
                Notify();
            }
        }

        [CreateProperty]
        public string Knowledge
        {
            get => _knowledge;
            set
            {
                if (_knowledge == value) return;
                _knowledge = value;
                Notify();
            }
        }

        [CreateProperty]
        public string Points
        {
            get => _points;
            set
            {
                if (_points == value) return;
                _points = value;
                Notify();
            }
        }

        [CreateProperty]
        public string Grade
        {
            get => _grade;
            set
            {
                if (_grade == value) return;
                _grade = value;
                Notify();
            }
        }

        void Notify([CallerMemberName] string property = "")
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property));
        }
    }

    public class AssignmentViewDataSource : INotifyBindablePropertyChanged
    {
        private string _assignmentName;
        private string _dueDate;
        private string _progressPercent;
        private int _pointValue;
        private string _pointsScore;
        private string _grade;

        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

        [CreateProperty]
        public string AssignmentName
        {
            get => _assignmentName;
            set
            {
                if (_assignmentName == value) return;
                _assignmentName = value;
                Notify();
            }
        }

        [CreateProperty]
        public string DueDate
        {
            get => _dueDate;
            set
            {
                if (_dueDate == value) return;
                _dueDate = value;
                Notify();
            }
        }

        [CreateProperty]
        public string ProgressPercent
        {
            get => _progressPercent;
            set
            {
                if (_progressPercent == value) return;
                _progressPercent = value;
                Notify();
            }
        }

        [CreateProperty]
        public int PointValue
        {
            get => _pointValue;
            set
            {
                if (_pointValue == value) return;
                _pointValue = value;
                Notify();
            }
        }

        [CreateProperty]
        public string PointsScore
        {
            get => _pointsScore;
            set
            {
                if (_pointsScore == value) return;
                _pointsScore = value;
                Notify();
            }
        }

        [CreateProperty]
        public string Grade
        {
            get => _grade;
            set
            {
                if (_grade == value) return;
                _grade = value;
                Notify();
            }
        }

        void Notify([CallerMemberName] string property = "")
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property));
        }
    }
}
