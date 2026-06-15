using SCFrame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI
{
    public class UIMonoDialogue : _ASCUIMonoBase
    {
        [Header("???????")]
        public Text txtName;
        [Header("???????")]
        public Text txtContent;
        [Header("???????")]
        public UIMonoCommonContainer monoSelectContainer;
        [Header("???????????")]
        public Image imgClickArea;
        [Header("????")]
        public Image imgPortrait;
        [Header("????")]
        public Image imgScene;
        [Header("???????????????")]
        public float dialogueTypewriterInterval = 0.04f;
        [Header("?›Ï???")]
        public Button btnSave;
        [Header("???????")]
        public Button btnLoad;
        [Header("??????")]
        public Button btnSetting;
        [Header("?????????")]
        public Button btnAuto;
        [Header("????UI???")]
        public Button btnHide;
        [Header("???????")]
        public Button btnSkip;
        [Header("??????")]
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
            if (imgPortrait == null)
            {
                Transform portraitTf = transform.Find("img_portrait_center");
                if (portraitTf != null)
                    imgPortrait = portraitTf.GetComponent<Image>();
            }
            if (imgScene == null)
            {
                Transform sceneTf = transform.Find("img_scene");
                if (sceneTf != null)
                    imgScene = sceneTf.GetComponent<Image>();
            }
        }
    }
}
