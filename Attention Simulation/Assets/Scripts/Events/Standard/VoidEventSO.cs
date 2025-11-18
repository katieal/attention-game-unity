using UnityEngine;
using UnityEngine.Events;

namespace Emyra.FocusGame.EventChannel
{
    [CreateAssetMenu(fileName = "VoidEventSO", menuName = "Events/Standard/Void")]
    public class VoidEventSO : BaseStandardEventSO 
    {
        public UnityAction OnInvokeEvent;

        public void InvokeEvent()
        {
            OnInvokeEvent?.Invoke();
        }
    }
}
