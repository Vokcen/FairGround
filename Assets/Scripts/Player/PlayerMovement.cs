using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    //===============================================================
    //MovementState
    //===============================================================
    [Header("Move State")]
    public CharacterController controller;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    //===============================================================
    //SystemState
    //===============================================================
    [Space(20)]
    [Header("Preset State")]
    [SerializeField] Transform cam;
    public CinemachineFreeLook cmLook;
    CinemachineComponentBase componentBase;
    float cameraDistance;
   float sensivity=1500f;


    PhotonView pv;
    UiControl ui;



    //===============================================================
    //Animstate
    //===============================================================
    [Space(20)]
    [Header("Ani State")]
    [SerializeField] Animator ani;
    string AnimState;
   
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        ui = FindObjectOfType<UiControl>();
    }
    private void Start()
    {
        if (!pv.IsMine) return;

        ui.pointButton.onClick.AddListener(AnimatorBool);
        


        cam = Camera.main.transform;
        cmLook = FindObjectOfType<CinemachineFreeLook>();
        cmLook.Follow = transform;
        cmLook.LookAt = transform;
        if (pv.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;

        }

    }
    private void Update()
    {
       
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            Debug.Log("work");
            cameraDistance = Input.GetAxis("Mouse ScrollWheel") * sensivity*Time.deltaTime;
            cmLook.m_CommonLens = true;
            
            cmLook.m_Lens.FieldOfView -=cameraDistance;
            cmLook.m_Lens.FieldOfView = Mathf.Clamp(cmLook.m_Lens.FieldOfView, 5, 60);
            Debug.Log(cmLook.m_Lens.FieldOfView);
            Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        }
        if (!pv.IsMine) return;

        

        if (true)
        {

        }

        if (AnimState!="Sit")
        {
            
            
            Move();

          
        }
        else
        {
            SitAnimations();
        }
      
    }

    private void SitAnimations()
    {
     
            if (Input.GetKeyDown(KeyCode.T))
            {
                ani.SetBool("Siting", true);
            Cursor.lockState = CursorLockMode.None;
                ui.emotePanel.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
            ani.SetBool("Siting", false);
            Cursor.lockState = CursorLockMode.Locked;
                ui.emotePanel.SetActive(false) ;
            }
      
    }
    private void Move()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }



        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if (direction.magnitude>=0.1f)
        {
            ani.SetBool("isMove", true);
            
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg+cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
               
                ani.SetBool("isRun", true);
                speed = 12;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                ani.SetBool("isRun", false);
                speed = 6;
            }
        }
        else
        {
            ani.SetBool("isMove", false);
            ani.SetBool("isRun", false);
        }
        // Changes the height position of the player..
     

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);


    }
    private void ToggleSitAnim(Collider other)
    {
       
        Debug.Log("basýyon!");

        if (AnimState!="Sit")
        {
            ui.gKeyPanel.SetActive(true);
            ani.SetBool("isSit", true);
            transform.rotation = other.gameObject.transform.rotation;
           
            AnimState = "Sit";
        }
        else
        {

            
              
                Debug.Log("basýyon!");
                ani.SetBool("isSit", false);
                AnimState = string.Empty;
            ui.gKeyPanel.SetActive(false);

        }



    }
    private void OnTriggerEnter(Collider other)
    {
      
  
        if (other.gameObject.tag == "Sit") ui.fKeyPanel.SetActive(true);

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Sit") ui.fKeyPanel.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        bool currentState = ui.fKeyPanel.activeSelf;
        if (!pv.IsMine) return;

        if (other.gameObject.tag=="Sit")
        { Debug.Log("<color=yellow>Oturabilir</color>");
           
            if (Input.GetKeyDown(KeyCode.F))
            {
                ui.fKeyPanel.SetActive(false);
              
                ToggleSitAnim(other);
               
            }


            //if (Input.GetKeyDown(KeyCode.G))
            //{
            //    ui.gKeyPanel.SetActive(true);
            //    Debug.Log("basýyon!");
            //    ani.SetBool("isSit", false);
            //    AnimState = string.Empty;


            //}
            //else
            //{
            //    ui.gKeyPanel.SetActive(false);
            //}


        }
    }

    public void AnimatorBool()
    {
        StartCoroutine(PointAnim());
    }
    IEnumerator PointAnim()
    {
        ani.SetBool("isPointing", true);
        ui.emotePanel.SetActive(false);
        yield return new WaitForSeconds(2f);
        ani.SetBool("isPointing", false);
    }

}
