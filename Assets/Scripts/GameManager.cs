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
    public bool timerOn = true;

    public static string FLOOR_TAG = "Floor";
    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (enemies.Count < (baseEnemyCount + addEnemyCountPerPlatform * _currentPlatformNumber) / 2)
            newPlatformReady = true;
        else
            newPlatformReady = false;

        if(_player.dead)
            timerOn = false;
    }

    public void NewPlatform()
    {
        /// Spawn Platform
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
        currentPlatform.GetComponent<Platform>().currectPlatformNumber = _currentPlatformNumber;

        _currentPlatformNumber++;
        _previousPos = newPosVector;
    }
}