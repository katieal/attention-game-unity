using Emyra.FocusGame.EventChannel;
using Emyra.FocusGame.GameData;
using Emyra.FocusGame.School;
using Emyra.FocusGame.Managers;
using Sirenix.OdinInspector;
using System;
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



        [Button]
        public void GetSubjectString(SubjectType subject)
        {
            string name = subject.ToString();

            // excluding the first character, insert a space before each capital letter
            for (int i = 1; i < name.Length; i++)
            {
                if (Char.IsUpper(name[i]))
                {
                    name = name.Insert(i, " ");
                    i++;
                    Debug.Log("inserting space");
                }
            }

            Debug.Log(name);
        }


        #region GameInfo Testing 
        [FoldoutGroup("GameInfo Events", order: 3)]
        [SerializeField] private VoidIntRequestEventSO _changeDayEvent;
        [FoldoutGroup("GameInfo Events")]
        [SerializeField] private IntEventSO _addTimeEvent;
        [FoldoutGroup("GameInfo Events")]
        [SerializeField] private VoidEventSO _sleepEvent;
        

        [TitleGroup("GameInfo Testing", order: 2)]
        [ButtonGroup("GameInfo Testing/Button")]
        public void ChangeDay() { _changeDayEvent.RequestEvent(); }
        [TitleGroup("GameInfo Testing")]
        [Button]
        public void AddTime(int minutes) { _addTimeEvent.InvokeEvent(minutes); }
        [ButtonGroup("GameInfo Testing/Button")]
        public void SleepEvent() { _sleepEvent.InvokeEvent(); }
        #endregion
    }
}
