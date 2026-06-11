using SCFrame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI
{
    public abstract class _AUIMonoContainerBase : _ASCUIMonoBase
    {
        [Header("Item模板资源obj名")]
        public string prefabItemObjName;
        [Header("布局组件")]
        public LayoutGroup layoutGroup;
    }
}
