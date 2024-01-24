using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeProjectile : MonoBehaviour
{
    float projectileSpeed;
    float projectileMaxDistance;
    Vector3 startingPosition;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        player = GameObject.Find("Player").transform;
        transform.LookAt(player.transform); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
        if (Vector3.Distance(startingPosition, transform.position) > projectileMaxDistance)
        {
            Destroy(this.gameObject);
        }
    }
    public void SetMembers(float ProjectileSpeed, float ProjectileMaxDistance )
    {
        projectileSpeed = ProjectileSpeed;
        projectileMaxDistance = ProjectileMaxDistance;
    }
}
