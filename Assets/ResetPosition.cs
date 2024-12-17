using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    void Update()
    {
        transform.position = transform.parent.position;
    }
}
