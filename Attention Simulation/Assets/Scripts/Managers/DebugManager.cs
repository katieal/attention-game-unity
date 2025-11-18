using Emyra.FocusGame.EventChannel;
using Emyra.FocusGame.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emyra.FocusGame.Testing
{
    public class DebugManager : MonoBehaviour
    {
        #region Singleton
        // singleton reference
        //private static Database _instance;
        //public static Database Instance { get { return _instance; } }

        //private void Awake()
        //{
        //    // singleton pattern
        //    if (_instance != null && _instance != this)
        //    {
        //        Destroy(this.gameObject);
        //    }
        //    else
        //    {
        //        _instance = this;
        //    }
        //}
        #endregion


        [FoldoutGroup("GameInfo Events", order: 2)]
        [SerializeField] private VoidIntRequestEventSO _changeDayEvent;
        [FoldoutGroup("GameInfo Events")]
        [SerializeField] private IntEventSO _addTimeEvent;
        [FoldoutGroup("GameInfo Events")]
        [SerializeField] private VoidEventSO _sleepEvent;
        //[FoldoutGroup("GameInfo Events")]
        //[SerializeField] private StringEventSO _locationChangedEvent;



        #region GameInfo Testing Methods
        [TitleGroup("GameInfo Testing", order: 1)]
        [ButtonGroup("GameInfo Testing/Button")]
        public void ChangeDay() { _changeDayEvent.RequestEvent(); }
        [TitleGroup("GameInfo Testing")]
        [Button]
        public void AddTime(int minutes) { _addTimeEvent.InvokeEvent(minutes); }
        [ButtonGroup("GameInfo Testing/Button")]
        public void SleepEvent() { _sleepEvent.InvokeEvent(); }
        //[TitleGroup("GameInfo Testing")]
        //[Button]
        //public void ChangeLocation(string location) { _locationChangedEvent.InvokeEvent(location);}

        #endregion
    }
}
