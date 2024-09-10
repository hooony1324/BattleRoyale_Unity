using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetHasyKey : MonoBehaviour
{
    [SerializeField]
    Button _getHashkeyButton;

    [SerializeField]
    TMP_InputField _inputField;

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
    }
}