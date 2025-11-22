using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emyra.FocusGame.School
{
    [CreateAssetMenu(fileName = "ExamSO", menuName = "Scriptable Objects/School/ExamSO")]
    public class ExamSO : SerializedScriptableObject
    {
        [Title("Exam Info")]
        [Tooltip("Display name for this exam.")]
        public string Name;
        public string Description;
        public ExamType Type;
        [ShowIf("Type", Value = ExamType.Regular)]
        [Range(1, 3)]
        public int Order;

        [Title("Knowledge")]
        [Tooltip("Knowledge range required to pass exam. " +
                "Less than min has high chance of failure." +
                "Greater than max reduces variance.")]
        [MinMaxSlider(0, 100, true)]
        public Vector2Int KnowledgeRequired;
        [Tooltip("Determines size of grade variance")]
        public DifficultyLevel Difficulty;
    }

    public enum ExamType { Regular, Final }

    public class ExamInstance
    {
        /// <summary>
        /// Display name
        /// </summary>
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ExamType Type { get; private set; }

        /// <summary>
        /// Regular exams only: the week # the exam is given in (1-3)
        /// </summary>
        public int Order { get; private set; }

        public Status Status;

        /// <summary>
        /// Knowledge % required to pass exam.
        /// Less than min has high chance of failure, greater than max reduces grade variance.
        /// </summary>
        public Vector2Int KnowledgeRequired { get; private set; }

        /// <summary>
        /// Determines knowledge % ranges for each grade.
        /// </summary>
        public DifficultyLevel Difficulty { get; private set; }

        /// <summary>
        /// Total points that can be earned in this exam.
        /// </summary>
        public int PointsTotal { get; private set; }

        /// <summary>
        /// Points earned in this exam (aka grade received).
        /// </summary>
        public int PointsEarned;


        public ExamInstance(ExamSO data, int pointsTotal)
        {
            this.Name = data.Name;
            this.Description = data.Description;
            this.Type = data.Type;
            this.Order = (data.Type == ExamType.Regular) ? data.Order : -1;
            this.KnowledgeRequired = data.KnowledgeRequired;
            this.Difficulty = data.Difficulty;

            this.Status = Status.Unassigned;
            this.PointsTotal = pointsTotal;
        }
    }
}
