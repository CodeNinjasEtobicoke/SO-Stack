using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HslfwayPoint : MonoBehaviour
{
    public Transform red;
    public Transform blue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Calculate();
    }
    void Calculate()
    {
        Vector3 diff = red.position - blue.position;
        diff = diff / 2f;

        transform.position = (diff + blue.position);
    }
}