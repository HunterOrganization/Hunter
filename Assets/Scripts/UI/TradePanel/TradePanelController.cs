using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hunter.GameCore;
using Hunter.TradeSystem;
using DG.Tweening;
using Hunter.EventSystem;

namespace Hunter.UI
{
    public enum TradeResultState
    {
        Finish,
        NotNeed,
        Enough,
        NotDelighted,
    }

    public class TradePanelController : MonoBehaviour
    {
        [SerializeField]
        GameObject HunterGoods;
        [SerializeField]
        GameObject NpcGoods;
        [SerializeField]
        float MaxWeight = 200;

        // 缓存UI对象
        Text tipDialog;
        Text tipWeight;
        Button confirmBtn;
        Button cancelBtn;
        GoodsListController hunterGoodsController;
        GoodsListController npcGoodsController;
        GameObject shadePanel;


        // 数据缓存
        ReportTradeData currTradeData;

        // 提示语句
        string[] tipStart = { "你想和我交换什么？", "好吧，让你看看我所拥有的.", "交易，为了更好的活下去.", "那得看看你有没有我想要的."};
        string[] tipFinish = { "成交!", "你不会后悔的."};
        string[] tipNotNeed = { "{0}?这东西我不需要.", "我想我不需要{0}.", "{0}对我来说一文不值."};
        string[] tipEnough = { "我不需要那么多的{0}", ""};
        string[] tipNotDelighted = { "我不认为这样的交易对我有利！", "你最好拿出让我满意的物品."};
        string[] tipCancelTrade = { "希望你以后不会后悔今天的决定." };
        string[] tipWeightEnough = { "" };

        #region 系统事件
        void Start()
        {
            // 初始化UI
            hunterGoodsController = HunterGoods.GetComponent<GoodsListController>();
            npcGoodsController = NpcGoods.GetComponent<GoodsListController>();

            shadePanel = transform.Find("ShadePanel").gameObject;

            confirmBtn = transform.Find("ConfirmBtn").GetComponent<Button>();
            cancelBtn = transform.Find("CancelBtn").GetComponent<Button>();

            confirmBtn.onClick.AddListener(delegate () { this.OnClickConfirm(); });
            cancelBtn.onClick.AddListener(delegate () { this.OnClickCancel(); });

            tipDialog = transform.Find("TipDialog").GetComponent<Text>();
            tipWeight = transform.Find("TipWeight").GetComponent<Text>();
            // -------------------------------------------- 注册监听 -------------------------------------------- //
            MessagingSystem.Instance.AttachListener(typeof(ReportTradeData), handleShowTradeGoods); // 开启交易界面
            MessagingSystem.Instance.AttachListener(typeof(ReportTradeState), handleClossPanel); // 关闭交易界面
            // -------------------------------------------------------------------------------------------------- //
        }

        void OnDestory()
        {
            if (MessagingSystem.IsAlive)
            {
                MessagingSystem.Instance.DetachListener(typeof(ReportTradeData), handleShowTradeGoods);
                MessagingSystem.Instance.DetachListener(typeof(ReportTradeState), handleClossPanel);
            }
        }

        #endregion

        #region 逻辑处理
        void OnClickConfirm()
        {
            if (CalculationWeight(currTradeData) > MaxWeight)
            {
                Tweener tweener = tipWeight.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).From();

            }
            else
            {
                MessagingSystem.Instance.QueueMessage(new UpdateTradeStepMsg(TradeStep.ConfirmTrade));
            }
        }

        void OnClickCancel()
        {
            int tipCancelTradeIdx = Random.Range(0, tipCancelTrade.Length);
            tipDialog.text = tipCancelTrade[tipCancelTradeIdx];

            confirmBtn.interactable = false;
            cancelBtn.interactable = false;
            ClossPanel();
        }

        void OpenPanel()
        {
            confirmBtn.interactable = true;
            cancelBtn.interactable = true;
            Tweener tweener = gameObject.transform.DOScaleX(1, 0.5f);
        }

        void ClossPanel()
        {
            Tweener tweener = gameObject.transform.DOScaleX(0, 0.5f);
            // 通知交易事件完毕
            MessagingSystem.Instance.QueueMessage(new UpdateEventStateMsg(EventState.EventEnd));
        }

        float CalculationWeight(ReportTradeData rtd)
        {
            // 计算猎人负重
            float result = 0.0f;
            foreach(var item in rtd.hunterGoods)
            {
                result += (item.weight * (item.count - item.giveCount));
            }
            foreach (var item in rtd.npcGoods)
            {
                if(item.giveCount > 0)
                    result += (item.weight * item.giveCount);
            }

            return result;
        }

        void OnGoodsCountChange()
        {
            float bagWeight = CalculationWeight(currTradeData);
            tipWeight.text = string.Format("背包({0}/{1})", bagWeight, MaxWeight);
            tipWeight.color = bagWeight > MaxWeight ? Color.red : Color.green;
        }
        #endregion

        #region 消息处理事件
        bool handleShowTradeGoods(BaseMessage bm)
        {
            currTradeData = bm as ReportTradeData;

            List<GoodsUIInfo> hUIInfo = new List<GoodsUIInfo>();
            foreach(var goods in currTradeData.hunterGoods)
            {
                hUIInfo.Add(new GoodsUIInfo(goods, OnGoodsCountChange));
            }

            List<GoodsUIInfo> nUIInfo = new List<GoodsUIInfo>();
            foreach (var goods in currTradeData.npcGoods)
            {
                nUIInfo.Add(new GoodsUIInfo(goods, OnGoodsCountChange));
            }

            hunterGoodsController.SetData(hUIInfo);
            npcGoodsController.SetData(nUIInfo);

            int tipStartIdx = Random.Range(0, tipStart.Length);
            tipDialog.text = tipStart[tipStartIdx];
            float bagWeight = CalculationWeight(currTradeData);
            tipWeight.text = string.Format("背包({0}/{1})", bagWeight, MaxWeight);
            tipWeight.color = bagWeight > MaxWeight ?  Color.red : Color.green;


            OpenPanel();
            return true;
        }

        bool handleClossPanel(BaseMessage bm)
        {
            ReportTradeState msg = bm as ReportTradeState;

            switch (msg.tradeState)
            {
                case TradeResultState.Finish:
                    int tipFinishIdx = Random.Range(0, tipFinish.Length);
                    tipDialog.text = tipFinish[tipFinishIdx];
                    confirmBtn.interactable = false;
                    cancelBtn.interactable = false;
                    ClossPanel();
                    break;
                case TradeResultState.Enough:
                    int tipEnoughIdx = Random.Range(0, tipEnough.Length);
                    tipDialog.text = string.Format(tipEnough[tipEnoughIdx], msg.goodsName);
                    break;
                case TradeResultState.NotDelighted:
                    int tipNotDelightedIdx = Random.Range(0, tipNotDelighted.Length);
                    tipDialog.text = tipNotDelighted[tipNotDelightedIdx];
                    break;
                case TradeResultState.NotNeed:
                    int tipNotNeedIdx = Random.Range(0, tipNotNeed.Length);
                    tipDialog.text = string.Format(tipNotNeed[tipNotNeedIdx], msg.goodsName); 
                    break;
                default:
                    Debug.Log("错误发生于TradePanleController/handleClossPanel, 无效的状态信息");
                    break;
            }
            return true;
        }
        #endregion
        
    }
    
    public class GoodsUIInfo
    {
        public delegate void OnClickEvent();

        public GoodsInfo gInfo;
        public OnClickEvent callback;

        public GoodsUIInfo(GoodsInfo gi, OnClickEvent cb)
        {
            gInfo = gi;
            callback = cb;
        }
    }
    
    #region 消息类型
    public class ReportTradeData: BaseMessage
    {
        public readonly List<GoodsInfo> npcGoods;
        public readonly List<GoodsInfo> hunterGoods;

        public ReportTradeData(List<GoodsInfo> ng, List<GoodsInfo> hg)
        {
            npcGoods = ng;
            hunterGoods = hg;
        }
    }

    public class ReportTradeState: BaseMessage
    {
        public readonly TradeResultState tradeState;
        public readonly string goodsName;
        public ReportTradeState(TradeResultState ts, string gn)
        {
            tradeState = ts;
            goodsName = gn;
        }
    }
    
    #endregion
}