using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField]
    private float
        maxDistance,
        maxDistanceModifier,
        minDistanceModifier,
        jointSpringValue,
        jointDamperValue,
        jointMassScaleValue;

    [SerializeField]
    private LayerMask grappleLayerMask;
    [SerializeField]
    private Transform shootPointTransform;

    public bool IsGrappling { get; private set; }

    public Vector3 GrapplePoint { get; private set; }

    private Transform cameraTransform;
    private LineRenderer lineRenderer;
    private SpringJoint joint;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        cameraTransform = Camera.main.transform;
    }


    private void LateUpdate()
    {
        DrawRope();
    }


    public void StartGrapple()
    {
        IsGrappling = true;

        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, maxDistance, grappleLayerMask))
        {
            GrapplePoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = GrapplePoint;

            float distanceFromPoint = Vector3.Distance(transform.position, GrapplePoint);

            joint.maxDistance = distanceFromPoint * maxDistanceModifier;
            joint.minDistance = distanceFromPoint * minDistanceModifier;

            joint.spring = jointSpringValue;
            joint.damper = jointDamperValue;
            joint.massScale = jointMassScaleValue;

            lineRenderer.positionCount = 2;
        }
    }


    public void StopGrapple()
    {
        IsGrappling = false;

        lineRenderer.positionCount = 0;

        if (!joint)
            return;

        Destroy(joint);
    }


    private void DrawRope()
    {
        if (!IsGrappling || !joint)
            return;

        if (!lineRenderer.enabled)
            lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, shootPointTransform.position);
        lineRenderer.SetPosition(1, GrapplePoint);
    }
}
