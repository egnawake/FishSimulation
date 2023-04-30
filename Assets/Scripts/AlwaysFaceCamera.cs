using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        transform.rotation = cam.transform.rotation;
    }
}
