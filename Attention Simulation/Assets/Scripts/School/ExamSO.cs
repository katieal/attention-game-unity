using Sirenix.OdinInspector;
using UnityEngine;

namespace Emyra.FocusGame.School
{
    public enum ExamType { Regular, Final }

    [CreateAssetMenu(fileName = "ExamSO", menuName = "Scriptable Objects/School/ExamSO")]
    public class ExamSO : ScriptableObject
    {
        [Title("Exam Info")]
        public string Name;
        public ExamType Type;

        [ShowIf("Type", Value = ExamType.Regular)]
        [Range(1, 3)]
        public int Order; // out of 3?

        public Status Status;

        [Tooltip("Knowledge range required for this exam.")]
        [MinMaxSlider(0, 100, true)]
        public Vector2Int KnowledgeRequired;
    }
}
