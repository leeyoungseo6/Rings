using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ring : MonoBehaviour
{
    private Camera _mainCam;
    private Material _material;
    private readonly int _fillAmountHash = Shader.PropertyToID("_FillAmount");
    private readonly int _colorHash = Shader.PropertyToID("_Color");
    private Color _ringColor;

    private void Awake()
    {
        _mainCam = Camera.main;
        _material = GetComponent<SpriteRenderer>().material;
    }

    private void Start()
    {
        GameStart();
    }

    public void GameStart()
    {
        _material.SetColor(_colorHash, _ringColor = _mainCam.backgroundColor - new Color(0.345f, 0.345f, 0.345f, 0));
        _material.SetFloat(_fillAmountHash, 1);
        transform.localScale = new Vector3(4.75f, 4.75f, 1);
    }

    public void Init(float ringScale, Action action)
    {
        _material.SetFloat(_fillAmountHash, 1);
        StopCoroutine(nameof(FillAmount));
        StartCoroutine(nameof(FillAmount));

        transform.localScale = new Vector3(ringScale, ringScale, 1);
        
        if (action != null) StartCoroutine(nameof(RedRing), action);
        else _material.SetColor(_colorHash, _ringColor);
    }
    
    private IEnumerator FillAmount()
    {
        float currentTime = 0;
        float percent = 0;
        float time = 4 - GameManager.Instance.Difficulty / 30;
        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / time;
            _material.SetFloat(_fillAmountHash, 1 - percent);
            yield return null;
        }
        
        _material.SetFloat(_fillAmountHash, 0);
        GameManager.Instance.GameOver(); 
    }

    private IEnumerator RedRing(Action action = null)
    {
        Color prevColor = _ringColor;
        _material.SetColor(_colorHash, _ringColor = new Color(0.6f, 0.25f, 0.25f, 1));
        yield return new WaitForSeconds(Random.Range(0.6f, 1.1f) - GameManager.Instance.Difficulty / 300);
        _material.SetColor(_colorHash, _ringColor = prevColor);
        action?.Invoke();
    }

    public void GameOver()
    {
        StopAllCoroutines();
    }
}