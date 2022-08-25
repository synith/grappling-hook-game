using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepChecker : MonoBehaviour
{
    [SerializeField] Transform leftFoot, rightFoot;

    [SerializeField, Range(-20, 20)] float leftFootThreshold, rightFootThreshold;

    [SerializeField] bool leftFootDown, rightFootDown;

    private void Update()
    {
        CheckFootPosition(leftFootDown, leftFoot.localEulerAngles.x, leftFootThreshold);
        CheckFootPosition(rightFootDown, rightFoot.localEulerAngles.x, rightFootThreshold);
    }

    private void CheckFootPosition(bool footDown, float footPosition, float footThreshold)
    {
        if (!footDown && footPosition <= footThreshold)
        {
            footDown = true;
            Debug.Log("Down!");
        }
        if (footDown && footPosition >= footThreshold)
        {
            footDown = false;
            Debug.Log("Up!");
        }
    }
}
