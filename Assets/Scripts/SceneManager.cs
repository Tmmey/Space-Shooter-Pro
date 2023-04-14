using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1); //current game scene
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
