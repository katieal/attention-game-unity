using Emyra.FocusGame.Locations;
using Emyra.FocusGame.School;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Emyra.FocusGame.GameData
{
    public class SchoolSubjectDatabase : MonoBehaviour
    {
        [AssetList]
        [SerializeField] private List<SchoolSubjectSO> _subjectDatabase;

        #region Singleton
        //singleton reference
        private static SchoolSubjectDatabase _instance;
        public static SchoolSubjectDatabase Instance { get { return _instance; } }

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

        /// <summary>
        /// Returns a SubjectInstance for a given Id, or null if Id is not in database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SubjectInstance GetSubjectInstance(string id)
        {
            if (FindSubject(id, out SchoolSubjectSO subject))
            {
                return new SubjectInstance(subject);
            }
            return null;
        }

        /// <summary>
        /// Returns the SubjectType for a given Id, or SubjectType.None if Id 
        /// is not in database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SubjectType GetSubjectType(string id)
        {
            if (FindSubject(id, out SchoolSubjectSO subject))
            {
                return subject.SubjectType;
            }
            return SubjectType.None;
        }

        public string GetSubjectName(string id)
        {
            if (FindSubject(id, out SchoolSubjectSO subject))
            {
                return subject.SubjectName;
            }
            return string.Empty;
        }

        private bool FindSubject(string id, out SchoolSubjectSO subject)
        {
            subject = _subjectDatabase.Find(x => x.Id == id);
            Debug.Assert(subject != null, "Subject not found in database!!");
            return (subject != null);
        }
    }
}
