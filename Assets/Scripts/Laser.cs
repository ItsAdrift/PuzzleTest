using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] GameObject particlesPrefab;
    ParticleSystem particles;

    [SerializeField] Transform start;
    [SerializeField] Transform end;

    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] EdgeCollider2D edgeCollider;

    [SerializeField] LayerMask laserObstruction;
    [SerializeField] Vector2 direction;

    bool active;

    bool obstructed;

    public void Start()
    {
        particles = Instantiate(particlesPrefab).GetComponent<ParticleSystem>();

        SetupEdgeCollider();
    }

    public void Update()
    {
        lineRenderer.SetPosition(0, start.localPosition); // an issue i faced was things being out, caused by using position over localPosition
        RaycastHit2D hit = Physics2D.Raycast(start.position, direction, Vector3.Distance(start.position, end.position), laserObstruction);
        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, transform.InverseTransformPoint(hit.point));
            SetupEdgeCollider();
            obstructed = true;

            // Particles
            particles.transform.position = hit.point;
            particles.Play();
        } else
        {
            lineRenderer.SetPosition(1, end.localPosition);
            if (obstructed) // make sure we don't end up setting edge collider points every frame
            {
                particles.Stop();
                SetupEdgeCollider();
                obstructed = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
            Debug.Log("ZZaapp");
    }

    public void SetActive(bool b)
    {
        active = b;

        lineRenderer.enabled = b;
    }

    public void SetupEdgeCollider()
    {
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);

        edgeCollider.points = ConvertArray(positions);
    }

    Vector2[] ConvertArray(Vector3[] v3)
    {
        Vector2[] v2 = new Vector2[v3.Length];
        for (int i = 0; i < v3.Length; i++)
        {
            Vector3 tempV3 = v3[i];
            v2[i] = new Vector2(tempV3.x, tempV3.y);
        }
        return v2;
    }

}
