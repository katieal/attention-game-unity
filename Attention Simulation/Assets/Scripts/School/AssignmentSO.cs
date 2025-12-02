using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities;
using System;
using UnityEditor;
using UnityEngine;

namespace Emyra.FocusGame.School
{
    [CreateAssetMenu(fileName = "AssignmentSO", menuName = "Scriptable Objects/School/AssignmentSO")]
    public class AssignmentSO : SerializedScriptableObject
    {
        [Title("General Info")]
        public string Name;
        public string Description;
        [EnumToggleButtons] public AssignmentType Type;

        [Title("Knowledge")]
        [Tooltip("Knowledge range required to complete assignment. " +
            "Less than min means higher completion time." +
            "Greater than max means lower completion time.")]
        [MinMaxSlider(0, 100, true)]
        public Vector2Int KnowledgeRequired;

        [Tooltip("Knowledge % points gained on assignment completion.")]
        [PropertyRange(0, 100), SuffixLabel("%", Overlay = true)]
        public int KnowledgeGained;

        [Title("Timing")]
        [Tooltip("Number of days player has to complete assignment (excluding weekends)")]
        [MinValue(1), SuffixLabel("days", Overlay = true)]
        public int DaysTotal;

        [Tooltip("Number of minutes player must spend to fully complete assignment.")]
        [SuffixLabel("minutes", Overlay = true)]
        public int TimeTotal;

        [Tooltip("Day this assignment will be assigned to player (x = Day, Y = Week)")]
        [ShowInInspector] [PropertyOrder(4)]
        public Vector2Int DateAssigned { get { return DayConverter(); } }

        [FoldoutGroup("Calendar"), PropertyOrder(5)]
        [TableMatrix(DrawElementMethod = nameof(DrawCell), HorizontalTitle = "Day",
            ResizableColumns = false, RowHeight = 30, VerticalTitle = "Week")]
        public bool[,] DaySelector = new bool[5, 4];


        private Vector2Int DayConverter()
        {
            for (int day = 0; day < 5; day++)
            {
                for (int week = 0; week < 4; week++)
                {
                    if (DaySelector[day, week])
                    {
                        return new Vector2Int(day, week);
                    }
                }
            }
            return new Vector2Int(0, 0);
        }
#if UNITY_EDITOR
        private static bool DrawCell(Rect rect, bool value)
        {
            if (Event.current.type == EventType.MouseDown &&
                rect.Contains(Event.current.mousePosition))
            {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }

            EditorGUI.DrawRect(rect.Padding(1),
                value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));

            return value;
        }
#endif
    }

    public enum Status { Unassigned, InProgress, Complete, Late }
    public enum AssignmentType { Homework, Project }

    [Serializable]
    public class AssignmentInstance // serializable for debug only
    {
        [field: SerializeField][field: ReadOnly] public string Name { get; private set; }
        public string Description { get; private set; }
        public AssignmentType Type { get; private set; }
        public Status Status;

        /// <summary>
        /// Knowledge % required to complete assignment. 
        /// Less than min means higher completion time, greater than max means lower completion time.
        /// </summary>
        public Vector2Int KnowledgeRequired { get; private set; }

        /// <summary>
        /// Knowledge points gained upon assignment completion
        /// </summary>
        public int KnowledgeGained { get; private set; }

        /// <summary>
        /// Date the assignment will be given. X = Day, Y = Week
        /// </summary>
        public Vector2Int DateAssigned { get; private set; }

        /// <summary>
        /// Number of days player has to complete assignment (excluding weekends)
        /// </summary>
        public int DaysTotal { get; private set; }

        /// <summary>
        /// Days left until deadline
        /// </summary>
        public int DaysLeft;

        /// <summary>
        /// Minutes required to complete assignment
        /// </summary>
        public int TimeTotal { get; private set; }

        /// <summary>
        /// Minutes spent on the assignment so far
        /// </summary>
        public int TimeSpent;

        /// <summary>
        /// Total points that can be earned from this assignment.
        /// </summary>
        [field: SerializeField][field: ReadOnly] public int PointsTotal { get; private set; }

        /// <summary>
        /// Points received for this assignment (aka Grade).
        /// </summary>
        public int PointsEarned;


        public AssignmentInstance(AssignmentSO data, int pointsTotal)
        {
            this.Name = data.Name;
            this.Description = data.Description;
            this.Type = data.Type;
            this.Status = Status.Unassigned;
            this.KnowledgeRequired = data.KnowledgeRequired;
            this.KnowledgeGained = data.KnowledgeGained;
            this.DateAssigned = data.DateAssigned;
            this.DaysTotal = data.DaysTotal;
            this.DaysLeft = data.DaysTotal;
            this.TimeTotal = data.TimeTotal;
            this.TimeSpent = 0;
            this.PointsTotal = pointsTotal;
            this.PointsEarned = 0;
        }
    }
}
