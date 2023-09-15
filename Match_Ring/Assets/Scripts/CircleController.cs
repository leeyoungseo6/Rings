using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class CircleController : MonoBehaviour
{
    private Camera _mainCam;

    [SerializeField] private Transform _ringTrm;
    [SerializeField] private SpriteRenderer _ringRenderer;
    private bool _isRedRing = false;
    private float _currentTime = 0;

    [SerializeField] private TextMeshProUGUI _scoreText;

    public UnityEvent OnGameOver;
    public UnityEvent OnStart;

    private void Awake()
    {
        _mainCam = Camera.main;
        _mainCam.backgroundColor = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1));
        _ringRenderer = _ringTrm.GetComponent<SpriteRenderer>();
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
            float scale = Mathf.Clamp(-3 + ((Vector2)_mainCam.ScreenToWorldPoint(Input.mousePosition)).magnitude * 2, 1, 5);
            transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    private void CheckCircle()
    {
        if (Mathf.Abs(_ringTrm.localScale.x - transform.localScale.x - 0.15f) < 0.15f)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > 0.1f)
            {
                if (_isRedRing) GameOver();
                RandomScalingRing();
            }
        }
        else _currentTime = 0;
    }

    private void RandomScalingRing()
    {
        float rand;
        do rand = Random.Range(1, 4.9f);
        while (Mathf.Abs(rand - _ringTrm.localScale.x) < 1);
        
        _ringTrm.localScale = new Vector3(rand, rand, 1);
        _scoreText.text = (int.Parse(_scoreText.text) + 1).ToString();

        int random = Random.Range(0, 5);
        if (random == 1) StartCoroutine(RedRing());
        else _ringRenderer.color = new Color(0.3f, 0.3f, 0.3f);
    }

    private IEnumerator RedRing()
    {
        _isRedRing = true;
        _ringRenderer.color = new Color(0.6f, 0.1f, 0.2f);
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        _ringRenderer.color = new Color(0.3f, 0.3f, 0.3f);
        _isRedRing = false;
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public void Restart()
    {
        _mainCam.backgroundColor = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1));
        transform.localScale = Vector3.one;
        _ringTrm.localScale = new Vector3(4, 4, 1);
        _ringRenderer.color = new Color(0.3f, 0.3f, 0.3f);
        _scoreText.text = "0";
        OnStart?.Invoke();
    }
}
