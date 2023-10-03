using System.Collections;
using UnityEngine;

public class RippleEffect : PoolableMono, IFeedback
{
    private float _startScale;
    
    private Material _mat;
    private readonly int _colorHash = Shader.PropertyToID("_Color1");

    private void Awake()
    {
        _mat = GetComponent<SpriteRenderer>().material;
    }
    
    public override void Init()
    {
        StartFeedback();
        StopCoroutine(nameof(DOColorAndDoScaleCoroutine));
        StartCoroutine(nameof(DOColorAndDoScaleCoroutine));
    }

    private IEnumerator DOColorAndDoScaleCoroutine()
    {
        yield return null;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale * 1.1f;
        Color startColor = new Color(1, 1, 1, 0);
        float currentTime = 0;
        float percent = 0;

        while (percent < 0.5f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 0.375f;
            transform.localScale = Vector3.Lerp(startScale, endScale, 1 - Mathf.Pow(1 - percent, 3));
            _mat.SetColor(_colorHash, Color.Lerp(startColor, Color.white, percent * 2));
            yield return null;
        }
        
        while (percent < 1f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 0.375f;
            transform.localScale = Vector3.Lerp(startScale, endScale, 1 - Mathf.Pow(1 - percent, 3));
            _mat.SetColor(_colorHash, Color.Lerp(Color.white, startColor, (percent - 0.5f) * 2));
            yield return null;
        }
        
        PoolManager.Instance.Push(this);
    }

    public void StartFeedback()
    {
        _startScale = transform.localScale.x;
    }

    public void FinishFeedback()
    {
        transform.localScale = new Vector3(_startScale, _startScale, 1);
    }
}
