using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Emyra.FocusGame.School
{
    /// <summary>
    /// Used to define the knowledge % range corresponding to each grade. 
    /// Ranges are modified by overall Game Difficulty.
    /// </summary>
    [CreateAssetMenu(fileName = "GradeRangeSO", menuName = "Scriptable Objects/School/GradeRangeSO")]
    public class GradeRangeSO : ScriptableObject
    {
        [field: Title("Difficulty Level")]
        [EnumToggleButtons]
        [field: SerializeField] public DifficultyLevel Difficulty { get; private set; }

        [field: Title("Assigned Ranges")]
        [field: Tooltip("Point percent ranges for each grade. Min inclusive, Max exclusive (100 is inclusive)")]
        [field: SerializeField]
        public GradeRange[] GradeRanges { get; private set; } 

        public LetterGrade GetGradeLetter(float percent)
        {
            foreach (GradeRange range in GradeRanges)
            {
                if (percent >= 100) { return LetterGrade.APlus; }
                if (percent >= range.RangeRequired.x && percent < range.RangeRequired.y) { return range.Grade; }
            }
            return LetterGrade.None;
        }

        public string GetGradeString(float percent)
        {
            LetterGrade letter = GetGradeLetter(percent);
            return GetStringFromEnum(letter);
        }

        public Vector2 GetRange(LetterGrade grade)
        {
            return GradeRanges[(int)grade].RangeRequired;
        }

        private string GetStringFromEnum(LetterGrade grade)
        {
            switch(grade)
            {
                case LetterGrade.None: return "None";
                case LetterGrade.DMinus: return "D-";
                case LetterGrade.DPlus: return "D+";
                case LetterGrade.CMinus: return "C-";
                case LetterGrade.CPlus: return "C+";
                case LetterGrade.BMinus: return "B-";
                case LetterGrade.BPlus: return "B+";
                case LetterGrade.AMinus: return "A-";
                case LetterGrade.APlus: return "A+";
                default: return grade.ToString();
            }
        }

#if UNITY_EDITOR
        [Button]
        public void AddGradeRanges()
        {
            GradeRanges = new GradeRange[Enum.GetValues(typeof(LetterGrade)).Length - 1];

            for (int i = 0; i < GradeRanges.Length; i++)
            {
                GradeRanges[i] = new GradeRange { Grade = (LetterGrade)(GradeRanges.Length - (i + 1)) };
            }
        }
#endif
    }

    public enum DifficultyLevel { Easiest = 1, Easy, Medium, Hard, Hardest }
    public enum LetterGrade { None = -1, F, DMinus, D, DPlus, CMinus, C, CPlus, 
        BMinus, B, BPlus, AMinus, A, APlus }

    [Serializable]
    public struct GradeRange
    {
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        [HorizontalGroup(Width = 0.1f, Gap = 8), HideLabel]
        public LetterGrade Grade;
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        [HorizontalGroup(Gap = 8), HideLabel]
        [MinMaxSlider(0, 100, true)]
        [Tooltip("Min (inclusive) and Max (inclusive) values for this range.")]
        public Vector2 RangeRequired;
    }
}
