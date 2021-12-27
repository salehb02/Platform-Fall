using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce;
    public float maxVelocity;
    public float turnSpeed;
    public bool grounded;
    private float _verticalInput, _horizontalInput;

    [Header("Shooting")]
    public GameObject bullet;
    public GameObject bulletSpawnPoint;
    public float bulletForce;
    public AudioSource shootSFX;

    private const string FloorTag = "Floor";
    private Rigidbody _rigid;

    #region Engine
    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        Inputs();
        Shoot();
    }
    #endregion

    #region Movement
    private void Inputs()
    {
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
    }

    private void Movement()
    {
        if (_rigid.velocity.magnitude < maxVelocity)
            _rigid.AddForce(transform.rotation * Vector3.forward * moveForce * _verticalInput);

        transform.Rotate(0, _horizontalInput * turnSpeed, 0);
    }
    #endregion

    #region Grounded Check
    private void OnCollisionExit(Collision collision) => CheckGrounded(collision);
    private void OnCollisionStay(Collision collision) => CheckGrounded(collision);
    private void CheckGrounded(Collision collision)
    {
        if (collision.collider.CompareTag(FloorTag))
        {
            if (collision.contactCount > 0)
                grounded = true;
            else
                grounded = false;
        }
    }
    #endregion

    #region Shooting
    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody rigid = Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation, null).GetComponent<Rigidbody>();
            rigid.AddForce(rigid.transform.rotation * Vector3.forward * bulletForce, ForceMode.Impulse);
            shootSFX.Play();
        }
    }
    #endregion
}