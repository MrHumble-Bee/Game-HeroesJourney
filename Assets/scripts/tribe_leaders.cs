using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribeLeaders : Hero
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        // RecoverAllStats();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None; // Adjust as needed
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints.FreezePositionX;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;

    }

    protected override void AttackPointScaling()
    {
        maxAttackPoints = baseAttackPoints * level;
    }
    protected override void HitPointScaling()
    {
        maxHitPoints = baseHitPoints * level;
    }
    protected override void DefenseScaling()
    {
        maxDefense = baseDefense * level;
    }
    protected override void SpeedScaling()
    {
        maxSpeed = baseSpeed * level;
    }
}
