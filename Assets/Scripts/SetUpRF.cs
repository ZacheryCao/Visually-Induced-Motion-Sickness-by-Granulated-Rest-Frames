using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sigtrap.VrTunnellingPro;
using System.Linq;
using System.Text;
using System.IO;

public class SetUpRF : MonoBehaviour
{
    [SerializeField]
    private float smoothDensity = 0.1F;
    [SerializeField]
    private float smoothAugularSize = 0.05F;
    [SerializeField]
    private float smoothTransparency = 0.05F;
    [SerializeField]
    private bool _setParam = false;
    [SerializeField][Tooltip("Disable VRTP Script to Setup RestFrames Parameters. Otherwise, while setting up the parameters with VRTP.")]
    private bool _vRTPDisable = true;
    [SerializeField]
    private RestFramesGenerator RestFrames;
    [SerializeField]
    private GameObject cages;
    [SerializeField]
    private Tunnelling VRTP;
    string paraFilePath = "Assets/CSV/" + "Save_Inventory.csv";
    // Start is called before the first frame update
    void Start()
    {
        VRTP.enabled = !_vRTPDisable;
        cages.SetActive(_vRTPDisable);
        if (_vRTPDisable)
        {
            int i = 0, n = RestFrames.transform.childCount;
            foreach(Transform child in RestFrames.transform)
            {
                if (i < n)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    break;
                }
                i++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_setParam && RestFrames.angularSize > 0 && !Input.GetKey(KeyCode.LeftShift)&&(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                RestFrames.angularSize += smoothAugularSize;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && RestFrames.angularSize > 0)
            {
                RestFrames.angularSize -= smoothAugularSize;
                RestFrames.angularSize = Mathf.Max(0, RestFrames.angularSize);
            }
            ClearChildren();
            cages.SetActive(true);
            RestFrames.Start();
            if (!_vRTPDisable)
            {
                cages.SetActive(false);
            }
        }

        if (_setParam && RestFrames.densityPercentage > 0 && ((Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift))|| (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftShift))))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)&&Input.GetKey(KeyCode.LeftShift))
            {
                RestFrames.densityPercentage += smoothDensity;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftShift) && RestFrames.densityPercentage > 0)
            {
                RestFrames.densityPercentage -= smoothDensity;
                RestFrames.densityPercentage = Mathf.Max(0,RestFrames.densityPercentage);
            }
            ClearChildren();
            cages.SetActive(true);
            RestFrames.Start();
            if (!_vRTPDisable)
            {
                cages.SetActive(false);
            }
        }

        if (_setParam &&(Input.GetKeyDown(KeyCode.LeftArrow)|| Input.GetKeyDown(KeyCode.RightArrow)) && RestFrames.transparency > 0.000F)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && RestFrames.transparency > smoothTransparency)
            {
                RestFrames.transparency -= smoothTransparency;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) )
            {
                RestFrames.transparency = Mathf.Min(1, RestFrames.transparency+ smoothTransparency);
            }
            RestFrames.material.SetColor("_Color", new Color(0, 0, 0, Mathf.Max(1.0F - RestFrames.transparency,0)));
            RestFrames.material.SetColor("_Color", new Color(0, 0, 0, Mathf.Max(1.0F - RestFrames.transparency,0)));
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            WriteCSVFile(paraFilePath);
        }
    }


    void ClearChildren()
    {
        foreach (Transform child in RestFrames.transform)
        {
            Destroy(child.gameObject);
            //child.gameObject.SetActive(false);
        }
    }

    void WriteCSVFile(string path)
    {
        if (!File.Exists(path))
        {
            StreamWriter writer = new StreamWriter(path);
            writer.WriteLine("Density, AngularSize, Transparency, Time");
            writer.Flush();
            writer.Close();
        }
        string parameters = RestFrames.densityPercentage.ToString() + "," + RestFrames.angularSize.ToString() + "," + RestFrames.transparency.ToString()+","+System.DateTime.Now.ToString();
        File.AppendAllText(path, parameters);
    }
}
