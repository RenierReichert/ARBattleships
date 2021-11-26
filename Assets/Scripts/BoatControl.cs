using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class BoatControl : MonoBehaviourPunCallbacks
{

    public Rigidbody boat;       
    public List<GameObject> leftHittable, rightHittable;
    public GameObject portSideCannon, starBoardCannon;
    public GameObject mast;
    public int hits = 3;
    public Canvas hpAboveShip;
    public Text boatHitPoints;
    protected Vector3 sailingDirection;
    private Joystick joystick;
    private float verticalInput, horizontalInput;
    private Material myBoat;
    private Button leftCannon, rightCannon;
    private Text hitpoints;
    private Image leftDisabled, rightDisabled;
    private float displacementAmount;
    private GameObject arCamera;

    private void Start()
    {
        joystick = GameObject.Find("Canvas/Fixed Joystick").GetComponent<Joystick>();
        leftCannon = GameObject.Find("Canvas/LeftCannon").GetComponent<Button>();
        rightCannon = GameObject.Find("Canvas/RightCannon").GetComponent<Button>();
        hitpoints = GameObject.Find("Canvas/HP").GetComponent<Text>();
        leftDisabled = GameObject.Find("Canvas/LeftCannon/Disabled").GetComponent<Image>();
        rightDisabled = GameObject.Find("Canvas/RightCannon/Disabled").GetComponent<Image>();
        arCamera = GameObject.Find("ARCamera");
        hitpoints.text = hits.ToString();

        leftCannon.onClick.AddListener(ShootLeftCannon);
        rightCannon.onClick.AddListener(ShootRightCannon);
        leftDisabled.fillAmount = 0;
        rightDisabled.fillAmount = 0;
        displacementAmount = this.GetComponentInChildren<Floatscript>().displacementAmount;
        StartCoroutine(FaceCamera());

        myBoat = Resources.Load<Material>("Materials/pinktexture");
        if (photonView.IsMine)
        {
            GetComponent<MeshRenderer>().material = (myBoat);
        }
    }
    private void FixedUpdate()
    {
        verticalInput = joystick.Vertical;
        horizontalInput = joystick.Horizontal;
        sailingDirection = transform.forward * verticalInput;

        //Boat should not be able to sail itself downwards or upwards strongly.
        sailingDirection.y /= 4;
        boat.AddForce(sailingDirection * 5, ForceMode.Acceleration);
        boat.AddTorque(transform.up * horizontalInput, ForceMode.Acceleration);
    }

    private void ShootLeftCannon()
    {
        if (!this.photonView.IsMine || !leftCannon.enabled)
            return;

        boat.AddForceAtPosition(boat.transform.right * 3 , mast.transform.position , ForceMode.Impulse);
        StartCoroutine(CannonCooldown(leftDisabled, leftCannon));

        if (leftHittable.Count > 0)
            HitLeftCannon();
        else
        {
            Rigidbody cannonBallClone;
            cannonBallClone = PhotonNetwork.Instantiate("Cannonball", portSideCannon.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            Vector3 direction = (boat.transform.right * -5);
            direction.y += 2;
            cannonBallClone.velocity = direction;
            Destroy(cannonBallClone.gameObject, 5);
        }
    }
    private void HitLeftCannon()
    {
        Rigidbody cannonBallClone;
        cannonBallClone = PhotonNetwork.Instantiate("Cannonball", portSideCannon.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        Vector3 autoAim = (leftHittable[0].transform.position - portSideCannon.transform.position);
        float distance = autoAim.magnitude;
        float secondsNeeded = distance / (-Physics.gravity.y);
        autoAim = autoAim.normalized;
        autoAim.y += secondsNeeded/2;
        cannonBallClone.velocity = autoAim *5;
        Destroy(cannonBallClone.gameObject, 5);
    }

    private void ShootRightCannon()
    {
        if (!this.photonView.IsMine || !rightCannon.enabled)
            return;

        boat.AddForceAtPosition(boat.transform.right * -3, mast.transform.position, ForceMode.Impulse);
        StartCoroutine(CannonCooldown(rightDisabled, rightCannon));
        
        if (rightHittable.Count > 0)
            HitRightCannon();
        else
        {
            Rigidbody cannonBallClone;
            cannonBallClone = PhotonNetwork.Instantiate("Cannonball", starBoardCannon.transform.position, Quaternion.identity).GetComponent<Rigidbody>(); ;
            Vector3 direction = (boat.transform.right * 5);
            direction.y += 2;
            cannonBallClone.velocity = direction;
            Destroy(cannonBallClone.gameObject, 5);
        }
    }

    private void HitRightCannon()
    {
        Rigidbody cannonBallClone;
        cannonBallClone = PhotonNetwork.Instantiate("Cannonball", starBoardCannon.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        Vector3 autoAim = (rightHittable[0].transform.position - starBoardCannon.transform.position);
        float distance = autoAim.magnitude;
        float secondsNeeded = distance / (-Physics.gravity.y);
        autoAim = autoAim.normalized;
        autoAim.y += secondsNeeded / 2;
        cannonBallClone.velocity = autoAim * 5;
        Destroy(cannonBallClone.gameObject, 5);
    }
    private void OnTriggerEnter(Collider cannonHit)
    {
        switch (cannonHit.gameObject.layer)
        {
            case 8:
                break;
            case 7:
                boat.AddForceAtPosition(cannonHit.GetComponent<Rigidbody>().velocity * 6 + new Vector3(0, 30, 0), boat.transform.position, ForceMode.Impulse);
                boat.AddTorque(boat.transform.forward * 60, ForceMode.Impulse);
                hits--;
                if(photonView.IsMine)
                    hitpoints.text = hits.ToString();
                boatHitPoints.text = hits.ToString();
                if (hits <= 0)
                {
                    StartCoroutine(Sunken());
                }
                Destroy(cannonHit, 0.2f);                
                break;
            case 6:
                boat.AddForceAtPosition((cannonHit.GetComponent<Rigidbody>().position - boat.position) * -50, cannonHit.transform.position, ForceMode.Impulse);
                break;
        }
    }

    private void OnTriggerStay(Collider stuckObject)
    {
        if(stuckObject.gameObject.layer == 6)
        {
            boat.AddForceAtPosition((stuckObject.GetComponent<Rigidbody>().position - boat.position).normalized * 5, stuckObject.transform.position, ForceMode.Force);
        }
    }

    IEnumerator CannonCooldown(Image disabled, Button cannon)
    {
        cannon.enabled = false;
        for (float i = 1; i > 0; i -= 0.01f)
        {
            disabled.fillAmount = i;
            yield return new WaitForSeconds(.02f);
        }
        cannon.enabled = true;
        yield break;
    }

    IEnumerator Sunken()
    {
        for (float i = 0.1f; i > 0; i -= 0.01f)
        {
            foreach(Floatscript childFloater in transform.GetComponentsInChildren<Floatscript>())
            {
                childFloater.displacementAmount = displacementAmount * i;
            }
            yield return new WaitForSeconds(.5f);
        }
        foreach (Floatscript childFloater in transform.GetComponentsInChildren<Floatscript>())
        {
            childFloater.displacementAmount = displacementAmount;
        }

        boat.transform.position = new Vector3(0, 1, 0);
        boat.velocity = Vector3.zero;
        boat.angularVelocity = Vector3.zero;
        boat.rotation = Quaternion.Euler(Vector3.zero);
        hits = 3;
        boatHitPoints.text = hits.ToString();
        if(photonView.IsMine)
            hitpoints.text = hits.ToString();
        yield break;

    }

    IEnumerator FaceCamera()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            hpAboveShip.transform.LookAt(2 * transform.position - arCamera.transform.position);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            int hitsvalue = hits;
            stream.Serialize(ref hitsvalue);
        }
        else
        {
            int hitsvalue = 0;
            stream.Serialize(ref hitsvalue);
            hits = hitsvalue;
        }
    }
}
