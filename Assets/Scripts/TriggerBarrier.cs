using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBarrier : MonoBehaviour
{
    public GameObject[] barriers;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Heyhey");
        if (other.gameObject.tag == "Player")
        {
            barriers[1].SetActive(true);
            barriers[0].SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
