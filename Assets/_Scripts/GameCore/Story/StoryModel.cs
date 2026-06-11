using SCFrame;
using System.Collections.Generic;

namespace GameCore.Story
{
    /// <summary>
    /// 剧情运行时状态：当前章节、节点与通关记录。
    /// </summary>
    public class StoryModel : Singleton<StoryModel>
    {
        public int currentChapterId { get; private set; }
        public long currentNodeId { get; private set; }
        public HashSet<int> clearedChapterIds { get; private set; } = new HashSet<int>();

        public override void OnInitialize()
        {
            clearedChapterIds = new HashSet<int>();
        }

        public void StartChapter(int _chapterId)
        {
            currentChapterId = _chapterId;
            var startNode = StoryRefDataHelper.GetChapterStartNode(_chapterId);
            currentNodeId = startNode != null ? startNode.id : 0;
        }

        public void SetCurrentNode(long _nodeId)
        {
            currentNodeId = _nodeId;
        }

        public void MarkChapterCleared(int _chapterId)
        {
            clearedChapterIds.Add(_chapterId);
        }
    }
}
