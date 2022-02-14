using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveForce;
    public float maxVelocity;
    public float turnSpeed = 5f;
    public bool grounded;

    private Player _player;
    private Rigidbody _rigid;
    private bool _destroyed;
    private GameManager _gameManager;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _rigid = GetComponent<Rigidbody>();
        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (_player.dead)
            return;

        PlayerLookAt();

        if (transform.position.y < -1 && !_destroyed)
        {
            _destroyed = true;
            StartCoroutine(Kill());
        }
    }

    private void FixedUpdate()
    {
        if (_player.dead)
            return;

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

    private IEnumerator Kill()
    {
        _gameManager.enemies.Remove(gameObject);
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    #region Grounded Check
    private void OnCollisionExit(Collision collision) => CheckGrounded(collision);
    private void OnCollisionStay(Collision collision) => CheckGrounded(collision);
    private void CheckGrounded(Collision collision)
    {
        if (collision.collider.CompareTag(GameManager.FLOOR_TAG))
        {
            if (collision.contactCount > 0)
                grounded = true;
            else
                grounded = false;
        }
    }
    #endregion
}