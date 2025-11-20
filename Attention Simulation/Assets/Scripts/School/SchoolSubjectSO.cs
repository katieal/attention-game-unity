using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emyra.FocusGame.School
{
    [CreateAssetMenu(fileName = "SchoolSubjectSO", menuName = "Scriptable Objects/School/SubjectSO")]
    public class SchoolSubjectSO : SerializedScriptableObject
    {
        // TODO: add difficulty?

        [TabGroup("Subject Info")]
        public SubjectType Subject;
        [TabGroup("Subject Info")]
        [Tooltip("The specific subject within a given type. Ex: Math/Geometry")]
        public string SubjectName;
        [TabGroup("Subject Info")]
        public string Description;

        [TabGroup("Grades")]
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

        [TabGroup("Assignments")]
        [Tooltip("Assignment schedule")]
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
        #endregion
    }

    public enum SubjectType
    {
        None = -1, Math, Science, LanguageArts, History, SocialStudies,
        ForeignLanguage
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
}
