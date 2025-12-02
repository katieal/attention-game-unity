using Emyra.FocusGame.EventChannel;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Emyra.FocusGame.Managers
{
    public class InputManager : MonoBehaviour
    {
        [TitleGroup("Events")]
        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private VoidEventSO _subjectMenuInputEvent;

        // action maps
        private InputActionMap _playerMap, _uiMap, _menuMap;

        // menu actions 
        private InputAction _subjectMenuAction;

        private void Awake()
        {
            // action maps
            _playerMap = InputSystem.actions.FindActionMap("Player");
            _uiMap = InputSystem.actions.FindActionMap("UI");
            _menuMap = InputSystem.actions.FindActionMap("Menus");

            // menu actions
            _subjectMenuAction = _menuMap.FindAction("SubjectMenu");
        }

        private void OnEnable()
        {
            // note: player actions should probably start disabled

            // menu action callbacks
            _subjectMenuAction.performed += OnSubjectMenu;
        }
        private void OnDisable()
        {

            // menu action callbacks
            _subjectMenuAction.performed -= OnSubjectMenu;
        }


        // UI Actions - open subject menu
        private void OnSubjectMenu(InputAction.CallbackContext context)
        {
            _subjectMenuInputEvent.InvokeEvent();
        }

    }
}
