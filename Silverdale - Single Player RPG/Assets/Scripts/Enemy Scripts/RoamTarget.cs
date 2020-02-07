using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamTarget : MonoBehaviour
{
    public GameObject enemy;
    public GameObject spawner;

    Vector3 startPos;
    EnemyEngine eEng;
    AIPath aiPath;

	// Use this for initialization
	void Start()
    {        
        startPos = spawner.transform.position;
        eEng = enemy.GetComponent<EnemyEngine>();
        aiPath = enemy.GetComponent<AIPath>();
        StartCoroutine(RoamRange());
    }

    void Update()
    {
        if (enemy == null)
            Destroy(gameObject);
    }

    IEnumerator RoamRange()
    {        
        if (eEng.roaming)
        {
            float x = Random.Range(-1 * eEng.roamRange, eEng.roamRange);
            float y = Random.Range(-1 * eEng.roamRange, eEng.roamRange);
            Vector3 newVec = new Vector3(x + startPos.x, y + startPos.y, 0);
            transform.position = newVec;
            aiPath.target = transform;
        }
        yield return new WaitForSeconds(8);
        StartCoroutine(RoamRange());
    }
}
