using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRemain : MonoBehaviour
{
    private static BlockRemain _instance;
    public static BlockRemain Instance => _instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        //first instance should be kept and do NOT destroy it on load
        _instance = this;
         DontDestroyOnLoad(_instance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
