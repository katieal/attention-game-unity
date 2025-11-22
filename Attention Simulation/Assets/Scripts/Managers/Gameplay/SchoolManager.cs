using Emyra.FocusGame.School;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Emyra.FocusGame.Managers
{
    public class SchoolManager : MonoBehaviour
    {
        public List<SchoolSubjectSO> SchoolSchedule;

        public List<SubjectInstance> Subjects;

        // temp const variable - update it to change with game difficulty?
        private int _passingGrade = 60;


        private void OnEnable()
        {
            Subjects = new List<SubjectInstance>();
            foreach (SchoolSubjectSO data in SchoolSchedule)
            {
                Subjects.Add(new SubjectInstance(data));
            }
        }

        private float CalculateScore(Vector2Int knowledgeRange, DifficultyLevel difficulty, int currentKnowledge)
        {
            // note: might add an alternate calculation for scores below passing in the future

            // calculate the size of the range of passing scores
            int range = knowledgeRange.y - knowledgeRange.x;
            int variance = GetVariance(difficulty);
            // calculate what percent of the range the user achieved
            float rangePercent = (currentKnowledge - knowledgeRange.x) / range;
            // if percent > 1 (aka currentKnowledge > max), reduce variance
            if (rangePercent > 1)
            {
                // reduce variance by amt of knowledge above max
                // note: might change this calculation in the future!!!
                variance = Mathf.Clamp((variance - (currentKnowledge - knowledgeRange.y)), 0, GetVariance(difficulty));
            }

            // calculate base grade
            float grade = (((100 - _passingGrade) * rangePercent) + _passingGrade);

            // factor in variance
            grade = Random.Range(grade - variance, grade + variance);
            return grade;
        }


        /// <summary>
        /// Determine level of variance in grade based on difficulty level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private int GetVariance(DifficultyLevel level)
        {
            // note: add in game difficulty here?
            switch (level)
            {
                case DifficultyLevel.Easiest: return 1;
                case DifficultyLevel.Easy: return 3;
                case DifficultyLevel.Medium: return 5;
                case DifficultyLevel.Hard: return 7;
                case DifficultyLevel.Hardest: return 10;
            }
            return 0;
        }
    }
}