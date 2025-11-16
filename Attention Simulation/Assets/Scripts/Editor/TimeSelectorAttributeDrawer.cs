using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace Emyra.Simulator.GameData
{
    public class TimeSelectorAttributeDrawer : OdinAttributeDrawer<TimeSelectorAttribute, int>
    {
        static readonly int[] hoursArray = Enumerable.Range(0, 24).ToArray();
        static readonly string[] hoursStrings = hoursArray.Select(i => string.Format("{0:00}", i)).ToArray();
        static readonly int[] minutesArray = Enumerable.Range(0, 12).Select(x => x * 5).ToArray();

        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            int value = this.ValueEntry.SmartValue;

            // property label
            if (label != null)
            {
                rect = EditorGUI.PrefixLabel(rect, label);
            }

            SirenixEditorFields.IntField(rect.AlignLeft(rect.width * 0.3f), value);

            //format is: totalMinutes | (rect1)HH hoursDropdown MM minutesDropdown
            Rect rect1 = rect.AlignRight(rect.width * 0.65f);

            // hours and minutes dropdwon
            GUIHelper.PushLabelWidth(30);
            EditorGUI.BeginChangeCheck();
            int hours = SirenixEditorFields.Dropdown<int>(rect1.AlignLeft(rect1.width * 0.45f), "HH", (value / 60), hoursArray, hoursStrings);
            int minutes = SirenixEditorFields.Dropdown<int>(rect1.AlignRight(rect1.width * 0.45f), new GUIContent("MM"), (value % 60), minutesArray);

            if (EditorGUI.EndChangeCheck())
            {
                this.ValueEntry.SmartValue = (hours * 60) + minutes;
            }
            GUIHelper.PopLabelWidth();
        }
    }
}
