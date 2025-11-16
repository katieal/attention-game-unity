using UnityEngine;

namespace Emyra.Simulator.GameData
{
    public enum Location { Home, School }
    public enum ActivityName
    {
        None = -1, Math, Science, LanguageArts, History, SocialStudies,
        ForeignLanguage, Lunch, Homework, Study, Eat, Relax, Sleep
    }
    public enum ActionType
    {
        None = -1, Subject, Custom, UserDefined
    }

    [CreateAssetMenu(fileName = "ActivitySO", menuName = "Scriptable Objects/ActivitySO")]
    public class ActivitySO : ScriptableObject
    {

        public Location Location;
        public ActivityName ActivityName;
        public ActionType ActionType;

    }
}
