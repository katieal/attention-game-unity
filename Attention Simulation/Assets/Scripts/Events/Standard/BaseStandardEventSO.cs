using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Emyra.Simulator.EventChannel
{
    /// <summary>
    /// Standard Event Channel. Can be invoked with 0, 1, or 2 args.
    /// All EditorOnly fields start with "Editor_" to avoid confusion.
    /// </summary>
    public abstract class BaseStandardEventSO : ScriptableObject
    {
#if UNITY_EDITOR
        [Title("Event Info")]
        [LabelText("Description")][TextArea(3, 10)]
        [PropertyOrder(1)] public string Editor_Description;

        [Title("Event Users")]
        [ListDrawerSettings(ShowIndexLabels = false)]
        [LabelText(SdfIconType.InfoCircleFill, Text = "Invoked By")]
        [PropertyOrder(2)] public List<string> Editor_InvokedBy;
        [ListDrawerSettings(ShowIndexLabels = false)]
        [LabelText(SdfIconType.InfoCircleFill, Text = "Event Listeners")]
        [PropertyOrder(3)] public List<string> Editor_Listeners;
#endif
    }

    /// <summary>
    /// Standard Event that allows sending 1 event arg.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericStandardEventSO<T> : BaseStandardEventSO
    {
#if UNITY_EDITOR
        [LabelText(SdfIconType.InfoCircleFill, Text = "Invoke With")]
        [PropertyOrder(1.5f)] public string Editor_InvokeWith;
#endif

        public UnityAction<T> OnInvokeEvent;

        public void InvokeEvent(T data)
        {
            OnInvokeEvent?.Invoke(data);
        }
    }

    /// <summary>
    /// Standard Event that allows sending 2 event args.
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public abstract class GenericGenericStandardEventSO<T0, T1> : BaseStandardEventSO
    {
#if UNITY_EDITOR
        [LabelText(SdfIconType.InfoCircleFill, Text = "Invoke With")]
        [PropertyOrder(1.5f)] public string Editor_InvokeWith;
#endif

        public UnityAction<T0, T1> OnInvokeEvent;

        public void InvokeEvent(T0 arg0, T1 arg1)
        {
            OnInvokeEvent?.Invoke(arg0, arg1);
        }
    }
}