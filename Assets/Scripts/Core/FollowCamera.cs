using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        // 캐릭터가 이동하기 시작한 후 카메라가 따라가도록 LateUpdate에서 호출
        private void LateUpdate()
        {
            transform.position = _target.position;
        }
    }

}