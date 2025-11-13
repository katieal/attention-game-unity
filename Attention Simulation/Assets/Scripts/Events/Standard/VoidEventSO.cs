using UnityEngine;
using UnityEngine.Events;

namespace Emyra.Simulator.EventChannel
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
