using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Cart))]
public class CartEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Cart myCart = target as Cart;
        GUI.backgroundColor = Color.green;
        //GUI.contentColor = Color.black;
        if(GUILayout.Button("Test Track"))
        {
            myCart.StartRide();
        }
    }
}


[RequireComponent(typeof(Rigidbody))]
public class Cart : MonoBehaviour
{
    //private List<Vector3> trackPoints;
    private List<Transform> trackTransforms;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private int trackPos = 0;
    //The only unrealistic Physics used to keep carts from falling backwards on tracks
    private float minSpeed = 1f, currentSpeed = 0f, mass = 10, startTime, moveLength;
    private float gravity = 9.81f;
    private Rigidbody myBody;

    public bool moveRide = false, isLeader = true;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody>();
        mass = myBody.mass;

        // transform.parent = null;

        //for now use this to add starting points
        //GetComponent<pathVerticies>().addPathPoints(transform.parent.parent.gameObject);
    }

    //Called every fixed framerate for physics
    private void FixedUpdate()
    {
        if (moveRide)
        {
            if (isLeader)
                FollowTrack();
            else
                FollowNextCart();
        }
    }

    private void LateUpdate()
    {
        if (moveRide)
        {
            if (trackPos < trackTransforms.Count-1)
            {
                Rotate(trackTransforms[trackPos+1]);
            }
        }
    }

    public void StartRide()
    {
        //trackPoints = GetComponent<pathVerticies>().pathPoints;
        trackTransforms = GetComponent<pathVerticies>().pathTransforms;
        if (!myBody)
        {
            myBody = GetComponent<Rigidbody>();
            //mass = myBody.mass;
        }
        moveRide = true;
        trackPos = 1;
        startTime = Time.fixedTime;
        //make sure cart is at the beginning of ride maybe point 1 for longer carts
        transform.position = trackTransforms[0].position;
        startPosition = transform.position;
        startRotation = transform.rotation;
        moveLength = Vector3.Distance(transform.position, trackTransforms[trackPos].position);
        currentSpeed = 1f;

        if (player != null)
        {
            player.transform.parent = transform;
            player.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            player.transform.rotation = transform.rotation;
            player.GetComponent<OVRPlayerController>().enabled = false;
            player.GetComponent<PlayerControls>().enabled = false;
        }

        //if not leader move to 0 positon - cart length
    }


    //lerp between current position and next track point
    private void FollowTrack()
    {
        //Caculate the current Speed the cart is going may have to check this for negative
        //if(Vector3.Angle(transform.forward,Vector3.down) <= 20)
        //{
        //    currentSpeed += Time.fixedDeltaTime * (mass * gravity);
        //}
        //else if (Vector3.Angle(transform.forward, Vector3.up) <= 20)
        //{
        //    currentSpeed -= Time.fixedDeltaTime * (mass * gravity);
        //    if (currentSpeed < minSpeed)
        //        currentSpeed = minSpeed;
        //}
        //else
        //{
        //    currentSpeed -= Time.fixedDeltaTime * (gravity);
        //    if (currentSpeed < minSpeed)
        //        currentSpeed = minSpeed;
        //}



        if (trackPos < trackTransforms.Count-1 && Vector3.Distance(transform.position, trackTransforms[trackPos].position) < 0.01f)
        {
            if (trackPos != 0)
            {
                trackPos++;
                moveLength = Vector3.Distance(transform.position, trackTransforms[trackPos].position);
                startPosition = transform.position;
                startRotation = transform.rotation;
                startTime = Time.fixedTime;
            }
        }else if(trackPos >= trackTransforms.Count-1)
        {
            moveLength = Vector3.Distance(transform.position, trackTransforms[0].position);
            startPosition = transform.position;
            startRotation = transform.rotation;
            startTime = Time.fixedTime;
            currentSpeed = 1f;
            trackPos = 0;
        }

        if (trackPos < trackTransforms.Count && trackPos != 0)
        {
            Move(trackTransforms[trackPos]);
        }
        else //reached end of ride
        {
            Move(trackTransforms[0]); //check for if completed for this later.  if not maybe go backwards instead of forwards or just stop.
            if (Vector3.Distance(transform.position, trackTransforms[0].position) < 0.01f)
            {
                moveRide = false;
                currentSpeed = 0f;
            }
        }
        
    }

    //Follows the cart ahead of it
    //For each follower slows down the total forward speed
    private void FollowNextCart()
    {
        //use move with target being the next cart
    }

    //lerps between current positon and target positon using current speed
    Transform currentTrack;
    private void Move(Transform target)
    {
        CalculateNextSpeed(target.position);
        float dist = (Time.fixedTime - startTime) * currentSpeed;

        myBody.MovePosition(Vector3.Lerp(startPosition, target.position, dist/moveLength));

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position, target.up), dist/moveLength);
        //Vector3.MoveTowards
    }

    private void Rotate(Transform target)
    {
        float dist = (Time.time - startTime) * currentSpeed; // or curr dist - total dist / total dist

        if (dist > moveLength)
            dist = moveLength;

        transform.rotation = Quaternion.Lerp(startRotation, Quaternion.LookRotation((target.position - startPosition), target.up), dist / moveLength);

        //transform.forward = Vector3.Lerp(startPosition, (target.position - transform.position), dist / moveLength);
    }

    private void CalculateNextSpeed(Vector3 target)
    {
        //KEa + PEa = KEb + PEb (- WNc)  //Wf = Fdcos(theta)
        //0.5mv1^2 + mgh1 = 0.5mv2^2 + mgh2
        //0.5mv1^2 + mgh1 - mgh2 = 0.5mv2^2
        //2(0.5mv1^2 + mgh1 - mgh2) = mv2^2
        //(2(0.5mv1^2 + mgh1 - mgh2))/m = v2^2
        //sqrt((2(0.5mv1^2 + mgh1 - mgh2))/m) = v2

        //Kinetic Energy A
        float KEa = 0.5f * Mathf.Pow(currentSpeed, 2);
        //Potential Energy A
        float PEa = gravity * transform.position.y;
        //potential Energy B
        float PEb = gravity * target.y;

        if (KEa + PEa - PEb > 0 )//&& Mathf.Abs(transform.position.y - target.y) > 0.01f)
            currentSpeed = Mathf.Sqrt(2f * (KEa + PEa - PEb));

        if (currentSpeed < minSpeed)
            currentSpeed = minSpeed;

    }
}
