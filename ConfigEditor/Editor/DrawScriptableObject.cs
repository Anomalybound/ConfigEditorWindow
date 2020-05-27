using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ConfigEditor
{
    public abstract class DrawScriptableObject<T> : DrawMenuModule where T : ScriptableObject
    {
        [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
        public T selected;

        [LabelWidth(100)]
        [PropertyOrder(-1)]
        [GUIColor(1f, 1f, 1f)]
        [FoldoutGroup("CreateNew")]
        [HorizontalGroup("CreateNew/Horizontal")]
        public string nameForNew;

        public virtual string CreationPath { get; private set; }

        [HorizontalGroup("CreateNew/Horizontal")]
        [GUIColor(0.7f, 0.7f, 1f)]
        [Button]
        public void CreateNew()
        {
            if (string.IsNullOrEmpty(nameForNew)) { return; }

            var newItem = ScriptableObject.CreateInstance<T>();
            newItem.name = typeof(T).Name;

            if (string.IsNullOrEmpty(CreationPath)) { CreationPath = AssetsPath; }

            var assetPath = Path.Combine(CreationPath, nameForNew + ".asset");
            AssetDatabase.CreateAsset(newItem, assetPath);
            AssetDatabase.SaveAssets();

            nameForNew = "";
        }

        [HorizontalGroup("CreateNew/Horizontal")]
        [GUIColor(1f, 0.7f, 0.7f)]
        [Button]
        public void DeleteSelected()
        {
            if (selected != null)
            {
                var assetPath = AssetDatabase.GetAssetPath(selected);
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.SaveAssets();
            }
        }

        public override void SetSelected(object item)
        {
            //ensure selection is of the correct type
            var attempt = item as T;
            if (attempt != null) { selected = attempt; }
        }

        public override bool IsSceneObject => false;

        public override Type Type => typeof(T);
    }
}
