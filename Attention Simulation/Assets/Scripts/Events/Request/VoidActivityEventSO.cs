using UnityEngine;

namespace Emyra.Simulator.EventChannel
{
    [CreateAssetMenu(fileName = "VoidActivityEventSO", menuName = "Events/Request/Custom/Void Activity")]
    public class VoidActivityEventSO : VoidGenericRequestEventSO<GameData.ActivityName> { }
}
