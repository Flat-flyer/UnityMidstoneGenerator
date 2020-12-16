using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float CameraMoveSpeed = 120.0f;
    public GameObject CameraFollowObj;
    public float clampAngle = 00.0f;
    public float inputSensitivity = 150.0f;
    public GameObject CameraObject;
    public GameObject PlayerObject;
    public float mouseX; 
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    [SerializeField]
    private float rotationY = 0.0f;
    [SerializeField]
    private float rotationX = 0.0f;
    [SerializeField]
    private bool isLockedOn;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject Camera;
    private Quaternion localRotation;
    [SerializeField]
    private Transform TargetFacer;
    private Vector3 rotation;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotationY = rot.y;
        rotationX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        //Controller Rotation Setup
        float inputX = Input.GetAxis("RightStickHorizontal");
        float inputZ = Input.GetAxis("RightStickVertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = inputX + mouseX;
        finalInputZ = inputZ + mouseY;
        if (isLockedOn == false)
        {
            rotationY += finalInputX * inputSensitivity * Time.deltaTime;
            rotationX += finalInputZ * inputSensitivity * Time.deltaTime;

            rotationX = Mathf.Clamp(rotationX, -clampAngle, clampAngle);

            localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
            transform.rotation = localRotation;
        }
        if (isLockedOn == true)
        {
            rotationX = this.transform.rotation.eulerAngles.x;
            rotationY = this.transform.rotation.eulerAngles.y;
            rotation = new Vector3(TargetFacer.transform.position.x, TargetFacer.transform.position.y, TargetFacer.transform.position.z);
            if (rotationX > 70)
            {
                rotationX = rotationX - 360;
            }
            this.transform.LookAt(rotation);
        }
    }

    void LateUpdate()
    {
        CameraUpdater();
    }

    void CameraUpdater()
    {
        //sets the followed object
        Transform target = CameraFollowObj.transform;

        //moves towards the target game object
        float step = CameraMoveSpeed * Time.deltaTime;


            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    public void getLockOn(bool lockonState)
    {
        isLockedOn = lockonState;
    }
}
