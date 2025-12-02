using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emyra.FocusGame.School
{
    [CreateAssetMenu(fileName = "SchoolSubjectSO", menuName = "Scriptable Objects/School/SubjectSO")]
    public class SchoolSubjectSO : SerializedScriptableObject
    {
        [TitleGroup("Subject Info")]
        public string Id;
        [TitleGroup("Subject Info")]
        public SubjectType SubjectType;
        [TitleGroup("Subject Info")]
        [Tooltip("Display name - the specific subject within a given type. Ex: Math/Geometry")]
        public string SubjectName;
        [TitleGroup("Subject Info")]
        public string Description;
        [TitleGroup("Subject Info")]
        [Tooltip("Subject Difficulty affects the speed/frequency at which knowlege points are earned.")]
        [HideIf("SubjectType", Value = SubjectType.Break)]
        public DifficultyLevel Difficulty;

        [TabGroup("Grades", VisibleIf = "@this.SubjectType != Emyra.FocusGame.School.SubjectType.Break")]
        [Tooltip("Total number of points that can be earned in this class. Used to calculate grade.")]
        public int TotalPoints = 1000;
        [TabGroup("Grades")]
        [Tooltip("Weight and number of entries in each grade category")]
        [ValidateInput(nameof(HasDistributedWeights), "Weights total must equal total points")]
        public List<PointDistribution> Weights = new List<PointDistribution>()
        {
            new PointDistribution() { Category = Category.Homework, Count = 0, PointValue = 0 },
            new PointDistribution() { Category = Category.Project, Count = 0, PointValue = 0 },
            new PointDistribution() { Category = Category.Exam, Count = 3, PointValue = 100 },
            new PointDistribution() { Category = Category.Final, Count = 1, PointValue = 500 }
        };
        [TabGroup("Grades")]
        [ShowInInspector][ReadOnly] public int CurrentTotalDistributed { get { return GetTotalDistributedPoints(); } }

        [TabGroup("Assignments")]
        [Tooltip("Assignment schedule")]
        [RequiredListLength(nameof(AssignmentLength))]
        public List<AssignmentSO> Assignments = new List<AssignmentSO>();

        [TabGroup("Exams")]
        [Tooltip("Exam SOs for the 3 regular exams")]
        [RequiredListLength(fixedLength: 3)] public ExamSO[] RegularExams;
        [TabGroup("Exams")]
        [Tooltip("Exam SO for the final exam")]
        [Required] public ExamSO FinalExam;

        #region Editor
        private bool HasDistributedWeights(List<PointDistribution> weights)
        {
            int total = 0;
            foreach (PointDistribution weight in weights)
            {
                total += weight.TotalPoints;
            }
            return total == this.TotalPoints;
        }
        private int GetTotalDistributedPoints()
        {
            int total = 0;
            foreach (PointDistribution weight in Weights)
            {
                total += weight.TotalPoints;
            }
            return total;
        }
        public int AssignmentLength { get { return Weights[0].Count + Weights[1].Count; } }
        #endregion

        public int GetPointDistribution(string categoryName)
        {
            foreach (PointDistribution weight in Weights)
            {
                if (weight.Category.ToString().Equals(categoryName)) { return weight.PointValue; }
            }
            return 0;
        }
    }

    public enum SubjectType
    {
        None = -1, Math, Science, LanguageArts, History, SocialStudies,
        ForeignLanguage, Break
    }

    public enum Category { Homework, Project, Exam, Final }

    [Serializable]
    public struct PointDistribution
    {
        [HideLabel] [HorizontalGroup(Width = 0.2f, Gap = 8)] public Category Category;
        [HorizontalGroup(Width = 0.15f, LabelWidth = 40, Gap = 8)] public int Count;
        [HorizontalGroup(Width = 0.3f, LabelWidth = 70, Gap = 8)] public int PointValue;

        [ShowInInspector][HorizontalGroup(Width = 0.3f, LabelWidth = 70, Gap = 8)] public int TotalPoints { get { return Count * PointValue; } }
    }

    [Serializable] // serializable for debug
    public class SubjectInstance
    {
        // TODO in future note: add "modifier" SOs to increase/decrease difficulty

        /// <summary>
        /// ID string used to look up subjects in the database
        /// </summary>
        public string Id { get; private set; }
        /// <summary>
        /// The general subject category (Math, Science, etc.)
        /// </summary>
        public SubjectType SubjectType { get; private set; }
        /// <summary>
        /// Display name - the specific course within a subject category. Ex: Math/Geometry)
        /// </summary>
        [field: SerializeField][field: ReadOnly] public string SubjectName { get; private set; }
        public string Description { get; private set; }
        public DifficultyLevel Difficulty { get; private set; }

        /// <summary>
        /// Player's current accumulated knowledge percentage out of 100%.
        /// </summary>
        [field: SerializeField] private int _currentKnowledge;
        /// <summary>
        /// Player's current accumulated knowledge percentage out of 100%.
        /// </summary>
        public int CurrentKnowledge
        {
            get {  return _currentKnowledge; }
            set { _currentKnowledge = Mathf.Clamp(value, 0, 100); }
        }

        /// <summary>
        /// The total number of points for the entire subject.
        /// </summary>
        [field: SerializeField][field: ReadOnly] public int TotalPoints { get; private set; }
        /// <summary>
        /// The current maximum number of points player could have earned.
        /// </summary>
        public int CurrentTotalPoints;
        /// <summary>
        /// The current number of points player has earned.
        /// </summary>
        public int PointsEarned;

        public List<AssignmentInstance> Assignments;
        public List<ExamInstance> RegularExams;
        public ExamInstance FinalExam;

        // constructor
        public SubjectInstance(SchoolSubjectSO data)
        {
            this.Id = data.Id;
            this.SubjectType = data.SubjectType;
            this.SubjectName = data.SubjectName;
            this.Description = data.Description;
            this.Difficulty = data.Difficulty;
            this.CurrentKnowledge = 0;
            this.TotalPoints = data.TotalPoints;
            this.CurrentTotalPoints = 0;
            this.PointsEarned = 0;

            // add assignments with corresponding point values
            this.Assignments = new List<AssignmentInstance>();
            foreach (AssignmentSO assignment in data.Assignments)
            {
                this.Assignments.Add(new AssignmentInstance(assignment, 
                    data.GetPointDistribution(assignment.Type.ToString())));
            }

            // add exams with corresponding point values
            this.RegularExams = new List<ExamInstance>();
            foreach (ExamSO exam in data.RegularExams)
            {
                this.RegularExams.Add(new ExamInstance(exam, data.GetPointDistribution(Category.Exam.ToString())));
            }

            // add final exam
            this.FinalExam = new ExamInstance(data.FinalExam, data.GetPointDistribution(Category.Final.ToString()));
        }
    }
}
