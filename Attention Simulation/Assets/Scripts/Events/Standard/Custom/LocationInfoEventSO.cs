using UnityEngine;

namespace Emyra.FocusGame.EventChannel
{
    [CreateAssetMenu(fileName = "LocationInfoEventSO", menuName = "Events/Standard/Custom/LocationInfo")]
    public class LocationInfoEventSO : GenericStandardEventSO<GameData.LocationInfo> { }
}
