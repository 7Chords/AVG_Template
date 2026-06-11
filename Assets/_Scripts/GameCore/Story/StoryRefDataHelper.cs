using GameCore.RefData;
using GameCore;
using System.Collections.Generic;
using System.Linq;
using SCFrame;

namespace GameCore.Story
{
    /// <summary>
    /// 剧情配表查询辅助：按章节获取节点、角色等。
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

        /// <summary>在指定章节表内查找节点（节点 id 仅在章内唯一即可）。</summary>
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

        public static bool IsChapterEndNode(StoryNodeRefObj _node)
        {
            if (_node == null)
                return false;
            return _node.flagType == EStoryNodeFlagType.END
                || (_node.nextList == null || _node.nextList.Count == 0 || _node.nextList[0] == 0);
        }
    }
}
