using Emyra.FocusGame.Locations;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Emyra.FocusGame.GameData
{
    public class RoomDatabase : MonoBehaviour
    {
        [Title("Database")]
        [AssetList]
        [SerializeField] private List<RoomSO> _rooms;

        #region Singleton
        // singleton reference
        private static RoomDatabase _instance;
        public static RoomDatabase Instance { get { return _instance; } }

        private void Awake()
        {
            // singleton pattern
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
        #endregion


        public RoomInfo GetInfo(RoomName roomName)
        {
            if (FindRoom(roomName, out RoomSO roomSO))
            {
                return roomSO.GetInfo();
            }
            else { return new RoomInfo(); }
        }


        private bool FindRoom(RoomName room, out RoomSO roomSO)
        {
            roomSO = _rooms.Find(x => x.RoomName == room);
            return roomSO != null;
        }
    }
}
