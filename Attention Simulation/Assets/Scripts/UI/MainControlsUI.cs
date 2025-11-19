using Emyra.FocusGame.EventChannel;
using Emyra.FocusGame.GameData;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using LocationInfo = Emyra.FocusGame.GameData.LocationInfo;

namespace Emyra.FocusGame.UI
{
    public class MainControlsUI : MonoBehaviour
    {
        [Title("References")]
        public UIDocument ControlsDocument;
        [SerializeField] private DurationPopupUI _durationPopup;


        [TitleGroup("Events")]
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private LocationInfoEventSO _locationChangedEvent;
        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private VoidEventSO _startGameEvent; // temp (debug only)
        [SerializeField] private ActivityIntEventSO _activitySelectedEvent;


        private Dictionary<ActivityName, Button> _buttonDict;
        // var for readability
        private ActivityDatabase _activityDb;

        // DEBUG (TEMP)
        private Button _startButton;

        private void Awake()
        {
            #region Debug
            _startButton = ControlsDocument.rootVisualElement.Q("start-button") as Button;
            #endregion

            _buttonDict = new Dictionary<ActivityName, Button>();
        }

        private void OnEnable()
        {
            #region Debug
            _startButton.RegisterCallback<ClickEvent>(StartGame);
            #endregion

            _activityDb = ActivityDatabase.Instance;
            // hide duration popup if needed
            if (_durationPopup.enabled) { _durationPopup.Close(); }
            
            // bind activity button callbacks
            InitButtons();

            // subscribe to events
            _locationChangedEvent.OnInvokeEvent += OnLocationChanged;
        }
        private void OnDisable()
        {
            #region Debug
            _startButton.UnregisterCallback<ClickEvent>(StartGame);
            #endregion

            // unbind activity button callbacks
            foreach(ActivityName activity in _buttonDict.Keys)
            {
                _buttonDict[activity].UnregisterCallback<ClickEvent, ActivityName>(OnActivityClicked);
            }

            // unsubscribe from events
            _locationChangedEvent.OnInvokeEvent -= OnLocationChanged;
        }

        private void InitButtons()
        {
            _buttonDict.Clear();

            // get all the buttons for each activity type and store them in a dictionary
            foreach (ActivityName activity in Enum.GetValues(typeof(ActivityName)))
            {
                string name = _activityDb.GetButtonName(activity);
                // skip if activity does not have matching button name
                if (name == null) { continue; }

                var ele = ControlsDocument.rootVisualElement.Q(name);
                if (ele != null)
                {
                    Button button = ele as Button;
                    button.RegisterCallback<ClickEvent, ActivityName>(OnActivityClicked, activity);
                    _buttonDict.Add(activity, button);
                }
            }
        }

        #region Debug
        private void StartGame(ClickEvent evt)
        {
            Debug.Log("starting game!");
            _startGameEvent.InvokeEvent();
            _startButton.style.display = DisplayStyle.None;
            _startButton.SetEnabled(false);
        }
        #endregion

        private void OnLocationChanged(LocationInfo info)
        {
            // update activity buttons shown
            foreach (ActivityName activity in _buttonDict.Keys)
            {
                // check if activity is enabled for given location
                if ((info.Activities & activity) == activity)
                {
                    // enable activity's button if not already enabled
                    if (!_buttonDict[activity].enabledSelf)
                    {
                        _buttonDict[activity].SetEnabled(true);
                        _buttonDict[activity].style.display = DisplayStyle.Flex;
                    }
                }
                else
                {
                    // disable button and remove it from layout
                    _buttonDict[activity].SetEnabled(false);
                    _buttonDict[activity].style.display = DisplayStyle.None;
                }
            }
        }

        #region Activity Buttons
        private void OnActivityClicked(ClickEvent evt, ActivityName activity)
        {
            // if activity has a fixed duration, execute it on click
            if (_activityDb.CheckDuration(activity))
            {
                // send -1 for default duration
                _activitySelectedEvent.InvokeEvent(activity, -1);
            }
            else
            {
                // else prompt user for duration 
                _durationPopup.OnSubmitEvent.AddListener((mins) => SendActivityWithDuration(mins, activity));
                _durationPopup.Show();
            }
        }

        private void SendActivityWithDuration(int minutes, ActivityName activity)
        {
            // send event
            _activitySelectedEvent.InvokeEvent(activity, minutes);
            // close popup
            _durationPopup.Close();
        }
        #endregion
    }
}
