using Emyra.FocusGame.EventChannel;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Emyra.FocusGame.UI
{
    public class StartMenuUI : MonoBehaviour
    {
        [Title("Components")]
        [SerializeField] private GameObject _startCanvas;
        [SerializeField] private Button _startButton;

        [TitleGroup("Events")]
        [FoldoutGroup("Events/Invoked Events")]
        [SerializeField] private VoidEventSO _startNewGameEvent;


        private void OnEnable()
        {
            _startButton.onClick.AddListener(() => OnStartNewGame());
        }
        private void OnDisable()
        {
            _startButton.onClick.RemoveAllListeners();
        }

        private void OnStartNewGame()
        {
            _startCanvas.SetActive(false);
            _startNewGameEvent.InvokeEvent();
        }
    }
}
