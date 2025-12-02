using UnityEngine;

namespace Emyra.FocusGame.EventChannel
{
    [CreateAssetMenu(fileName = "ActivityIntEventSO", menuName = "Events/Standard/Custom/ActivityName-Int")]
    public class ActivityIntEventSO : GenericGenericStandardEventSO<Locations.ActivityName, int> { }
}
