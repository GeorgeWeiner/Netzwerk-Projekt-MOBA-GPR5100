using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace S_Extensions
{
    public static class ExtensionMethods
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
                Debug.LogError("Please add a line-renderer");
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
        public static void DrawRectangle(this GameObject center,LineRenderer lineRenderer,Vector3 size)
        {
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = 0;

            Vector3[] edgePoints = new Vector3[5];
            Vector3 centerPos = center.transform.position;

            float width = size.x;
            float height = size.z;
            float angle = center.transform.rotation.y;

            Vector3 topRightCorner = new(0, centerPos.y, 0);
            Vector3 topLeftCorner = new(0, centerPos.y, 0);
            Vector3 bottomRightCorner = new(0, centerPos.y, 0);
            Vector3 bottomLeftCorner = new(0, centerPos.y, 0);

            topRightCorner.x = centerPos.x + ((width / 2) * Mathf.Cos(angle)) - ((height / 2) * Mathf.Sin(angle));
            topRightCorner.z = centerPos.z + ((width / 2) * Mathf.Sin(angle)) + ((height / 2) * Mathf.Cos(angle));

            topLeftCorner.x = centerPos.x - ((width / 2) * Mathf.Cos(angle)) - ((height / 2) * Mathf.Sin(angle));
            topLeftCorner.z = centerPos.z - ((width / 2) * Mathf.Sin(angle)) + ((height / 2) * Mathf.Cos(angle));

            bottomLeftCorner.x = centerPos.x - ((width / 2) * Mathf.Cos(angle)) + ((height / 2) * Mathf.Sin(angle));
            bottomLeftCorner.z = centerPos.z - ((width / 2) * Mathf.Sin(angle)) - ((height / 2) * Mathf.Cos(angle));
                                                
            bottomRightCorner.x = centerPos.x + ((width / 2) * Mathf.Cos(angle)) + ((height / 2) * Mathf.Sin(angle));
            bottomRightCorner.z = centerPos.z + ((width / 2) * Mathf.Sin(angle)) - ((height / 2) * Mathf.Cos(angle));

            edgePoints[0] = topRightCorner;
            edgePoints[1] = topLeftCorner;
            edgePoints[2] = bottomLeftCorner;
            edgePoints[3] = bottomRightCorner;
            edgePoints[4] = topRightCorner;

            lineRenderer.positionCount = 5;
            lineRenderer.SetPositions(edgePoints);
        }
        /// <summary>
        /// A method for adding methods to the EventTrigger Component
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public static void AddEventTrigger(this GameObject obj, EventTriggerType type, UnityAction<BaseEventData> data)
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
        /// <param name="xSpaceBetweenFields"></param>
        /// <param name="ySpaceBetweenFields"></param>
        /// <param name="numberOfFields"></param>
        /// <param name="numberOfSlots"></param>
        /// <returns></returns>
        public static Vector3[] CreateGridUiLayout(int xSpaceBetweenFields, int ySpaceBetweenFields, int numberOfFields, int numberOfSlots)
        {
            Vector3[] gridPositions = new Vector3[numberOfSlots];

            for (int i = 0; i < gridPositions.Length; i++)
            {
                Vector3 uiPos = new Vector3(xSpaceBetweenFields * (i % numberOfFields), -ySpaceBetweenFields * (i / numberOfFields), 0);
                gridPositions[i] = uiPos;
            }

            return gridPositions;
        }
    }
}
