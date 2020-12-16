using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour
{
    public GameObject AttackData;
    [SerializeField]
    private Collider HitCollider;
    public float PlayerHealth = 100f;
    [SerializeField]
    private float PlayerMaxHealth = 100f;
    private EnemyAttackData attackData;
    public bool IsHit = false;
    private List<GameObject> attackHits = new List<GameObject>();
    public bool DisableColliders;
    [SerializeField]
    private PlayerController PlayerControl;
    [SerializeField]
    private Text HPText;
    [SerializeField]
    private Image BlackScreen;
    private float DamageTaken;
    private float FlinchTaken;

    // Start is called before the first frame update
    void Start()
    {
        //gets the player controller script from the player
        PlayerControl = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //prevents players health from overflowing from max
        if (PlayerHealth > PlayerMaxHealth)
        {
            PlayerHealth = PlayerMaxHealth;
        }
        //checks if there has been a registered attack on the player and returns that the player has been hit
        if (attackHits.Count >= 1)
        {
            IsHit = true;
        }
        //checks if the player has run out of health
        if (PlayerHealth <= 0f)
        {
            Debug.Log("Player Has Died");
            PlayerControl.CanMove = false;
            PlayerControl.IsGuarding = true;
            BlackScreen.CrossFadeAlpha(2.0f, 2.0f, false);
            StartCoroutine(ReturnToMenu(4f));
        }
        //removes excess hits from the list of ones that landed 
        if (attackHits.Count >= 2)
        {
            attackHits.Remove(attackHits.Last());
        }
        //checks if there is only one attack left in the list and executes the takedamage function with that attack's data
        if (attackHits.Count == 1)
        {
            TakeDamage(attackHits[0]);
            attackHits.Remove(attackHits[0]);
            IsHit = false;
        }
        //activates whenever the player is healing to prevent them from moving while restoring their health
        if (PlayerControl.PlayerHealing == true)
        {
            PlayerControl.CanMove = false;
            StartCoroutine(UseHealingItem(1.5f, PlayerControl.HealingType));
            PlayerControl.PlayerHealing = false;
        }
        //displays players HP on UI in-game
        HPText.text = "HP: " + PlayerHealth.ToString() + " / " + PlayerMaxHealth.ToString();
    }

    public void TakeDamage(GameObject HitData)
    {
        //gets the hit data from the enemy that attacked the player
        attackData = HitData.GetComponent<EnemyAttackData>();
        //fills in variables with the values from the hit data
        DamageTaken = attackData.DamageValue;
        FlinchTaken = attackData.FlinchPower;
        //if the player perfect guarded the attack, sets the variables to 0
        if (PlayerControl.GuardedAttack == true && PlayerControl.PerfectGuardTime <= 0.3f)
        {
            DamageTaken = 0;
            FlinchTaken = 0;
            Debug.Log("Player Perfect Guarded");
        }
        //if the player normal guarded the attack, reduces the damage taken by half
        if (PlayerControl.GuardedAttack == true)
        {
            DamageTaken = DamageTaken / 2;
        }
        //checks if the amount of damage taken from a hit is more than zero
        if (DamageTaken > 0)
        {
            //removes health from the players current health based on how much damage they took
            PlayerHealth = PlayerHealth - DamageTaken;
            //checks if player has wave attack upgrade and removes it if so
            if (PlayerControl.WaveAttackActive == true)
            {
                PlayerControl.WaveAttackActive = false;
            }
            Debug.Log("Player Health: " + PlayerHealth);
            //checks the value for how much time the player should spend flinched from the attack
            if (FlinchTaken == 1)
            {
                PlayerControl.CanMove = false;
                StartCoroutine(ExitFlinchState(1f));
            }
            if (FlinchTaken >= 2)
            {
                PlayerControl.CanMove = false;
                StartCoroutine(ExitFlinchState(3f));
            }
        }
        PlayerControl.GuardedAttack = false;
    }

    //waits a set amount of time and then allows the player to move again
    IEnumerator ExitFlinchState(float flinchTime)
    {
        yield return new WaitForSeconds(flinchTime);
        PlayerControl.CanMove = true;
        Debug.Log("Player Finished flinch");
    }

    //checks the variable type of the healing item used and restores the proper amount of health to the player
    IEnumerator UseHealingItem(float UseTime, int ItemType)
    {
        yield return new WaitForSeconds(UseTime);
        if (ItemType == 1)
        {
            PlayerHealth = PlayerHealth + (PlayerMaxHealth / 2);
            ItemType = 0;
        }
        PlayerControl.CanMove = true;
        
    }

    IEnumerator ReturnToMenu(float TimeDelay)
    {
        yield return new WaitForSeconds(TimeDelay);
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        //checks if something has hit the player and they are not currently in the state of being hit already
        if (other.CompareTag("EnemyAttack") == true && IsHit == false)
        {
            Debug.Log("Player Hit");
            //gets the gameobject of what hit the player and adds it to the list
            AttackData = other.gameObject;
            attackHits.Add(AttackData);
        }
        //cheecks if player collided with healing object and adds it to their amount of healing items
        if (other.gameObject.CompareTag("CollectableItemHealing") == true)
        {
            PlayerControl.HealingItemACount = PlayerControl.HealingItemACount + 1;
        }
        if (other.gameObject.CompareTag("CollectableItemCombat") == true)
        {
            PlayerControl.WaveAttackActive = true;
        }
    }
}
