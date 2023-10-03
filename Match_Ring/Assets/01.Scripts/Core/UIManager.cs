using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _topScoreText;
    private GameObject _restartButton;

    private int _score = 0;
    private int _topScore;

    public UIManager(Transform canvasTrm)
    {
        _scoreText = canvasTrm.Find("Canvas_Dynamic/Scores/ScoreText").GetComponent<TextMeshProUGUI>();
        _topScoreText = canvasTrm.Find("Canvas_Dynamic/Scores/TopScoreText").GetComponent<TextMeshProUGUI>();
        _restartButton = canvasTrm.Find("Canvas_Static/RestartButtonIMG").gameObject;
        _topScoreText.text = DataManager.Instance.data.TopScore.ToString();

        GameManager.Instance.OnGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= OnGameOver;
    }

    public void SetScore()
    {
        _score += 1;
        _scoreText.text = _score.ToString();

        if (_score > DataManager.Instance.data.TopScore)
        {
            DataManager.Instance.data.TopScore = _score;
            _topScoreText.text = _score.ToString();
        }
    }

    private void OnGameOver()
    {
        _score = 0;
        _restartButton.SetActive(true);
    }
}