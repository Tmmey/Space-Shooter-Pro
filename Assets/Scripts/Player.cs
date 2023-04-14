using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotLaserPrefab;
    [SerializeField] private float _offset = 0.8f;
    [SerializeField] private float _fireRate = 0.25f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private SpriteRenderer _shieldSprite;
    [SerializeField] private int _score;
    [SerializeField] private Transform _leftEngineDamage;
    [SerializeField] private Transform _rightEngineDamage;
    [SerializeField] private AudioClip _laserAudioClip;

    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;
    private float _canFire = -1f;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        //_spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _spawnManager = FindObjectOfType<SpawnManager>();
        //_uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _uiManager = FindObjectOfType<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("SpanManager is null!");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UIManager is null!");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Player audioSource is null!");
        }
        else
        {
            _audioSource.clip = _laserAudioClip;
        }

        // _shieldSprite = (GetComponentsInChildren<SpriteRenderer>()).FirstOrDefault(x => x.tag == "Shield");

        // if (_shieldSprite != null)
        // {
        //     Debug.Log("Found shieldsprite");
        // }
        // else
        // {
        //     Debug.Log("Not found shieldsprite");
        // }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FireLaser();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_isSpeedActive)
        {
            transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11f)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11f)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;

            if (_isTripleShotActive)
            {
                Instantiate(_tripleShotLaserPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, _offset, 0), Quaternion.identity);
            }

            _audioSource.Play();
        }
    }



    public void Damage(int amount)
    {
        if (this._isShieldActive)
        {
            this._shieldSprite.enabled = false;
            this._isShieldActive = false;
            return;
        }

        this._lives -= amount;
        VisualizeDamage();

        _uiManager.UpdateLives(this._lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            //_uiManager.GameOverSequence();
        }

        Debug.Log("Health:" + _lives);
    }

    private void VisualizeDamage()
    {
        if (_lives == 2)
        {
            var engineIndex = Random.Range(1, 3);
            if (engineIndex == 1)
            {
                _leftEngineDamage.gameObject.SetActive(true);
            }
            else
            {
                _rightEngineDamage.gameObject.SetActive(true);
            }
        }

        if (_lives == 1)
        {
            _leftEngineDamage.gameObject.SetActive(true);
            _rightEngineDamage.gameObject.SetActive(true);
        }
    }

    public void TripleShotActive()
    {
        this._isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSecondsRealtime(5f);
        this._isTripleShotActive = false;
    }

    public void SpeedActive()
    {
        this._isSpeedActive = true;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSecondsRealtime(5f);
        this._isSpeedActive = false;
    }

    public void ShieldActive()
    {
        this._shieldSprite.enabled = true;
        this._isShieldActive = true;
        //StartCoroutine(ShieldPowerDownRoutine());
    }

    // IEnumerator ShieldPowerDownRoutine()
    // {
    //     yield return new WaitForSecondsRealtime(5f);
    //     this._isShieldActive = false;
    // }

    public void AddScore(int amount)
    {
        this._score += amount;
        _uiManager.UpdateScoreText(this._score);
    }
}
