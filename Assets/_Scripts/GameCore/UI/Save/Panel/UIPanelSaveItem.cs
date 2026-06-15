using DG.Tweening;
using GameCore;
using GameCore.Story;
using SCFrame;
using SCFrame.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCore.UI
{
    public class UIPanelSaveItem : _ASCUIPanelBase<UIMonoSaveItem>
    {
        private int _m_slotIndex;
        private ESavePanelMode _m_mode;
        private Action<int> _m_onSlotClick;
        private TweenContainer _m_tweenContainer;

        public UIPanelSaveItem(UIMonoSaveItem _mono, SCUIShowType _showType) : base(_mono, _showType)
        {
        }

        public override void AfterInitialize()
        {
            _m_tweenContainer = new TweenContainer();
        }

        public override void BeforeDiscard()
        {
            _m_tweenContainer?.KillAllDoTween();
            _m_tweenContainer = null;
        }

        public override void OnHidePanel()
        {
            if (mono.btnSlot != null)
            {
                mono.btnSlot.RemoveMouseLeftClickDown(onBtnSlotClickDown);
                mono.btnSlot.RemoveMouseEnter(onBtnSlotMouseEnter);
                mono.btnSlot.RemoveMouseExit(onBtnSlotMouseExit);
            }
            GetGameObject().transform.localScale = Vector3.one;
        }

        public override void OnShowPanel()
        {
            if (mono.btnSlot == null)
                return;
            mono.btnSlot.AddMouseLeftClickDown(onBtnSlotClickDown);
            mono.btnSlot.AddMouseEnter(onBtnSlotMouseEnter);
            mono.btnSlot.AddMouseExit(onBtnSlotMouseExit);
        }

        public void SetInfo(int _slotIndex, ESavePanelMode _mode, StorySaveSlotData _data, Action<int> _onSlotClick)
        {
            _m_slotIndex = _slotIndex;
            _m_mode = _mode;
            _m_onSlotClick = _onSlotClick;
            refreshShow(_data);
        }

        private void refreshShow(StorySaveSlotData _data)
        {
            bool hasData = _data != null && _data.HasData();
            if (mono.txtSlotIndex != null)
                mono.txtSlotIndex.text = (_m_slotIndex + 1).ToString();

            if (mono.goHasData != null)
                mono.goHasData.SetActive(hasData);
            if (mono.goEmpty != null)
                mono.goEmpty.SetActive(!hasData);

            if (!hasData)
            {
                if (mono.txtEmpty != null)
                    mono.txtEmpty.text = _m_mode == ESavePanelMode.SAVE ? "空" : "无存档";
                return;
            }

            if (mono.txtChapter != null)
                mono.txtChapter.text = _data.chapterName ?? string.Empty;
            if (mono.txtTime != null)
                mono.txtTime.text = _data.saveTimeText ?? string.Empty;
        }

        private void onBtnSlotClickDown(PointerEventData _data, object[] _objs)
        {
            if (_m_mode == ESavePanelMode.LOAD && !StorySaveMgr.HasSlotData(_m_slotIndex))
                return;
            _m_onSlotClick?.Invoke(_m_slotIndex);
        }

        private void onBtnSlotMouseEnter(PointerEventData _data, object[] _objs)
        {
            _m_tweenContainer?.RegDoTween(
                GetGameObject().transform.DOScale(mono.scaleMouseEnter, mono.scaleChgDuration));
        }

        private void onBtnSlotMouseExit(PointerEventData _data, object[] _objs)
        {
            _m_tweenContainer?.RegDoTween(
                GetGameObject().transform.DOScale(Vector3.one, mono.scaleChgDuration));
        }
    }
}
