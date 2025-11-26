using Emyra.FocusGame.GameData;
using Emyra.FocusGame.Locations;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Emyra.FocusGame.UI
{
    public class ActionsUI : SerializedMonoBehaviour
    {
        [Title("Components")]
        public GameObject ButtonPanel;
        public Dictionary<ActivityName, Button> ActionButtons = new Dictionary<ActivityName, Button>();
    }
}
