using GameCore.RefData;
using GameCore.Story;
using GameCore;
using SCFrame;
using SCFrame.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        private readonly List<GameObject> _m_choiceBtnList = new List<GameObject>();

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
            ensureChoiceRoot();
        }

        public override void BeforeDiscard()
        {
            stopDialogueTypewriter();
            hideChoices();
        }

        public override void OnHidePanel()
        {
            if (mono.imgClickArea != null)
                mono.imgClickArea.RemoveMouseLeftClickDown(onMouseClickDialogue);
            stopDialogueTypewriter();
            hideChoices();
        }

        public override void OnShowPanel()
        {
            if (mono.imgClickArea != null)
                mono.imgClickArea.AddMouseLeftClickDown(onMouseClickDialogue);

            StoryModel.instance.StartChapter(_m_chapterId);
            _m_currentNode = StoryRefDataHelper.GetChapterStartNode(_m_chapterId);
            refreshShow();
        }

        private void ensureChoiceRoot()
        {
            if (mono.choiceRoot != null)
                return;

            GameObject rootGo = new GameObject("choice_root", typeof(RectTransform), typeof(VerticalLayoutGroup));
            RectTransform rect = rootGo.GetComponent<RectTransform>();
            rect.SetParent(mono.transform, false);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 120f);
            rect.sizeDelta = new Vector2(600f, 300f);

            VerticalLayoutGroup layout = rootGo.GetComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.spacing = 12f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            mono.choiceRoot = rect;
        }

        private void refreshShow()
        {
            hideChoices();
            if (_m_currentNode == null)
            {
                Debug.LogError($"章节 {_m_chapterId} 找不到起始剧情节点");
                return;
            }

            StoryModel.instance.SetCurrentNode(_m_currentNode.id);
            stopDialogueTypewriter();

            if (_m_currentNode.nodeType == EStoryNodeType.STANDARD)
            {
                refreshSpeakerName(_m_currentNode.name);
                _m_currentDialogueFullContent = _m_currentNode.content ?? string.Empty;
                if (mono.txtContent != null)
                    mono.txtContent.text = string.Empty;

                if (string.IsNullOrEmpty(_m_currentDialogueFullContent))
                {
                    _m_dialogueLineRevealComplete = true;
                }
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

            if (StoryRefDataHelper.IsChapterEndNode(_m_currentNode))
            {
                onChapterEnd();
                return;
            }

            if (_m_currentNode.nextList != null && _m_currentNode.nextList.Count > 1)
            {
                showChoices(_m_currentNode);
                return;
            }

            advanceToNextSingleNode();
        }

        private void advanceToNextSingleNode()
        {
            if (_m_currentNode == null || _m_currentNode.nextList == null || _m_currentNode.nextList.Count < 1)
                return;

            long nextId = _m_currentNode.nextList[0];
            if (nextId <= 0)
            {
                onChapterEnd();
                return;
            }

            _m_currentNode = StoryRefDataHelper.GetNode(_m_chapterId, nextId);
            refreshShow();
        }

        private void showChoices(StoryNodeRefObj _parentNode)
        {
            hideChoices();
            if (_parentNode?.nextList == null)
                return;

            foreach (long nextId in _parentNode.nextList)
            {
                StoryNodeRefObj choiceNode = StoryRefDataHelper.GetNode(_m_chapterId, nextId);
                if (choiceNode == null || choiceNode.nodeType != EStoryNodeType.SELECT)
                    continue;
                createChoiceButton(choiceNode);
            }

            _m_isSelecting = _m_choiceBtnList.Count > 0;
        }

        private void createChoiceButton(StoryNodeRefObj _choiceNode)
        {
            GameObject btnGo = new GameObject($"choice_{_choiceNode.id}",
                typeof(RectTransform), typeof(Image), typeof(Button));
            btnGo.transform.SetParent(mono.choiceRoot, false);

            RectTransform btnRect = btnGo.GetComponent<RectTransform>();
            btnRect.sizeDelta = new Vector2(520f, 56f);

            Image btnImage = btnGo.GetComponent<Image>();
            btnImage.color = new Color(0f, 0f, 0f, 0.55f);

            GameObject textGo = new GameObject("txt", typeof(RectTransform), typeof(Text));
            textGo.transform.SetParent(btnGo.transform, false);
            RectTransform textRect = textGo.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(16f, 8f);
            textRect.offsetMax = new Vector2(-16f, -8f);

            Text txt = textGo.GetComponent<Text>();
            txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            txt.fontSize = 28;
            txt.alignment = TextAnchor.MiddleLeft;
            txt.color = Color.white;
            txt.text = _choiceNode.name ?? string.Empty;

            StoryNodeRefObj captured = _choiceNode;
            btnGo.GetComponent<Button>().onClick.AddListener(() => onChoiceClick(captured));
            _m_choiceBtnList.Add(btnGo);
        }

        private void onChoiceClick(StoryNodeRefObj _choiceNode)
        {
            if (_choiceNode == null || _choiceNode.nextList == null || _choiceNode.nextList.Count < 1)
                return;

            _m_isSelecting = false;
            hideChoices();

            long nextId = _choiceNode.nextList[0];
            if (nextId <= 0)
            {
                onChapterEnd();
                return;
            }

            _m_currentNode = StoryRefDataHelper.GetNode(_m_chapterId, nextId);
            refreshShow();
        }

        private void hideChoices()
        {
            _m_isSelecting = false;
            for (int i = 0; i < _m_choiceBtnList.Count; i++)
                SCCommon.DestoryGameObject(_m_choiceBtnList[i]);
            _m_choiceBtnList.Clear();
        }

        private void onChapterEnd()
        {
            StoryModel.instance.MarkChapterCleared(_m_chapterId);

            ChapterRefObj nextChapter = null;
            foreach (ChapterRefObj chapter in StoryRefDataHelper.GetAllChaptersSorted())
            {
                if (chapter.id <= _m_chapterId)
                    continue;
                if (StoryRefDataHelper.IsChapterUnlocked(chapter.id, StoryModel.instance.clearedChapterIds))
                {
                    nextChapter = chapter;
                    break;
                }
            }

            if (nextChapter == null)
            {
                if (mono.txtContent != null)
                    mono.txtContent.text = "（剧情演示结束）";
                if (mono.txtName != null)
                    mono.txtName.text = string.Empty;
                return;
            }

            _m_chapterId = nextChapter.id;
            StoryModel.instance.StartChapter(_m_chapterId);
            _m_currentNode = StoryRefDataHelper.GetChapterStartNode(_m_chapterId);
            refreshShow();
        }
    }
}
