using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private bool lockOnState;

    [SerializeField]
    private float jumpForce = 7;
    [SerializeField]
    private float groundDistance = 0.2f;

    public LayerMask groundLayers;

    private Rigidbody rb;

    [SerializeField]
    private Transform groundChecker;

    [SerializeField]
    private GameObject CameraRot;
    [SerializeField]
    private GameObject LockFacer;
    [SerializeField]
    private PlayerController Player;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundChecker = transform.GetChild(2);
    }

    public void Move (Vector3 PVelocity)
    {
        velocity = PVelocity;
    }
    public void LockOnState( bool LockOn)
    {
        lockOnState = LockOn;
    }

    public void Rotate (Vector3 PRotation)
    {
        rotation = PRotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && Player.CanMove == true)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    private void FixedUpdate()
    {
        if (lockOnState == false)
        {
            rotation = new Vector3(0f, CameraRot.transform.rotation.eulerAngles.y, 0);
            PerformRotation();
        }
        if (lockOnState == true)
        {
            rotation = new Vector3(LockFacer.transform.position.x, this.transform.position.y, LockFacer.transform.position.z);
            this.transform.LookAt(rotation);
        }
        PerformMovement();

    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundChecker.position, groundDistance, groundLayers, QueryTriggerInteraction.Ignore);


    }

    //performs movement based on velocity varaible
    void PerformMovement ()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }
    void PerformRotation()
    {
        rb.MoveRotation(Quaternion.Euler (rotation));
    }
    public void SetLastFacedDirection(Vector3 LastRotation)
    {
        rotation = LastRotation;
        rb.MoveRotation(Quaternion.Euler(rotation));
    }
}
