using UnityEngine;
using UnityEngine.Events;

public class BoxBehavior : MonoBehaviour
{
    public BoxPooler boxPooler;
    public Rigidbody playerRb;
    private Rigidbody rb;
    private float maxSpeed = 5;

    public UnityEvent boxEvent;
    public LineRendererControl lineRendererControl;
    public LineRenderer lineRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            boxPooler.HandleCollision(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            boxEvent.Invoke();
        }
    }

    public void SetInitialValues(BoxPooler boxPooler, Rigidbody playerRb)
    {
        this.boxPooler = boxPooler;
        this.playerRb = playerRb;
    }

    public void DisableComponents()
    {
        LineRendererControl(false);
        Destroy(GetComponent<SpringJoint>());
    }

    public void ActiveComponents()
    {
        LineRendererControl(true);
        SpringJointControl();
    }

    void LineRendererControl(bool state)
    {
        lineRendererControl.enabled = state;
        lineRenderer.enabled = state;
    }

    void SpringJointControl()
    {
        if (gameObject.GetComponent<SpringJoint>() == null)
        {
            gameObject.AddComponent<SpringJoint>();
        }
        SpringJoint springJoint = GetComponent<SpringJoint>();

        springJoint.connectedBody = playerRb;

        springJoint.minDistance = 0.2f;
        springJoint.maxDistance = Random.Range(1.5F, 3);

        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.anchor = new Vector3(0, 0, 0);
        springJoint.connectedAnchor = new Vector3(0, 0, 0);

        springJoint.spring = 3;
        springJoint.damper = 1;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }
}
