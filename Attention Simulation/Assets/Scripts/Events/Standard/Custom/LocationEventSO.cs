using Emyra.Simulator.EventChannel;
using UnityEngine;

namespace Emyra.Simulator
{
    [CreateAssetMenu(fileName = "LocationEventSO", menuName = "Events/Standard/Custom/Location")]
    public class LocationEventSO : GenericStandardEventSO<GameData.Location> { }
}
