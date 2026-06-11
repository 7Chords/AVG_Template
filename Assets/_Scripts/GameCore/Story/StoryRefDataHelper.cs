using GameCore.RefData;
using GameCore;
using System.Collections.Generic;
using System.Linq;
using SCFrame;

namespace GameCore.Story
{
    /// <summary>
    /// 剧情配表查询辅助。
    /// </summary>
    public static class StoryRefDataHelper
    {
        public static ChapterRefObj GetChapter(int _chapterId)
        {
            return SCRefDataMgr.instance.chapterRefList.refDataList.Find(x => x.id == _chapterId);
        }

        public static List<ChapterRefObj> GetAllChaptersSorted()
        {
            return SCRefDataMgr.instance.chapterRefList.refDataList
                .OrderBy(x => x.sortOrder)
                .ThenBy(x => x.id)
                .ToList();
        }

        public static StoryNodeRefObj GetNode(int _chapterId, long _nodeId)
        {
            SCRefDataList<StoryNodeRefObj> list = SCRefDataMgr.instance.GetStoryNodeList(_chapterId);
            if (list == null)
                return null;
            return list.refDataList.Find(x => x.id == _nodeId);
        }

        public static List<StoryNodeRefObj> GetNodesByChapter(int _chapterId)
        {
            SCRefDataList<StoryNodeRefObj> list = SCRefDataMgr.instance.GetStoryNodeList(_chapterId);
            return list?.refDataList ?? new List<StoryNodeRefObj>();
        }

        public static StoryNodeRefObj GetChapterStartNode(int _chapterId)
        {
            ChapterRefObj chapter = GetChapter(_chapterId);
            if (chapter == null)
                return null;
            return GetNode(_chapterId, chapter.startNodeId);
        }

        public static CharacterRefObj GetCharacterByName(string _name)
        {
            if (string.IsNullOrEmpty(_name))
                return null;
            return SCRefDataMgr.instance.characterRefList.refDataList.Find(x => x.name == _name);
        }

        public static bool IsChapterUnlocked(int _chapterId, HashSet<int> _clearedChapterIds)
        {
            ChapterRefObj chapter = GetChapter(_chapterId);
            if (chapter == null)
                return false;
            if (chapter.unlockChapterId <= 0)
                return true;
            return _clearedChapterIds != null && _clearedChapterIds.Contains(chapter.unlockChapterId);
        }

        /// <summary>
        /// 解析节点的跳转目标。nextChapterId=0 表示留在当前章；nextList 为空或 0 时取目标章 startNodeId。
        /// </summary>
        public static bool TryGetJumpTarget(StoryNodeRefObj _node, int _currentChapterId, out int _chapterId, out long _nodeId)
        {
            _chapterId = 0;
            _nodeId = 0;
            if (_node == null)
                return false;

            bool hasChapter = _node.nextChapterId > 0;
            bool hasNode = _node.nextList != null && _node.nextList.Count > 0 && _node.nextList[0] > 0;

            if (!hasChapter && !hasNode)
                return false;

            _chapterId = hasChapter ? _node.nextChapterId : _currentChapterId;

            if (hasNode)
            {
                _nodeId = _node.nextList[0];
                return GetNode(_chapterId, _nodeId) != null;
            }

            StoryNodeRefObj startNode = GetChapterStartNode(_chapterId);
            if (startNode == null)
                return false;

            _nodeId = startNode.id;
            return true;
        }

        /// <summary>无有效跳转目标时视为剧情停止（全剧终或该线结束）。</summary>
        public static bool IsStoryStopNode(StoryNodeRefObj _node, int _currentChapterId)
        {
            return !TryGetJumpTarget(_node, _currentChapterId, out _, out _);
        }
    }
}
