namespace GameCore
{
    /// <summary>
    /// 剧情节点类型
    /// </summary>
    public enum EStoryNodeType
    {
        /// <summary>普通对白（name 为说话人）</summary>
        STANDARD,
        /// <summary>选项（name 为选项文本）</summary>
        SELECT,
    }

    /// <summary>
    /// 剧情节点标记
    /// </summary>
    public enum EStoryNodeFlagType
    {
        NONE,
        /// <summary>章节/段落起点</summary>
        BEGIN,
        /// <summary>章节/段落终点</summary>
        END,
    }

    /// <summary>存档面板模式</summary>
    public enum ESavePanelMode
    {
        SAVE,
        LOAD,
    }
}
