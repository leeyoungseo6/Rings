using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CircleController : MonoBehaviour
{
    private Camera _mainCam;

    [SerializeField] private Transform _ringTrm;
    private Material _material;
    private readonly int _fillAmountHash = Shader.PropertyToID("_FillAmount");
    private readonly int _ringColorHash = Shader.PropertyToID("_Color1");
    private Color _ringColor;
    private float _ringScale = 5;
    private bool _isRedRing = false;
    private float _currentTime = 0;

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _topScoreText;

    public UnityEvent OnGameOver;
    private float _startTime;

    [SerializeField] private AudioSource _audioSource;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        DataManager.Instance.LoadGameData();
        _topScoreText.text = DataManager.Instance.data.TopScore.ToString();
        
        _mainCam = Camera.main;
        _material = _ringTrm.GetComponent<SpriteRenderer>().material;
        Init();
    }

    private void Update()
    {
        ScalingCircle();
        CheckCircle();
    }

    private void ScalingCircle()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                float scale =
                    Mathf.Clamp(
                        -4f + ((Vector2)_mainCam.ScreenToWorldPoint(Input.touches[0].position)).magnitude * 2.25f, 1.5f,
                        5.5f);
                transform.localScale = new Vector3(scale, scale, 1);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                float scale =
                    Mathf.Clamp(
                        -4f + ((Vector2)_mainCam.ScreenToWorldPoint(Input.mousePosition)).magnitude * 2.25f, 1.5f,
                        5.5f);
                transform.localScale = new Vector3(scale, scale, 1);
            }
        }
    }

    private void CheckCircle()
    {
        if (Mathf.Abs(_ringScale - transform.localScale.x) < 0.2f)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > 0.1f)
            {
                PlaySound();
                RandomScalingRing();
                SetScore();
                StopCoroutine(nameof(DrainAmount));
                StartCoroutine(nameof(DrainAmount));
            }
            else if (_isRedRing) GameOver();
        }
        else _currentTime = 0;
    }

    private void PlaySound()
    {
        _audioSource.Play();
        _audioSource.pitch = Random.Range(0.9f, 1.1f);
    }

    private void RandomScalingRing()
    {
        do _ringScale = Random.Range(1.5f, 5.4f);
        while (Mathf.Abs(_ringScale - _ringTrm.localScale.x) < 1);
        _ringTrm.localScale = new Vector3(_ringScale, _ringScale, 1);

        int random = Random.Range(0, 5);
        if (random == 0) StartCoroutine(RedRing());
        else _material.SetColor(_ringColorHash, _ringColor);
    }

    private IEnumerator DrainAmount()
    {
        float currentTime = 0;
        float percent = 0;
        float time = Mathf.Clamp(4 - (Time.time - _startTime) / 30, 1f, 10);
        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / time;
            _material.SetFloat(_fillAmountHash, 1 - percent);
            yield return null;
        }
        GameOver();
    }

    private void SetScore()
    {
        var score = int.Parse(_scoreText.text) + 1;
        _scoreText.text = score.ToString();

        if (score > DataManager.Instance.data.TopScore)
        {
            DataManager.Instance.data.TopScore = score;
            _topScoreText.text = score.ToString();
        }
    }

    private IEnumerator RedRing()
    {
        _isRedRing = true;
        _material.SetColor(_ringColorHash, new Color(0.53f, 0.23f, 0.23f, 1));
        yield return new WaitForSeconds(Mathf.Clamp(Random.Range(0.6f, 1.1f) - float.Parse(_scoreText.text) / 300, 0.5f, 1));
        _material.SetColor(_ringColorHash, _ringColor);
        _isRedRing = false;
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public void Init()
    {
        _startTime = Time.time;
        StopCoroutine(nameof(DrainAmount));
        _isRedRing = false;
        _mainCam.backgroundColor = Random.ColorHSV(0, 1, .03f, .03f, 0.83f, 0.83f);
        _material.SetColor(_ringColorHash, _ringColor = _mainCam.backgroundColor - new Color(0.345f, 0.345f, 0.345f, 0));
        transform.localScale = new Vector3(1.5f, 1.5f, 1);
        _ringScale = 5; _ringTrm.localScale = new Vector3(_ringScale, _ringScale, 1);
        _material.SetFloat(_fillAmountHash, 1);
        _scoreText.text = "0";
    }
}
