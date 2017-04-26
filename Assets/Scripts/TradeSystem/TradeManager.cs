using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.GameCore;
using Hunter.Tools;
using System;
using System.Reflection;
using Hunter.SettingMgr;
using Basic.ResourceMgr;

namespace Hunter.TradeSystem
{
    public class TradeManager : MonoBehaviour
    {
        // 表格对象
        NpcGoodsTypeSettingPro goodsTypeSetting;
        NpcGoodsNumSettingPro goodsNumSetting;

        #region 系统事件
        void Start()
        {
            // 读取表格
            ResourceMgr rm = new ResourceMgr();
            goodsTypeSetting = new NpcGoodsTypeSettingPro(rm);
            goodsNumSetting = new NpcGoodsNumSettingPro(rm);

            // 注册监听
            MessagingSystem.Instance.AttachListener(typeof(RequestNpcDataMsg), handleRequestNpcData);
            
        }

        void OnDestory()
        {
            if (MessagingSystem.IsAlive)
                MessagingSystem.Instance.DetachListener(typeof(RequestNpcDataMsg), handleRequestNpcData);
        }
        #endregion

        #region 逻辑需求
        /// <summary>
        /// 根据持有(需求)物品列表和持有(需求)物品数量列表 随机生成n种种类的持有(需求)物品的并且每种物品随机数量
        /// </summary>
        /// <param name="goodsCountRange">物品种类数量范围</param>
        /// <param name="gos">物品种类权限列表<物品id, 物品权值></param>
        /// <param name="gns">物品种类数量范围列表<物品id，范围></param>
        /// <returns><物品id，物品数量></returns>
        Dictionary<string, int> GenerateTradeInfo(string typeId, string numId)
        {
            // 根据配置ID而获得的两个setting对象
            string goodsCountRange = goodsTypeSetting.GetItem(typeId).CountRange;
            INpcGoodsTypeSetting gtSetting = goodsTypeSetting.GetItem(typeId);
            INpcGoodsNumSetting gnSetting = goodsNumSetting.GetItem(numId);


            // 权值列表<物品id, 权值>
            Dictionary<string, int> gos = new Dictionary<string, int>();
            PropertyInfo[] pGos = gtSetting.GetType().GetProperties();

            for (int i = 0; i < pGos.Length; ++i)
            {
                if (pGos[i].Name == "CountRange" || pGos[i].Name == "Id")
                    continue;

                if ((int)(pGos[i].GetValue(gtSetting, null)) != 0)
                {
                    gos.Add(pGos[i].Name, (int)(pGos[i].GetValue(gtSetting, null)));
                }
            }

            // 物品数量范围列表<物品id, 数量范围>
            Dictionary<string, string> gns = new Dictionary<string, string>();
            PropertyInfo[] pGns = gnSetting.GetType().GetProperties();

            for (int i = 0; i < pGns.Length; ++i)
            {
                if (pGos[i].Name == "Id")
                    continue;
                gns.Add(pGns[i].Name, (string)(pGns[i].GetValue(gnSetting, null)));
            }


            if (gos.Count < 0 || gns.Count < 0)
            {
                Debug.Log("错误发生于TradeSystem/GenerateTradeInfo中，参数对象为空");
                return null;
            }

            // 获得到物品数量范围
            List<int> countList = Lib.GetIntInString(goodsCountRange);
            int countOfType = UnityEngine.Random.Range(countList[0], countList[1]);

            // 获得物品权值列表
            int[] weightArray = new int[gos.Count];
            gos.Values.CopyTo(weightArray, 0);
            // 获得物品ID列表
            string[] idArray = new string[gos.Count];
            gos.Keys.CopyTo(idArray, 0);

            if (countOfType > weightArray.Length)
                countOfType = weightArray.Length;
            
            // 根据物品种类列表 对每种物品随机出数量
            Dictionary<string, int> result = new Dictionary<string, int>();

            if (countOfType == 0)
                return result;

            // 根据权值列表随机获取
            int[] goodsList = Lib.GetIndexByWeight(weightArray, countOfType);

            for (int i = 0; i < goodsList.Length; ++i)
            {
                string goodsId = idArray[goodsList[i]];
                string countRange = gns[goodsId];
                if (goodsId != null && countRange != null)
                {
                    countList = Lib.GetIntInString(countRange);
                    countOfType = UnityEngine.Random.Range(countList[0], countList[1]);
                    if(countOfType > 0)
                        result.Add(goodsId, countOfType);
                }
                else
                {
                    Debug.Log(string.Format("不存在的物品ID:{0}", goodsId));
                }
            }
            return result;
        }

        #endregion

        #region 消息处理事件
        bool handleRequestNpcData(BaseMessage bm)
        {
            RequestNpcDataMsg msg = bm as RequestNpcDataMsg;

            UpdateTradeStepMsg ntMsg = new UpdateTradeStepMsg(
                TradeStep.ReportTradeAI,
                GenerateTradeInfo(msg.demandId, msg.demandCountId),
                GenerateTradeInfo(msg.ownId, msg.ownCountId));

            MessagingSystem.Instance.QueueMessage(ntMsg);
            return true;
        }
        #endregion
    }

    #region 消息类

    class RequestNpcDataMsg : BaseMessage
    {
        public readonly string ownId, ownCountId, demandId, demandCountId;

        public RequestNpcDataMsg(string oId, string ocId, string dId, string dcId)
        {
            ownId = oId;
            ownCountId = ocId;
            demandId = dId;
            demandCountId = dcId;
        }
    }
    #endregion
}