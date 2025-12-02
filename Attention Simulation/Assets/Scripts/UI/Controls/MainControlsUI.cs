using Emyra.FocusGame.EventChannel;
using Emyra.FocusGame.Locations;
using Emyra.FocusGame.GameData;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using RoomInfo = Emyra.FocusGame.Locations.RoomInfo;

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
        [SerializeField] private ActivityInfoEventSO _activitySelectedEvent;


        private Dictionary<ActivityName, Button> _buttonDict;
        private List<Button> _roomButtonList;
        private VisualElement _roomButtonsElement;

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

            _roomButtonsElement = ControlsDocument.rootVisualElement.Q("room-buttons");
        }

        private void OnEnable()
        {
            #region Debug
            _startButton.RegisterCallback<ClickEvent>(StartGame);
            #endregion

            _activityDb = ActivityDatabase.Instance;

            // bind buttons and callbacks
            InitActivityButtons();
            InitRoomButtons();

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

        private void Start()
        {
            // hide duration popup if needed
            if (_durationPopup.enabled) { _durationPopup.Close(); }
        }

        private void InitActivityButtons()
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
                    //if (activity != ActivityName.Move)
                    //{
                        // callback for move button is registered when location changes
                        button.RegisterCallback<ClickEvent, ActivityName>(OnActivityClicked, activity);
                    //}

                    _buttonDict.Add(activity, button);
                }
            }
        }

        private void InitRoomButtons()
        {
            _roomButtonList = new List<Button>();

            foreach (VisualElement ele in _roomButtonsElement.Children())
            {
                _roomButtonList.Add(ele as Button);
            }

            // hide room options at start
            ShowRoomButtonBar(false);
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

        private void OnLocationChanged(RoomInfo info)
        {
            ClearRoomButtonCallbacks();

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

                    // set move button text and callbacks
                    if (activity == ActivityName.Move)
                    {
                        SetRoomButtons(info.ConnectedRooms);
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
            if (activity == ActivityName.Move)
            {
                ShowRoomButtonBar(true);
            }
            // if activity has a fixed duration, execute it on click
            else if (_activityDb.CheckDuration(activity))
            {
                // send -1 for default duration
                _activitySelectedEvent.InvokeEvent(GetActivityInfo(activity));
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
            _activitySelectedEvent.InvokeEvent(GetActivityInfo(activity, minutes));
            // close popup
            _durationPopup.Close();
        }
        #endregion

        #region Room Buttons
        private void ShowRoomButtonBar(bool isVisible)
        {
            _roomButtonsElement.SetEnabled(isVisible);
            _roomButtonsElement.visible = isVisible;
        }

        private void SetRoomButtons(RoomName roomMask)
        {
            // current index in room buttons list
            int buttonIndex = 0;

            foreach (RoomName room in Enum.GetValues(typeof(RoomName)))
            {
                // check if bitmask has this room name
                if ((room & roomMask) == room)
                {
                    // set button text and add callback
                    _roomButtonList[buttonIndex].text = GetRoomString(room);
                    _roomButtonList[buttonIndex].RegisterCallback<ClickEvent, RoomName>(OnRoomButtonClicked, room);
                }
            }
        }

        private void ClearRoomButtonCallbacks()
        {
            foreach (Button button in _roomButtonList)
            {
                button.UnregisterCallback<ClickEvent, RoomName>(OnRoomButtonClicked);
            }
        }

        private void OnRoomButtonClicked(ClickEvent evt, RoomName room)
        {
            _activitySelectedEvent.InvokeEvent(GetActivityInfo(ActivityName.Move, nextRoom: room));
        }

        private string GetRoomString(RoomName room)
        {
            switch (room)
            {
                case RoomName.Classroom: return "Next Class";
                case RoomName.SchoolLibrary: return "School Library";
                default: return room.ToString();
            }
        }
        #endregion

        private SelectedActivityInfo GetActivityInfo(ActivityName activity, int duration = -1, RoomName nextRoom = RoomName.None)
        {
            return new SelectedActivityInfo()
            {
                SelectedActivity = activity,
                Duration = duration,
                NextRoom = nextRoom
            };
        }
    }


    public struct SelectedActivityInfo
    {
        public ActivityName SelectedActivity;
        public int Duration;
        public RoomName NextRoom;
    }
}
