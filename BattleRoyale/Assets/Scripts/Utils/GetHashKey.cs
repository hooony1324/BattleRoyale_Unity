using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GetHashKey : MonoBehaviour
{
    [SerializeField]
    Button _getHashkeyButton;

    [SerializeField]
    TMP_InputField _inputField;

    [SerializeField]
    Button _goLoginSceneButton;

    void Awake()
    {
        var bro = Backend.Initialize();

        if (bro.IsSuccess())
        {
            Debug.Log("Backend초기화 성공");
        }
        else
        {
            Debug.Log("Backend초기화 실패");
        }
    }

    void Start()
    {
        _getHashkeyButton.onClick.AddListener(()=>
        {
            var key = Backend.Utils.GetGoogleHash();
            Debug.Log(key);
            _inputField.text = key;
        });

        _goLoginSceneButton.onClick.AddListener(() => 
        {
            SceneManager.LoadScene("Login");
        });
    }
}