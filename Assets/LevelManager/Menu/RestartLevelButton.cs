using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartLevelButton : MonoBehaviour
{
    [SerializeField]
    string Name;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(RestartLevel);
    }
    public void RestartLevel() 
    {
        Debug.Log(Name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
