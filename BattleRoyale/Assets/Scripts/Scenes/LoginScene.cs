using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Util;

public class LoginScene : BaseScene
{
    public delegate void LoadingValueChangeHandler(string key, int current, int total);

    public LoadingValueChangeHandler OnLoadingStatusChanged;
    public enum ELoginSceneState
    {
        None = 0,
        CalculatingSize,
        NothingToDownload,
        AskingDownload,
        Downloading,
        DownloadFinished,
    }

    private ELoginSceneState _loginSceneState = ELoginSceneState.None;
    public ELoginSceneState LoginSceneState
    {
        get => _loginSceneState;
        set
        {
            _loginSceneState = value;
            // OnUpdateLogin?.Invoke();
        }
    }


    [SerializeField]
    private UI_LoginScene _loginScene;
    Downloader _downloader;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = EScene.Login;
        Managers.Scene.SetCurrentScene(this);

        _downloader = gameObject.GetOrAddComponent<Downloader>();
        _downloader.DownloadLabel = "Login";

        Managers.Resource.LoadAllAsync<Object>("Preload", (key, current, total) =>
        {
            if (current == total)
            {
                StartCoroutine(StartDownLoad());
            }
        });

        return true;
    }

    private IEnumerator StartDownLoad()
    {
        yield return _downloader.StartDownload((events) =>
        {
            events.OnInitialized += OnInitialized;
            events.OnCatalogUpdated += OnCatalogUpdated;
            events.OnSizeDownloaded += OnSizeDownloaded;
            events.OnProgressUpdated += OnProgress;
            events.OnFinished += OnFinished;
        });
    }

    private void OnInitialized()
    {
        _downloader.GoNext();
    }

    private void OnCatalogUpdated()
    {
        _downloader.GoNext();
    }

    private void OnSizeDownloaded(long size)
    {
        Debug.Log($"다운로드 완료 ! : {Util.GetConvertedByteString(size, ESizeUnits.KB)} ({size}바이트)");

        if (size == 0)
            LoginSceneState = ELoginSceneState.DownloadFinished;
        else
        {
            // 다운로드 하시겠습니까?? 용량이 많습니다 부분
            var sizeUnit = Util.GetProperByteUnit(size);
            Util.ConvertByteByUnit(size, sizeUnit);

            LoginSceneState = ELoginSceneState.AskingDownload;

            //Managers.UI.ShowPopupUI<UI_MessagePopup>();
            //LoginSceneState = ELoginSceneState.Downloading;
            //_downloader.GoNext();
        }

        Managers.UI.ShowPopupUI<UI_MessagePopup>();
    }

    private void OnProgress(DownloadProgressStatus progresStatus)
    {

    }

    private void OnFinished(bool isSuccess)
    {
        LoginSceneState = ELoginSceneState.DownloadFinished;
        _downloader.GoNext();
    }

    public override void Clear()
    {
        
    }
}
