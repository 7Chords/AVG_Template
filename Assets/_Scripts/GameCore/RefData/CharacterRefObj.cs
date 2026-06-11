using SCFrame;

namespace GameCore.RefData
{
    /// <summary>
    /// 角色配表：名称、颜色与立绘资源。
    /// </summary>
    public class CharacterRefObj : SCRefDataCore
    {
        public int id;
        public string name;
        public string nameColor;
        public string spriteAsset;

        protected override void _parseFromString()
        {
            id = getInt("id");
            name = getString("name");
            nameColor = getString("nameColor");
            spriteAsset = getString("spriteAsset");
        }

        public static string assetPath => "RefData/ExportTxt";
        public static string sheetName => "character";
    }
}
