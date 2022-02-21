using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This component spawns a bundle of logs when the player chops up a larger log. 
public class LogBundleSpawner : MonoBehaviour
{
    [Header("Small log prefab variations")]
    [SerializeField] List<GameObject> _smallLogPrefabVariations;

    private readonly float _spacing = .2f;

    private static LogBundleSpawner _instance;

    private void Awake()
    {
        _instance = this;
    }


    public static LogBundleSpawner GetInstance()
    {
        return _instance;
    }

    //The log the player splits apart calls this to spawn the smaller log pieces. Quantity represents the size of the log that was just split apart. 
    public void SpawnLogs(int numOfLogsToSpawn, Transform center)
    {
       List<Vector3> spawnPoints = _ComputeLogSpawnPositions(numOfLogsToSpawn);
        foreach (var sp in spawnPoints)
        {
            GameObject logPrefabToSpawn = _smallLogPrefabVariations[Random.Range(0, _smallLogPrefabVariations.Count - 1)];
            GameObject spawnedLog = Instantiate(logPrefabToSpawn, center.position + sp, center.rotation);
            spawnedLog.transform.parent = null;
        }
    }


    //Gernerate the list of spawn points for the logs (up to 9 possible)
    public List<Vector3> _ComputeLogSpawnPositions(int numOfLogsToSpawn)
    {
        List<Vector3> logSpawnPoints = new List<Vector3>();
        logSpawnPoints.Add(new Vector3(0, 0, 0));
        if (numOfLogsToSpawn == 1) return logSpawnPoints;
        logSpawnPoints.Add(new Vector3(_spacing, 0, 0));
        if (numOfLogsToSpawn == 2) return logSpawnPoints;
        logSpawnPoints.Add(new Vector3(-_spacing, 0,0));
        if (numOfLogsToSpawn == 3) return logSpawnPoints;
        logSpawnPoints.Add(new Vector3(0, 0, _spacing));
        if (numOfLogsToSpawn == 4) return logSpawnPoints;
        logSpawnPoints.Add(new Vector3(0, 0, -_spacing));
        if (numOfLogsToSpawn == 5) return logSpawnPoints;

        if (numOfLogsToSpawn == 6) return logSpawnPoints;
        logSpawnPoints.Add(new Vector3(_spacing, 0, _spacing));
        if (numOfLogsToSpawn == 7) return logSpawnPoints;
        logSpawnPoints.Add(new Vector3(_spacing, 0, -_spacing));

        if (numOfLogsToSpawn == 8) return logSpawnPoints;
        logSpawnPoints.Add(new Vector3(-_spacing, 0, _spacing));
        if (numOfLogsToSpawn == 9) return logSpawnPoints;
        logSpawnPoints.Add(new Vector3(-_spacing, 0, -_spacing));

        return logSpawnPoints;
    }
}
