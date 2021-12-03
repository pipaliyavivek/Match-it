using UnityEngine;

[ExecuteInEditMode]
public class FillScreen : MonoBehaviour
{
    public float distance = 1;
    public float Position_pading;
    public float width_padding;
    public float height_padding;

    void Start()
    {
        Camera cam = Camera.main;
        float pos = (cam.nearClipPlane + distance);
        transform.position = cam.transform.position + cam.transform.forward * pos;
        float h = (Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * pos * 2f) / 10.0f;
        transform.localScale = new Vector3(h * cam.aspect- width_padding, 1.0f, h- height_padding);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position .z- Position_pading);
    }
}