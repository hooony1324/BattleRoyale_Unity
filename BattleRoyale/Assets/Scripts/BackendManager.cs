using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BackEnd;
using UnityEngine.Pool;

public class BackendManager : MonoBehaviour
{

    private void Start()
    {
        var bro = Backend.Initialize();

        if (bro.IsSuccess())
        {
            Debug.Log("�ʱ�ȭ ����: " + bro);
        }
        else
        {
            Debug.LogError("�ʱ�ȭ ����: " + bro);
        }

        Test();
    }

    void Test()
    {
        BackendLogin.Instance.CustomLogin("user1", "1234"); // �ڳ� �α���

        BackendGameData.Instance.GetGameData(); // ������ ���� �Լ�

        // [�߰�] ������ �ҷ��� �����Ͱ� �������� ���� ���, �����͸� ���� �����Ͽ� ����
        if (BackendGameData.userData == null)
        {
            BackendGameData.Instance.InsertGameData();
        }

        BackendGameData.Instance.LevelUp(); // [�߰�] ���ÿ� ����� �����͸� ����

        BackendGameData.Instance.GameDataUpdate(); //[�߰�] ������ ����� �����͸� �����(����� �κи�)

        Debug.Log("�׽�Ʈ�� �����մϴ�.");
    }
}
