using UnityEngine;

public class InputForFireball : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<FireballCasting>().UseAttack();
        }
    }
}
