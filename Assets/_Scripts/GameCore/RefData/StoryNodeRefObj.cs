using SCFrame;
using System.Collections.Generic;
using GameCore;

namespace GameCore.RefData
{
    /// <summary>
    /// 剧情节点配表：每章独立一张表。
    /// nextList 为跳转节点 id；nextChapterId 为目标章节（0 表示当前章）。
    /// </summary>
    public class StoryNodeRefObj : SCRefDataCore
    {
        public long id;
        /// <summary>说话人名或选项文本</summary>
        public string name;
        public string content;
        public EStoryNodeType nodeType;
        public EStoryNodeFlagType flagType;
        public List<long> nextList;
        /// <summary>跳转目标章节 id，0 表示 nextList 节点位于当前章</summary>
        public int nextChapterId;
        public string bgAsset;
        public string characterAsset;

        protected override void _parseFromString()
        {
            id = getLong("id");
            name = getString("name");
            content = getString("content");
            nodeType = (EStoryNodeType)getEnum("nodeType", typeof(EStoryNodeType));
            flagType = (EStoryNodeFlagType)getEnum("flagType", typeof(EStoryNodeFlagType));
            nextList = getList<long>("nextList");
            nextChapterId = getInt("nextChapterId");
            bgAsset = getString("bgAsset");
            characterAsset = getString("characterAsset");
        }

        public static string assetPath => "RefData/ExportTxt";
    }
}
