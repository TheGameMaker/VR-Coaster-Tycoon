using UnityEngine;

public class isGrabbed : MonoBehaviour
{
    private Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //change this to an event or method instead of constant checks
        //if (transform.GetComponent<OVRGrabbable>().isGrabbed == true)
        //{
        //    m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        //   // m_Rigidbody.constraints =
        //}
        //else
        //{
        //    m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        //}
    }
}
