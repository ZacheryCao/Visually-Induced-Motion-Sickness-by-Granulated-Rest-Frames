using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonClick : MonoBehaviour
{
    public KeyCode[] keys;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(KeyCode i in keys)
        {
            if (Input.GetKeyDown(i))
            {
                button.onClick.Invoke();
            }
        }
    }
}
