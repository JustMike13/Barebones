using UnityEngine;

public class MeshController : MonoBehaviour
{
    void Update()
    {
        transform.position = transform.parent.position;
    }
    public void SetIsBusy()
    {
        this.GetComponentInParent<ThirdPersonController>().IsBusy = true;
    }

    public void ResetIsBusy()
    {
        this.GetComponentInParent<ThirdPersonController>().IsBusy = false;
    }
}