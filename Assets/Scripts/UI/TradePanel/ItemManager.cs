using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.GameCore;

namespace Hunter.TrandeSystem
{
    /// <summary>
    /// 道具管理者 在全局消息驱动的情况下没办法和正常一样使用 所以针对性请求数据
    /// </summary>
    public class ItemManager : MonoBehaviour
    {
        // 物品列表<物品ID, 物品信息>
        Dictionary<string, string> goodsDict;

        void Start()
        {
            goodsDict = new Dictionary<string, string>();
            goodsDict.Add("pingguo0", "苹果0");
            goodsDict.Add("pingguo1", "苹果1");
            goodsDict.Add("pingguo2", "苹果2");
            goodsDict.Add("pingguo3", "苹果3");
            goodsDict.Add("pingguo4", "苹果4");
            goodsDict.Add("pingguo5", "苹果5");
            goodsDict.Add("pingguo6", "苹果6");
            goodsDict.Add("pingguo7", "苹果7");
            goodsDict.Add("pingguo8", "苹果8");
            goodsDict.Add("pingguo9", "苹果9");
            
            //MessagingSystem.Instance.AttachListener();
        }



        #region 对外接口

        #endregion
    }

    class GetItemInfoMsg: BaseMessage
    {

    }

    
}