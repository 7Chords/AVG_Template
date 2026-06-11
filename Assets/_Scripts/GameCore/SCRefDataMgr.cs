using GameCore.RefData;
using SCFrame;
using System.Collections.Generic;

namespace GameCore
{
    /// <summary>
    /// 配表管理器：章节表全局加载，剧情节点按章懒加载。
    /// </summary>
    public class SCRefDataMgr : Singleton<SCRefDataMgr>
    {
        public SCRefDataList<ChapterRefObj> chapterRefList =
            new SCRefDataList<ChapterRefObj>(ChapterRefObj.assetPath, ChapterRefObj.sheetName);

        public SCRefDataList<CharacterRefObj> characterRefList =
            new SCRefDataList<CharacterRefObj>(CharacterRefObj.assetPath, CharacterRefObj.sheetName);

        public SCRefDataList<TextLanguageRefObj> textLanguageRefList =
            new SCRefDataList<TextLanguageRefObj>(TextLanguageRefObj.assetPath, TextLanguageRefObj.sheetName);

        private Dictionary<int, SCRefDataList<StoryNodeRefObj>> _m_storyNodeDict =
            new Dictionary<int, SCRefDataList<StoryNodeRefObj>>();

        public override void OnInitialize()
        {
            _m_storyNodeDict.Clear();
            chapterRefList.readFromTxt();
            characterRefList.readFromTxt();
            textLanguageRefList.readFromTxt();
            preloadAllStoryNodes();
        }

        /// <summary>获取指定章节的剧情节点表（已加载则直接返回缓存）。</summary>
        public SCRefDataList<StoryNodeRefObj> GetStoryNodeList(int _chapterId)
        {
            if (_m_storyNodeDict.TryGetValue(_chapterId, out SCRefDataList<StoryNodeRefObj> cachedList))
                return cachedList;

            ChapterRefObj chapter = chapterRefList.refDataList.Find(x => x.id == _chapterId);
            if (chapter == null)
            {
                UnityEngine.Debug.LogError($"章节不存在，无法加载剧情表：chapterId={_chapterId}");
                return null;
            }

            string sheetName = chapter.GetNodeSheetName();
            SCRefDataList<StoryNodeRefObj> list =
                new SCRefDataList<StoryNodeRefObj>(StoryNodeRefObj.assetPath, sheetName);
            list.readFromTxt();
            _m_storyNodeDict[_chapterId] = list;
            return list;
        }

        private void preloadAllStoryNodes()
        {
            foreach (ChapterRefObj chapter in chapterRefList.refDataList)
                GetStoryNodeList(chapter.id);
        }
    }
}
