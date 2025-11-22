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
        [field: Tooltip("Point percent ranges for each grade. Min/Max inclusive.")]
        [field: SerializeField]
        public GradeRange[] GradeRanges { get; private set; } 

        public LetterGrade GetGrade(float value)
        {
            foreach (GradeRange range in GradeRanges)
            {
                if (value >= range.RangeRequired.x && value <= range.RangeRequired.y) { return range.Grade; }
            }
            return LetterGrade.None;
        }

        public Vector2 GetRange(LetterGrade grade)
        {
            return GradeRanges[(int)grade].RangeRequired;
        }

#if UNITY_EDITOR
        [Button]
        public void AddGradeRanges()
        {
            GradeRanges = new GradeRange[Enum.GetValues(typeof(LetterGrade)).Length - 1];

            for (int i = 0; i < GradeRanges.Length; i++)
            {
                GradeRanges[i] = new GradeRange { Grade = (LetterGrade)i };
            }
        }
#endif
    }

    public enum DifficultyLevel { Easiest = 1, Easy, Medium, Hard, Hardest }
    public enum LetterGrade { None = -1, F, FPlus, DMinus, D, DPlus, CMinus, C, CPlus, 
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
