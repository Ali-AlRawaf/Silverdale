using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadForObjects : MonoBehaviour
{
    static bool exists;

    void Start()
    {
        if (!exists)
        {
            exists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
