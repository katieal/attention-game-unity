using UnityEngine;

namespace Emyra.FocusGame.EventChannel
{
    [CreateAssetMenu(fileName = "StringListEventSO", menuName = "Events/Standard/OneArg/StringList")]
    public class StringListEventSO : GenericStandardEventSO<System.Collections.Generic.List<string>> { }
}
