using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] Image aimReticle;
    [SerializeField] Image hipFireReticle;

    private Color reticleColor;

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

        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, maxDistance, grappleLayerMask))
        {
            reticleColor = Color.green;
        }
        else
        {
            reticleColor = Color.red;
        }

        hipFireReticle.color = reticleColor;
        aimReticle.color = reticleColor;

    }

    public void StartGrapple()
    {
        if (GameManager.Instance.currentState != GameManager.GameState.Playing)
            return;


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

            SoundManager.Instance.PlaySound(SoundManager.Sound.GrappleShoot);
            Invoke(nameof(GrappleShotFlyingSound), time: 0.02f);
        }
        else
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.GrappleMiss);
        }
    }

    void GrappleShotFlyingSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.GrappleShotFlying);
    }
    public void StopGrapple()
    {
        if (GameManager.Instance.currentState != GameManager.GameState.Playing)
            return;

        if (!IsGrappling) return;


        lineRenderer.positionCount = 0;

        
        var joints = GetComponents<SpringJoint>();

        foreach (var springJoint in joints)
        {
            Destroy(springJoint);
        }

        SoundManager.Instance.PlaySound(SoundManager.Sound.GrappleRelease);
        IsGrappling = false;

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
