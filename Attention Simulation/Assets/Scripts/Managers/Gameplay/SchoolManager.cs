using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Emyra.FocusGame.Managers
{
    public class SchoolManager : MonoBehaviour
    {

    }
}

namespace Emyra.FocusGame.School
{

    public class SubjectInstance
    {
        // add class difficulty?

        public int CurrentKnowledge; // % out of 100%
        // scores on tests will be knowledge % += some lvl of variance - maybe add a confidence factor?

        // current grade
        public int CurrentPoints;
        public int TotalPoints; // final total is out of 1000?

        public List<AssignmentInstance> Assignments;
        public List<ExamInstance> Exams;
    }


    public class AssignmentInstance // can create "modifiers" to increase/decrease difficulty
    {
        public string Name;
        public string Description;
        public Status Status;
        public AssignmentType Type;

        /// <summary>
        /// Knowledge % required to complete assignment. 
        /// Less than min means higher completion time, greater than max means lower completion time.
        /// </summary>
        public Vector2Int KnowledgeRequired;

        /// <summary>
        /// Date the assignment will be given. X = Day, Y = Week
        /// </summary>
        public Vector2Int DateAssigned;
        /// <summary>
        /// Number of days player has to complete assignment (excluding weekends)
        /// </summary>
        public int Duration;
        /// <summary>
        /// Days left until deadline
        /// </summary>
        public int DaysLeft;

        /// <summary>
        /// Minutes required to complete assignment
        /// </summary>
        public int TimeRequired;
        /// <summary>
        /// Remaining time needed to complete assignment
        /// </summary>
        public int TimeLeft;

        /// <summary>
        /// Knowledge points gained upon assignment completion
        /// </summary>
        public int KnowledgeGained;

        /// <summary>
        /// Grade received for this assignment
        /// </summary>
        public int Grade;
    }


    public class ExamInstance
    {
        public string Name;
        public ExamType Type;

        [ShowIf("Type", Value = ExamType.Regular)]
        public int Order; // out of 3?
        public Vector2Int KnowledgeRequired;
        public Status Status;

        public int PointsTotal;
        public int PointsEarned;
    }
}