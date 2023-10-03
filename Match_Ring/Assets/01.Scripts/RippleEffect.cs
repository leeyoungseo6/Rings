using System.Collections;
using UnityEngine;

public class RippleEffect : PoolableMono
{
    private Material _mat;
    private readonly int _colorHash = Shader.PropertyToID("_Color1");

    private void Awake()
    {
        _mat = GetComponent<SpriteRenderer>().material;
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        StopCoroutine(nameof(DOColorAndDoScaleCoroutine));
        StartCoroutine(nameof(DOColorAndDoScaleCoroutine));
    }

    private IEnumerator DOColorAndDoScaleCoroutine()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale * 1.2f;
        Color startColor = new Color(1, 1, 1, 0);
        float currentTime = 0;
        float percent = 0;

        while (percent < 0.5f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 0.5f;
            transform.localScale = Vector3.Lerp(startScale, endScale, 1 - Mathf.Pow(1 - percent, 3));
            _mat.SetColor(_colorHash, Color.Lerp(startColor, Color.white, percent * 2));
            yield return null;
        }
        
        while (percent < 1f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 0.5f;
            transform.localScale = Vector3.Lerp(startScale, endScale, 1 - Mathf.Pow(1 - percent, 3));
            _mat.SetColor(_colorHash, Color.Lerp(Color.white, startColor, (percent - 0.5f) * 2));
            yield return null;
        }
    }
}
