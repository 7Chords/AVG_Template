using DG.Tweening;
using GameCore.RefData;
using SCFrame;
using SCFrame.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCore.UI
{
    public class UIPanelStorySelectItem : _ASCUIPanelBase<UIMonoStorySelectItem>
    {
        private StoryNodeRefObj _m_storyNodeRefObj;
        private TweenContainer _m_tweenContainer;

        public UIPanelStorySelectItem(UIMonoStorySelectItem _mono, SCUIShowType _showType) : base(_mono, _showType)
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
            if (mono.btnSelect != null)
            {
                mono.btnSelect.RemoveMouseLeftClickDown(onBtnSelectClickDown);
                mono.btnSelect.RemoveMouseEnter(onBtnSelectMouseEnter);
                mono.btnSelect.RemoveMouseExit(onBtnSelectMouseExit);
            }
            GetGameObject().transform.localScale = Vector3.one;
        }

        public override void OnShowPanel()
        {
            if (mono.btnSelect == null)
                return;
            mono.btnSelect.AddMouseLeftClickDown(onBtnSelectClickDown);
            mono.btnSelect.AddMouseEnter(onBtnSelectMouseEnter);
            mono.btnSelect.AddMouseExit(onBtnSelectMouseExit);
        }

        public void SetInfo(StoryNodeRefObj _refObj)
        {
            _m_storyNodeRefObj = _refObj;
            refreshShow();
        }

        private void refreshShow()
        {
            if (_m_storyNodeRefObj == null || mono.txtContent == null)
                return;

            if (!string.IsNullOrEmpty(_m_storyNodeRefObj.content))
                mono.txtContent.text = _m_storyNodeRefObj.content;
            else
                mono.txtContent.text = _m_storyNodeRefObj.name ?? string.Empty;
        }

        private void onBtnSelectClickDown(PointerEventData _data, object[] _objs)
        {
            SCMsgCenter.SendMsg(SCMsgConst.STORY_SELECT_CONFIRM, _m_storyNodeRefObj);
        }

        private void onBtnSelectMouseEnter(PointerEventData _data, object[] _objs)
        {
            _m_tweenContainer?.RegDoTween(
                GetGameObject().transform.DOScale(mono.scaleMouseEnter, mono.scaleChgDuration));
        }

        private void onBtnSelectMouseExit(PointerEventData _data, object[] _objs)
        {
            _m_tweenContainer?.RegDoTween(
                GetGameObject().transform.DOScale(Vector3.one, mono.scaleChgDuration));
        }
    }
}
