using SCFrame;
using System.Collections.Generic;
using GameCore;

namespace GameCore.RefData
{
    /// <summary>
    /// 剧情节点配表：每章独立一张表，通过 nextList 串联本章剧情链。
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
            bgAsset = getString("bgAsset");
            characterAsset = getString("characterAsset");
        }

        public static string assetPath => "RefData/ExportTxt";
    }
}
