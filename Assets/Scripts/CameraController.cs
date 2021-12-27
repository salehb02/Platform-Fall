using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothness = 1;

    void FixedUpdate()
    {
        if (target.GetComponent<Player>().grounded)
            transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * smoothness);
    }
}
