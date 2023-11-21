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
    }

    private void ScalingCircle()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (_touch.phase == TouchPhase.Moved)
            {
                var scale =
                    Mathf.Clamp
                    (
                        transform.localScale.y - Input.GetAxisRaw("Mouse Y") / 8.25f,
                        1.6f,
                        5.1f
                    );
                transform.localScale = new Vector3(scale, scale, 1);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                var scale =
                    Mathf.Clamp
                    (
                        transform.localScale.y - Input.GetAxisRaw("Mouse Y") / 3f,
                        1.6f,
                        5.1f
                    );
                transform.localScale = new Vector3(scale, scale, 1);
            }
        }
    }

    private void CheckCircle()
    {
        if (Mathf.Abs(_ringScale - transform.localScale.x) < 0.23f)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > 0.1f)
            {
                UIManager.Instance.AddScore();
                PlayRippleEffect(new Vector3(_ringScale, _ringScale, 1));
                NewRing();
            }
            else if (_isRedRing)
            {
                GameManager.Instance.GameOver(); 
            }
        }
        else _currentTime = 0;
    }
    
    private void PlayRippleEffect(Vector3 scale)
    {
        var effect = PoolManager.Instance.Pop("RippleEffect") as RippleEffect;
        effect.transform.localScale = scale;
        effect.StartFeedback(scale + new Vector3(0.425f, 0.425f));
    }

    private void NewRing()
    {
        var prevScale = _ringScale;
        do _ringScale = Random.Range(1.65f, 4.85f);
        while (Mathf.Abs(_ringScale - prevScale) < 1f);

        _isRedRing = Random.Range(0, 5) == 0; 
        
        OnRingChecked?.Invoke(_ringScale, _isRedRing ? () => _isRedRing = false : null);
    }

    public void Init()
    {
        _ringScale = 4.75f;
        _isRedRing = false;
        UIManager.Instance.SetScore();
        GameManager.Instance.Difficulty = 0;
        transform.localScale = new Vector3(1.6f, 1.6f, 1);
        _mainCam.backgroundColor = Random.ColorHSV(0, 1, .05f, .05f, 0.85f, 0.85f);
    }
}