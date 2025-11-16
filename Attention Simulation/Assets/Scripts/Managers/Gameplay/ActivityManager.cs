using Emyra.Simulator.EventChannel;
using Emyra.Simulator.GameData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emyra.Simulator.Managers
{
    /// <summary>
    /// Class to manage the player's current activity (current subject at school/current activity at home, etc.)
    /// Also determines which activity/subject comes next
    /// </summary>

    public class ActivityManager : MonoBehaviour
    {
        public ActivitySO[] Schedule;

        [TitleGroup("Events")]
        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private LocationEventSO _locationChangedEvent;
        [FoldoutGroup("Events/Responding to Events")]
        [SerializeField] private VoidActivityEventSO _nextActivityEvent;

        private int _cActivity;

        private void OnEnable()
        {
            _nextActivityEvent.OnRequestEvent += OnNextActivityRequestEvent;
        }
        private void OnDisable()
        {
            _nextActivityEvent.OnRequestEvent -= OnNextActivityRequestEvent;
        }

        private void OnNextActivityRequestEvent()
        {
            if (_cActivity + 1 == Schedule.Length)
            {
                Debug.Log("end of schedule!");
                return;
            }

            // broadcast location changed
            if (Schedule[_cActivity].Location != Schedule[_cActivity + 1].Location)
            {
                _locationChangedEvent.InvokeEvent(Schedule[_cActivity + 1].Location);
            }

            // broadcast next activity
            _cActivity++;
            _nextActivityEvent.SendResult(Schedule[_cActivity].ActivityName);
        }


    }
}
