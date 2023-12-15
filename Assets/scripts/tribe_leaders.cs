using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribeLeaders : Unit
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None; // Adjust as needed
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints.FreezePositionX;
        rb.constraints = RigidbodyConstraints.FreezePositionY;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void AttackPointScaling()
    {
        maxAttackPoints = baseAttackPoints * level;
    }
    public override void HitPointScaling()
    {
        maxHitPoints = baseHitPoints * level;
    }
    public override void DefenseScaling()
    {
        maxDefense = baseDefense * level;
    }
    public override void SpeedScaling()
    {
        maxSpeed = baseSpeed * level;
    }
}