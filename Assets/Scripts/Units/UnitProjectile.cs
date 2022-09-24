using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class UnitProjectile : NetworkBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float lifeTime = 5f;
    [SerializeField] float launchForce = 10f;


    private void Start() {
        rb.velocity = transform.forward * launchForce;
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), lifeTime);
    }

    [Server] void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

}
