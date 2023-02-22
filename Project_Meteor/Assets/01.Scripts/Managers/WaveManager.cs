using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveTime
{
    public EnemyType enemyType = EnemyType.BUG01;
    public float enemySpawnTime = 0f;
    public WaypointSO wayPointSO;
    public Vector2 wayPointOffset;
    public bool flipX = false;
    public bool flipY = false;
}

public class WaveManager : MonoBehaviour
{
    [SerializeField] EnemyWaypointTrailEffect trailEffectPrefab;
    [SerializeField] EnemyBase[] enemyPrefabs;
    [SerializeField] EnemyInfoSO[] enemyInfo;
    [SerializeField] List<WaveRushSO> waveRushes = new List<WaveRushSO>();

    private Dictionary<EnemyType, EnemyBase> enemyDic = new Dictionary<EnemyType, EnemyBase>();
    private Dictionary<EnemyType, int> enemySpawnCountDic = new Dictionary<EnemyType, int>();
    private Dictionary<EnemyType, EnemyInfoSO> enemyInfoDic = new Dictionary<EnemyType, EnemyInfoSO>();

    private bool isWaveStart = false;
    private float waveTimer = 0f;
    private int waveRushIndex = 0;
    private int waveTimeIndex = 0;
    public int TotalEnemyCount { get; private set; } = 0;

    void Start()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemyDic[enemyPrefabs[i].enemyType] = enemyPrefabs[i];
            enemyPrefabs[i].CreatePool(enemyPrefabs[i]);
            enemySpawnCountDic[enemyPrefabs[i].enemyType] = 0;
        }

        for (int i = 0; i < enemyInfo.Length; i++)
        {
            enemyInfoDic[enemyInfo[i].enemyType] = enemyInfo[i];
        }

        for (int i = 0; i < waveRushes.Count; i++)
        {
            TotalEnemyCount += waveRushes[i].waveTimes.Length;
        }

        Global.Pool.CreatePool<EnemyWaypointTrailEffect>(trailEffectPrefab.gameObject, transform, 10);

        isWaveStart = true;
        print(TotalEnemyCount);
    }

    void Update()
    {
        if (isWaveStart)
        {
            waveTimer += Time.deltaTime;
            if (waveRushIndex < waveRushes.Count)
            {

                if (waveTimeIndex < waveRushes[waveRushIndex].waveTimes.Length)
                {
                    if (waveRushes[waveRushIndex].waveTimes[waveTimeIndex].enemySpawnTime <= waveTimer)
                    {
                        StartCoroutine(SpawnDelayCo(
                            waveRushes[waveRushIndex].waveTimes[waveTimeIndex],
                            waveRushes[waveRushIndex].waveTimes[waveTimeIndex].enemyType,
                            enemyDic[waveRushes[waveRushIndex].waveTimes[waveTimeIndex].enemyType]));
                        waveTimeIndex++;
                    }
                }
                else
                {
                    waveRushIndex++;
                    waveTimeIndex = 0;
                    waveTimer = 0;
                }
            }
            else
            {
                isWaveStart = false;
            }
        }
    }

    public IEnumerator SpawnDelayCo(WaveTime waveTime, EnemyType enemyType, EnemyBase enemyPrefab)
    {
        if (enemyType != EnemyType.BUGBOSS)
        {
            EnemyWaypointTrailEffect trailEffect = Global.Pool.GetItem<EnemyWaypointTrailEffect>();
            trailEffect.Init(waveTime);
        }
        yield return new WaitForSeconds(3);
        enemyPrefab.PoolInit(waveTime);

        if (enemySpawnCountDic[enemyType] == 0)
        {
            InGameUI.UI.EnemyInfo.AddInfoQueue(enemyInfoDic[enemyType]);
        }
        enemySpawnCountDic[enemyType]++;
    }
}
