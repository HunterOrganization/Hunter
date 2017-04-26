using UnityEngine;
using System.Collections;
using Hunter.UISystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class DGItemList 
{
    private RectTransform _trans;
    private GameObject _go;

    private RectTransform transContent;
    private ScrollRect scrollRect;
    private GameObject item;

    private DataGrid dataGrid;



    public DGItemList(RectTransform trans)
    {
        _trans = trans;
        _go = _trans.gameObject;

        Init();
    }
	
    private void Init()
    {
        transContent = _trans.Find("Content").GetComponent<RectTransform>();
        scrollRect = _trans.GetComponent<ScrollRect>();
        item = _trans.Find("Content/Item").gameObject;

        dataGrid = _go.AddComponent<DataGrid>();
        dataGrid.SetItemRender(item, typeof(DGItemRender));
        dataGrid.useLoopItems = true;
    }


    public void Show()
    {
        if (_go != null)
        {
            _go.SetActive(true);
        }
        // 应该是传递一个数据结构 然后到每个Item中再去解析获取数据
        List<string> datas = new List<string>();
        for (int i = 0; i < 20;i++ )
        {
            datas.Add("DataGrid data -->" + i);
        }
        dataGrid.ResetScrollPosition();
        dataGrid.Data = datas.ToArray();
    }
}

