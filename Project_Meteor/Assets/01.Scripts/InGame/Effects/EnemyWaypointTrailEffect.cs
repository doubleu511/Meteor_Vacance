using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypointTrailEffect : MonoBehaviour
{
    private TrailRenderer[] trailRenderers;
    private ParticleSystem particle;

    private int currentPlayIndex = 0;

    [SerializeField] float moveSpeed = 5f;

    private void Awake()
    {
        trailRenderers = GetComponentsInChildren<TrailRenderer>();
        particle = GetComponent<ParticleSystem>();
    }

    public void Init(WaveTime waveTime)
    {
        gameObject.SetActive(true);

        Vector2Int flipPin = waveTime.wayPointSO.GetFlipedPos(waveTime.wayPointSO.enemyWayPoints[currentPlayIndex].enemyWayPoint, waveTime.flipX, waveTime.flipY);
        Vector3 targetPinPos = GameManager.MapData.Position3D[flipPin.y, flipPin.x].position;
        transform.position = targetPinPos;

        foreach (TrailRenderer trail in trailRenderers)
        {
            trail.Clear();
        }

        particle.Play();
        StartCoroutine(MoveCoroutine(waveTime.wayPointSO, waveTime.flipX, waveTime.flipY));
    }

    private IEnumerator MoveCoroutine(WaypointSO wayPoint, bool flipX, bool flipY)
    {
        while (true)
        {
            if (wayPoint.enemyWayPoints.Length > currentPlayIndex)
            {
                Vector2Int flipPin = wayPoint.GetFlipedPos(wayPoint.enemyWayPoints[currentPlayIndex].enemyWayPoint, flipX, flipY);
                Vector3 targetPinPos = GameManager.MapData.Position3D[flipPin.y, flipPin.x].position;
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
                particle.Stop();
                Invoke("SetDisable", trailRenderers[0].time);
                break;
            }
            yield return null;
        }
    }

    private void SetDisable()
    {
        currentPlayIndex = 0;
        gameObject.SetActive(false);
    }
}
