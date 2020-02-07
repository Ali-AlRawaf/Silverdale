using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezekiel : MonoBehaviour
{
    public GameObject[] magEffects = new GameObject[4];

    [Header("Attributes")]
    public GameObject fireball;
    public Transform mouth;
    public float health;
    public float meleeCoolDown;
    public float rangeCoolDown;
    public float str;
    public float def;
    public int weak;
    public int pow;

    int pDef;
    float dist;
    float mCountDown;
    float rCountDown;
    string nameO;

    Animator bossAnim;
    GameObject player;
    QuestManager theQM;
    StrengthSkill strSkill;
    DefenseSkill defSkill;
    MagicSkill magSkill;
    DialogManager theDM;

    void Start()
    {
        player = FindObjectOfType<PlayerEngine>().gameObject;
        strSkill = player.GetComponent<StrengthSkill>();
        defSkill = player.GetComponent<DefenseSkill>();
        magSkill = player.GetComponent<MagicSkill>();
        theQM = FindObjectOfType<QuestManager>();
        theDM = FindObjectOfType<DialogManager>();
        pDef = DefenseSkill.defLvl;
        bossAnim = transform.GetChild(0).GetComponent<Animator>();

        nameO = "Ezekiel";
    }

    void Update()
    {
        dist = Vector2.Distance(transform.position, player.transform.position);        
        if (dist > 1 && dist <= 3.5 && rCountDown >= rangeCoolDown)
        {
            RangedAttack();
            rCountDown = 0;
        }
        else if (dist <= 1 && mCountDown >= meleeCoolDown)
        {
            MeleeAttack();
            mCountDown = 0;
        }

        if (rCountDown < rangeCoolDown)
            rCountDown += Time.deltaTime;

        if (mCountDown < meleeCoolDown)
            mCountDown += Time.deltaTime;

        if (health <= 0)
        {
            bossAnim.SetBool("Dead", true);
            theQM.enemyKilled = nameO;
            Destroy(gameObject, 3f);
        }
    }

    void MeleeAttack()
    {
        str = 30;
        def = 30;
        weak = 1;

        float aff = 65;
        float hitChance = aff * (str + 5) / (pDef + PlayerEngine.pArmourLvl * 2);
        float hit = Random.Range(0, 100);
        if (hit < hitChance)
        {
            float maxHit = Mathf.Ceil(0.5f * (str + 5f));
            int hitRoll = Mathf.RoundToInt(Random.Range(1, maxHit));
            PlayerDamageController.CreateDamageText(hitRoll.ToString(), player.transform);
            PlayerHealth phealth = player.GetComponent<PlayerHealth>();
            phealth.LoseHealth(hitRoll);
        }
        else
        {
            PlayerDamageController.CreateDamageText("0", player.transform);
        }
    }

    void RangedAttack()
    {
        //Rotate Object; please ignore
        Vector3 pla = player.transform.position;
        pla.z = 0;
        pla.x = pla.x - mouth.position.x;
        pla.y = pla.y - mouth.position.y;
        float angle = Mathf.Atan2(pla.y, pla.x) * Mathf.Rad2Deg;
        StartCoroutine(Shoot(angle)); 
    }

    IEnumerator Shoot(float angle)
    {
        bossAnim.SetBool("Shoot", true);

        yield return new WaitForSeconds(0.5f);

        GameObject fire = Instantiate(fireball, mouth.position, Quaternion.Euler(new Vector3(0, 0, angle)));
        Fireball fball = fire.GetComponent<Fireball>();
        str = 20;
        def = 20;
        weak = 2;
        if (fball != null)
            fball.SeekTarget(player.transform, str);

        bossAnim.SetBool("Shoot", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1f);
        Gizmos.DrawWireSphere(transform.position, 3.5f);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (dist <= 3.5f)
            {
                MagicSkill.magTar = transform;
                MagicSkill.eDef = Mathf.RoundToInt(def);
                magSkill.Attack();
            }
            else
            {
                theDM.dLines = new string[1];
                theDM.dLines[0] = "You're too far away. Get closer!";
                theDM.currentLine = 0;
                theDM.ShowDialog();
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
            StrengthSkill.eDef = Mathf.RoundToInt(def);
        }
        else
        {
            defSkill.ePos = transform;
            DefenseSkill.defAtt = true;
            DefenseSkill.clickOnEnemy = true;
            DefenseSkill.eDef = Mathf.RoundToInt(def);
        }
    }
}