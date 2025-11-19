using Emyra.FocusGame.EventChannel;
using Emyra.FocusGame.GameData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emyra.FocusGame.Managers
{
    /// <summary>
    /// Class to manage the player's current activity (current subject at school/current activity at home, etc.)
    /// Also determines which activity/subject comes next
    /// </summary>

    public class GameplayManager : MonoBehaviour
    {
        public LocationSO[] Schedule;

        [TitleGroup("Events")]
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private VoidEventSO _startGameEvent;
        [SerializeField] private ActivityIntEventSO _activitySelectedEvent;
        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private LocationInfoEventSO _locationChangedEvent;
        [SerializeField] private IntEventSO _addTimeEvent;
        [SerializeField] private IntEventSO _setTimeEvent;
        [SerializeField] private VoidEventSO _sleepEvent;

        // current index in schedule
        private int _cIndex;

        private void OnEnable()
        {
            _startGameEvent.OnInvokeEvent += OnStartGame;
            _activitySelectedEvent.OnInvokeEvent += OnActivitySelected;
        }
        private void OnDisable()
        {
            _startGameEvent.OnInvokeEvent -= OnStartGame;
            _activitySelectedEvent.OnInvokeEvent -= OnActivitySelected;
        }

        private void OnStartGame()
        {
            RestartSchedule();
        }

        // send -1 for default duration
        private void OnActivitySelected(ActivityName activity, int duration)
        {
            // activity specific logic
            if (activity == ActivityName.Sleep)
            {
                // sleep until the start of the next/current morning
                _sleepEvent.InvokeEvent();
                RestartSchedule();
            }
            else if (activity == ActivityName.LeaveForSchool)
            {
                // set time to school start time
                _setTimeEvent.InvokeEvent(DayManager.Instance.SchoolStartTime);
                SendNextLocation();
            }
            // classroom activities always last 1 hr (the entire duration)
            else if (Schedule[_cIndex].Room == Room.Classroom)
            {
                // advance time by 60 mins
                _addTimeEvent.InvokeEvent(60); // keeping duration hard coded here for now
                SendNextLocation();
            }
            else
            {
                // advance time
                _addTimeEvent.InvokeEvent(duration);
            }
        }
        
        private void RestartSchedule()
        {
            _cIndex = 0;
            // broadcast next location
            _locationChangedEvent.InvokeEvent(Schedule[_cIndex].GetInfo());
        }

        private void SendNextLocation()
        {
            if (_cIndex + 1 == Schedule.Length)
            {
                Debug.Log("end of schedule!");
                return;
            }

            _cIndex++;
            // broadcast next location
            _locationChangedEvent.InvokeEvent(Schedule[_cIndex].GetInfo());
        }


    }
}
