using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6f;
    [SerializeField] Transform cam;
 [SerializeField]   CinemachineFreeLook cmLook;
    PhotonView pv;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    [SerializeField] Transform follow;
    [SerializeField] Transform lookat;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if (!pv.IsMine) return;
       

        
        cam = Camera.main.transform;
        cmLook = FindObjectOfType<CinemachineFreeLook>();
        cmLook.Follow = follow;
        cmLook.LookAt = lookat;
        if (pv.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;

        }

    }
    private void Update()
    {
        if (!pv.IsMine) return;

        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if (direction.magnitude>=0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg+cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }


}
