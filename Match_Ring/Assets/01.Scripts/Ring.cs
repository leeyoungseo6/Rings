using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ring : MonoBehaviour
{
    private Camera _mainCam;
    private Material _material;
    private readonly int _fillAmountHash = Shader.PropertyToID("_FillAmount");
    private readonly int _ringColorHash = Shader.PropertyToID("_Color1");
    private Color _ringColor;

    private void Awake()
    {
        _mainCam = Camera.main;
        _material = GetComponent<SpriteRenderer>().material;
    }

    private void Start() => GameStart();

    public void GameStart()
    {
        _material.SetColor(_ringColorHash, _ringColor = _mainCam.backgroundColor - new Color(0.345f, 0.345f, 0.345f, 0));
        _material.SetFloat(_fillAmountHash, 1);
        transform.localScale = new Vector3(5, 5, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) GameOver();
    }

    public void GameOver()
    {
        Debug.Log("asdf");
        StopAllCoroutines();
    }

    public void Init(float ringScale, Action action)
    {
        StopCoroutine(nameof(FillAmount));
        StartCoroutine(nameof(FillAmount));
        
        transform.localScale = new Vector3(ringScale, ringScale, 1);
        
        if (action != null) StartCoroutine(nameof(RedRing), action);
        else _material.SetColor(_ringColorHash, _ringColor);
    }
    
    private IEnumerator FillAmount()
    {
        float currentTime = 0;
        float percent = 0;
        float time = Mathf.Clamp(4 - Time.time / 30, 1f, 10);
        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / time;
            _material.SetFloat(_fillAmountHash, 1 - percent);
            yield return null;
        }
        
        GameManager.Instance.GameOver();
    }

    private IEnumerator RedRing(Action action = null)
    {
        _material.SetColor(_ringColorHash, new Color(0.55f, 0.24f, 0.24f, 1));
        yield return new WaitForSeconds(Mathf.Clamp(Random.Range(0.6f, 1.1f) - GameManager.Instance.Difficulty / 300, 0.5f, 1));
        _material.SetColor(_ringColorHash, _ringColor);
        action?.Invoke();
    }
}