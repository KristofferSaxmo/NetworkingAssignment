using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Medkit : NetworkBehaviour
{
    [SerializeField] GameObject medkitPrefab;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;
        
        Health health = other.GetComponent<Health>();
        if (!health) return;

        if (health.currentHealth.Value == 100) return;
        
        health.Heal(25);
        
        int xPosition = Random.Range(-4, 4);
        int yPosition = Random.Range(-2, 2);


        GameObject newMedkit = Instantiate(medkitPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
        NetworkObject no = newMedkit.GetComponent<NetworkObject>();
        no.Spawn();

        NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
        networkObject.Despawn();
    }
}
