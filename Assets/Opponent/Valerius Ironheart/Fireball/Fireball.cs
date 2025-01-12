using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    float startSpeed;
    [SerializeField]
    float targetingSpeed;
    [SerializeField]
    float accelerationTime;

    Attack attack;
    GameObject target;
    GameObject trajectoryModifier;

    public CharacterManager characterManager;
    BaseContoller controller;
    float timePassed;
    float currentSpeed;
    float damage;
    float parryTime;
    private bool toDestroy = false;
    private bool valuesSet = false;

    List<Vector3> positions;

    internal void SetValues(CharacterManager Cm,
                            BaseContoller Controller,
                            GameObject Player,
                            GameObject TrajectoryModifier,
                            float Damage,
                            float ParryTime)
    {
        characterManager = Cm;
        controller = Controller;
        target = Player;
        trajectoryModifier = TrajectoryModifier;
        damage = Damage;
        parryTime = ParryTime;
        valuesSet = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timePassed = 0;
        positions = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!valuesSet)
        {
            return;
        }
        if (toDestroy)
        {
            Destroy(this.gameObject);
            return;
        }
        if (timePassed < accelerationTime) timePassed += Time.deltaTime;
        float accelerationPercentage = timePassed / accelerationTime;
        if (accelerationPercentage > 1) accelerationTime = 1;
        //currentSpeed = Mathf.Lerp(startSpeed, targetingSpeed, accelerationPercentage);
        currentSpeed = accelerationPercentage < 1 ?startSpeed : targetingSpeed;
        transform.position = Vector3.MoveTowards(transform.position,
                                       trajectoryModifier.transform.position, 
                                       currentSpeed * Time.deltaTime * (1 - accelerationPercentage));
        transform.position = Vector3.MoveTowards(transform.position,
                                       target.transform.position + new Vector3(0,1.5f,0), 
                                       currentSpeed * Time.deltaTime * accelerationPercentage);
        positions.Insert(0, transform.position);
    }

    bool CorrectParry(float hitTime)
    {
        return Time.time - hitTime < parryTime;
    }

    protected void OnTriggerEnter(Collider other)
    {
        CharacterManager enemy = other.GetComponent<CharacterManager>();
        if (enemy == null || enemy == characterManager) 
        {
            return;
        }

        float hit = enemy.Hit(characterManager, damage, true, positions[3]);

        if (hit != Attack.SUCCESS && CorrectParry(hit))
        {
            controller.GetStunned();
        }
        // TOD: Don't use Destroy, create manager instead
        if (other.gameObject == target)
        {
            toDestroy = true;
        }
    }

    internal void SetAttack(Attack a)
    {
        attack = a;
    }
}
