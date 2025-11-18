using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Emyra.FocusGame.GameData
{
    public class ActivityDatabase : MonoBehaviour
    {
        [Title("Database")]
        [AssetList]
        [SerializeField] private List<ActivitySO> _activities;

        #region Singleton
        // singleton reference
        private static ActivityDatabase _instance;
        public static ActivityDatabase Instance { get { return _instance; } }

        private void Awake()
        {
            // singleton pattern
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
        #endregion


        public string GetDisplayName(ActivityType activity)
        {
            return _activities.Find(x => x.Activity == activity).DisplayName;
        }

        public string GetButtonName(ActivityType activity)
        {
            ActivitySO activitySO = _activities.Find(x => x.Activity == activity);
            if (activitySO != null) { return activitySO.ButtonName; }
            return null;
        }
    }
}
