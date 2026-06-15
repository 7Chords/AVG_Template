using SCFrame;

namespace GameCore.RefData
{
    /// <summary>
    /// 角色配表：名称与名字颜色。立绘/背景由 story_node 的 characterAsset、bgAsset 配置。
    /// </summary>
    public class CharacterRefObj : SCRefDataCore
    {
        public int id;
        public string name;
        public string nameColor;

        protected override void _parseFromString()
        {
            id = getInt("id");
            name = getString("name");
            nameColor = getString("nameColor");
        }

        public static string assetPath => "RefData/ExportTxt";
        public static string sheetName => "character";
    }
}
