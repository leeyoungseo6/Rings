using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _topScoreText;

    public int Score { get; private set; }

    public void Init(Transform canvasTrm)
    {
        _scoreText = canvasTrm.Find("Scores/ScoreText").GetComponent<TextMeshProUGUI>();
        _topScoreText = canvasTrm.Find("Scores/TopScoreText").GetComponent<TextMeshProUGUI>();
        _topScoreText.text = DataManager.Instance.Data.TopScore.ToString();
    }

    public void AddScore()
    {
        Score += 1;
        _scoreText.text = Score.ToString();
        StartCoroutine(DOScaleScoreText(1.44f, 0.25f));
        GameManager.Instance.Difficulty -= 0.2f;

        if (Score <= DataManager.Instance.Data.TopScore) return;
        DataManager.Instance.Data.TopScore = Score;
        _topScoreText.text = Score.ToString();
    }

    public void SetScore()
    {
        Score = 0;
        _scoreText.text = "0";
    }

    private IEnumerator DOScaleScoreText(float value, float time)
    {
        Vector3 startValue = Vector3.one;
        Vector3 endValue = new Vector3(value, value, value);
        float currentTime = 0, percent = 0;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / (time / 2);
            _scoreText.rectTransform.localScale = Vector3.Lerp(startValue, endValue, 1 - Mathf.Pow(1 - percent, 3));
            yield return null;
        }

        currentTime = percent = 0;
        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / (time / 2);
            _scoreText.rectTransform.localScale = Vector3.Lerp(endValue, startValue, -(Mathf.Cos(Mathf.PI * percent) - 1) / 2);
            yield return null;
        }

        _scoreText.rectTransform.localScale = startValue;
    }
}