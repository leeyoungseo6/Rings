using System.Collections;
using UnityEngine;

public class RippleEffect : PoolableMono, IFeedback
{
    private Camera _mainCam;
    private Material _mat;
    private readonly int _colorHash = Shader.PropertyToID("_Color");
    private Color _color;

    private void Awake()
    {
        _mainCam = Camera.main;
        _mat = GetComponent<SpriteRenderer>().material;
    }
    
    public override void Init()
    {
        _color = _mainCam.backgroundColor - new Color(0.1f, 0.1f, 0.1f, 0);
        _mat.SetColor(_colorHash, _color - new Color(0, 0, 0, 1));
    }

    private IEnumerator DOColorAndDoScaleCoroutine(Vector3 endScale) 
    {
        Vector3 startScale = transform.localScale;
        Color startColor = _mat.GetColor(_colorHash);
        float currentTime = 0;
        float percent = 0;

        while (percent < 0.5f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 0.4f;
            transform.localScale = Vector3.Lerp(startScale, endScale, 1 - Mathf.Pow(1 - percent, 3));
            _mat.SetColor(_colorHash, Color.Lerp(startColor, _color, percent * 2));
            yield return null;
        }

        while (percent < 1f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 0.4f;
            transform.localScale = Vector3.Lerp(startScale, endScale, 1 - Mathf.Pow(1 - percent, 3));
            _mat.SetColor(_colorHash, Color.Lerp(_color, startColor, (percent - 0.5f) * 2));
            yield return null;
        }

        FinishFeedback();
    }

    public void StartFeedback(Vector3 endScale)
    {
        StartCoroutine(DOColorAndDoScaleCoroutine(endScale));
    }

    public void FinishFeedback()
    {
        PoolManager.Instance.Push(this);
    }
}
