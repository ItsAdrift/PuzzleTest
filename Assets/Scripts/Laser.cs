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

    bool active = true;

    bool obstructed;

    public void Start()
    {
        particles = Instantiate(particlesPrefab).GetComponent<ParticleSystem>();

        SetupEdgeCollider();
    }

    public void Update()
    {
        if (!active)
            return;

        lineRenderer.SetPosition(0, start.localPosition); // an issue i faced was things being out, caused by using position over localPosition
        RaycastHit2D hit = Physics2D.Raycast(start.position, direction, Vector3.Distance(start.position, end.position), laserObstruction);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<BreakableObject>() != null)
            {
                hit.collider.GetComponent<BreakableObject>()?.Damage(1);
            }

            lineRenderer.SetPosition(1, transform.InverseTransformPoint(hit.point));
            SetupEdgeCollider();
            obstructed = true;

            // Particles
            particles.transform.position = hit.point - (direction * 0.1f);
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
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.Collapse();
            player.Die();
        }
        else
        {
            //collision.GetComponent<BreakableObject>()?.Damage(1);
        }
    }

    public void SetActive(bool b)
    {
        active = b;

        lineRenderer.enabled = b;
        edgeCollider.enabled = b;
    }

    public void Toggle()
    {
        SetActive(!active);
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
