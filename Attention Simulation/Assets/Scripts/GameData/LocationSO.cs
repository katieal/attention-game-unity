using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Emyra.FocusGame.GameData
{
    public enum Place { Home, School }
    public enum Room
    {
         Classroom, Library, Cafeteria, Bedroom, Kitchen
    }

    public enum Subject
    {
        None = -1, Math, Science, LanguageArts, History, SocialStudies,
        ForeignLanguage
    }

    [Flags]
    public enum ActivityType
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

        Classroom = ListenYes | ListenNo | Nap,
        Bedroom = Work | Study | Nap | Relax
    }

    [CreateAssetMenu(fileName = "LocationSO", menuName = "Scriptable Objects/Location")]
    public class LocationSO : ScriptableObject
    {
        [EnumToggleButtons] public Place Place;
        public Room Room;
        [ShowIf("Room", Value = Room.Classroom)]
        public Subject Subject;

        public ActivityType Activities;

        public LocationInfo GetInfo()
        {
            LocationInfo info = new LocationInfo()
            {
                Place = this.Place,
                Room = this.Room,
                Subject = this.Subject,
                Activities = this.Activities
            };

            return info;
        }
    }

    public struct LocationInfo
    {
        public Place Place;
        public Room Room;
        public Subject Subject;
        public ActivityType Activities;
    }
}
