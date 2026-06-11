using SCFrame;
using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// 游戏初始化入口：加载配表与框架单例。
    /// </summary>
    public class GameInit : SingletonPersistent<GameInit>
    {
        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Discard();
        }

        public override void OnInitialize()
        {
            SCRefDataMgr.instance.Initialize();
            SCTaskHelper.instance.Initialize();
            SCMsgCenter.instance.Initialize();
            SCPoolMgr.instance.Initialize();
            SCInputListener.instance.Initialize();
        }

        public override void OnDiscard()
        {
            SCInputListener.instance.Discard();
            SCPoolMgr.instance.Discard();
            SCMsgCenter.instance.Discard();
            SCTaskHelper.instance.Discard();
            SCRefDataMgr.instance.Discard();
        }
    }
}
