using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Managers : MonoBehaviour
{
    public static bool Initialized { get; set; }

    private static Managers s_instance;
    private static Managers Instance { get { Init(); return s_instance; } }

    // Core
    private PoolManager _pool = new PoolManager();
    private ResourceManager _resource = new ResourceManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private UIManager _ui = new UIManager();

    public static PoolManager Pool => Instance?._pool;
    public static ResourceManager Resource => Instance?._resource;
    public static SceneManagerEx Scene => Instance?._scene;
    public static UIManager UI => Instance?._ui;

    // Contents
    private GameManager _game = new GameManager();

    public static GameManager Game => Instance?._game;

    // Server
    private static BackEndServerManager _server;
    
    private static BackEndMatchManager _match;
    private static Dispatcher _dispatcher;

    private static BackEndChatManager _chat;

    public static BackEndServerManager Server => _server;
    public static BackEndMatchManager Match => _match;
    public static Dispatcher Dispatcher => _dispatcher;
    public static BackEndChatManager Chat => _chat;

    private static SendQueueManager _sendQueueManager;

    public static SendQueueManager SendQueueManager => _sendQueueManager;

    public static void Init()
    {
        if (s_instance == null && Initialized == false)
        {
            Initialized = true;

            Application.targetFrameRate = 60;
            Application.runInBackground = true;
            QualitySettings.vSyncCount = 0;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            // Initialize Monobehaviour Managers
            _server = go.GetComponent<BackEndServerManager>();
            _match = go.GetComponent<BackEndMatchManager>();
            _dispatcher = go.GetComponent<Dispatcher>();
            _sendQueueManager = go.GetComponent<SendQueueManager>();
            _chat = go.GetComponent<BackEndChatManager>();

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            Object eventSystem = GameObject.FindObjectOfType(typeof(EventSystem));
            DontDestroyOnLoad(eventSystem);
            
        }
    }

    public static void Clear()
    {
        //Event.Clear();
        //Scene.Clear();
        //UI.Clear();
        //Object.Clear();
        //Pool.Clear();
    }
}
