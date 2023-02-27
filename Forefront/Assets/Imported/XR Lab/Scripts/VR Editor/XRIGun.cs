using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using XRLab;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(LineRenderer))]
public class XRIGun : MonoBehaviour
{
    [Header("Projectile Parameters")]
    [Tooltip("Reference to GameObject to use as a projectile")]
    [SerializeField] GameObject m_projectile;
    [Tooltip("The force applied to the projectile (Default: 100000f")]
    [SerializeField] float m_projectileForce = 100000f;

    [Header("Shooting Parameters")]
    [Tooltip("The position the projectile should spawn at. Also affects fire rotation. If unassigned, transform if attached object is used")]
    [SerializeField] Transform m_shootPos;
    [Tooltip("How fast should the gun fire")]
    [SerializeField] float m_fireRate = 0.1f;
    [Tooltip("Should a projectile be fired")]
    [SerializeField] bool m_fireProjectile = true;
    [Tooltip("Should a ray be fired")]
    [SerializeField] bool m_fireRaycast = true;
    [Tooltip("The force applied to rigidbodies when shot by the raycast")]
    [SerializeField] float m_raycastForce = 100;
    [Tooltip("Misc")]
    [SerializeField] AudioSource m_audioSource;

    XRGrabInteractable m_InteractableBase; //used to reference the interactor that activates the gun
    float m_TriggerHeldTime = 999f; //used to count time between shots
    bool m_TriggerDown; //used to check if trigger is down in the update
    GameObject m_firedProjectile; //used to temporarily reference the bullet for force application and deletion 
    LineRenderer m_tracerLine; //the line renderer used to show where the ray is firing
    
    GameObject m_raycastCollider; //used for a crude workaround to allow onTriggerEnter to function with the raycast

    void Start()
    {
        m_raycastCollider = new GameObject();
        SphereCollider col = m_raycastCollider.AddComponent<SphereCollider>();
        col.radius = 0.1f;
        col.isTrigger = true;

        m_raycastCollider.AddComponent<Rigidbody>().isKinematic = true;

        m_tracerLine = GetComponent<LineRenderer>();

        //if shootPos is null then assign attached gameObject transform
        if (m_shootPos == null)
            m_shootPos = gameObject.transform;

        if(m_audioSource == null)
            m_audioSource = GetComponent<AudioSource>();
        //Get a reference to the grab interactable 
        m_InteractableBase = GetComponent<XRGrabInteractable>();

        //set up listeners
        m_InteractableBase.selectExited.AddListener(DroppedGun);
        m_InteractableBase.activated.AddListener(TriggerPulled);
        m_InteractableBase.deactivated.AddListener(TriggerReleased);
    }

    void FixedUpdate()
    {
        if (m_TriggerDown)
        {
            m_TriggerHeldTime += Time.deltaTime;

            if (m_TriggerHeldTime >= m_fireRate)
            {
                if (m_audioSource != null)
                    m_audioSource.Play();

                m_tracerLine.enabled = true;

                if (m_fireRaycast)
                    ShootRay();

                if (m_fireProjectile)
                    ShootProjectile();

                m_TriggerHeldTime = 0; //reset to zero for auto fire


            }
            else
            {
                m_tracerLine.enabled = false;
                m_raycastCollider.transform.position = transform.position; //call the collider back to the hand so it is only sent out when firing
            }
        }
        else
        {
            m_TriggerHeldTime = 0; //reset to zero for auto fire
        }
    }

    /// <summary>
    /// shoot raycast and apply force to rigidbody
    /// components of objects the raycase hits
    /// 
    /// Largely based off code available at 
    /// <href>https://learn.unity.com/tutorial/let-s-try-shooting-with-raycasts</href>
    /// </summary>
    void ShootRay()
    {
        
        // Declare a raycast hit to store information about what our raycast has hit
        RaycastHit hit;

        // Set the start position for our visual effect for our laser to the position of gunEnd
        m_tracerLine.SetPosition(0, m_shootPos.position);

        // Check if our raycast has hit anything
        if (Physics.Raycast(m_shootPos.position, m_shootPos.forward, out hit, 2000))
        {
            m_raycastCollider.transform.position = hit.point; //move collider to ray location to allow oncollisionenter and 

            // Check if the object we hit has a rigidbody attached
            if (hit.rigidbody != null)
            {
                // Add force to the rigidbody we hit, in the direction from which it was hit
                hit.rigidbody.AddForce(-hit.normal * m_raycastForce);
            }
        }

        m_tracerLine.SetPosition(1, m_shootPos.forward * 2000);
    }

    void ShootProjectile()
    {
        m_firedProjectile = Instantiate(m_projectile, m_shootPos.position, m_shootPos.rotation);
        m_firedProjectile.GetComponent<Rigidbody>().AddForce(m_shootPos.forward * m_projectileForce, ForceMode.Impulse);

        Destroy(m_firedProjectile, 10); //cleanup
    }
    void TriggerReleased(DeactivateEventArgs args)
    {
        m_TriggerDown = false;
        m_TriggerHeldTime = 0f;
    }

    void TriggerPulled(ActivateEventArgs args)
    {
        m_TriggerDown = true;
    }

    void DroppedGun(SelectExitEventArgs args)
    {
        m_TriggerDown = false;
        m_TriggerHeldTime = 0f;
    }
}
