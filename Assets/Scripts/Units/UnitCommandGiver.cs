using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask = new LayerMask();
    [SerializeField] private UnitSelectionHandler unitSelectionHandler = null;

    private Camera mainCam;

    private void Start() {
        mainCam = Camera.main;
    }

    private void Update() {
        if (!Mouse.current.rightButton.wasPressedThisFrame) {return;}

        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }

         if (hit.collider.TryGetComponent<Targetable>(out Targetable target))
        {
            if (target.hasAuthority)
            {
                TryMove(hit.point);
                return;
            }

            TryTarget(target);
            return;
        }


        TryMove(hit.point);
    }

    private void TryTarget(Targetable target)
    {
        foreach (Unit unit in unitSelectionHandler.SelectedUnits)
        {
            unit.GetTargeter().CmdSetTarget(target.gameObject);
        }

    }

    private void TryMove(Vector3 point)
    {
        foreach (Unit unit in unitSelectionHandler.SelectedUnits)
        {
            unit.GetUnitMovement().CmdMove(point);
        }
    }
}
