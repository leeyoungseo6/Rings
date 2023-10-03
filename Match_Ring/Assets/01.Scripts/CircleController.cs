using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CircleController : MonoBehaviour
{
    private Camera _mainCam;
    private Touch _touch => Input.touches[0];

    [SerializeField] private Transform _ringTrm;
    private Material _material;
    private readonly int _fillAmountHash = Shader.PropertyToID("_FillAmount");
    private readonly int _ringColorHash = Shader.PropertyToID("_Color1");
    private Color _ringColor;
    private float _ringScale = 5;
    private bool _isRedRing = false;
    private float _currentTime = 0;

    [SerializeField] private TextMeshProUGUI _scoreText;

    public UnityEvent OnGameOver;
    private float _startTime;

    [SerializeField] private AudioSource _audioSource;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        
        _mainCam = Camera.main;
        _material = _ringTrm.GetComponent<SpriteRenderer>().material;
        Init();

        GameManager.Instance.OnGameOver += () => gameObject.SetActive(false);
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
            if (_touch.phase == TouchPhase.Moved)
            {
                float scale =
                    Mathf.Clamp(
                        -4f + ((Vector2)_mainCam.ScreenToWorldPoint(_touch.position)).magnitude * 2.25f, 1.5f,
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
                RippleEffect();
                RandomScalingRing();
                UIManager.Instance.SetScore();
                StopCoroutine(nameof(FillAmount));
                StartCoroutine(nameof(FillAmount));
            }
            else if (_isRedRing) GameManager.Instance.OnGameOver?.Invoke();
        }
        else _currentTime = 0;
    }

    private void RippleEffect()
    {
        Transform ripple = PoolManager.Instance.Pop("RippleEffect").transform;
        ripple.localScale = transform.localScale;
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

    private IEnumerator FillAmount()
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
        
        GameManager.Instance.OnGameOver?.Invoke();
    }

    private IEnumerator RedRing()
    {
        _isRedRing = true;
        _material.SetColor(_ringColorHash, new Color(0.55f, 0.24f, 0.24f, 1));
        yield return new WaitForSeconds(Mathf.Clamp(Random.Range(0.6f, 1.1f) - float.Parse(_scoreText.text) / 300, 0.5f, 1));
        _material.SetColor(_ringColorHash, _ringColor);
        _isRedRing = false;
    }

    public void Init()
    {
        _startTime = Time.time;
        StopCoroutine(nameof(FillAmount));
        _isRedRing = false;
        _mainCam.backgroundColor = Random.ColorHSV(0, 1, .04f, .04f, 0.83f, 0.83f);
        _material.SetColor(_ringColorHash, _ringColor = _mainCam.backgroundColor - new Color(0.345f, 0.345f, 0.345f, 0));
        transform.localScale = new Vector3(1.5f, 1.5f, 1);
        _ringScale = 5; _ringTrm.localScale = new Vector3(_ringScale, _ringScale, 1);
        _material.SetFloat(_fillAmountHash, 1);
    }
}