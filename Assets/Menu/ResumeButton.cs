using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    [SerializeField]
    OverlayMenu overlay;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ResumeLevel);
    }
    public void ResumeLevel()
    {
        overlay.ChangeState(OverlayMenu.INGAME);
    }
}
