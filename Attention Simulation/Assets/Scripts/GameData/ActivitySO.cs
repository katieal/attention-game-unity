using UnityEngine;

namespace Emyra.FocusGame.GameData
{
    // bind each ActivityType with a display name and UIToolkit button name
    [CreateAssetMenu(fileName = "ActivitySO", menuName = "Scriptable Objects/ActivitySO")]
    public class ActivitySO : ScriptableObject
    {
        public ActivityType Activity;
        public string DisplayName;
        public string ButtonName;
     
    }
}
