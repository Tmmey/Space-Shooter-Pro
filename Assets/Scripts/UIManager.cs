using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Image _livesImg;
    private SceneManager _sceneManager;
    //[SerializeField] private Text _restartText;

    // Start is called before the first frame update
    void Start()
    {
        //_livesSprites
        _scoreText.text = "Score: 0";
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _sceneManager = FindObjectOfType<SceneManager>();

        if (_sceneManager == null)
        {
            Debug.LogError("SceneManager is null!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = $"Score: {score}";
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void GameOverSequence()
    {
        _sceneManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
