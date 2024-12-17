using Gamekit3D;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] Attack swordAttack;
    [SerializeField] HealingAbility heal;
    [SerializeField] InputReader input;
    //[SerializeField] Animator animator;


    bool isBlocking;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessBlocking();
        heal.IsHealing = Input.GetKey(KeyCode.F) && !isBlocking;
    }

    private void ProcessBlocking()
    {
        bool oldIsBlocking = isBlocking;
        //isBlocking = Input.GetKey(KeyCode.Mouse1) && !IsAttacking ? true : false;
        //if (isBlocking && !oldIsBlocking)
        //{
        //    StartBlockingTime = Time.time;
        //}
        //else if (!isBlocking && oldIsBlocking)
        //{
        //    StartBlockingTime = Mathf.Infinity;
        //}
    }
}
