using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class UnitFiring :NetworkBehaviour
{
    [SerializeField] Targeter  targeter = null;
    [SerializeField] GameObject projectilePrefab = null;
    [SerializeField] Transform projectileSpawnLoc = null;
    [SerializeField] float fireRange = 5f;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float rotationSpeed = 20f;
    float lastFireTime;

    [Server] bool canFireTarget()
    {
        return (targeter.GetTarget().transform.position - transform.position).sqrMagnitude <= fireRange * fireRange;
    }

    [ServerCallback] private void Update() 
    {
        Targetable target = targeter.GetTarget();

        if (target == null) { return; }

        if (!canFireTarget()) { return; }

        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Time.time > (1/fireRate) + lastFireTime)
        {
            Quaternion projectileRotation = Quaternion.LookRotation(target.gameObject.GetComponent<Targetable>().GetAimAtPoint().position - projectileSpawnLoc.position);

            GameObject projectileInstance = Instantiate(projectilePrefab, projectileSpawnLoc.position, projectileRotation);

            NetworkServer.Spawn(projectileInstance, connectionToClient);

            lastFireTime = Time.time;
        }
    }
}
