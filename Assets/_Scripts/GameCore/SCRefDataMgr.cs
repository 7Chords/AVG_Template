using GameCore.RefData;
using SCFrame;

namespace GameCore
{
    /// <summary>
    /// 配表管理器：统一加载全部剧情相关表。
    /// </summary>
    public class SCRefDataMgr : Singleton<SCRefDataMgr>
    {
        public SCRefDataList<ChapterRefObj> chapterRefList =
            new SCRefDataList<ChapterRefObj>(ChapterRefObj.assetPath, ChapterRefObj.sheetName);

        public SCRefDataList<StoryNodeRefObj> storyNodeRefList =
            new SCRefDataList<StoryNodeRefObj>(StoryNodeRefObj.assetPath, StoryNodeRefObj.sheetName);

        public SCRefDataList<CharacterRefObj> characterRefList =
            new SCRefDataList<CharacterRefObj>(CharacterRefObj.assetPath, CharacterRefObj.sheetName);

        public SCRefDataList<TextLanguageRefObj> textLanguageRefList =
            new SCRefDataList<TextLanguageRefObj>(TextLanguageRefObj.assetPath, TextLanguageRefObj.sheetName);

        public override void OnInitialize()
        {
            chapterRefList.readFromTxt();
            storyNodeRefList.readFromTxt();
            characterRefList.readFromTxt();
            textLanguageRefList.readFromTxt();
        }
    }
}
