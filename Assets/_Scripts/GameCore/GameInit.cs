using GameCore.UI;
using SCFrame;
using SCFrame.UI;
using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// 游戏初始化入口：加载配表、UI 栈，并进入剧情。
    /// </summary>
    public class GameInit : SingletonPersistent<GameInit>
    {
        [Header("启动章节 id")]
        [SerializeField] private int _m_startChapterId = 1;

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
            Story.StoryModel.instance.Initialize();
            SCTaskHelper.instance.Initialize();
            SCMsgCenter.instance.Initialize();
            SCPoolMgr.instance.Initialize();
            SCInputListener.instance.Initialize();
            UINodeMgr.instance.Initialize();
            startGame();
        }

        public override void OnDiscard()
        {
            UINodeMgr.instance.Discard();
            SCInputListener.instance.Discard();
            SCPoolMgr.instance.Discard();
            SCMsgCenter.instance.Discard();
            SCTaskHelper.instance.Discard();
            Story.StoryModel.instance.Discard();
            SCRefDataMgr.instance.Discard();
        }

        private void startGame()
        {
            UINodeMgr.instance.AddNode(new UINodeDialogue(SCUIShowType.FULL, _m_startChapterId));
        }
    }
}
