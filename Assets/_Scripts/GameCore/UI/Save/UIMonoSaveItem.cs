using SCFrame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI
{
    public class UIMonoSaveItem : _ASCUIMonoBase
    {
        [Header("槽位按钮")]
        public Button btnSlot;
        [Header("章节标题")]
        public Text txtChapter;
        [Header("存档时间")]
        public Text txtTime;
        [Header("空槽提示")]
        public Text txtEmpty;
        [Header("有档内容根节点")]
        public GameObject goHasData;
        [Header("空槽根节点")]
        public GameObject goEmpty;
        [Header("槽位序号（从1开始显示）")]
        public Text txtSlotIndex;
        [Header("鼠标移入缩放")]
        public float scaleMouseEnter = 1.05f;
        [Header("缩放动画时长")]
        public float scaleChgDuration = 0.12f;
    }
}
