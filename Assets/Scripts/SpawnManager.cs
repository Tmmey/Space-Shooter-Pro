using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        //StartSpawning();
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        yield return null;

        while (!_stopSpawning)
        {
            var xPosition = Random.Range(-8f, 8f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(xPosition, 7, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSecondsRealtime(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            yield return new WaitForSecondsRealtime(Random.Range(3.0f, 7.0f));
            int randomPowerUp = Random.Range(0, 3);
            var xPosition = Random.Range(-8f, 8f);
            GameObject powerup = Instantiate(_powerups[randomPowerUp], new Vector3(xPosition, 7, 0), Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
