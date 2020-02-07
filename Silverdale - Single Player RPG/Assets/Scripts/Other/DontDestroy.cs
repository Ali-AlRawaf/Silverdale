using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DontDestroy : MonoBehaviour
{
    public static bool exists;

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
