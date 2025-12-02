using Emyra.FocusGame.GameData;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Emyra.FocusGame.Locations
{
    [Flags]
    public enum ActivityName
    {
        None = 0,
        ListenYes = 1 << 0,
        ListenNo = 1 << 1,
        Work = 1 << 2, // classwork or hw
        Study = 1 << 3,
        Nap = 1 << 4,
        Eat = 1 << 5,
        Relax = 1 << 6,
        Sleep = 1 << 7,
        LeaveForSchool = 1 << 8,

        Classroom = ListenYes | ListenNo | Nap,
        Bedroom = Work | Study | Nap | Relax | Sleep
    }


    // bind each ActivityType with a display name and UIToolkit button name
    [CreateAssetMenu(fileName = "ActivitySO", menuName = "Scriptable Objects/ActivitySO")]
    public class ActivitySO : ScriptableObject
    {
        public ActivityName Activity;
        public string DisplayName;
        public string ButtonName;
        public bool HasFixedDuration;
        //[SuffixLabel("Minutes", Overlay = true)]
        //[ShowIf("HasFixedDuration", Value = true)] public int DefaultDuration;
        public bool HasFixedStartTime;
        [ShowIf("HasFixedStartTime", Value = true)]
        [TimeSelector] public int StartTime;
    }
}
