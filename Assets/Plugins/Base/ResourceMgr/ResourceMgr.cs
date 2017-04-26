/******************************************************************************** 
** Copyright(c) 2016 ixinyou All Rights Reserved. 
** auth： JunC
** date： 2016/7/26 17:26:47 
** desc： 资源管理模块(只是负责Resource或AssetBoundle目录下的资源管理)的声明和实现
**        加载道具配置的例子:
**        IEnumerator LoadResource(IResourceMgr resourceMgr)
**        {
**             ResourceRequest<TextAsset> textToBundle = resourceMgr.LoadAsync<TextAsset>("config/prop");
**             yield return textToBundle;
**             TextAsset textAsset = textToBundle.Asset;
**             Debug.Log(textAsset.text);
**        }
** Ver : V1.0.0 
*********************************************************************************/

using System;
using UnityEngine;

namespace Basic.ResourceMgr
{
    /// <summary>
    /// 资源管理接口声明
    /// </summary>
    public interface IResourceMgr
    {
        /// <summary>
        /// 异步加载指定类型的资源
        /// </summary>
        /// <typeparam name="T">资源的类型</typeparam>
        /// <param name="path">资源的路径</param>
        /// <returns>资源加载的操作请求对象</returns>
        ResourceRequest<T> LoadAsync<T>(string path) where T : UnityEngine.Object;

        /// <summary>
        /// 同步加载指定类型的资源
        /// </summary>
        /// <typeparam name="T">资源的类型</typeparam>
        /// <param name="path">资源的路径</param>
        /// <returns>资源对象</returns>
        T Load<T>(string path) where T : UnityEngine.Object;
    }

    /// <summary>
    /// 资源加载的请求
    /// </summary>
    public class ResourceRequest<T> : CustomYieldInstruction
        where T : UnityEngine.Object
    {
        /// <summary>
        /// 资源加载的请求
        /// </summary>
        private UnityEngine.ResourceRequest resourceRequest;

        /// <summary>
        /// 是否继续等待
        /// </summary>
        public override bool keepWaiting
        {
            get
            {
                return !resourceRequest.isDone;
            }
        }

        /// <summary>
        /// 资源对象
        /// </summary>
        public T Asset { get { return resourceRequest.asset as T; } }
        
        /// <summary>
        /// 总的进度
        /// </summary>
        public float Progress { get { return resourceRequest.progress; } }
        
        public ResourceRequest(string path)
        {
            resourceRequest = UnityEngine.Resources.LoadAsync<T>(path);
        }
    }
    /// <summary>
    /// 资源管理接口的简单实现
    /// </summary>
    public class ResourceMgr : IResourceMgr
    {
        /// <summary>
        /// 同步加载指定类型的资源
        /// </summary>
        /// <typeparam name="T">资源的类型</typeparam>
        /// <param name="path">资源的路径</param>
        /// <returns>资源对象</returns>
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        /// <summary>
        /// 加载指定类型的资源
        /// </summary>
        /// <typeparam name="T">资源的类型</typeparam>
        /// <param name="path">资源的路径</param>
        public ResourceRequest<T> LoadAsync<T>(string path) where T : UnityEngine.Object
        {
            return new ResourceRequest<T>(path);
        }


    }
}