using GameCore;
using SCFrame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI
{
    public class UIMonoSave : _ASCUIMonoBase
    {
        [Header("标题")]
        public Text txtTitle;
        [Header("关闭按钮")]
        public Button btnClose;
        [Header("存档槽容器")]
        public UIMonoCommonContainer monoSaveContainer;
    }
}
