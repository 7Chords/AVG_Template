using System;

namespace GameCore.Story
{
    [Serializable]
    public class StorySaveSlotData
    {
        public const int CurrentSaveVersion = 1;

        public int saveVersion;
        public int chapterId;
        public long nodeId;
        public int[] clearedChapterIds;
        public string chapterName;
        public string saveTimeText;
        public long saveTimeTicks;

        public bool HasData()
        {
            return chapterId > 0 && nodeId > 0;
        }
    }
}
