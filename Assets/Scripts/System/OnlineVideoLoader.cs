using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Photon.Realtime;
using Photon.Pun;
using System;
using TMPro;


public class OnlineVideoLoader : MonoBehaviourPunCallbacks,IPunObservable
{
    

    public VideoPlayer videoPlayer;
    public VideoClip clip;
    public string videoUrl = "yourvideourl";
    PhotonView pv;
   [SerializeField] float time;
   [SerializeField] TMP_Text videoTime;

    [SerializeField] Image progresbar;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update


    //
    //TODO----
    //  video iki tarfta senkronize olmuyo frame olayýný dene tekrar serialize etmeyi dene custom prop dene 
    void Start()
    { 
        //videoPlayer.url = videoUrl;
        //videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        //videoPlayer.EnableAudioTrack(0, true);
        //videoPlayer.playOnAwake = false;
        //videoPlayer.Prepare();

        




    }

    private void Update()
    {
        if (videoPlayer.frameCount>0)
        {
            progresbar.fillAmount = (float)videoPlayer.frame / (float)videoPlayer.frameCount;
        }

      
       // videoTime.text = videoPlayer.time.ToString();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pv.RPC("Video", RpcTarget.AllBufferedViaServer);
        }
    }

    [PunRPC]
    public void Video()
    {
        videoPlayer.Play();
       
       
       
    }






    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(videoPlayer.frame);
        }
        else if (stream.IsReading)
        {
            videoPlayer.frame = (long)stream.ReceiveNext();
        }
    }

  
}