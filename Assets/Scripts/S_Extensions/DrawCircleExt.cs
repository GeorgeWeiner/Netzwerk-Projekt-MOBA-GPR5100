using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

static public class ExtensionMehtods
{
    /// <summary>
    /// A method for drawing a gizmos circle with a line renderer
    /// </summary>
    /// <param name="container"></param>
    /// <param name="radius"></param>
    /// <param name="lineWidth"></param>
    public static void DrawCircle(this GameObject container, float radius, float lineWidth)
    {
        if (!container.TryGetComponent<LineRenderer>(out LineRenderer lineRenderer))
        {
            Debug.LogError("Pls add a linerenderer");
            return;
        }

        var segments = 360;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = segments + 1;

        var pointCount = segments + 1;
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);

            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        lineRenderer.SetPositions(points);
    }
    /// <summary>
    /// A method for adding methods to the EventTrigger Component
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type"></param>
    /// <param name="data"></param>
    static public void AddEventTrigger(this GameObject obj, EventTriggerType type, UnityAction<BaseEventData> data)
    {
        var trigger = obj.GetComponentInChildren<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(data);
        trigger.triggers.Add(eventTrigger);
    }
    /// <summary>
    /// Creates a grid layout for something like a inventory
    /// </summary>
    /// <param name="xSpaceBewtweenFields"></param>
    /// <param name="ySpaceBetwenFields"></param>
    /// <param name="numberOfFields"></param>
    /// <param name="numberOfSlots"></param>
    /// <returns></returns>
    static public Vector3[] CreateGridUiLayout(int xSpaceBewtweenFields, int ySpaceBetwenFields, int numberOfFields, int numberOfSlots)
    {
        Vector3[] gridPositions = new Vector3[numberOfSlots];

        for (int i = 0; i < gridPositions.Length; i++)
        {
            Vector3 uiPos = new Vector3(xSpaceBewtweenFields * (i % numberOfFields), -ySpaceBetwenFields * (i / numberOfFields), 0);
            gridPositions[i] = uiPos;
        }

        return gridPositions;
    }
}
