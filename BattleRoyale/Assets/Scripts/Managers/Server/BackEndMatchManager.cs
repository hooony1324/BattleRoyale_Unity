using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using System.Collections.Generic;
using Protocol;
using System.Linq;
using System;

public class ServerInfo
{
    public string Host;
    public ushort Port;
    public string RoomToken;
}

public partial class BackEndMatchManager : MonoBehaviour
{
    public class MatchInfo
    {
        public string Title;                // 매칭 명
        public string InDate;               // 매칭 inDate (UUID)
        public MatchType MatchType;         // 매치 타입
        public MatchModeType MatchModeType; // 매치 모드 타입
        public string HeadCount;            // 매칭 인원
        public bool IsSandboxEnable;        // 샌드박스 모드 (AI매칭)
    }

    // 콘솔에서 생성한 매칭 카드들의 리스트
    public List<MatchInfo> MatchInfos { get; private set; } = new List<MatchInfo>();
    // 매칭 중인 유저들의 세션
    public List<SessionId> SessionIdList { get; private set; }

    private string _inGameRoomToken = string.Empty;

    public SessionId HostSession { get; private set; }
    private ServerInfo _roomInfo = null;

    public bool IsReconnectEnable { get; private set; } = false;
    public bool IsConnectMatchServer { get; private set; } = false;
    public bool IsReconnectProcess { get; private set; } = false;
    public bool IsSandboxGame { get; private set; } = false;
    private bool _isConnectInGameServer = false;
    private bool _isJoinGameRoom = false;
    private int _numOfClient = 2;       // 매치에 참여한 유저의 합

    #region Host
    private bool _isHost = false;       // 호스트 여부는 서버에서 설정(SuperGamer)
    public bool IsHost => _isHost;
    private Queue<KeyMessage> localQueue = null;
    #endregion

    void OnApplicationQuit()
    {
        if (IsConnectMatchServer)
        {
            LeaveMatchServer();
            Debug.Log("BackEndMatchManager::LeaveMatchServer");
        }
    }

    public void Init()
    {
        //Managers.Game.OnGameReconnect
    }

    public void GetMatchList(Action<bool, string> action)
    {
        MatchInfos.Clear();

        Backend.Match.GetMatchList(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.Log($"매칭카드 리스트 불러오기 실패\n{callback}");
                Managers.Dispatcher.BeginInvoke(() =>
                {
                    GetMatchList(action);
                });
                return;
            }

            foreach (LitJson.JsonData row in callback.Rows())
            {
                MatchInfo matchInfo = new MatchInfo();
                matchInfo.Title = row["matchTitle"]["S"].ToString();
                matchInfo.InDate = row["inDate"]["S"].ToString();
                matchInfo.HeadCount = row["matchHeadCount"]["N"].ToString();
                matchInfo.IsSandboxEnable = row["enable_sandbox"]["BOOL"].ToString().Equals("True") ? true : false;

                foreach (MatchType type in Enum.GetValues(typeof(MatchType)))
                {
                    if (type.ToString().ToLower().Equals(row["matchType"]["S"].ToString().ToLower()))
                    {
                        matchInfo.MatchType = type;
                    }
                }

                foreach (MatchModeType type in Enum.GetValues(typeof(MatchModeType)))
                {
                    if (type.ToString().ToLower().Equals(row["matchModeType"]["S"].ToString().ToLower()))
                    {
                        matchInfo.MatchModeType = type;
                    }
                }

                MatchInfos.Add(matchInfo);
            }
            
            Debug.Log("매칭카드 리스트 불러오기 성공 : " + MatchInfos.Count);
            action(true, string.Empty);
        });
    }

}