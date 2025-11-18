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
        [SerializeField] private ActivityTypeEventSO _activitySelectedEvent;
        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private LocationInfoEventSO _locationChangedEvent;

        private int _currentIndex;

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
            _locationChangedEvent.InvokeEvent(Schedule[_currentIndex].GetInfo());
        }

        private void OnActivitySelected(ActivityType activity)
        {

            // using None as a temp next button
            if (activity == ActivityType.None) { SendNextLocation(); }
        }
        
        private void SendNextLocation()
        {
            if (_currentIndex + 1 == Schedule.Length)
            {
                Debug.Log("end of schedule!");
                return;
            }

            _currentIndex++;
            // broadcast next location
            _locationChangedEvent.InvokeEvent(Schedule[_currentIndex].GetInfo());
        }


    }
}
