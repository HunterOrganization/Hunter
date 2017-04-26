/******************************************************************************** 
** Copyright(c) ixinyou All Rights Reserved. 
** auth： HHao	
** date： 10/09/2016
** desc： 基础数据类（临时）
** Ver : V1.0.0 
*********************************************************************************/
using Hunter.Tools;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Hunter.GameData
{
    public interface IDataManager
    {
        bool LoadAllData(DataManager data);

  //      Dictionary<string, MapHomeData> DicMapHomes { get; set; }
  //      Dictionary<string, BuildingData> DicBuildings { get; set; }
  //      HomeResource HomeResource { get; set; }
  //      Dictionary<string, MonsterData> DicMonsters { get; set; }
  //      List<MonsterBoxData> PlaceMonsterBoxList { get; set; }
  //      Dictionary<string, MonsterData> DicBagMonsters { get; set; }
  //      Dictionary<string, BagItemData> DicBagItems { get; set; }
  //      Dictionary<string, BagTransmitData> DicBagTransmits { get; set; }
  //      Dictionary<string, MonsterData> DicTransmitsMonsters { get; set; }
  //      Dictionary<string, BarrierData> DicHomeBarrier { get; set; }
		//WaitingAreaData WaitingAreaData { get; set; }
  //      RocketStationData RocketStationData { get; set; }
  //      List<RocketLog> AllRocketLogData { get; set; }
  //      List<BuildingData> DicDecorations { get; set; }

        long SaveTime { get; }
    }

    [Serializable]
    public class DataManager : IDataManager
    {
        long saveTime;

        public long SaveTime
        {
            get { return saveTime; }
            set { saveTime = value; }
        }

        public DataManager()
        {
            
        }

        // 管理所有数据
        public bool LoadAllData(DataManager data)
        {
            // 如果没有存档数据时返回新的数据结果
            //SaveTime = (data != null && data.SaveTime != null) ? data.SaveTime : 0;
            //DicMapHomes = (data != null && data.DicMapHomes != null) ? data.DicMapHomes : new Dictionary<string, MapHomeData>();
            //HomeResource = (data != null && data.HomeResource != null) ? data.HomeResource : new HomeResource();
            //HomeResource.BaseResources = (data != null && data.HomeResource != null && data.HomeResource.BaseResources != null) ? data.HomeResource.BaseResources : new BaseResource();
            //HomeResource.DicProduction = (data != null && data.HomeResource != null && data.HomeResource.DicProduction != null) ? data.HomeResource.DicProduction : new Dictionary<string, ProductionData>();
            //DicBuildings = (data != null && data.DicBuildings != null) ? data.DicBuildings : new Dictionary<string, BuildingData>();
            //DicMonsters = (data != null && data.DicMonsters != null) ? data.DicMonsters : new Dictionary<string, MonsterData>();
            //PlaceMonsterBoxList = (data != null && data.PlaceMonsterBoxList != null) ? data.PlaceMonsterBoxList : new List<MonsterBoxData>();
            //DicBagMonsters = (data != null && data.DicBagMonsters != null) ? data.DicBagMonsters : new Dictionary<string, MonsterData>();
            //DicBagItems = (data != null && data.DicBagItems != null) ? data.DicBagItems : new Dictionary<string, BagItemData>();
            //DicBagTransmits = (data != null && data.DicBagTransmits != null) ? data.DicBagTransmits : new Dictionary<string, BagTransmitData>();
            //DicTransmitsMonsters = (data != null && data.DicTransmitsMonsters != null) ? data.DicTransmitsMonsters : new Dictionary<string, MonsterData>();
            //DicHomeBarrier = (data != null && data.DicHomeBarrier != null) ? data.DicHomeBarrier : new Dictionary<string, BarrierData>();
            //WaitingAreaData = (data != null && data.WaitingAreaData != null) ? data.WaitingAreaData : new WaitingAreaData();
            //RocketStationData = (data != null && data.RocketStationData != null) ? data.RocketStationData : new RocketStationData();
            //AllRocketLogData = (data != null && data.AllRocketLogData != null) ? data.AllRocketLogData : new List<RocketLog>();
            //DicDecorations = (data != null && data.DicDecorations != null) ? data.DicDecorations : new List<BuildingData>();
            return true;
        }

        // 大地图中所有基地的数据
  //      private Dictionary<string, MapHomeData> dicMapHomes;
  //      public Dictionary<string, MapHomeData> DicMapHomes
  //      {
  //          get { return dicMapHomes; }
  //          set { dicMapHomes = value; }
  //      }

  //      // 基地的建筑列表
  //      private Dictionary<string, BuildingData> dicBuildings; // 用于生成基地建筑数据
  //      public Dictionary<string, BuildingData> DicBuildings
  //      {
  //          get { return dicBuildings; }
  //          set { dicBuildings = value; }
  //      }

  //      // 基地资源数据
  //      private HomeResource homeResource; // 其中的建筑资源列表用于生成建筑对应的资源信息
  //      public HomeResource HomeResource
  //      {
  //          get { return homeResource; }
  //          set { homeResource = value; }
  //      }

  //      // 怪物列表
  //      private Dictionary<string, MonsterData> dicMonsters; // 基地中所有怪物数据
  //      public Dictionary<string, MonsterData> DicMonsters
  //      {
  //          get { return dicMonsters; }
  //          set { dicMonsters = value; }
  //      }

  //      // 果盒列表
  //      private List<MonsterBoxData> placeMonsterBoxList = new List<MonsterBoxData>();
  //      public List<MonsterBoxData> PlaceMonsterBoxList
  //      {
  //          get { return placeMonsterBoxList; }
  //          set { placeMonsterBoxList = value; }
  //      }

  //      // 背包怪物列表
  //      private Dictionary<string, MonsterData> dicBagMonsters; // 地图背包中所有怪物数据
  //      public Dictionary<string, MonsterData> DicBagMonsters
  //      {
  //          get { return dicBagMonsters; }
  //          set { dicBagMonsters = value; }
  //      }

  //      // 背包道具列表
  //      private Dictionary<string, BagItemData> dicBagItems; // 地图背包中所有道具数据
  //      public Dictionary<string, BagItemData> DicBagItems
  //      {
  //          get { return dicBagItems; }
  //          set { dicBagItems = value; }
  //      }

  //      // 背包传送器列表
  //      private Dictionary<string, BagTransmitData> dicBagTransmits; // 正在使用的传送器数据
  //      public Dictionary<string, BagTransmitData> DicBagTransmits
  //      {
  //          get { return dicBagTransmits; }
  //          set { dicBagTransmits = value; }
  //      }

  //      // 正在传送中怪物列表
  //      private Dictionary<string, MonsterData> dicTransmitsMonsters; // 地图传送器中所有怪物数据
  //      public Dictionary<string, MonsterData> DicTransmitsMonsters
  //      {
  //          get { return dicTransmitsMonsters; }
  //          set { dicTransmitsMonsters = value; }
  //      }

  //      // 基地障碍物列表
  //      private Dictionary<string, BarrierData> dicHomeBarrier;
  //      public Dictionary<string, BarrierData> DicHomeBarrier
  //      {
  //          get { return dicHomeBarrier; }
  //          set { dicHomeBarrier = value; }
  //      }
		
		// // 等候区数据
		//private WaitingAreaData waitingAreaData;
  //      public WaitingAreaData WaitingAreaData
		//{
  //          get { return waitingAreaData; }
  //          set { waitingAreaData = value; }
		//}
		
  //      // 发射站数据
  //      private RocketStationData rocketStationData;
  //      public RocketStationData RocketStationData
  //      {
  //          get { return rocketStationData; }
  //          set { rocketStationData = value; }
  //      }

  //      private List<RocketLog> allRocketLogData;

  //      public List<RocketLog> AllRocketLogData
  //      {
  //          get { return allRocketLogData; }
  //          set { allRocketLogData = value; }
  //      }

  //      // 装饰管理模块
  //      private List<BuildingData> dicDecorations;

  //      public List<BuildingData> DicDecorations
  //      {
  //          get { return dicDecorations; }
  //          set { dicDecorations = value; }
  //      }
    }
}