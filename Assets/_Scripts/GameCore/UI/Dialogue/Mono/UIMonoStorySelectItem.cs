using SCFrame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI
{
    public class UIMonoStorySelectItem : _ASCUIMonoBase
    {
        [Header("选项按钮")]
        public Button btnSelect;
        [Header("选项文本")]
        public Text txtContent;
        [Header("鼠标移入缩放")]
        public float scaleMouseEnter = 1.05f;
        [Header("缩放动画时长")]
        public float scaleChgDuration = 0.12f;
    }
}
