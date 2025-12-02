using UnityEngine;

namespace Emyra.FocusGame.EventChannel
{
    [CreateAssetMenu(fileName = "LocationInfoEventSO", menuName = "Events/Standard/Custom/RoomInfo")]
    public class LocationInfoEventSO : GenericStandardEventSO<Locations.RoomInfo> { }
}
