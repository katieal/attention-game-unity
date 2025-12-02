using Emyra.FocusGame.School;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Emyra.FocusGame.Locations
{
    public enum Place { Home, School }

    [Flags]
    public enum RoomName
    {
        None = 0,
        Bedroom = 1 << 0,
        Kitchen = 1 << 1,
        Outside = 1 << 2,
        Bathroom = 1 << 3,
        Classroom = 1 << 4,
        SchoolLibrary = 1 << 5,
        Cafeteria = 1 << 6,
        Home = 1 << 7,
    }

    [CreateAssetMenu(fileName = "RoomSO", menuName = "Scriptable Objects/Room SO")]
    public class RoomSO : ScriptableObject
    {
        [EnumToggleButtons] public Place Place;
        public RoomName RoomName;

        public ActivityName Activities;

        public RoomName ConnectedRooms;

        public RoomInfo GetInfo()
        {
            RoomInfo info = new RoomInfo()
            {
                Place = this.Place,
                Room = this.RoomName,
                SubjectName = string.Empty,
                ConnectedRooms = this.ConnectedRooms,
                Activities = this.Activities
            };

            return info;
        }
    }

    public struct RoomInfo
    {
        public Place Place;
        public RoomName Room;
        public string SubjectName;
        public RoomName ConnectedRooms;
        public ActivityName Activities;
    }
}
