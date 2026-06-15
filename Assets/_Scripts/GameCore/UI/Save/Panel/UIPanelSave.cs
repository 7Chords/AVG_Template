using GameCore;
using GameCore.Story;
using SCFrame;
using SCFrame.UI;
using UnityEngine.EventSystems;

namespace GameCore.UI
{
    public class UIPanelSave : _ASCUIPanelBase<UIMonoSave>
    {
        private ESavePanelMode _m_mode;
        private UIPanelSaveContainer _m_saveContainer;

        public UIPanelSave(UIMonoSave _mono, SCUIShowType _showType) : base(_mono, _showType)
        {
        }

        public void SetMode(ESavePanelMode _mode)
        {
            _m_mode = _mode;
        }

        public override void AfterInitialize()
        {
            if (mono.monoSaveContainer != null)
                _m_saveContainer = new UIPanelSaveContainer(mono.monoSaveContainer, SCUIShowType.INTERNAL);
        }

        public override void BeforeDiscard()
        {
            _m_saveContainer?.Discard();
        }

        public override void OnHidePanel()
        {
            if (mono.btnClose != null)
                mono.btnClose.RemoveMouseLeftClickDown(onBtnCloseClickDown);
            _m_saveContainer?.HidePanel();
        }

        public override void OnShowPanel()
        {
            if (mono.btnClose != null)
                mono.btnClose.AddMouseLeftClickDown(onBtnCloseClickDown);

            if (mono.txtTitle != null)
                mono.txtTitle.text = _m_mode == ESavePanelMode.SAVE ? "存档" : "读档";

            _m_saveContainer?.ShowPanel();
            _m_saveContainer?.Refresh(_m_mode, onSlotClick);
        }

        private void onBtnCloseClickDown(PointerEventData _data, object[] _objs)
        {
            UINodeMgr.instance.CloseTopNode();
        }

        private void onSlotClick(int _slotIndex)
        {
            if (_m_mode == ESavePanelMode.SAVE)
            {
                if (StorySaveMgr.SaveCurrentToSlot(_slotIndex))
                    _m_saveContainer?.Refresh(_m_mode, onSlotClick);
                return;
            }

            if (StorySaveMgr.LoadSlotToModel(_slotIndex))
                UINodeMgr.instance.CloseTopNode();
        }
    }
}
