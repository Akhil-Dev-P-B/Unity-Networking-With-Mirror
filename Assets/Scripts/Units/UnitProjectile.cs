using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class UnitProjectile : NetworkBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float lifeTime = 5f;
    [SerializeField] float launchForce = 10f;
    [SerializeField] int damageValue = 20;


    private void Start() {
        rb.velocity = transform.forward * launchForce;
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), lifeTime);
    }

    [ServerCallback] private void OnTriggerEnter(Collider other) 
    {
        if (other.TryGetComponent<NetworkIdentity>(out NetworkIdentity netid))
        {
            if (netid.connectionToClient == connectionToClient) { return; }
        }

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDAmage(damageValue);
        }

        DestroySelf();
    }

    [Server] void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

}
