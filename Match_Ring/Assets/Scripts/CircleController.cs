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
            float scale = 1 + _mainCam.ScreenToWorldPoint(Input.mousePosition).magnitude / 3;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
