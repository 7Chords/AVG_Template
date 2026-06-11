using SCFrame.UI;
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
        [Header("选项容器")]
        public UIMonoCommonContainer monoSelectContainer;
        [Header("点击继续区域")]
        public Image imgClickArea;
        [Header("对话打字机间隔（秒）")]
        public float dialogueTypewriterInterval = 0.04f;
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

        public void TryResolveRefs()
        {
            if (txtName == null)
            {
                Transform nameTf = transform.Find("img_dialogue_bg/txt_name");
                if (nameTf != null)
                    txtName = nameTf.GetComponent<Text>();
            }
            if (txtContent == null)
            {
                Transform contentTf = transform.Find("img_dialogue_bg/txt_content");
                if (contentTf != null)
                    txtContent = contentTf.GetComponent<Text>();
            }
            if (imgClickArea == null)
                imgClickArea = GetComponent<Image>();
        }
    }
}
