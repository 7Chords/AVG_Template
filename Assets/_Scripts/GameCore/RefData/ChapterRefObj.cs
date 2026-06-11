using SCFrame;

namespace GameCore.RefData
{
    /// <summary>
    /// 章节配表：定义章节元数据与起始节点。
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

        protected override void _parseFromString()
        {
            id = getInt("id");
            sortOrder = getInt("sortOrder");
            chapterName = getString("chapterName");
            chapterTitle = getString("chapterTitle");
            description = getString("description");
            startNodeId = getLong("startNodeId");
            unlockChapterId = getInt("unlockChapterId");
        }

        public static string assetPath => "RefData/ExportTxt";
        public static string sheetName => "chapter";
    }
}
