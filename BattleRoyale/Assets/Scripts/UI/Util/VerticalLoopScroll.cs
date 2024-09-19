using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ILoopScrollDataSource
{
    /// <summary>활성화 Slot index를 제공 해당 함수를 통해서 index를 이용 데이터를 갱신</summary>
    public void ProvideData(Transform slotTransform, int index);
}

public class VerticalLoopScroll : UI_Base
{
    private ILoopScrollDataSource _dataSource;

    private int _dataCount;
    private string _slotPrefabKey;
    private RectTransform _content;
    private List<GameObject> _slotList = new List<GameObject>();
    
    private int _columns = 1;
    private Vector2 _cellSize;
    private float _previousContentPosY; // 이전 콘텐츠의 Y 위치
    private float _threshold = 20.0f;  // 스크롤 변경 임계값 (픽셀 단위)
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        gameObject.GetComponent<ScrollRect>().onValueChanged.AddListener(OnScrollValueChanged);

        return true;
    }

    private void OnEnable()
    {
        if (_content != null)
        { 
           _content.anchoredPosition = Vector2.zero;
        }
    }

    public void SetInfo(ILoopScrollDataSource dataSource, int dataCount, string slotPrefabKey, GameObject content)
    {
        _dataSource = dataSource;
        _dataCount = dataCount;
        _slotPrefabKey = slotPrefabKey;
        _content = content.GetComponent<RectTransform>();

        _cellSize = new Vector2(_content.rect.width, 100); // (width + padding, height + padding)
        //_columns = Mathf.CeilToInt(_content.rect.width / _cellSize.x);

        ResetAndCreateSlots();
    }

    public void UpdateLoopScrollData(int dataCount, string slotPrefabKey)
    {
        _dataCount = dataCount;
        _slotPrefabKey = slotPrefabKey;
        _content.anchoredPosition = Vector2.zero;



        ResetAndCreateSlots();
    }

    private void ResetAndCreateSlots()
    {
        foreach (var slot in _slotList)
        {
            Destroy(slot);
        }
        _slotList.Clear();

        CalculateContentHeight();
        CreateSlots();
        RefreshUI();
    }

    private void CalculateContentHeight()
    {
        int rows = Mathf.CeilToInt((float)_dataCount / _columns);
        _content.sizeDelta = new Vector2(_content.sizeDelta.x, rows * _cellSize.y);
    }

    private void CreateSlots()
    {
        float viewportHeight = GetComponent<RectTransform>().rect.height;
        int slotCount = Mathf.CeilToInt(viewportHeight / _cellSize.y) * _columns + _columns;

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Managers.Resource.Instantiate(_slotPrefabKey, _content);
            slot.SetActive(false);
            _slotList.Add(slot);
        }
    }

    private void OnScrollValueChanged(Vector2 pos)
    {
        // _dataCount가 생성된 Slot 갯수보다 작으면 갱신할 필요 x
        if (_dataCount <= Mathf.CeilToInt(GetComponent<RectTransform>().rect.height / _cellSize.y) * _columns + _columns)
        {
            return;
        }

        float currentContentPosY = _content.anchoredPosition.y;
        if (Mathf.Abs(currentContentPosY - _previousContentPosY) >= _threshold)
        {
            RefreshUI();
            _previousContentPosY = currentContentPosY;
        }
    }

    public void RefreshUI()
    {
        float contentY = _content.anchoredPosition.y;
        int firstVisibleRow = Mathf.FloorToInt(contentY / _cellSize.y);

        for (int i = 0; i < _slotList.Count; i++)
        {
            int rowIndex = firstVisibleRow + (i / _columns);
            int columnIndex = i % _columns;
            int slotIndex = rowIndex * _columns + columnIndex;

            // slotIndex 데이터 범위를 벗어나면 함수 return
            if (slotIndex >= _dataCount)
            {
                return;
            }

            if (slotIndex >= 0 && slotIndex < _dataCount)
            {
                GameObject slot = _slotList[i];
                slot.SetActive(true);
                RectTransform slotRect = slot.GetComponent<RectTransform>();
                slotRect.anchorMin = new Vector2(0, 1);
                slotRect.anchorMax = new Vector2(0, 1);
                slotRect.pivot = new Vector2(0, 1);
                slotRect.anchoredPosition = new Vector2(columnIndex * _cellSize.x, -rowIndex * _cellSize.y);
                _dataSource.ProvideData(slot.transform, slotIndex);
            }
            else
            {
                _slotList[i].SetActive(false);
            }
        }
    }

}