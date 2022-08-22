using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField]
    Transform followTargetPosition;

    private void Update()
    {
        transform.SetPositionAndRotation(followTargetPosition.position, followTargetPosition.rotation);
    }
}
