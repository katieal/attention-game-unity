using Emyra.Simulator.EventChannel;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace Emyra.Simulator.UI
{
    public class MainControlsUI : MonoBehaviour
    {
        public UIDocument ControlsDocument;

        [TitleGroup("Events")]
        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private VoidActivityEventSO _nextActivityEvent;

        private Button _nextButton;

        private void Awake()
        {
            _nextButton = ControlsDocument.rootVisualElement.Q("next-button") as Button;

        }

        private void OnEnable()
        {
            _nextButton.RegisterCallback<ClickEvent>(RequestNextActivity);
        }
        private void OnDisable()
        {
            _nextButton.UnregisterCallback<ClickEvent>(RequestNextActivity);
        }

        private void RequestNextActivity(ClickEvent evt)
        {
            _nextActivityEvent.RequestEvent();
        }
    }
}
