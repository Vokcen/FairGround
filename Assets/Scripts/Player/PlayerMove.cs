using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
public class PlayerMove : MonoBehaviour
{
    [Header("Move State")]
    public CharacterController controller;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    [Space(20)]
    [Header("Preset State")]
    [SerializeField] Transform cam;
    [SerializeField] CinemachineFreeLook cmLook;
    PhotonView pv;
    UiControl ui;

    //===============================================================
    //Animstate
    //===============================================================
    [Space(20)]
    [Header("Ani State")]
    [SerializeField] Animator ani;
    const string PLAYER_IDLE = "Idle";
    const string PLAYER_WALK = "Walk";
    const string PLAYER_RUN = "Run";
    const string PLAYER_SÝT = "Sit";
    const string PLAYER_SÝTTÝNG = "Sitting";
    const string PLAYER_POÝNT = "Point";
    private string currentState;
    private string AnimationState;

    //===============================================================
    //Animstate bools
    //===============================================================
    private bool isWalking;
    private bool isRunning;
    private bool isSitPressed;



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
        if (!pv.IsMine) return;


        if (AnimationState!="Sit")
        {
            Move();
        }
        else
        {
            SitAnimations();
        }
         

    }


    void ChangeAnimationState(string newState)
    {// stop same animaton interruptin itself
        if (currentState == newState) return;
       
        //play the animation
         ani.Play(newState);

        //reassing the current state
        currentState = newState;
    }
 
    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
           

            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            if (direction.magnitude >= 0.1f&& speed==6 )
            {
                ChangeAnimationState(PLAYER_WALK);

            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ChangeAnimationState(PLAYER_RUN);
                isRunning = true;
                speed = 12;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                
                isRunning = false;
                speed = 6;
            }
        }
        else
        {
            ChangeAnimationState(PLAYER_IDLE);
        }



    }
    private void ToggleSitAnim(Collider other)
    {

       
            ChangeAnimationState(PLAYER_SÝT);
            ui.gKeyPanel.SetActive(true);
            ani.SetBool("isSit", true);
            transform.rotation = other.gameObject.transform.rotation;
        AnimationState = "Sit";
        //else
        //{
        //    Debug.Log("kalkýyon!");
        //    ChangeAnimationState(PLAYER_IDLE);

        //    ui.gKeyPanel.SetActive(false);
        //}














    }
    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "Sit") ui.fKeyPanel.SetActive(true);

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Sit")
        {
            ui.fKeyPanel.SetActive(false);
            ui.gKeyPanel.SetActive(false);
           
        }
    }
    private void OnTriggerStay(Collider other)
    {
      
        if (!pv.IsMine) return;

        if (other.gameObject.tag == "Sit")
        {
            Debug.Log("<color=yellow>Oturabilir</color>");

            if (Input.GetKeyDown(KeyCode.F))
            {
                ui.fKeyPanel.SetActive(false);

                ToggleSitAnim(other);

            }
         




        }
    }

    private void SitAnimations()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            Cursor.lockState = CursorLockMode.None;
            ui.emotePanel.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            ui.emotePanel.SetActive(false);
            AnimationState = string.Empty;
        }

    }

    public void AnimatorBool()
    {
        StartCoroutine(PointAnim());
    }
    IEnumerator PointAnim()
    {
        ChangeAnimationState(PLAYER_POÝNT);
        yield return new WaitForSeconds(2f);
        ChangeAnimationState(PLAYER_SÝTTÝNG);
    }

}
