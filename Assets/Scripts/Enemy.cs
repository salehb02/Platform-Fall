using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveForce;
    public float maxVelocity;
    public float turnSpeed = 5f;
    public bool grounded;

    private Player _player;
    private Rigidbody _rigid;
    private const string FloorTag = "Floor";
    private float _destroyTimer = 5f;

    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        PlayerLookAt();

        if (!grounded)
            _destroyTimer -= Time.deltaTime;
        else
            _destroyTimer = 5f;

        if(_destroyTimer <= 0)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (_rigid.velocity.magnitude < maxVelocity)
            _rigid.AddForce(transform.rotation * Vector3.forward * moveForce);
    }

    private void PlayerLookAt()
    {
        Vector3 targetDirection = _player.transform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * turnSpeed, 0);
        newDirection.y = 0;

        transform.rotation = Quaternion.LookRotation(newDirection);
    }

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
}