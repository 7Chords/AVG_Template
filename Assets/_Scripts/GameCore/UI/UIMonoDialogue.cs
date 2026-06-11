using SCFrame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI
{
    public class UIMonoDialogue : _ASCUIMonoBase
    {
        [Header("名字文本")]
        public Text txtName;
        [Header("内容文本")]
        public Text txtContent;
        [Header("存档按钮")]
        public Button btnSave;
        [Header("读档按钮")]
        public Button btnLoad;
        [Header("设置按钮")]
        public Button btnSetting;
        [Header("自动播放按钮")]
        public Button btnAuto;
        [Header("隐藏UI按钮")]
        public Button btnHide;
        [Header("跳过按钮")]
        public Button btnSkip;
        [Header("历史按钮")]
        public Button btnHistory;
    }

}