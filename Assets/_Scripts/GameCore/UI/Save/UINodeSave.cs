using SCFrame;
using SCFrame.UI;
using UnityEngine;
using GameCore;

namespace GameCore.UI
{
    public class UINodeSave : _ASCUINodeBase
    {
        private ESavePanelMode _m_mode;
        private GameObject _m_panelGO;
        private UIPanelSave _m_savePanel;
        private UIMonoSave _m_saveMono;

        public UINodeSave(SCUIShowType _showType, ESavePanelMode _mode) : base(_showType)
        {
            _m_mode = _mode;
        }

        public override bool needHideWhenEnterNewSameTypeNode => false;
        public override bool needShowWhenQuitNewSameTypeNode => false;
        public override bool canQuitByEsc => true;
        public override bool canQuitByMouseRight => true;
        public override bool ignoreOnUIList => false;
        public override bool needMoveToBottomWhenHide => false;

        public override void CopyData(_ASCUINodeBase _anotherNode)
        {
            if (_anotherNode is UINodeSave saveNode)
                _m_mode = saveNode._m_mode;
        }

        public override string GetNodeName()
        {
            return nameof(UINodeSave);
        }

        public override string GetResName()
        {
            return "panel_save";
        }

        public override void OnEnterNode()
        {
            _m_panelGO = ResourcesHelper.LoadGameObject(GetResName(), GetRootTransform(), true);
            if (_m_panelGO == null)
            {
                Debug.LogError("未找到资源名为" + GetResName() + "的资源!!!");
                return;
            }

            _m_saveMono = _m_panelGO.GetComponent<UIMonoSave>();
            if (_m_saveMono == null)
            {
                Debug.LogError("资源名为" + GetResName() + "的资源上不存在对应的Mono!!!");
                return;
            }

            _m_savePanel = new UIPanelSave(_m_saveMono, _m_showType);
            _m_savePanel.SetMode(_m_mode);
            _m_savePanel.Initialize();
        }

        public override void OnHideNode()
        {
            _m_savePanel?.HidePanel();
        }

        public override void OnQuitNode()
        {
            _m_savePanel?.Discard();
        }

        public override void OnShowNode()
        {
            _m_savePanel?.SetMode(_m_mode);
            _m_savePanel?.ShowPanel();
        }
    }
}
