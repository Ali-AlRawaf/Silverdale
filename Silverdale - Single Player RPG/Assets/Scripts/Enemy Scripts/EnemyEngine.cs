using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEngine : MonoBehaviour
{    
    public GameObject eTarget;
    
    public float eHealth;
    public float eSpeed;
    public int eCoinReward;
    public float eDefense;
    public float eStrength;
    public float eRange;
    public float roamRange;
    public float coolDown;
    public int eWeak = 3; //0 - Weak to strength (bonus of +20; base of 45), 1 - Weak to defense, 2 - Weak to magic, 3 - null
    public int ePower = 3; //0 - Strong against strength (nerf of -20; base of 45), 1 - Strong against defense, 2 - Strong against magic, 3 - null
    public bool Aggro; //If enemy is aggressive
    public bool nAggro; //If enemy is normally aggressive

    int pDef;  
    float countDown;
    string questName;
    bool ded;

    [HideInInspector]
    public bool roaming;
    [HideInInspector]
    public Vector3 startPos;
    [HideInInspector]
    public GameObject spawner;

    Vector3 previous, velocity;
    Animator anim;

    GameObject player;
    AIPath aiPath;
    QuestManager theQM;
    SpawnEnemy spawnScript;
    RoamTarget eTar;
    StrengthSkill strSkill;
    DefenseSkill defSkill;
    MagicSkill magSkill;
    DialogManager theDM;

    public GameObject[] magEffects = new GameObject[4];
    
    void Start()
    {
        anim = gameObject.transform.GetChild(0).GetComponent<Animator>();

        foreach (Transform child in transform) if (child.CompareTag("Enemy"))
            {
                eHealth = 10f;
                eSpeed = 0.5f;
                eCoinReward = 5;
                eDefense = 3f;
                eStrength = 1f;
                eRange = 5.75f;
                roamRange = 1.5f;
                coolDown = 3f;
                eWeak = 2;
                ePower = 0;
                questName = "Enemy";
                Aggro = false;
            }
            else if (child.CompareTag("Chicken"))
            {
                eHealth = 3f;
                eSpeed = 0.75f;
                eCoinReward = 2;
                eDefense = 1f;
                eStrength = 1f;
                eRange = 3f;
                roamRange = 4f;
                coolDown = 2f;
                questName = "Chicken";
                Aggro = false;
            }
            else if (child.CompareTag("Zombie"))
            {
                eHealth = 10f;
                eSpeed = 0.3f;
                eCoinReward = 15;
                eDefense = 5f;
                eStrength = 5f;
                eRange = 5.75f;
                roamRange = 4f;
                coolDown = 3f;
                eWeak = 2;
                ePower = 0;
                questName = "Zombie";
                Aggro = true;
            }
            else if (child.CompareTag("Skeleton"))
            {
                eHealth = 15f;
                eSpeed = 0.75f;
                eCoinReward = 20;
                eDefense = 7f;
                eStrength = 7f;
                eRange = 5.75f;
                roamRange = 8f;
                coolDown = 1f;
                eWeak = 0;
                ePower = 2;
                questName = "Skeleton";
                Aggro = true;
            }
            else if (child.CompareTag("Goblin"))
            {
                eHealth = 20f;
                eSpeed = 0.6f;
                eCoinReward = 50;
                eDefense = 15f;
                eStrength = 15f;
                eRange = 3.75f;
                roamRange = 5f;
                coolDown = 2f;
                eWeak = 1;
                ePower = 0;
                questName = "Goblin";
                Aggro = false;
            }

        player = GameObject.FindGameObjectWithTag("Player");

        eTar = Instantiate(eTarget, transform.position, Quaternion.identity).GetComponent<RoamTarget>();
        eTar.spawner = spawner;
        eTar.enemy = gameObject;

        aiPath = GetComponent<AIPath>();
        theQM = FindObjectOfType<QuestManager>();
        theDM = FindObjectOfType<DialogManager>();
        spawnScript = spawner.GetComponent<SpawnEnemy>();
        strSkill = player.GetComponent<StrengthSkill>();
        defSkill = player.GetComponent<DefenseSkill>();
        magSkill = player.GetComponent<MagicSkill>();        

        nAggro = Aggro;
        countDown = coolDown;
        startPos = transform.position;
        aiPath.speed = eSpeed;
        roaming = true;
        pDef = DefenseSkill.defLvl;               
    }

    void Update()
    {
        velocity = (transform.position - previous) / Time.deltaTime;
        previous = transform.position;

        if (velocity.x < 0.05f && velocity.x > -0.05f && velocity.y < 0.05f && velocity.y > -0.05f)
            anim.SetInteger("Direction", 0);
        else if (velocity.y > 0.2f)
            anim.SetInteger("Direction", 1);
        else if (velocity.y < -0.2f)
            anim.SetInteger("Direction", 2);
        else if (velocity.x > 0.2f)
            anim.SetInteger("Direction", 3);
        else if (velocity.x < -0.2f)
            anim.SetInteger("Direction", 4);

        if (Aggro && Vector3.Distance(transform.position, player.transform.position) <= eRange)
        {
            roaming = false;
            FindPlayer();
            if (countDown >= coolDown)
            {
                if (Vector3.Distance(transform.position, player.transform.position) <= 0.5f)
                {
                    anim.SetBool("Attack", true);
                    AttackPlayer();
                }
                else
                    anim.SetBool("Attack", false);

                countDown = 0;
            }
            countDown += Time.deltaTime;
        }
        else
        {
            anim.SetBool("Attack", false);
            roaming = true;
            Aggro = nAggro;
        }

        if (eHealth <= 0)
        {
            if (!ded)
                PlayerEngine.pCash += eCoinReward;                
            theQM.enemyKilled = questName;            
            //theDM.dLines = new string[1];
            //theDM.dLines[0] = "You've gained " + eCoinReward + " ducats.";
            //theDM.currentLine = 0;
            //theDM.ShowDialog();          

            anim.SetBool("Dead", true);
            Destroy(gameObject, 1.4f);

            StrengthSkill.eDef = 0;
            StrengthSkill.strAtt = false;
            DefenseSkill.eDef = 0;
            DefenseSkill.defAtt = false;

            spawnScript.enemiesAlive--;
            ded = true;
        }
    }

    //Attacks player
    void AttackPlayer()
    {
        float aff = 55;
        float hitChance = aff * eStrength / (pDef + PlayerEngine.pArmourLvl * 2);
        float hit = Random.Range(0, 100);
        if (hit < hitChance)
        {
            float maxHit = Mathf.Ceil(0.5f * eStrength);            
            int hitAmount = Mathf.RoundToInt(Random.Range(1, maxHit));
            PlayerDamageController.CreateDamageText(hitAmount.ToString(), player.transform);
            PlayerHealth phealth = player.GetComponent<PlayerHealth>();
            phealth.LoseHealth(hitAmount);
        }
        else
            PlayerDamageController.CreateDamageText("0", player.transform);
    }

    //Finds players
    void FindPlayer()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= eRange)
            if (dist <= 0.4f)
                aiPath.target = transform;
            else
                aiPath.target = player.transform;
        else
        {
            roaming = true;
            Aggro = nAggro;
        }
            
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= 3.5f)
            {
                MagicSkill.magTar = transform;
                MagicSkill.eDef = Mathf.RoundToInt(eDefense);
                magSkill.Attack();
            }
            else
            {
                theDM.dLines = new string[1];
                theDM.dLines[0] = "You're too far away. Get closer!";
                theDM.currentLine = 0;
                theDM.ShowDialog();
                Debug.Log("Get Closer!");
            }
                                        
        }
            
    }

    void OnMouseDown()
    {
        if (CombatOption.isOffense)
        {
            strSkill.ePos = transform;
            StrengthSkill.strAtt = true;
            StrengthSkill.clickOnEnemy = true;
            StrengthSkill.eDef = Mathf.RoundToInt(eDefense);
        }
        else
        {
            defSkill.ePos = transform;
            DefenseSkill.defAtt = true;
            DefenseSkill.clickOnEnemy = true;
            DefenseSkill.eDef = Mathf.RoundToInt(eDefense);
        }        
    }
}