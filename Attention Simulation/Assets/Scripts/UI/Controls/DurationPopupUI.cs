using Sirenix.OdinInspector;
using System;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Emyra.FocusGame.UI
{
    public class DurationPopupUI : MonoBehaviour
    {
        [Title("References")]
        public UIDocument ControlsDocument;

        // UI elements
        private VisualElement _durationPopup;
        private Button _submitButton;
        private Button _cancelButton;
        private SliderInt _slider;

        // data
        public DurationPopupDataSource DataSource;

        private int _minTime = 10;
        private int _maxTime = 120;

        // submit button event
        public UnityEvent<int> OnSubmitEvent;

        private void Awake()
        {
            DataSource = new DurationPopupDataSource()
            {
                MinTime = _minTime,
                MaxTime = _maxTime,
                Minutes = _minTime
            };

            _durationPopup = ControlsDocument.rootVisualElement.Q("duration-popup");
            _durationPopup.dataSource = DataSource;
            _submitButton = _durationPopup.Q("submit-button") as Button;
            _cancelButton = _durationPopup.Q("cancel-button") as Button;
            _slider = _durationPopup.Q("minutes-slider") as SliderInt;
        }

        private void OnEnable()
        {
            // register callbacks
            _submitButton.RegisterCallback<ClickEvent>(OnSubmitButton);
            _cancelButton.RegisterCallback<ClickEvent>(OnCancelButton);
        }
        private void OnDisable()
        {
            // unregister callbacks
            OnSubmitEvent.RemoveAllListeners();
            _submitButton.UnregisterCallback<ClickEvent>(OnSubmitButton);
            _cancelButton.UnregisterCallback<ClickEvent>(OnCancelButton);
        }

        public void Show()
        {
            // open popup
            this.enabled = true;
            _durationPopup.visible = true;
        }
        public void Close()
        {
            // close popup
            _durationPopup.visible = false;
            // reset data source
            DataSource.Minutes = DataSource.MinTime;
            _slider.value = DataSource.MinTime;
            this.enabled = false;
        }

        private void OnSubmitButton(ClickEvent evt)
        {
            OnSubmitEvent.Invoke(DataSource.Minutes);
        }
        private void OnCancelButton(ClickEvent evt)
        {
            // close popup
            Close();
        }

    }

    public class DurationPopupDataSource : INotifyBindablePropertyChanged
    {
        private int _minTime;
        private int _maxTime;
        private int _minutes;

        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

        [CreateProperty]
        public int MinTime
        {
            get => _minTime;
            set
            {
                if (_minTime == value) return;
                _minTime = value;
                Notify();
            }
        }

        [CreateProperty]
        public int MaxTime
        {
            get => _maxTime;
            set
            {
                if (_maxTime == value) return;
                _maxTime = value;
                Notify();
            }
        }

        [CreateProperty]
        public int Minutes
        {
            get => _minutes;
            set
            {
                if (_minutes == value) return;
                _minutes = value;
                Notify();
            }
        }


        void Notify([CallerMemberName] string property = "")
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property));
        }
    }
}
