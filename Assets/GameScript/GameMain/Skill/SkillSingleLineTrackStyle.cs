using UnityEditor;
using UnityEngine.UIElements;

namespace GameScript.GameMain.Skill
{
    public class SkillSingleLineTrackStyle : SkillTrackStyleBase
    {   //TODO::
        private const string MenuAssetpath = "Assets/GameScript/GameEditor/SkillEditor/SingleLineTrackMenu.uxml";
        private const string TrackAssetpath = "Assets/GameScript/GameEditor/SkillEditor/SingleLineContent.uxml";

        /// <summary>
        /// 初始化一条轨道
        /// </summary>
        /// <param name="menueparent"></param>
        /// <param name="contentParent"></param>
        /// <param name="title"></param>
        public void Init(VisualElement menueparent, VisualElement contentParent, string title)
        {
            this.menuParent = menueparent;
            this.contentParent = contentParent;

            this.menuRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(MenuAssetpath).Instantiate().Query().ToList()[1];
            this.contentRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TrackAssetpath).Instantiate().Query().ToList()[1];

            titlelabel = (Label)menuRoot;
            titlelabel.text = title;

            menuParent.Add(menuRoot);
            contentParent.Add(contentRoot);
        }
    }
}