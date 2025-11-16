using UnityEngine;

namespace Emyra.Simulator.EventChannel
{
    [CreateAssetMenu(fileName = "StringEventSO", menuName = "Events/Standard/OneArg/String")]
    public class StringEventSO : GenericStandardEventSO<string> { }
}
