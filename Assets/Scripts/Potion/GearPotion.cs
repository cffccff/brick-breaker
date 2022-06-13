using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearPotion : MonoBehaviour
{
    private Rigidbody2D rg;
    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        rg.AddForce(Vector3.down * 0.5f * rg.mass);
    }
}
