using GameCore;
using GameCore.Story;
using SCFrame;
using SCFrame.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.UI
{
    public class UIPanelSaveContainer : UIPanelContainerBase<UIMonoCommonContainer, UIPanelSaveItem, UIMonoSaveItem>
    {
        private readonly List<UIPanelSaveItem> _m_itemList = new List<UIPanelSaveItem>();
        private ESavePanelMode _m_mode;
        private Action<int> _m_onSlotClick;

        public UIPanelSaveContainer(UIMonoCommonContainer _mono, SCUIShowType _showType = SCUIShowType.INTERNAL)
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

        protected override UIPanelSaveItem creatItemPanel(UIMonoSaveItem _itemMono)
        {
            return new UIPanelSaveItem(_itemMono, SCUIShowType.INTERNAL);
        }

        public override void BeforeDiscard()
        {
            clearAllItems();
        }

        public void Refresh(ESavePanelMode _mode, Action<int> _onSlotClick)
        {
            _m_mode = _mode;
            _m_onSlotClick = _onSlotClick;
            clearAllItems();

            for (int i = 0; i < StorySaveMgr.MaxSlotCount; i++)
            {
                StorySaveSlotData data = StorySaveMgr.ReadSlot(i);
                addItem(i, data);
            }
        }

        private void addItem(int _slotIndex, StorySaveSlotData _data)
        {
            GameObject go = creatItemGO();
            if (go == null)
                return;

            UIMonoSaveItem itemMono = go.GetComponent<UIMonoSaveItem>();
            if (itemMono == null)
            {
                Debug.LogError($"存档项预制体缺少 {nameof(UIMonoSaveItem)} 组件");
                SCCommon.DestoryGameObject(go);
                return;
            }

            UIPanelSaveItem panel = creatItemPanel(itemMono);
            panel.SetInfo(_slotIndex, _m_mode, _data, _m_onSlotClick);
            panel.ShowPanel();
            _m_itemList.Add(panel);
        }

        private void clearAllItems()
        {
            foreach (UIPanelSaveItem item in _m_itemList)
                item?.Discard();
            _m_itemList.Clear();
        }

        public override void OnShowPanel()
        {
        }

        public override void OnHidePanel()
        {
        }

        public override void AfterInitialize()
        {
        }
    }
}
