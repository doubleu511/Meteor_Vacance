using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypointTrailEffect : MonoBehaviour
{
    private TrailRenderer[] trailRenderers;
    private ParticleSystem particle;

    private Vector2Int[] savedWaypoints;
    private int currentPlayIndex = 0;
    private bool isPlaying = false;

    [SerializeField] float moveSpeed = 5f;

    private void Awake()
    {
        trailRenderers = GetComponentsInChildren<TrailRenderer>();
        particle = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        Init(new Vector2Int[4] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 2), new Vector2Int(5, 2) });
    }

    public void Init(Vector2Int[] wayPoints)
    {
        savedWaypoints = wayPoints;
        Vector3 targetPinPos = GameManager.MapData.Position3D[savedWaypoints[currentPlayIndex].y, savedWaypoints[currentPlayIndex].x].position;
        transform.position = targetPinPos;

        foreach (TrailRenderer trail in trailRenderers)
        {
            trail.Clear();
        }

        particle.Play();
        isPlaying = true;
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (savedWaypoints.Length > currentPlayIndex)
            {
                Vector3 targetPinPos = GameManager.MapData.Position3D[savedWaypoints[currentPlayIndex].y, savedWaypoints[currentPlayIndex].x].position;
                Vector3 dir = targetPinPos - transform.position;

                if (dir.sqrMagnitude >= 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPinPos, moveSpeed * Time.deltaTime);
                }
                else
                {
                    currentPlayIndex++;
                }
            }
            else
            {
                isPlaying = false;
                particle.Stop();
                Invoke("SetDisable", trailRenderers[0].time);
            }
        }
    }

    private void SetDisable()
    {
        gameObject.SetActive(false);
    }
}
