using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Emyra.FocusGame.EventChannel
{
    /// <summary>
    /// Class to combine two related Standard Events for organization and readability.
    /// First event broadcasts the "Request," Second event broadcasts the response/result 
    /// of that request.
    /// All EditorOnly fields start with "Editor_" to avoid confusion.
    /// </summary>
    public abstract class BaseRequestEventSO : ScriptableObject
    {
#if UNITY_EDITOR
        [Title("Info")]
        [LabelText("Description")][TextArea(3, 10)]
        [PropertyOrder(1)] public string Editor_Description;

        [Title("Event Data")]
        [LabelText(SdfIconType.InfoCircleFill, Text = "Event Responder")]
        [PropertyOrder(2)] public string Editor_EventResponder;

        [Title("Event Users")]
        [ListDrawerSettings(ShowIndexLabels = false)]
        [LabelText(SdfIconType.InfoCircleFill, Text = "Invoked By")]
        [PropertyOrder(3)] public List<string> Editor_InvokedBy;
        [ListDrawerSettings(ShowIndexLabels = false)]
        [LabelText(SdfIconType.InfoCircleFill, Text = "Results Listeners")]
        [PropertyOrder(4)] public List<string> Editor_ResultListeners;
#endif
    }

    /// <summary>
    /// Request Event class that allows a 1 arg response.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class VoidGenericRequestEventSO<T> : BaseRequestEventSO
    {
#if UNITY_EDITOR
        [LabelText(SdfIconType.InfoCircleFill, Text = "Invoke Result With")]
        [PropertyOrder(2.5f)] public string Editor_InvokeResultWith;
#endif

        public UnityAction OnRequestEvent;
        public UnityAction<T> OnResultEvent;

        public void RequestEvent()
        {
            OnRequestEvent?.Invoke();
        }
        public void SendResult(T result)
        {
            OnResultEvent?.Invoke(result);
        }
    }

    /// <summary>
    /// Request Event class that allows 1 arg in the request and 1 arg in the response.
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public abstract class GenericGenericRequestEventSO<T0, T1> : BaseRequestEventSO
    {
#if UNITY_EDITOR
        [LabelText(SdfIconType.InfoCircleFill, Text = "Invoke Request With")]
        [PropertyOrder(2.3f)] public string Editor_InvokeRequestWith;
        [LabelText(SdfIconType.InfoCircleFill, Text = "Invoke Result With")]
        [PropertyOrder(2.5f)] public string Editor_InvokeResultWith;
#endif

        public UnityAction<T0> OnRequestEvent;
        public UnityAction<T1> OnResultEvent;

        public void RequestEvent(T0 input)
        {
            OnRequestEvent?.Invoke(input);
        }
        public void SendResult(T1 result)
        {
            OnResultEvent?.Invoke(result);
        }
    }
}