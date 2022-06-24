using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField]
    private float
        _maxDistance,
        _maxDistanceModifier,
        _minDistanceModifier,
        _jointSpringValue,
        _jointDamperValue,
        _jointMassScaleValue;

    [SerializeField]
    private LayerMask _grappleLayerMask;
    [SerializeField]
    private Transform _shootPointTransform;

    public bool IsGrappling { get; private set; }
    public Vector3 GrapplePoint { get; private set; }

    private LineRenderer _lineRenderer;
    private Transform _cameraTransform;
    private SpringJoint _joint;



    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _cameraTransform = Camera.main.transform;
    }

    public void StartGrapple()
    {
        IsGrappling = true;

        RaycastHit hit;
        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, _maxDistance, _grappleLayerMask))
        {
            GrapplePoint = hit.point;
            _joint = gameObject.AddComponent<SpringJoint>();
            _joint.autoConfigureConnectedAnchor = false;
            _joint.connectedAnchor = GrapplePoint;

            float distanceFromPoint = Vector3.Distance(transform.position, GrapplePoint);

            _joint.maxDistance = distanceFromPoint * _maxDistanceModifier;
            _joint.minDistance = distanceFromPoint * _minDistanceModifier;

            _joint.spring = _jointSpringValue;
            _joint.damper = _jointDamperValue;
            _joint.massScale = _jointMassScaleValue;

            _lineRenderer.positionCount = 2;
        }

    }

    private void DrawRope()
    {
        if (!IsGrappling) return;
        _lineRenderer.SetPosition(0, _shootPointTransform.position);
        _lineRenderer.SetPosition(1, GrapplePoint);
    }

    public void StopGrapple()
    {
        IsGrappling = false;

        _lineRenderer.positionCount = 0;
        if (!_joint) return;
        Destroy(_joint);

    }

    private void LateUpdate()
    {
        DrawRope();
    }

}