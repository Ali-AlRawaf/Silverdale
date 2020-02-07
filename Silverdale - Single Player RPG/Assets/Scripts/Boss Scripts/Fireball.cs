using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float projectileSpeed;

    GameObject impactEffect;
    Transform target;
    Vector3 dir;    
    float str;
    float countDown;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (countDown > 5f)
        {
            Destroy(gameObject);
        }

        transform.Translate(dir.normalized * projectileSpeed * Time.deltaTime, Space.World);
        countDown += Time.deltaTime;
    }

	void HitTarget()
    {
        float aff = 65;
        float hitChance = aff * (str + 5) / (DefenseSkill.defLvl + PlayerEngine.pArmourLvl * 2);
        float hit = Random.Range(0, 100);
        if (hit < hitChance)
        {
            float maxHit = Mathf.Ceil(0.5f * (str + 5f));
            int hitRoll = Mathf.RoundToInt(Random.Range(1, maxHit));
            PlayerDamageController.CreateDamageText(hitRoll.ToString(), target.transform);
            PlayerHealth phealth = target.GetComponent<PlayerHealth>();
            phealth.LoseHealth(hitRoll);
        }
        else
        {
            PlayerDamageController.CreateDamageText("0", target.transform);
        }
    }

    public void SeekTarget(Transform player, float _str)
    {
        target = player;
        str = _str;
        dir = target.position - transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            HitTarget();
            Destroy(gameObject);
        }
    }
}
