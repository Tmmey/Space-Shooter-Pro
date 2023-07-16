using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 4f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _offset = 0.8f;
    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;
    private float _canFire = -1;
    private float _fireRate = 3.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        
        if (_player == null)
        {
            Debug.LogError("Player is null!");
        }

        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator is null!");
        }

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on enemy is null!");
        }

        //StartCoroutine(FireCr());
    }

    // Update is called once per frame
    void Update()
    {
        if (_canFire < Time.time)
        {
            _fireRate = Random.Range(3.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0, _offset, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            lasers[0].AssignEnemyLaser();
            lasers[1].AssignEnemyLaser();
        }

        CalculateMovement();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4f)
        {
            var xPosition = Random.Range(-8f, 8f);
            transform.position = new Vector3(xPosition, 7, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit:" + other.transform.name);

        if (other.tag == "Player")
        {
            var player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage(1);
                _animator.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _audioSource.Play();
                Destroy(gameObject, 2.8f);
            }
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
        }
    }

    private IEnumerator FireCr()
    {
        yield return new WaitForSecondsRealtime(Random.Range(1.5f, 5.0f));
        Instantiate(_laserPrefab, transform.position + new Vector3(0, _offset, 0), Quaternion.identity);
    }
}
