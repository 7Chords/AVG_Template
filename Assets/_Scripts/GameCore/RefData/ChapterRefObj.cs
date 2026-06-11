using SCFrame;

namespace GameCore.RefData
{
    /// <summary>
    /// 章节配表：定义章节元数据与起始节点。
    /// sortOrder / unlockChapterId 用于章节选单；剧情流转由 story_node 的 nextChapterId 决定。
    /// </summary>
    public class ChapterRefObj : SCRefDataCore
    {
        public int id;
        public int sortOrder;
        public string chapterName;
        public string chapterTitle;
        public string description;
        public long startNodeId;
        /// <summary>解锁所需通关章节 id，0 表示默认解锁</summary>
        public int unlockChapterId;
        /// <summary>本章剧情节点表名（对应 ExportTxt 下 {nodeSheetName}.txt），留空则默认 story_node_{id}</summary>
        public string nodeSheetName;

        protected override void _parseFromString()
        {
            id = getInt("id");
            sortOrder = getInt("sortOrder");
            chapterName = getString("chapterName");
            chapterTitle = getString("chapterTitle");
            description = getString("description");
            startNodeId = getLong("startNodeId");
            unlockChapterId = getInt("unlockChapterId");
            nodeSheetName = getString("nodeSheetName");
        }

        /// <summary>获取本章剧情节点表名。</summary>
        public string GetNodeSheetName()
        {
            if (!string.IsNullOrEmpty(nodeSheetName))
                return nodeSheetName;
            return $"story_node_{id}";
        }

        public static string assetPath => "RefData/ExportTxt";
        public static string sheetName => "chapter";
    }
}
