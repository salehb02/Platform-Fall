using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float rotateSpeed;
    public GameObject contactVFX;

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    private void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Enemy"))
        {
            Instantiate(contactVFX, collision.contacts[0].point, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
