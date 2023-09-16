using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CircleController : MonoBehaviour
{
    private Camera _mainCam;

    [SerializeField] private Transform _ringTrm;
    private SpriteRenderer _ringRenderer;
    private Material _material;
    private readonly int _fillAmountHash = Shader.PropertyToID("_FillAmount");
    private Color _ringColor;
    private bool _isRedRing = false;
    private float _currentTime = 0;

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _topScoreText;

    public UnityEvent OnGameOver;
    public UnityEvent OnStart;

    [SerializeField] private AudioSource _audioSource;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        DataManager.Instance.LoadGameData();
        _topScoreText.text = DataManager.Instance.data.TopScore.ToString();
        
        _mainCam = Camera.main;
        _ringRenderer = _ringTrm.GetComponent<SpriteRenderer>();
        _material = _ringRenderer.material;
        Init();
    }

    private void Update()
    {
        ScalingCircle();
        CheckCircle();
    }

    private void ScalingCircle()
    {
        if (Input.GetMouseButton(0))
        {
            float scale = Mathf.Clamp(-3.5f + ((Vector2)_mainCam.ScreenToWorldPoint(Input.mousePosition)).magnitude * 2.5f, 1.25f, 5);
            transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    private void CheckCircle()
    {
        if (Mathf.Abs(_ringTrm.localScale.x - transform.localScale.x) < 0.2f)
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
        float rand;
        do rand = Random.Range(1.25f, 4.9f);
        while (Mathf.Abs(rand - _ringTrm.localScale.x) < 1);
        _ringTrm.localScale = new Vector3(rand, rand, 1);

        int random = Random.Range(0, 5);
        if (random == 1) StartCoroutine(RedRing());
        else _ringRenderer.color = _ringColor;
    }

    private IEnumerator DrainAmount()
    {
        float currentTime = 0;
        float percent = 0;
        float time = Mathf.Clamp(10 - float.Parse(_scoreText.text) / 20, 1.9f, 10);
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
        int score = int.Parse(_scoreText.text) + 1;
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
        _ringRenderer.color = new Color(0.5f, 0.2f, 0.2f, 1);
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        _ringRenderer.color = _ringColor;
        _isRedRing = false;
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public void Restart()
    {
        Init();
        OnStart?.Invoke();
    }

    private void Init()
    {
        StopCoroutine(nameof(DrainAmount));
        _isRedRing = false;
        _mainCam.backgroundColor = new Color(Random.Range(0.85f, 0.9f), Random.Range(0.85f, 0.9f), Random.Range(0.85f, 0.9f));
        transform.localScale = new Vector3(1.25f, 1.25f, 1);
        _ringTrm.localScale = new Vector3(4, 4, 1);
        _ringColor = _mainCam.backgroundColor - new Color(0.5f, 0.5f, 0.5f, 0);
        _material.SetFloat(_fillAmountHash, 1);
        _ringRenderer.color = _ringColor;
        _scoreText.text = "0";
    }
}
