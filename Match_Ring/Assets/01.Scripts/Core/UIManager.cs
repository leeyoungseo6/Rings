using TMPro;
using UnityEngine;

public class UIManager
{
    public static UIManager Instance;
    
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _topScoreText;

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
        _score += 1;
        _scoreText.text = _score.ToString();

        if (_score > DataManager.Instance.data.TopScore)
        {
            DataManager.Instance.data.TopScore = _score;
            _topScoreText.text = _score.ToString();
        }
    }

    public void SetScore()
    {
        _score = 0;
        _scoreText.text = "0";
    }
}