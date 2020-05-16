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
        if (transform.GetComponent<OVRGrabbable>().isGrabbed == true)
        {
            m_Rigidbody.constraints = RigidbodyConstraints.None;
        }
        else
        {
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
