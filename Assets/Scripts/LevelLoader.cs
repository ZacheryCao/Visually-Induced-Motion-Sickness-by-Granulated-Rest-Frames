using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Text timerText;

    [SerializeField]
    private string scene = "";

    float time;

    private void Start()
    {
        time = 30.0F;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if ((int)time > 9)
        {
            timerText.text = "00:" + ((int)time).ToString();
        }
        else
        {
            timerText.text = "00:0" + ((int)time).ToString();
        }

        if(Input.GetKeyDown(KeyCode.Q) && (int)time <= 0)
        {
            LoadLevel(scene);
        }
    }


    public void LoadLevel(string scene)
    {
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
    }
}
