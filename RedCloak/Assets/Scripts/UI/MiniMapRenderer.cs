using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MiniMapRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField]
    private PolygonCollider2D[] tutorialCollider;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (tutorialCollider != null && tutorialCollider.Length > 0)
        {
            UpdateLine();
        }
    }

    private void UpdateLine()
    {
        int totalPoints = 0;
        foreach(PolygonCollider2D colider in tutorialCollider)
        {
            totalPoints += colider.points.Length + 1;
        }

        Vector3[] linePosition = new Vector3[totalPoints];
        int index = 0;

        foreach(PolygonCollider2D collider in tutorialCollider)
        {
            Vector2[] points = collider.points;

            for(int i = 0; i < points.Length; i++)
            {
                Vector3 worldPoint = collider.transform.TransformPoint(points[i]);
                linePosition[index++] = worldPoint;
            }

            linePosition[index++] = collider.transform.TransformPoint(points[0]);
        }

        lineRenderer.positionCount = linePosition.Length;
        lineRenderer.SetPositions(linePosition);
    }
}
