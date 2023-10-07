using System.Collections;
using UnityEngine;

public class RippleEffect : PoolableMono, IFeedback
{
    private Material _mat;
    private readonly int _colorHash = Shader.PropertyToID("_Color1");

    private void Awake()
    {
        _mat = GetComponent<SpriteRenderer>().material;
    }
    
    public override void Init()
    {
        _mat.SetColor(_colorHash, new Color(1, 1, 1, 0));
    }

    private IEnumerator DOColorAndDoScaleCoroutine(bool isSpread)
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale + new Vector3(0.5f, 0.5f);
        Color startColor = new Color(1, 1, 1, 0);
        float currentTime = 0;
        float percent = 0;

        if (isSpread == false) transform.localScale += new Vector3(0.2f, 0.2f);
        while (percent < 0.5f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 0.375f;
            if (isSpread) transform.localScale = Vector3.Lerp(startScale, endScale, 1 - Mathf.Pow(1 - percent, 3));
            _mat.SetColor(_colorHash, Color.Lerp(startColor, Color.white, percent * 2));
            yield return null;
        }

        while (percent < 1f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 0.375f;
            if (isSpread) transform.localScale = Vector3.Lerp(startScale, endScale, 1 - Mathf.Pow(1 - percent, 3));
            _mat.SetColor(_colorHash, Color.Lerp(Color.white, startColor, (percent - 0.5f) * 2));
            yield return null;
        }

        FinishFeedback();
    }

    public void StartFeedback(bool value = true)
    {
        StartCoroutine(DOColorAndDoScaleCoroutine(value));
    }

    public void FinishFeedback()
    {
        PoolManager.Instance.Push(this);
    }
}
