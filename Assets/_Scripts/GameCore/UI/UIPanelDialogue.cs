using GameCore.RefData;
using GameCore.Story;
using GameCore;
using SCFrame;
using SCFrame.UI;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCore.UI
{
    public class UIPanelDialogue : _ASCUIPanelBase<UIMonoDialogue>
    {
        private int _m_chapterId;
        private StoryNodeRefObj _m_currentNode;
        private bool _m_isSelecting;
        private string _m_currentDialogueFullContent;
        private bool _m_dialogueLineRevealComplete;
        private CoroutineContainer _m_coroutineContainer = new CoroutineContainer();
        private UIPanelStorySelectContainer _m_selectContainer;

        public UIPanelDialogue(UIMonoDialogue _mono, SCUIShowType _showType) : base(_mono, _showType)
        {
        }

        public void SetChapterId(int _chapterId)
        {
            _m_chapterId = _chapterId;
        }

        public override void AfterInitialize()
        {
            mono.TryResolveRefs();
            if (mono.monoSelectContainer != null)
                _m_selectContainer = new UIPanelStorySelectContainer(mono.monoSelectContainer, SCUIShowType.INTERNAL);
        }

        public override void BeforeDiscard()
        {
            stopDialogueTypewriter();
            _m_selectContainer?.Discard();
        }

        public override void OnHidePanel()
        {
            SCMsgCenter.UnregisterMsg(SCMsgConst.STORY_SELECT_CONFIRM, onStorySelectConfirm);
            if (mono.imgClickArea != null)
                mono.imgClickArea.RemoveMouseLeftClickDown(onMouseClickDialogue);
            stopDialogueTypewriter();
            _m_selectContainer?.HidePanel();
        }

        public override void OnShowPanel()
        {
            SCMsgCenter.RegisterMsg(SCMsgConst.STORY_SELECT_CONFIRM, onStorySelectConfirm);
            if (mono.imgClickArea != null)
                mono.imgClickArea.AddMouseLeftClickDown(onMouseClickDialogue);

            _m_selectContainer?.ShowPanel();
            StoryModel.instance.StartChapter(_m_chapterId);
            _m_currentNode = StoryRefDataHelper.GetChapterStartNode(_m_chapterId);
            refreshShow();
        }

        private void refreshShow()
        {
            endSelectIfNeeded();
            if (_m_currentNode == null)
                return;

            StoryModel.instance.SetCurrentNode(_m_currentNode.id);
            stopDialogueTypewriter();

            if (_m_currentNode.nodeType == EStoryNodeType.STANDARD)
            {
                refreshSpeakerName(_m_currentNode.name);
                _m_currentDialogueFullContent = _m_currentNode.content ?? string.Empty;
                if (mono.txtContent != null)
                    mono.txtContent.text = string.Empty;

                if (string.IsNullOrEmpty(_m_currentDialogueFullContent))
                    _m_dialogueLineRevealComplete = true;
                else
                {
                    _m_dialogueLineRevealComplete = false;
                    _m_coroutineContainer.Run(dialogueTypewriterRoutine(), "DialogueTypewriter");
                }
            }
            else
            {
                _m_dialogueLineRevealComplete = true;
                _m_currentDialogueFullContent = string.Empty;
            }
        }

        private void refreshSpeakerName(string _speakerName)
        {
            if (mono.txtName == null)
                return;

            mono.txtName.text = _speakerName ?? string.Empty;
            CharacterRefObj character = StoryRefDataHelper.GetCharacterByName(_speakerName);
            if (character != null && ColorUtility.TryParseHtmlString(character.nameColor, out Color color))
                mono.txtName.color = color;
            else
                mono.txtName.color = Color.black;
        }

        private void stopDialogueTypewriter()
        {
            _m_coroutineContainer?.KillAll();
        }

        private IEnumerator dialogueTypewriterRoutine()
        {
            if (mono.txtContent == null)
            {
                _m_dialogueLineRevealComplete = true;
                yield break;
            }

            string full = _m_currentDialogueFullContent ?? string.Empty;
            StringBuilder sb = new StringBuilder(full.Length);
            float interval = Mathf.Max(0.001f, mono.dialogueTypewriterInterval);
            foreach (char c in full)
            {
                sb.Append(c);
                mono.txtContent.text = sb.ToString();
                yield return new WaitForSeconds(interval);
            }
            _m_dialogueLineRevealComplete = true;
        }

        private void onMouseClickDialogue(PointerEventData _data, object[] _objs)
        {
            if (_m_currentNode == null || _m_isSelecting)
                return;

            if (_m_currentNode.nodeType == EStoryNodeType.STANDARD && !_m_dialogueLineRevealComplete)
            {
                stopDialogueTypewriter();
                if (mono.txtContent != null)
                    mono.txtContent.text = _m_currentDialogueFullContent ?? string.Empty;
                _m_dialogueLineRevealComplete = true;
                return;
            }

            if (StoryRefDataHelper.IsStoryStopNode(_m_currentNode, _m_chapterId))
            {
                onStoryStop();
                return;
            }

            if (_m_currentNode.nextList != null && _m_currentNode.nextList.Count > 1)
            {
                _m_isSelecting = true;
                SCMsgCenter.SendMsg(SCMsgConst.STORY_START_SELECT, _m_currentNode);
                return;
            }

            jumpFromNode(_m_currentNode);
        }

        private void onStorySelectConfirm(object[] _objs)
        {
            if (_objs == null || _objs.Length == 0)
                return;

            StoryNodeRefObj selectNode = _objs[0] as StoryNodeRefObj;
            if (selectNode == null)
                return;

            SCMsgCenter.SendMsg(SCMsgConst.STORY_END_SELECT);
            _m_isSelecting = false;

            if (selectNode.nextList != null && selectNode.nextList.Count > 1)
            {
                _m_isSelecting = true;
                SCMsgCenter.SendMsg(SCMsgConst.STORY_START_SELECT, selectNode);
                return;
            }

            if (StoryRefDataHelper.IsStoryStopNode(selectNode, _m_chapterId))
            {
                onStoryStop();
                return;
            }

            jumpFromNode(selectNode);
        }

        /// <summary>按节点上的 nextChapterId + nextList 执行跳转。</summary>
        private void jumpFromNode(StoryNodeRefObj _node)
        {
            if (!StoryRefDataHelper.TryGetJumpTarget(_node, _m_chapterId, out int targetChapterId, out long targetNodeId))
            {
                onStoryStop();
                return;
            }

            if (targetChapterId != _m_chapterId)
                StoryModel.instance.MarkChapterCleared(_m_chapterId);

            _m_chapterId = targetChapterId;
            StoryModel.instance.StartChapter(_m_chapterId);
            _m_currentNode = StoryRefDataHelper.GetNode(_m_chapterId, targetNodeId);
            if (_m_currentNode == null)
            {
                Debug.LogError($"跳转失败：chapter={_m_chapterId}, node={targetNodeId}");
                onStoryStop();
                return;
            }
            refreshShow();
        }

        private void endSelectIfNeeded()
        {
            if (!_m_isSelecting)
                return;
            SCMsgCenter.SendMsg(SCMsgConst.STORY_END_SELECT);
            _m_isSelecting = false;
        }

        private void onStoryStop()
        {
            endSelectIfNeeded();
            StoryModel.instance.MarkChapterCleared(_m_chapterId);
            _m_currentNode = null;
        }
    }
}
