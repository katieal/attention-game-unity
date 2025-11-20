using Emyra.FocusGame.School;
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

    [CreateAssetMenu(fileName = "LocationSO", menuName = "Scriptable Objects/Location")]
    public class LocationSO : ScriptableObject
    {
        [EnumToggleButtons] public Place Place;
        public Room Room;
        [ShowIf("Room", Value = Room.Classroom)]
        public SubjectType Subject;

        public ActivityName Activities;

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
        public SubjectType Subject;
        public ActivityName Activities;
    }
}
