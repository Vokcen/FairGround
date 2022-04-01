using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
public class PlayerFpsLook : MonoBehaviour
{
    //[SerializeField] GameObject fpsCam;
    //[SerializeField]GameObject ThirdPersonCam;
    [SerializeField] Transform head;
    [SerializeField] PlayerMovement pm;
    CinemachineVirtualCamera virtualCam;
    UiControl ui;

    PhotonView pv;
    bool isFps;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        ui = FindObjectOfType<UiControl>();
       
        
    }
    private void Start()
    {

      
    }
    private void Update()
    {
        if (!pv.IsMine) return;
       

        
        if (isFps&& Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchThirdPerson();
        }
    }

    private void SwitchThirdPerson()
    {
        ui.virtualCam.SetActive(false);
        ui.freeLookCam.SetActive(true);
        pm.cmLook.LookAt = head;
        pm.cmLook.Follow = head;



        isFps = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!pv.IsMine) return;
        if (other.gameObject.tag == "FpsAreaStarter"&& Input.GetKeyDown(KeyCode.F)&& !isFps)
        {
            Debug.Log("bastýn");
            ui.freeLookCam.SetActive(false);
            ui.virtualCam.SetActive(true);
            virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
            virtualCam.Follow = head;

            isFps = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isFps&& other.gameObject.tag == "FpsAreaStarter")
        {
            SwitchThirdPerson();
        }
    
    }
}
