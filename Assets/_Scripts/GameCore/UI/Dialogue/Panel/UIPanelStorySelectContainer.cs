using GameCore.RefData;
using GameCore.Story;
using GameCore;
using SCFrame;
using SCFrame.UI;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.UI
{
    public class UIPanelStorySelectContainer : UIPanelContainerBase<UIMonoCommonContainer, UIPanelStorySelectItem, UIMonoStorySelectItem>
    {
        private List<UIPanelStorySelectItem> _m_selectItemList;

        public UIPanelStorySelectContainer(UIMonoCommonContainer _mono, SCUIShowType _showType = SCUIShowType.INTERNAL)
            : base(_mono, _showType)
        {
        }

        protected override GameObject creatItemGO()
        {
            if (mono.layoutGroup == null)
            {
                Debug.LogError("UIMonoCommonContainer.layoutGroup 未绑定");
                return null;
            }
            if (string.IsNullOrEmpty(mono.prefabItemObjName))
            {
                Debug.LogError("UIMonoCommonContainer.prefabItemObjName 未配置");
                return null;
            }
            return ResourcesHelper.LoadGameObject(mono.prefabItemObjName, mono.layoutGroup.transform);
        }

        protected override UIPanelStorySelectItem creatItemPanel(UIMonoStorySelectItem _mono)
        {
            return new UIPanelStorySelectItem(_mono, SCUIShowType.INTERNAL);
        }

        public override void AfterInitialize()
        {
            _m_selectItemList = new List<UIPanelStorySelectItem>();
        }

        public override void BeforeDiscard()
        {
            clearAllItems();
        }

        public override void OnHidePanel()
        {
            SCMsgCenter.UnregisterMsg(SCMsgConst.STORY_START_SELECT, onStoryStartSelect);
            SCMsgCenter.UnregisterMsgAct(SCMsgConst.STORY_END_SELECT, onStoryEndSelect);
        }

        public override void OnShowPanel()
        {
            SCMsgCenter.RegisterMsg(SCMsgConst.STORY_START_SELECT, onStoryStartSelect);
            SCMsgCenter.RegisterMsgAct(SCMsgConst.STORY_END_SELECT, onStoryEndSelect);
        }

        private void onStoryStartSelect(object[] _objs)
        {
            if (_objs == null || _objs.Length == 0)
                return;

            StoryNodeRefObj parentNode = _objs[0] as StoryNodeRefObj;
            if (parentNode?.nextList == null)
                return;

            int chapterId = StoryModel.instance.currentChapterId;
            for (int i = 0; i < parentNode.nextList.Count; i++)
            {
                StoryNodeRefObj selectNode = StoryRefDataHelper.GetNode(chapterId, parentNode.nextList[i]);
                if (selectNode == null || selectNode.nodeType != EStoryNodeType.SELECT)
                    continue;
                addItem(selectNode);
            }
        }

        private void onStoryEndSelect()
        {
            clearAllItems();
        }

        private void addItem(StoryNodeRefObj _storyNodeRefObj)
        {
            GameObject go = creatItemGO();
            if (go == null)
                return;

            UIMonoStorySelectItem itemMono = go.GetComponent<UIMonoStorySelectItem>();
            if (itemMono == null)
            {
                Debug.LogError($"选项预制体缺少 {nameof(UIMonoStorySelectItem)} 组件");
                SCCommon.DestoryGameObject(go);
                return;
            }

            UIPanelStorySelectItem panel = creatItemPanel(itemMono);
            panel.SetInfo(_storyNodeRefObj);
            panel.ShowPanel();
            _m_selectItemList.Add(panel);
        }

        private void clearAllItems()
        {
            if (_m_selectItemList == null)
                return;
            foreach (UIPanelStorySelectItem item in _m_selectItemList)
                item?.Discard();
            _m_selectItemList.Clear();
        }
    }
}
