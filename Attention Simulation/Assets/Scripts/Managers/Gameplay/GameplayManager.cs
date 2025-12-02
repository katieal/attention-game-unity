using Emyra.FocusGame.EventChannel;
using Emyra.FocusGame.GameData;
using Emyra.FocusGame.Locations;
using Emyra.FocusGame.School;
using Emyra.FocusGame.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using RoomInfo = Emyra.FocusGame.Locations.RoomInfo;

namespace Emyra.FocusGame.Managers
{
    /// <summary>
    /// Class to manage the player's current activity (current subject at school/current activity at home, etc.)
    /// Also determines which activity/subject comes next
    /// </summary>

    public class GameplayManager : MonoBehaviour
    {
        //public LocationSO[] Schedule;
        [Title("Schedules")]
        public RoomSO StartingLocation;
        public RoomSO ClassroomSO; // temp var while I think of a better solution
        public SchoolSubjectSO[] SchoolScheduleList;

        [TitleGroup("Events")]
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private VoidEventSO _startGameEvent;
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private ActivityInfoEventSO _activitySelectedEvent;

        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private StringListEventSO _schoolScheduleEvent;
        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private LocationInfoEventSO _locationChangedEvent;
        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private IntEventSO _addTimeEvent;
        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private IntEventSO _setTimeEvent;
        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private VoidEventSO _sleepEvent;

        /// <summary>
        /// Index of current school subject in schedule
        /// </summary>
        private int _schoolIndex;
        private RoomInfo _currentLocation;

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
        private void OnActivitySelected(SelectedActivityInfo info)
        {
            // activity specific logic
            if (info.SelectedActivity == ActivityName.Sleep)
            {
                // sleep until the start of the next/current morning
                _sleepEvent.InvokeEvent();
                RestartSchedule();
                ChangeLocation(StartingLocation.GetInfo());
            }
            else if (info.SelectedActivity == ActivityName.LeaveForSchool)
            {
                // set time to school start time
                _setTimeEvent.InvokeEvent(DayManager.Instance.SchoolStartTime);
                ChangeLocation(GetNextSubject());
            }
            else if (info.SelectedActivity == ActivityName.Move)
            {
                // check if moving to next class
                if (info.NextRoom == RoomName.Classroom)
                {
                    // move to next class in schedule
                    _schoolIndex++;
                    ChangeLocation(GetNextSubject());
                }
                else
                {
                    // move to specified room
                    ChangeLocation(RoomDatabase.Instance.GetInfo(info.NextRoom));
                }
            }
            // classroom activities always last 1 hr (the entire duration)
            //else if (ScheduleList[_scheduleIndex].Room == Room.Classroom)
            //{
            //    // advance time by 60 mins
            //    _addTimeEvent.InvokeEvent(60); // keeping duration hard coded here for now
            //    BroadcastNextLocation();
            //}
            else
            {
                // advance time
                _addTimeEvent.InvokeEvent(info.Duration);
            }
        }

        private void RestartSchedule()
        {
            //_scheduleIndex = 0;
            _schoolIndex = 0;
        }

        private void ChangeLocation(RoomInfo info)
        {
            _currentLocation = info;
            // broadcast changed location
            _locationChangedEvent.InvokeEvent(info);
        }

        private void BroadcastNextLocation()
        {
            //if (_scheduleIndex + 1 == ScheduleList.Length)
            //{
            //    Debug.Log("end of schedule!");
            //    return;
            //}

            // increment school index if in school
            //if (ScheduleList[_scheduleIndex].Place == Place.School) { _schoolIndex++; }
            // go to next location in schedule
            //_scheduleIndex++;

            BroadcastLocation();
        }

        private void BroadcastLocation()
        {
            // get location info
            //LocationInfo info = ScheduleList[_scheduleIndex].GetInfo();
            // insert subject if needed
            //if (ScheduleList[_scheduleIndex].Room == Room.Classroom)
            //{
            //    info.SubjectName = SchoolScheduleList[_schoolIndex].SubjectName;
            //}

            //// broadcast next location
            //_locationChangedEvent.InvokeEvent(info);
        }

        // get room info for next subject in schedule
        private RoomInfo GetNextSubject()
        {
            // get location info
            RoomInfo info = ClassroomSO.GetInfo();
            // insert subject
            info.SubjectName = SchoolScheduleList[_schoolIndex].SubjectName;

            // check if class is last in schedule
            if (_schoolIndex == SchoolScheduleList.Length - 1)
            {
                // remove option for next class, add option for home
                info.ConnectedRooms &= ~RoomName.Classroom;
                info.ConnectedRooms |= RoomName.Home;
            }

            return info;
        }
    }
}

