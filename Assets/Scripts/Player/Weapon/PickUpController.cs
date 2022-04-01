
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [SerializeField] ProjectileGun gunScript;
    [SerializeField] Rigidbody rb;
    [SerializeField] BoxCollider coll;
    [SerializeField] Transform player, gunContainer, fpsCam;

    [SerializeField] float pickUpRange,pickUpTime;

    [SerializeField]float dropForwardForce,dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private void Start()
    {
        //setup
        if (!equipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            gunScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
        }
    }
    void Update()
    {
        //Check if player is in range and "E" is pressed
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E)) PickUp();

        //Drop if equipped and"G" is pressed
        if (equipped && Input.GetKeyDown(KeyCode.G)) Drop();
        
    }
    private void PickUp()
    {
        equipped = true;
        slotFull = true;
        //make weapon a child of the camera and move it to default position
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        //make rigidbody and boxcollider trigger
        rb.isKinematic = true;
        coll.isTrigger = true;
        //enableScript
        gunScript.enabled = true;
    }
    private void Drop()
    {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);


        //adforce
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        rb.velocity = new Vector3(2f, 2f, 2f);
        float random =Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random)*10);
        //make rigidbody and boxcollider trigger normal
        rb.isKinematic = false;
        coll.isTrigger = false;
        //disable
        gunScript.enabled = false;
    }

    
}
