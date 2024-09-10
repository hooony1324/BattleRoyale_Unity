using UnityEngine;
using BackEnd;
using BackEnd.Tcp;

/// <summary>
/// 매칭서버 접속
/// 매칭서버 종료
/// 매칭 신청
/// 매칭 신청 취소
/// </summary>
public partial class BackEndMatchManager : MonoBehaviour
{


    public void LeaveMatchServer()
    {
        IsConnectMatchServer = false;
        Backend.Match.LeaveMatchMakingServer();
    }
}