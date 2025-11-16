using Emyra.Simulator.EventChannel;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emyra.Simulator.Testing
{
    public class DebugManager : MonoBehaviour
    {

        [FoldoutGroup("GameInfo Events", order: 2)]
        [SerializeField] private VoidIntRequestEventSO _changeDayEvent;
        [FoldoutGroup("GameInfo Events")]
        [SerializeField] private IntEventSO _addTimeEvent;
        [FoldoutGroup("GameInfo Events")]
        [SerializeField] private VoidEventSO _sleepEvent;
        [FoldoutGroup("GameInfo Events")]
        [SerializeField] private StringEventSO _locationChangedEvent;
        [FoldoutGroup("GameInfo Events")]
        [SerializeField] private StringEventSO _subjectChangedEvent;


        #region GameInfo Testing Methods
        [TitleGroup("GameInfo Testing", order: 1)]
        [ButtonGroup("GameInfo Testing/Button")]
        public void ChangeDay() { _changeDayEvent.RequestEvent(); }
        [TitleGroup("GameInfo Testing")]
        [Button]
        public void AddTime(int minutes) { _addTimeEvent.InvokeEvent(minutes); }
        [ButtonGroup("GameInfo Testing/Button")]
        public void SleepEvent() { _sleepEvent.InvokeEvent(); }
        [TitleGroup("GameInfo Testing")]
        [Button]
        public void ChangeLocation(string location) { _locationChangedEvent.InvokeEvent(location);}
        [TitleGroup("GameInfo Testing")]
        [Button]
        public void ChangeSubject(string subject) { _subjectChangedEvent.InvokeEvent(subject);}
        #endregion
    }
}
