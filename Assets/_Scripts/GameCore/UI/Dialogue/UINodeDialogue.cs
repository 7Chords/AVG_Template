using SCFrame;
using SCFrame.UI;
using UnityEngine;

namespace GameCore.UI
{
    public class UINodeDialogue : _ASCUINodeBase
    {
        private int _m_chapterId;
        private GameObject _m_panelGO;
        private UIPanelDialogue _m_dialoguePanel;
        private UIMonoDialogue _m_dialogueMono;

        public int chapterId => _m_chapterId;

        public UINodeDialogue(SCUIShowType _showType, int _chapterId = 1) : base(_showType)
        {
            _m_chapterId = _chapterId;
        }

        public override bool needHideWhenEnterNewSameTypeNode => true;
        public override bool needShowWhenQuitNewSameTypeNode => true;
        public override bool canQuitByEsc => false;
        public override bool canQuitByMouseRight => false;
        public override bool ignoreOnUIList => false;
        public override bool needMoveToBottomWhenHide => false;

        public override void CopyData(_ASCUINodeBase _anotherNode)
        {
            if (_anotherNode is UINodeDialogue dialogueNode)
                _m_chapterId = dialogueNode._m_chapterId;
        }

        public override string GetNodeName()
        {
            return nameof(UINodeDialogue);
        }

        public override string GetResName()
        {
            return "panel_dialogue";
        }

        public override void OnEnterNode()
        {
            _m_panelGO = ResourcesHelper.LoadGameObject(GetResName(), GetRootTransform(), true);
            if (_m_panelGO == null)
            {
                Debug.LogError("未找到资源名为" + GetResName() + "的资源!!!");
                return;
            }

            _m_dialogueMono = _m_panelGO.GetComponent<UIMonoDialogue>();
            if (_m_dialogueMono == null)
            {
                Debug.LogError("资源名为" + GetResName() + "的资源上不存在对应的Mono!!!");
                return;
            }

            _m_dialoguePanel = new UIPanelDialogue(_m_dialogueMono, _m_showType);
            _m_dialoguePanel.SetChapterId(_m_chapterId);
            _m_dialoguePanel.Initialize();
        }

        public override void OnHideNode()
        {
            _m_dialoguePanel?.HidePanel();
        }

        public override void OnQuitNode()
        {
            _m_dialoguePanel?.Discard();
        }

        public override void OnShowNode()
        {
            _m_dialoguePanel?.ShowPanel();
        }
    }
}
