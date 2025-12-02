using UnityEngine;

namespace Emyra.FocusGame.EventChannel
{
    [CreateAssetMenu(fileName = "ActivityInfoEventSO", menuName = "Events/Standard/Custom/ActivityInfo")]
    public class ActivityInfoEventSO : GenericStandardEventSO<Emyra.FocusGame.UI.SelectedActivityInfo> { }
}
