using UnityEngine;

namespace C2M2.Interaction.VR
{
    using Utils;
    using VR;
    using Interaction;
    /// <summary>
    /// Add Rigidbody, Collider, and OVRGrabbable to object
    /// </summary>
    public class VRGrabbable : MonoBehaviour
    {
        private void Awake()
        {
            // Initialize Rigidbody
            Rigidbody rb = GetComponent<Rigidbody>();
            if(rb == null) gameObject.AddComponent<Rigidbody>();

            // Initialize Colliders
            RefreshColliders();
        }
        private void Start()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.SetDefaultState();
        }
        private void RefreshColliders()
        {
            // Initialize new collider array
            Collider[] grabColliders = new Collider[1];
            grabColliders = NonConvexMeshCollider.Calculate(gameObject);

            // If there is no OVRGrabbable, we can't make these colliders meaningful
            PublicOVRGrabbable ovr = GetComponent<PublicOVRGrabbable>();
            if (ovr == null) ovr = gameObject.AddComponent<PublicOVRGrabbable>();
            ovr.M_GrabPoints = grabColliders;
        }
    }
}
