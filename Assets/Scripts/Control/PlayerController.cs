using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover _mover;
        private Fighter _fighter;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (InteractWithCombat())
                return;

            if (InteractWithMovement())
                return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (var item in hits)
            {
                if (item.transform.TryGetComponent(out CombatTarget target))
                {
                    if (Input.GetMouseButtonDown(0))
                        _fighter.Attack(target);

                    return true;
                }
                else
                    continue;
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            // 파라미터 힌트 ctrl + shift +spacebar
            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hitInfo);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                    _mover.StartMoveAction(hitInfo.point);

                return true;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
