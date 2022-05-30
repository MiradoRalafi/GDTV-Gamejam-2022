using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region PARAMETERS

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float sprintSpeed = 10f;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private Transform groundCheckPoint;
    [SerializeField]
    private Vector3 groundCheckExtents;
    [SerializeField]
    private float jumpForce = 2f;

    [SerializeField]
    private float initialHungerValue = 20f;
    [SerializeField]
    private Slider hungerBar;

    [Header("Audios")]
    [SerializeField]
    private AudioClip[] deathClips;

    [Header("Debug")]
    public bool HungerActive = true;

    #endregion

    #region CACHES

    private Rigidbody rb;
    private Camera camera;
    private AudioSource audioSource;
    private Animator animator;

    #endregion

    #region STATES

    private bool isSprinting = false;
    private bool grounded = false;
    private MovingObject currentLog;
    private float hunger;
    //private bool onLog = false;
    private int xMovement;
    private int zMovement;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        camera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ResetHunger();
    }

    private void Update()
    {
        xMovement = (int)Input.GetAxisRaw("Horizontal");
        zMovement = (int)Input.GetAxisRaw("Vertical");
        if (xMovement == 0 && zMovement == 0)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
        }
        isSprinting = Input.GetButton("Sprint");
        CheckGround();
        if (currentLog)
        {
            transform.position += currentLog.transform.right * currentLog.MoveSpeed * Time.deltaTime;
            if (xMovement != 0 || zMovement != 0)
            {
                RotateAlongCamera(xMovement, zMovement);
                transform.position += transform.forward * (isSprinting ? sprintSpeed : moveSpeed) * Time.deltaTime;
                animator.SetBool("Walk", true);
                animator.SetBool("Run", isSprinting);
            }
            transform.eulerAngles = Vector3.up * transform.eulerAngles.y;
        }
        if (Input.GetButtonDown("Jump"))
        {
            TryJump();
        }
        if (HungerActive)
        {
            hunger -= Time.deltaTime;
            hungerBar.value = hunger / initialHungerValue;
        }
        if (transform.position.y <= -1 || hunger <= 0)
        {
            Die();
            return;
        }
        CheckOutOfArea();
    }

    private void CheckOutOfArea()
    {
        float distance = Mathf.Abs(transform.position.x - 100);
        if (distance > 50)
        {
            if(distance < 65)
            {
                GameManager.Instance.ShowWarning();
            }
            else
            {
                Die();
            }
        }
        else
        {
            GameManager.Instance.HideWarning();
        }
    }

    private void CheckGround()
    {
        Collider[] ground = Physics.OverlapBox(groundCheckPoint.position, groundCheckExtents, Quaternion.identity, groundLayer);
        grounded = ground.Length != 0;
    }

    private void TryJump()
    {
        if (!grounded){ return; }
        if (currentLog)
        {
            currentLog = null;
            rb.isKinematic = false;
            //transform.parent = null;
            transform.position += Vector3.up * .1f;
        }
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    private void FixedUpdate()
    {
        if (!currentLog && (xMovement != 0 || zMovement != 0))
        {
            RotateAlongCamera(xMovement, zMovement);
            if (grounded)
            {
                rb.velocity = transform.forward * (isSprinting ? sprintSpeed : moveSpeed) + Vector3.up * rb.velocity.y;
            }
            else
            {
                rb.velocity = transform.forward * moveSpeed + Vector3.up * rb.velocity.y;
            }
            animator.SetBool("Walk", true);
            animator.SetBool("Run", isSprinting);
        }
        else
        {
            rb.velocity = Vector3.up * rb.velocity.y;
        }
    }

    private void RotateAlongCamera(int xMovement, int zMovement)
    {
        FreezeRotationY(); // freezing rotation so we can manually rotate
        Vector3 right = new Vector3(camera.transform.right.x, 0, camera.transform.right.z) * xMovement;
        Vector3 forward = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z) * zMovement;
        transform.rotation = Quaternion.LookRotation(right + forward, Vector3.up);
        UnfreezeRotationY(); // unfreezing rotation so the physics system can take over
    }

    private void FreezeRotationY()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX
                    | RigidbodyConstraints.FreezeRotationY
                    | RigidbodyConstraints.FreezeRotationZ;
    }

    private void UnfreezeRotationY()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX
            | RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Log"))
        {
            print("Enter");
            //transform.position = new Vector3(transform.position.x, collision.contacts[0].point.y, transform.position.z);
            //transform.parent = collision.collider.transform;
            //onLog = true;
            //rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            currentLog = collision.collider.GetComponent<MovingObject>();
        }
        else if (collision.collider.GetComponent<Car>())
        {
            Die();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Log"))
        {
            print("Exit");
            //transform.parent = null;
            //onLog = false;
            rb.isKinematic = false;
            currentLog = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Timeline"))
        {
            FindObjectOfType<TimelinePlayer>().StartTimeline();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckExtents * 2);
    }

    public void Die()
    {
        gameObject.SetActive(false);
        AudioManager.Instance.PlayOneShot(deathClips[Random.Range(0, deathClips.Length)]);
        GameManager.Instance.ReloadLevel();
        ResetHunger();
    }

    private void ResetHunger()
    {
        hunger = initialHungerValue;
        hungerBar.value = hunger / initialHungerValue;
    }

    public void Eat()
    {
        hunger += 10;
        if(hunger > initialHungerValue)
        {
            hunger = initialHungerValue;
        }
    }
}
