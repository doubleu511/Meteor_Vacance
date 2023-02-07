using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypointTrailEffect : MonoBehaviour
{
    private static MapData MapData;

    private TrailRenderer trailRenderer;
    private ParticleSystem particle;

    private Vector2Int[] savedWaypoints;
    private int currentPlayIndex = 0;
    private bool isPlaying = false;

    [SerializeField] float moveSpeed = 5f;

    private void Awake()
    {
        MapData = FindObjectOfType<MapData>();
        trailRenderer = GetComponent<TrailRenderer>();
        particle = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        Init(new Vector2Int[4] { new Vector2Int(0, 0), new Vector2Int(0, 10), new Vector2Int(2, 10), new Vector2Int(2, 5) });
        PlayEffect();
    }

    public void Init(Vector2Int[] wayPoints)
    {
        savedWaypoints = wayPoints;
    }

    public void PlayEffect()
    {
        Vector3 targetPinPos = MapData.Position3D[savedWaypoints[currentPlayIndex].y, savedWaypoints[currentPlayIndex].x].position;
        transform.position = targetPinPos;
        trailRenderer.Clear();

        isPlaying = true;
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (savedWaypoints.Length > currentPlayIndex)
            {
                Vector3 targetPinPos = MapData.Position3D[savedWaypoints[currentPlayIndex].x, savedWaypoints[currentPlayIndex].y].position;
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
            }
        }
    }
}
