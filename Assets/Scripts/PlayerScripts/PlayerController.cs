using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float LookSensitivty = 150f;
    [SerializeField]
    private GameObject CameraFollowObject;
    [SerializeField]
    private Collider basicAttackCol;
    [SerializeField]
    private Collider SweepAttackCol;
    [SerializeField]
    private Collider SweepAttackChargedCol;
    [SerializeField]
    private Collider ThrustAttackCol;
    [SerializeField]
    private GameObject SweepAttackFX;
    [SerializeField]
    private GameObject SweepAttackChargedFX;
    [SerializeField]
    private Collider GuardCol;
    [SerializeField]
    private Collider PlayerCol;
    [SerializeField]
    private GameObject GuardFX;
    [SerializeField]
    private GameObject AttackWave;
    [SerializeField]
    private Material PerfectGuardMat;
    [SerializeField]
    private Material GuardMat;
    [SerializeField]
    private GameObject PauseUI;
    [SerializeField]
    private GameObject OptionsUI;
    [SerializeField]
    private AudioSource BasicSwingSound;
    [SerializeField]
    private AudioSource ChargeSwingSound;
    [SerializeField]
    private AudioSource DashSwingSound;

    [SerializeField]
    private CameraFollow FollowCam;

    private Vector3 LastPlayerRotation;
    [SerializeField]
    private float TimePassed = 0f;
    private int SelectedTarget = 0;
    private float SavedSpeed;
    private float ChargeSpeed;
    [SerializeField]
    private float ProjectileSpeed = 12f;
    [SerializeField]
    private float EP;
    [SerializeField]
    private float EPMax = 100;
    [SerializeField]
    private float EPRegenMultiplier;

    private Transform TargetFacer;
    private Transform FirstCollider;
    private Transform Player;
    private Rigidbody Projectile;
    private Animator PlayerAnimator;


    private bool IsLockedOn = false;
    [SerializeField]
    private List<Collider> TargetColliders = new List<Collider>();
    public bool CanMove = true;
    public bool WaveAttackActive;
    private Vector3 MoveVertical;
    private Vector3 MoveHoritontal;
    public bool ThrustCollided = false;
    public bool BasicAttackHit = false;
    public bool GuardedAttack = false;
    public bool IsGuarding = false;
    private bool ThrustAttackEnabled;
    private bool ChargeEPCostPaid = false;
    private float EPRegenTimer;
    private bool EPRegenEnabled = true;
    [SerializeField]
    public float PerfectGuardTime;
    public BasicAttackHitDetection BasicHit;
    [SerializeField]
    private float DisplayedEP;
    [SerializeField]
    private Text EPText;
    public int HealingItemACount = 3;
    public bool PlayerHealing;
    public int HealingType;
    [SerializeField]
    private Text HealItemAText;


    private PlayerMotor motor;

    private void Start()
    {
        //gets components for the script to use from the player object and its children
        motor = GetComponent<PlayerMotor>();
        TargetFacer = transform.GetChild(1);
        Player = GetComponent<Transform>();
        PlayerAnimator = GetComponent<Animator>();
        //stores the base speed of the player
        SavedSpeed = speed;
        ChargeSpeed = 20;
        EP = 100;
        EPRegenMultiplier = 2;
    }

    private void Update()
    {

        //calculates movement velocity as a 3d vector
        float XMove = Input.GetAxisRaw("Horizontal");
        float ZMove = Input.GetAxisRaw("Vertical");

        Vector3 MoveHoritontal = transform.right * XMove;
        Vector3 MoveVertical = transform.forward * ZMove;

        Vector3 velocity = Vector3.zero;

        //final movement vector variable
        if (ThrustAttackEnabled == false)
        {
            velocity = (MoveHoritontal + MoveVertical).normalized * speed;
        }

        if (ThrustAttackEnabled == true)
        {
            //if thrust attack is active, checks to see if the charge duration has passed or the player has collided
            if (TimePassed >= 1 || ThrustCollided == true)
            {
                //ends the thrust attack and animation
                PlayerAnimator.SetBool("ChargeFinished", true);
                ThrustAttackEnabled = false;
                ThrustCollided = false;
                ThrustAttackCol.enabled = false;
                Debug.Log("Travel Attack Ended");
            }
            else
            {
                TimePassed += Time.deltaTime;
                //moves the player forward at a set speed while charge is happening
                velocity = (MoveHoritontal).normalized * speed + (Player.transform.forward * ChargeSpeed);
                //velocity = velocity + (Player.transform.forward * ChargeSpeed);
            }
        }
        //apply movement
        if (CanMove == true && IsGuarding == false)
        {
            motor.Move(velocity);
            //Passive EP Regeneration check
            if (EP < EPMax && EPRegenEnabled == true)
            {
                EP = EP += Time.deltaTime * EPRegenMultiplier;
            }
        }
        else
        {
            velocity = Vector3.zero;
            motor.Move(velocity);
        }

        if (BasicHit.BasicHitCollision == true)
        {
            //adds EP if basic attack collides with something
            EP = EP + 10;
            BasicHit.BasicHitCollision = false;
        }
        //prevents EP amount from overflowing above the limit
        if (EP > EPMax)
        {
            EP = EPMax;
        }
        DisplayedEP = Mathf.Round(EP);
        //displays EP amount and HP Item amount on screen
        EPText.text = "EP: " + DisplayedEP.ToString() + " / " + EPMax.ToString();
        HealItemAText.text = "1 \n  Healing Item A \n            " + HealingItemACount.ToString();

        TargetColliders.RemoveAll(Collider => Collider == null);


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //if player is already locked on, disables lock on and empties list of targets
            if (IsLockedOn == true)
            {
                IsLockedOn = false;
                SelectedTarget = 0;
                Debug.Log("Ending Lock On");
            }
            // sets the player to lock on mode if there is atleast one targetable entity in range
            else if (TargetColliders.Count != 0)
            {
                FirstCollider = TargetColliders[SelectedTarget].transform;
                IsLockedOn = true;
                Debug.Log("Attempting to lock on");
            }
        }

        //sets the lock on focus object to the position of the currently locked on target or back to the player if its disabled
        if (IsLockedOn == true)
        {
            if (FirstCollider != null)
            {
                TargetFacer.position = FirstCollider.position;
            }
            else
            {
                SelectedTarget = 0;
                IsLockedOn = false;
            }
        }
        if (IsLockedOn == false)
        {
            TargetFacer.position = Player.position;
        }

        //changes the chosen lock on target
        if (Input.GetKeyDown(KeyCode.Tab) && IsLockedOn == true && TargetColliders.Count >= 1)
        {
            SelectedTarget++;
            if (SelectedTarget >= TargetColliders.Count)
            {
                SelectedTarget = 0;
            }
            FirstCollider = TargetColliders[SelectedTarget].transform;
            Debug.Log("Changing Lock On Target");
        }
        //starts charge attack if player has enough EP and is currently able to attack
        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftShift) && CanMove == true && EP >= 20 && IsGuarding == false)
        {
            
            Debug.Log("Player Attacked with travel forward attack");
            PlayerAnimator.SetBool("ChargeFinished", false);
            TimePassed = 0;
            ThrustAttackCol.enabled = true;
            ThrustAttackEnabled = true;
            DashSwingSound.Play();
            PlayerAnimator.Play("PlayerDashAttack");
            EP = EP - 20;
        }
        //starts basic attack
        if (Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.LeftShift) && CanMove == true && IsGuarding == false)
        {
            CanMove = false;
            basicAttackCol.enabled = true;
            StartCoroutine(DisableAttack(basicAttackCol, 0.3f, null));
            if (WaveAttackActive == true)
            {
                var PlayerProjectile = Instantiate(AttackWave, basicAttackCol.transform);
                Projectile = PlayerProjectile.GetComponent<Rigidbody>();
                Projectile.velocity = Player.transform.forward * ProjectileSpeed;
            }
            BasicSwingSound.Play();
            PlayerAnimator.Play("PlayerBasicSwing");
            Debug.Log("Player Attacked");
        }
        //starts Sweep attack if player has enough EP and is currently able to attack
        if (Input.GetKeyDown(KeyCode.Mouse1) && CanMove == true && EP >= 15 && IsGuarding == false)
        {
            EP = EP - 15;
            ChargeEPCostPaid = true;
            TimePassed = 0;
            CanMove = false;
            SweepAttackCol.enabled = true;
            SweepAttackFX.SetActive(true);
            StartCoroutine(DisableAttack(SweepAttackCol, 0.3f, SweepAttackFX));
            ChargeSwingSound.Play();
            PlayerAnimator.Play("PlayerSweepSwing");
            Debug.Log("Player Attacked with initial sweep attack");
            
        }
        //holds sweep attack charge
        if (Input.GetKey(KeyCode.Mouse1) && CanMove == true && IsGuarding == false && ChargeEPCostPaid == true)
        {
            //reduces players speed to half of base
            if (speed == SavedSpeed)
            {
                speed = speed / 2;
            }
            EPRegenEnabled = false;
            TimePassed += Time.deltaTime;
        }
        //finishes sweep attack
        if (Input.GetKeyUp(KeyCode.Mouse1) && CanMove == true && IsGuarding == false)
        {
            EPRegenEnabled = true;
            //checks if enough time has passed while charing sweep attack, if so, performs the charged attack
            if (TimePassed >= 1 && EP >= 15)
            {
                EP = EP - 15;
                CanMove = false;
                SweepAttackChargedCol.enabled = true;
                SweepAttackChargedFX.SetActive(true);
                ChargeSwingSound.Play();
                StartCoroutine(DisableAttack(SweepAttackChargedCol, 0.5f, SweepAttackChargedFX));
                Debug.Log("Player Attacked with charged sweep attack");
                TimePassed = 0;
            }
        }
        //resets player speed to normal if mouse 1 is no longer held
        if (!Input.GetKey(KeyCode.Mouse1))
        {
            speed = SavedSpeed;
            ChargeEPCostPaid = false;
        }
        //enables guard if middle mouse is pressed down
        if (Input.GetKey(KeyCode.Mouse2) && CanMove == true)
        {
            GuardCol.enabled = true;
            GuardFX.SetActive(true);
            IsGuarding = true;
            //counts timing for a perfect guard
            PerfectGuardTime += Time.deltaTime;
        }
        //disables guard if middle mouse is let go
        if (Input.GetKeyUp(KeyCode.Mouse2))
        {
            GuardCol.enabled = false;
            GuardFX.SetActive(false);
            IsGuarding = false;
            PerfectGuardTime = 0;
        }
        //changes material color depending on if perfect guard is possible or not
        if (PerfectGuardTime <= 0.3f)
        {
            GuardFX.GetComponent<MeshRenderer>().material = PerfectGuardMat;
        }
        if (PerfectGuardTime > 0.3f)
        {
            GuardFX.GetComponent<MeshRenderer>().material = GuardMat;
        }
        //performs heal if the player pressed the 1 button
        if (Input.GetKeyDown(KeyCode.Alpha1) && PlayerHealing != true && CanMove == true && IsGuarding == false)
        {
            if (HealingItemACount > 0)
            {
                PlayerHealing = true;
                HealingType = 1;
                HealingItemACount = HealingItemACount - 1;
            }
        }
        if (Player.position.y <= -50)
        {
            Player.transform.position = new Vector3(0, 5, 0);
        }
        //tells other scripts if the player is locked onto something or not
        motor.LockOnState(IsLockedOn);
        FollowCam.getLockOn(IsLockedOn);
        //basicAttackCol.enabled = false;



    }



    private void OnTriggerStay(Collider other)
    {
        //checks if colliding collider is a targetable source and adds it to list of targetable sources
        if (!TargetColliders.Contains(other) && other.CompareTag("Target") == true && other.enabled == true) {
            TargetColliders.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //removes targetable source from list when out of range
        TargetColliders.Remove(other);
    }

    public void OnPauseClose()
    {
        PauseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnOptionsPressed()
    {
        PauseUI.SetActive(false);
        OptionsUI.SetActive(true);
    }

    public void OnPauseBackPressed()
    {
        PauseUI.SetActive(true);
        OptionsUI.SetActive(false);
    }

    public void OnReturnPressed()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator DisableAttack (Collider activeAttack, float attackDelay, GameObject AttackFX)
    {
            
        yield return new WaitForSeconds(attackDelay);
        activeAttack.enabled = false;
        if (AttackFX != null)
        {
            AttackFX.SetActive(false);
        }
        CanMove = true;
    }




}
