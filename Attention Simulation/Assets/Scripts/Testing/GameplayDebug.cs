using Emyra.FocusGame.School;
using Emyra.FocusGame.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow;

namespace Emyra.FocusGame.Testing
{
    public class GameplayDebug : MonoBehaviour
    {
        public GradeRangeSO GradeRange;
        public SubjectMenuUI SubjectMenu;

        [Tooltip("current starting day (x = Day, Y = Week)")]
        public Vector2Int CurrentDay = new Vector2Int();

        [AssetSelector(DrawDropdownForListElements = false, IsUniqueList = false, Paths = "Assets/Scriptable Objects/School/Examples")]
        public List<SchoolSubjectSO> Examples;
        public List<SubjectInstance> Subjects = new List<SubjectInstance>();

        [Button]
        public void AddSubjectInstances()
        {
            foreach (SchoolSubjectSO subject in Examples)
            {
                Subjects.Add(new SubjectInstance(subject));
            }
        }

        [Button]
        public void RandomizeInstanceData()
        {
            foreach (SubjectInstance subject in Subjects)
            {
                foreach (AssignmentInstance assignment in subject.Assignments)
                {
                    CompareDates(CurrentDay, assignment.DateAssigned, assignment.DaysTotal, out int status, out int daysLeft);
                    // if assignment already passed
                    if (status == -1)
                    {
                        assignment.Status = Status.Complete;
                        assignment.DaysLeft = 0;
                        assignment.TimeSpent = 0;
                        assignment.PointsEarned = Random.Range(1, assignment.PointsTotal + 1);

                        subject.CurrentTotalPoints += assignment.PointsTotal;
                        subject.PointsEarned += assignment.PointsEarned;
                        subject.CurrentKnowledge += assignment.KnowledgeGained;
                    }
                    else if (status == 0)
                    {
                        assignment.Status = Status.InProgress;
                        assignment.DaysLeft = daysLeft;
                        assignment.TimeSpent = Random.Range(0, assignment.TimeTotal);
                    }
                    else { assignment.Status = Status.Unassigned; }
                }

                for (int i = 0; i < subject.RegularExams.Count; i++)
                {
                    CompareDates(CurrentDay, new Vector2Int(4, i), 0, out int status, out int daysLeft);

                    if (status == -1)
                    {
                        subject.RegularExams[i].Status = Status.Complete;
                        subject.RegularExams[i].PointsEarned = Random.Range(1, subject.RegularExams[i].PointsTotal + 1);

                        subject.CurrentTotalPoints += subject.RegularExams[i].PointsTotal;
                        subject.PointsEarned += subject.RegularExams[i].PointsEarned;
                    }
                    else if (status == 0)
                    {
                        subject.RegularExams[i].Status = Status.InProgress;
                    }
                }
            }
        }

        // returns -1 if date passed, 0 if matches, and 1 if in future
        private void CompareDates(Vector2Int current, Vector2Int date, int duration, out int status, out int daysLeft)
        {
            int currentDay = (current.y * 5) + (current.x + 1);
            int assignedDay = (date.y * 5) + (date.x + 1);

            if (currentDay == assignedDay) 
            {
                status = 0;
                daysLeft = duration;
            }
            // if assigned day has already passed
            else if (assignedDay < currentDay)
            {
                // if assignment is ongoing
                if (assignedDay + duration >= currentDay) 
                { 
                    status = 0; 
                    daysLeft = (assignedDay + duration) - currentDay;
                }
                else
                {
                    status = -1;
                    daysLeft = 0;
                }
            }
            // if assignment is in the future
            else
            {
                status = 1;
                daysLeft = duration;
            }
        }


        [Button]
        public void SendSubjectViewData()
        {
            List<SubjectViewData> dataList = new List<SubjectViewData>();

            foreach (SubjectInstance instance in Subjects)
            {
                dataList.Add(GetSubjectViewData(instance));
            }

            SubjectMenu.SetSubjectData(dataList);
        }

        private SubjectViewData GetSubjectViewData(SubjectInstance subject)
        {
            float grade = (float)subject.PointsEarned / (float)subject.CurrentTotalPoints;
            string gradeLetter = GradeRange.GetGradeString(grade * 100);

            SubjectViewData data = new SubjectViewData()
            {
                SubjectName = subject.SubjectName,
                Knowledge = $"{subject.CurrentKnowledge}%",
                Points = $"{subject.PointsEarned}/{subject.CurrentTotalPoints}",
                Grade = $"{grade:P2} {gradeLetter}",
                Assignments = GetCurrentAssignmentsData(subject)
            };


            return data;
        }

        // only get completed assignments 
        private List<AssignmentViewData> GetCurrentAssignmentsData(SubjectInstance subject)
        {
            List<AssignmentViewData> data = new List<AssignmentViewData>();

            foreach (AssignmentInstance assignment in subject.Assignments)
            {
                if (assignment.Status == Status.Complete)
                {
                    float grade = (float)assignment.PointsEarned / (float)assignment.PointsTotal;
                    string gradeLetter = GradeRange.GetGradeString(grade * 100);

                    data.Add(new AssignmentViewData()
                    {
                        AssignmentName = assignment.Name,
                        //DueDate = GetDayString(assignment.DaysLeft),
                        //Progress = $"{progress:P2}",
                        //PointValue = assignment.PointsTotal
                        PointsScore = $"{assignment.PointsEarned}/{assignment.PointsTotal}",
                        Grade = $"{grade:P2} {gradeLetter}"
                    });
                }
            }

            foreach (ExamInstance exam in subject.RegularExams)
            {
                if (exam.Status == Status.Complete)
                {
                    float grade = (float)exam.PointsEarned / (float)exam.PointsTotal;
                    string gradeLetter = GradeRange.GetGradeString(grade * 100);

                    data.Add(new AssignmentViewData()
                    {
                        AssignmentName = exam.Name,
                        PointsScore = $"{exam.PointsEarned}/{exam.PointsTotal}",
                        Grade = $"{grade:P2}-{gradeLetter}"
                    });
                }
            }

            return data;
        }

        private string GetDayString(int daysLeft)
        {
            if (daysLeft == 0) { return "Today"; }
            if (daysLeft == 1) { return "Tomorrow"; }

            if (daysLeft < 5)
            {
                int day = (CurrentDay.x + daysLeft) % 5;

                return (((DayOfTheWeek)day).ToString());
            }
            else
            {
                int day = (CurrentDay.x + daysLeft) % 5;

                return "Next " + (((DayOfTheWeek)day).ToString());
            }
        }
    }

    public enum DayOfTheWeek { Monday, Tuesday, Wednesday, Thursday, Friday }
}
