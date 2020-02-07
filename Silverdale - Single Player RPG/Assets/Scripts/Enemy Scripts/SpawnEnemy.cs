using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public int enemyCap;
    public float radius;
    public GameObject enemy;
    public GameObject eClone;
   
    [HideInInspector]
    public int enemiesAlive;    

    EnemyEngine eEng;

    void Start()
    {

        //for (int i = 0; i < enemyCap; i++)
        //{
        //    float spawnPosx = Random.Range(-1 * radius, radius);
        //    float spawnPosy = Random.Range(-1 * radius, radius);
        //    eClone = Instantiate(enemy, new Vector2(spawnPosx + transform.position.x, spawnPosy + transform.position.y), Quaternion.identity);
        //    eClone.GetComponent<EnemyEngine>().spawner = gameObject;
        //    enemiesAlive++;
        //}
        StartCoroutine(CheckEnemiesAlive());
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    IEnumerator CheckEnemiesAlive()
    {
        if (enemiesAlive < enemyCap)
        {            
            float spawnPosx = Random.Range(-1 * radius, radius);
            float spawnPosy = Random.Range(-1 * radius, radius);
            eClone = Instantiate(enemy, new Vector2(spawnPosx + transform.position.x, spawnPosy + transform.position.y), Quaternion.identity);
            eClone.GetComponent<EnemyEngine>().spawner = gameObject;
            enemiesAlive++;
            yield return new WaitForSeconds(1);
        }
        
        yield return new WaitForSeconds(10);
        StartCoroutine(CheckEnemiesAlive());
    }
}
