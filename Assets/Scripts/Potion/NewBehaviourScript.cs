using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    List<float> list;
    // Start is called before the first frame update
    void Start()
    {
        list = new List<float>();
        list.Add(1f);
        list.Add(3f);
        list.Add(5f);
        list.Add(7f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            list.RemoveAt(0);
           
        }
        if (Input.GetKey(KeyCode.A))
        {
            foreach (float number in list)
            {
                Debug.Log(number);
            }

        }
    }

}
