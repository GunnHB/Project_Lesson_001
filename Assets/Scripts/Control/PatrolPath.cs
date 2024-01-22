using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            for (int index = 0; index < transform.childCount; index++)
            {
                int nextIndex = GetNextIndex(index);

                Gizmos.color = Color.blue;

                Gizmos.DrawSphere(GetWayPoint(index), .15f);
                Gizmos.DrawLine(GetWayPoint(index), GetWayPoint(nextIndex));
            }
        }

        public int GetNextIndex(int index)
        {
            if (index + 1 == transform.childCount)
                return 0;

            return index + 1;
        }

        public Vector3 GetWayPoint(int index)
        {
            return transform.GetChild(index).position;
        }
    }
}