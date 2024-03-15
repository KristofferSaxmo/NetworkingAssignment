using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SingleBulletDamage : MonoBehaviour
{
    [SerializeField] int damage = 5;
    public ulong shooterNetworkID;

    void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.transform.GetComponent<Health>();
        if(health == null) return;
        health.TakeDamage(damage, shooterNetworkID);
    }
}
