using UnityEngine;

public class CircleController : MonoBehaviour
{
    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float scale = Mathf.Clamp(-3 + ((Vector2)_mainCam.ScreenToWorldPoint(Input.mousePosition)).magnitude * 2, 1, 5);
            transform.localScale = new Vector3(scale, scale, 1);
        }
    }
}
