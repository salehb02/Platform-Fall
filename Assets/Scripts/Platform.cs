using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Platform : MonoBehaviour
{
    public TextMeshPro timer;
    public float time = 30;
    public float timerFadeSpeed = 1;
    public List<SpawnPointTrigger> enemySpawnPoints;

    public int currectPlatformNumber;
    private GameManager gameManager;

    private float _currentTime;
    private Color _color;
    private bool _createNewPlatform;
    private bool _timeout;
    public AudioSource fallSFX;
    public AudioSource timerSFX;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        _currentTime = time;
        _color = timer.color;

        StartCoroutine(TickTack());
        Invoke("SpawnEnemies", 0.2f);
    }

    private void Update()
    {
        if (gameManager.timerOn)
        {
            _currentTime -= Time.deltaTime;
            _currentTime = Mathf.Clamp(_currentTime, 0, time);

            timerSFX.pitch = Mathf.Lerp(0.25f, 0.75f, Mathf.Abs(Mathf.Sin(Time.time / 2)));

            if (_currentTime > 0) timer.text = $"{(int)_currentTime}s";

            if (_currentTime < 5)
            {
                NearlyFall();
            }

            if (_currentTime <= 0)
            {
                _color = Color.Lerp(_color, new Color(_color.r, _color.g, _color.b, 0), Time.deltaTime * timerFadeSpeed);
                timer.color = _color;

                Fall();
            }
        }
    }
    
    private void NearlyFall()
    {
        if (_createNewPlatform)
            return;

        _createNewPlatform = true;

        timerSFX.volume = 0;

        if(gameManager.newPlatformReady)
            FindObjectOfType<GameManager>().NewPlatform();
    }

    private void Fall()
    {
        if (_timeout)
            return;

        _timeout = true;

        fallSFX.Play();
        gameObject.AddComponent<Rigidbody>();
        Destroy(gameObject, 10);
    }

    private IEnumerator TickTack()
    {
        timerSFX.Play();
        yield return new WaitForSeconds(1);
        StartCoroutine(TickTack());
    }

    private void SpawnEnemies()
    {
        List<SpawnPointTrigger> currectSpawnPoints = enemySpawnPoints;

        for (int i = 0; i < gameManager.baseEnemyCount + gameManager.addEnemyCountPerPlatform * currectPlatformNumber; i++)
        {
            int randomSpawnPoint = Random.Range(0, currectSpawnPoints.Count);

            if (currectSpawnPoints[randomSpawnPoint].full == false)
            {
                GameObject currentPoint = currectSpawnPoints[randomSpawnPoint].gameObject;
                currectSpawnPoints.RemoveAt(randomSpawnPoint);

                GameObject enemy = Instantiate(gameManager.enemyPrefab, currentPoint.transform.position, currentPoint.transform.rotation, transform);
                gameManager.enemies.Add(enemy);
            }
        }

        foreach (var enemy in enemySpawnPoints)
            enemy.gameObject.SetActive(false);
    }
}