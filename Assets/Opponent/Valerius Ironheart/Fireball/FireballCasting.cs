using UnityEngine;

public class FireballCasting : Attack
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject fireballPrefab;
    [SerializeField]
    GameObject castingBase;
    [SerializeField]
    GameObject trajectoryModifier;
    [SerializeField]
    float ParryTime;
    [SerializeField]
    float duration;

    new void Start()
    {
        base.Start();
    }

    void Update()
    {
        UpdateCooldown();
    }

    override public void UseAttack()
    {

        if (SetBusy(true))
        {
            GameObject fireball = Instantiate(fireballPrefab, castingBase.transform.position, castingBase.transform.rotation);
            fireball.GetComponent<Fireball>().SetValues(characterManager,
                                                        controller,
                                                        player,
                                                        trajectoryModifier,
                                                        damage,
                                                        ParryTime);
            fireball.GetComponent<Fireball>().SetAttack(this);
            cooldownLeft = cooldown;
            Invoke("Interrupt", duration);
            animator.Play("FireballCasting");
        }
    }

    override public void Interrupt()
    {
        NotBusy();
    }
}
