using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Effects/CircleRenderer")]
[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class CircleRenderer : MonoBehaviour {

    LineRenderer line;
    EdgeCollider2D col;

    [SerializeField]
    [Tooltip("The more points the greater the definiton of the circle")]
    [Range(2, 1000)]
    private int m_points = 50;

    [SerializeField]
    [Tooltip("Radius of the circle")]
    private float m_radius = 5;

    [SerializeField]
    [Tooltip("Do you want an open circle? Set this value to the amount of degrees you require for your opening")]
    [Range(0,359)]
    private float m_opening = 0;

    [SerializeField]
    [Tooltip("Thickness of circle")]
    [Range(0.01f,1f)]
    private float m_thickness = 1;

    [SerializeField]
    [Tooltip("Do you want to add an Edge Collider to the circle?")]
    private bool m_collider = true;

    private float degrees;

    private float prev_radius;
    private float prev_points;
    private float prev_opening;
    private bool prev_collider;
    private float prev_thickness;
    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (line == null) line = GetComponent<LineRenderer>();

        line.useWorldSpace = false;

        if (prev_radius != m_radius || prev_points != m_points || prev_opening != m_opening || prev_collider != m_collider)
        {
            CreatePoints();
            prev_points = m_points;
            prev_collider = m_collider;
            prev_opening = m_opening;
            prev_radius = m_radius;

        }

        if (m_collider && (( col.edgeRadius != m_thickness/2) || (line.startWidth != m_thickness) || prev_thickness != m_thickness))
        {
            UpdateThickness();
            prev_thickness = m_thickness;
        }

        if(!m_collider && gameObject.GetComponent<EdgeCollider2D>() != null)
        {
            DestroyImmediate(gameObject.GetComponent<EdgeCollider2D>());
        }
	}

    [ContextMenu("Update Circle")]
    void CreatePoints()
    {
        if(line == null) line = GetComponent<LineRenderer>();

        if (col == null && m_collider)
        {
            if(gameObject.GetComponent<EdgeCollider2D>() != null) col = GetComponent<EdgeCollider2D>();
            else col = gameObject.AddComponent<EdgeCollider2D>();

            col.edgeRadius = m_thickness/2;
        }

        line.positionCount = m_points+1;
        degrees = 360f - m_opening;

        float x;
        float y;
        float z = 0f;
        Vector2[] points = new Vector2[m_points+1];

        float angle = 360f;

        for (int i = 0; i < (m_points +1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * m_radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * m_radius;

            line.SetPosition(i, new Vector3(x, y, z));
            points[i] = new Vector2(x, y);
            angle -= ( degrees/ m_points);
        }

        if (m_opening == 0)
        {       
            line.SetPosition(line.positionCount - 1, line.GetPosition(0));
            points[points.Length - 1] = points[0];
        }


        if(m_collider)col.points = points;
    }

    void UpdateThickness()
    {
        line.startWidth = m_thickness;
        col.edgeRadius = m_thickness/ 2;
        CreatePoints();
    }
   
}
