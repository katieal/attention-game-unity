using Emyra.FocusGame.EventChannel;
using Emyra.FocusGame.Locations;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;
using RoomInfo = Emyra.FocusGame.Locations.RoomInfo;

namespace Emyra.FocusGame.UI
{
    public class BackgroundUI : MonoBehaviour
    {
        [Title("Components")]
        [SerializeField] private Image _background;
        [SerializeField] private SpriteLibrary _spriteLibrary;

        [TitleGroup("Events")]
        [FoldoutGroup("Events/Listening to Events")]
        [SerializeField] private LocationInfoEventSO _locationChangedEvent;

        private void OnEnable()
        {
            _locationChangedEvent.OnInvokeEvent += OnLocationChanged;
        }
        private void OnDisable()
        {
            _locationChangedEvent.OnInvokeEvent -= OnLocationChanged;
        }

        private void OnLocationChanged(RoomInfo info)
        {
            // assign bg image if possible
            if (_spriteLibrary.spriteLibraryAsset.GetCategoryNames().Contains(info.Place.ToString()))
            {
                string label;
                if (info.Room == RoomName.Classroom) { label = info.SubjectName; }
                else { label = info.Room.ToString(); }

                if (_spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(info.Place.ToString()).Contains(label))
                {
                    _background.sprite = _spriteLibrary.GetSprite(info.Place.ToString(), label);
                }

            }
        }
    }
}
