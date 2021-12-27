using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject enemyPrefab;
    public int baseEnemyCount = 3;
    public int addEnemyCountPerPlatform = 1;
    public List<GameObject> enemies = new List<GameObject>();

    [Header("Platforms")]
    public GameObject platform;
    public GameObject currentPlatform;
    public float platformSize = 13;
    public bool newPlatformReady;
    private Vector3 _previousPos;
    private int _currentPlatformNumber = 1;

    private void Update()
    {
        if (enemies.Count < baseEnemyCount + addEnemyCountPerPlatform * _currentPlatformNumber)
            newPlatformReady = true;
        else
            newPlatformReady = false;
    }

    public void NewPlatform()
    {
        float newPos = Random.value;
        Vector3 newPosVector = Vector3.zero;

        if (newPos >= 0 && newPos < .25f)
            newPosVector = new Vector3(platformSize, 0, 0);
        else if (newPos >= .25f && newPos < .5f)
            newPosVector = new Vector3(-platformSize, 0, 0);
        else if (newPos >= .5f && newPos < .75f)
            newPosVector = new Vector3(0, 0, platformSize);
        else
            newPosVector = new Vector3(0, 0, -platformSize);

        if (newPosVector == _previousPos)
        {
            NewPlatform();
            return;
        }

        currentPlatform = Instantiate(platform, currentPlatform.transform.position + newPosVector, Quaternion.identity, transform);

        List<GameObject> currectSpawnPoints = currentPlatform.GetComponent<Platform>().enemySpawnPoints;

        for (int i = 0; i < baseEnemyCount + addEnemyCountPerPlatform * _currentPlatformNumber; i++)
        {
            int randomSpawnPoint = Random.Range(0, currectSpawnPoints.Count);
            GameObject currentPoint = currectSpawnPoints[randomSpawnPoint];
            currectSpawnPoints.RemoveAt(randomSpawnPoint);

            GameObject enemy = Instantiate(enemyPrefab, currentPoint.transform.position, currentPoint.transform.rotation, transform);
            enemies.Add(enemy);
        }

        _currentPlatformNumber++;
        _previousPos = newPosVector;
    }
}