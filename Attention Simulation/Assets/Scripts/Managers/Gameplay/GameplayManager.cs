using Emyra.FocusGame.EventChannel;
using Emyra.FocusGame.Locations;
using Emyra.FocusGame.GameData;
using Emyra.FocusGame.School;
using Sirenix.OdinInspector;
using UnityEngine;
using LocationInfo = Emyra.FocusGame.Locations.LocationInfo;
using System.Collections.Generic;

namespace Emyra.FocusGame.Managers
{
    /// <summary>
    /// Class to manage the player's current activity (current subject at school/current activity at home, etc.)
    /// Also determines which activity/subject comes next
    /// </summary>

    public class GameplayManager : MonoBehaviour
    {
        [Title("Gameplay Schedule")]
        public LocationSO[] ScheduleList;
        public SchoolSubjectSO[] SchoolScheduleList;

        [TitleGroup("Events")]
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private VoidEventSO _startGameEvent;
        [SerializeField] private ActivityIntEventSO _activitySelectedEvent;
        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private StringListEventSO _schoolScheduleEvent;
        [SerializeField] private LocationInfoEventSO _locationChangedEvent;
        [SerializeField] private IntEventSO _addTimeEvent;
        [SerializeField] private IntEventSO _setTimeEvent;
        [SerializeField] private VoidEventSO _sleepEvent;

        /// <summary>
        /// Index of current place in schedule list
        /// </summary>
        private int _scheduleIndex;
        /// <summary>
        /// Index of current school subject in schedule
        /// </summary>
        private int _schoolIndex;

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

            // send schedule of school subjects
            List<string> subjectIds = new List<string>();
            foreach (SchoolSubjectSO subject in SchoolScheduleList)
            {
                subjectIds.Add(subject.Id);
            }
            _schoolScheduleEvent.InvokeEvent(subjectIds);

            // send starting location
            BroadcastLocation();
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
                BroadcastLocation();
            }
            else if (activity == ActivityName.LeaveForSchool)
            {
                // set time to school start time
                _setTimeEvent.InvokeEvent(DayManager.Instance.SchoolStartTime);
                BroadcastNextLocation();
            }
            // classroom activities always last 1 hr (the entire duration)
            else if (ScheduleList[_scheduleIndex].Room == Room.Classroom)
            {
                // advance time by 60 mins
                _addTimeEvent.InvokeEvent(60); // keeping duration hard coded here for now
                BroadcastNextLocation();
            }
            else
            {
                // advance time
                _addTimeEvent.InvokeEvent(duration);
            }
        }
        
        private void RestartSchedule()
        {
            _scheduleIndex = 0;
            _schoolIndex = 0;
        }

        private void BroadcastNextLocation()
        {
            if (_scheduleIndex + 1 == ScheduleList.Length)
            {
                Debug.Log("end of schedule!");
                return;
            }

            // increment school index if in school
            if (ScheduleList[_scheduleIndex].Place == Place.School) { _schoolIndex++; }
            // go to next location in schedule
            _scheduleIndex++;

            BroadcastLocation();
        }

        private void BroadcastLocation()
        {
            // get location info
            LocationInfo info = ScheduleList[_scheduleIndex].GetInfo();
            // insert subject if needed
            if (ScheduleList[_scheduleIndex].Room == Room.Classroom)
            {
                info.SubjectName = SchoolScheduleList[_schoolIndex].SubjectName;
                //info.SubjectIndex = _schoolIndex;
            }

            // broadcast next location
            _locationChangedEvent.InvokeEvent(info);
        }

    }
}
