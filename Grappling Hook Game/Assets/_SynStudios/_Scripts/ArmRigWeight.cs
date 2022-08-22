using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ArmRigWeight : MonoBehaviour
{
    float timer = 0;
    float timerMax = 0.3f;

    bool isGrappling;

    Rig armRig;

    private void Awake()
    {
        armRig = GetComponent<Rig>();
    }
    private void OnEnable()
    {
        PlayerInput.OnStartGrapple += StartGrappleAnimation;
        PlayerInput.OnStopGrapple += StopGrappleAnimation;
    }

    private void OnDisable()
    {
        PlayerInput.OnStartGrapple -= StartGrappleAnimation;
        PlayerInput.OnStopGrapple -= StopGrappleAnimation;
    }

    private void StopGrappleAnimation()
    {
        isGrappling = false;
    }

    private void StartGrappleAnimation()
    {
        isGrappling = true;
    }

    private void Update()
    {
        if (isGrappling)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        timer = Mathf.Clamp(timer, 0, timerMax);

        armRig.weight = Mathf.Lerp(0, 1, timer / timerMax);
    }
}
