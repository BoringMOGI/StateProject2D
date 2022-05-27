using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(2, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += new Vector3(1 * Time.deltaTime, 0, 0);
        //transform.Translate(new Vector3(1 * Time.deltaTime, 0, 0));
    }
}
