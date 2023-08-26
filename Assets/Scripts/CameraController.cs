using Unity.Netcode;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player target;
    public Vector3 offset;
    public float smoothness = 1;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnConnectPlayer;
    }

    private void OnConnectPlayer(ulong obj)
    {
        target = FindObjectOfType<Player>();
    }

    void FixedUpdate()
    {
        if (!target)
            return;

        if (target.grounded)
            transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, Time.deltaTime * smoothness);
    }
}