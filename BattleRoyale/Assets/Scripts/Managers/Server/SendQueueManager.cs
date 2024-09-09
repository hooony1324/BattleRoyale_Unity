using System;
using UnityEngine;
using BackEnd;

public class SendQueueManager : MonoBehaviour
{
    public void Init()
    {
        DontDestroyOnLoad(this);

        if (SendQueue.IsInitialize == false)
        {
            SendQueue.StartSendQueue(true, ExceptionEvent);
        }
    }

    void Update()
    {
        if (SendQueue.IsInitialize)
        {
            SendQueue.Poll();
        }
    }

    void ExceptionEvent(Exception e)
    {
        Debug.Log(e.ToString());
    }


    void OnApplicationPause(bool isPause)
    {
        if (SendQueue.IsInitialize == false)
            return;

        if (isPause == false)
        {
            SendQueue.ResumeSendQueue();
        }
        else
        {
            SendQueue.PauseSendQueue();
        }
    }

    void OnApplicationQuit()
    {
        if (SendQueue.IsInitialize == false)
            return;
            
        SendQueue.StopSendQueue();
    }
}