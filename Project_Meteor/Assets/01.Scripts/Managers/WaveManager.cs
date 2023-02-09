using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveTime
{
    public EnemyType enemyType = EnemyType.BUG01;
    public float enemySpawnTime = 0f;
    public Vector2Int[] enemyWayPoints;
}

public class WaveManager : MonoBehaviour
{
    [SerializeField] EnemyWaypointTrailEffect trailEffectPrefab;
    [SerializeField] EnemyBase[] enemyPrefabs;
    [SerializeField] List<WaveTime> waveTimes = new List<WaveTime>();

    private Dictionary<EnemyType, EnemyBase> enemyDic = new Dictionary<EnemyType, EnemyBase>();

    private bool isWaveStart = false;
    private float waveTimer = 0f;
    private int waveTimeIndex = 0;

    void Start()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemyDic[enemyPrefabs[i].enemyType] = enemyPrefabs[i];
            enemyPrefabs[i].CreatePool(enemyPrefabs[i]);
        }

        Global.Pool.CreatePool<EnemyWaypointTrailEffect>(trailEffectPrefab.gameObject, transform, 10);

        isWaveStart = true;
    }

    void Update()
    {
        if (isWaveStart)
        {
            if (waveTimeIndex < waveTimes.Count)
            {
                waveTimer += Time.deltaTime;

                if (waveTimes[waveTimeIndex].enemySpawnTime <= waveTimer)
                {
                    StartCoroutine(SpawnDelayCo(waveTimes[waveTimeIndex].enemyWayPoints, enemyDic[waveTimes[waveTimeIndex].enemyType]));
                    waveTimeIndex++;
                }
            }
        }
    }

    private IEnumerator SpawnDelayCo(Vector2Int[] wayPoints, EnemyBase prefab)
    {
        EnemyWaypointTrailEffect trailEffect = Global.Pool.GetItem<EnemyWaypointTrailEffect>();
        trailEffect.Init(wayPoints);
        yield return new WaitForSeconds(3);
        prefab.PoolInit(wayPoints);
    }
}
