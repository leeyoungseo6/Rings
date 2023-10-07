using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CircleController : MonoBehaviour
{
    private Camera _mainCam;
    private Touch _touch => Input.touches[0];

    private Color _ringColor;
    private float _ringScale = 5;
    private bool _isRedRing;
    private float _currentTime;

    public UnityEvent<float, Action> OnRingChecked;
    
    private void Awake()
    {
        _mainCam = Camera.main;
        Init();
    }

    private void Update()
    {
        ScalingCircle();
        CheckCircle();
        GameManager.Instance.Difficulty += Time.deltaTime;
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
                UIManager.Instance.AddScore();
                NewRing();
            }
            else if (_isRedRing)
            {
                GameManager.Instance.Difficulty += 10;
                NewRing();
            }
        }
        else _currentTime = 0;
    }

    private void NewRing()
    {
        float prevScale = _ringScale;
        do _ringScale = Random.Range(1.5f, 5.4f);
        while (Mathf.Abs(_ringScale - prevScale) < 1);

        _isRedRing = Random.Range(0, 5) == 0;
        
        OnRingChecked?.Invoke(_ringScale, _isRedRing ? () => _isRedRing = false : null);
    }

    public void Init()
    {
        _ringScale = 5;
        _isRedRing = false;
        UIManager.Instance.SetScore();
        GameManager.Instance.Difficulty = 0;
        transform.localScale = new Vector3(1.5f, 1.5f, 1);
        _mainCam.backgroundColor = Random.ColorHSV(0, 1, .04f, .04f, 0.83f, 0.83f);
    }
}