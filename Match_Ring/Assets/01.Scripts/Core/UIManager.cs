using TMPro;
using UnityEngine;

public class UIManager
{
    public static UIManager Instance;
    
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _topScoreText;

    public int Score { get => _score; set => _score = value; }
    private int _score = 0;
    private int _topScore;

    public UIManager(Transform canvasTrm)
    {
        _scoreText = canvasTrm.Find("Canvas_Dynamic/Scores/ScoreText").GetComponent<TextMeshProUGUI>();
        _topScoreText = canvasTrm.Find("Canvas_Dynamic/Scores/TopScoreText").GetComponent<TextMeshProUGUI>();
        _topScoreText.text = DataManager.Instance.data.TopScore.ToString();
    }

    public void AddScore()
    {
        Score += 1;
        _scoreText.text = Score.ToString();

        if (Score > DataManager.Instance.data.TopScore)
        {
            DataManager.Instance.data.TopScore = Score;
            _topScoreText.text = Score.ToString();
        }
    }

    public void SetScore()
    {
        _score = 0;
        _scoreText.text = "0";
    }
}