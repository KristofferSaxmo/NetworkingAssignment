using Unity.Mathematics;
using Unity.Netcode;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> currentHealth = new();
    public NetworkVariable<int> lives = new(3);

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        
        currentHealth.Value = 100;
    }

    
    public void TakeDamage(int damage)
    {
        if (!IsServer) return;
        
        Debug.Log("Player took damage!");
        
        currentHealth.Value -= damage;
        if (currentHealth.Value <= 0)
        {
            Die();
        }
    }
    
    public void Heal(int heal)
    {
        if (!IsServer) return;
        
        currentHealth.Value += heal;
        if (currentHealth.Value > 100)
            currentHealth.Value = 100;
    }

    private void Die()
    {
        if (!IsServer) return;
        
        lives.Value--;
        if (lives.Value > 0)
        {
            TriggerRespawnClientRpc();
            currentHealth.Value = 100;
        }
        else
        {
            NetworkObject networkObject = GetComponent<NetworkObject>();
            networkObject.Despawn();
        }
    }

    [ClientRpc]
    private void TriggerRespawnClientRpc()
    {
        if (!IsOwner) return;
        Respawn();
    }

    private void Respawn()
    {
        transform.position = Vector3.zero;
        transform.rotation = quaternion.identity;
    }
}
