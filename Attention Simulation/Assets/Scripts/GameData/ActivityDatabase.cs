using Emyra.FocusGame.Locations;
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


        public string GetDisplayName(ActivityName activity)
        {
            if (FindActivity(activity, out ActivitySO activitySO)) 
            { 
                return activitySO.DisplayName;
            }
            else { return null; }
        }

        public string GetButtonName(ActivityName activity)
        {
            if (FindActivity(activity, out ActivitySO activitySO))
            {
                return activitySO.ButtonName;
            }
            else { return null; }
        }

        /// <summary>
        /// Returns true if activity has a fixed duration
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public bool CheckDuration(ActivityName activity)
        {
            if (FindActivity(activity, out ActivitySO activitySO))
            {
                return activitySO.HasFixedDuration;
            }
            else { return false; }
        }

        private bool FindActivity(ActivityName activity, out ActivitySO activitySO)
        {
            activitySO = _activities.Find(x => x.Activity == activity);
            return (activitySO != null);
        }
    }
}
