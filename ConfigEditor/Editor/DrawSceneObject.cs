using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ConfigEditor
{
    public abstract class DrawSceneObject<T> : DrawMenuModule where T : MonoBehaviour
    {
        [Title("Scene Object")]
        [ShowIf("@myObject != null")]
        [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
        public T myObject;

        public override void Initialize()
        {
            if (myObject == null) { myObject = Object.FindObjectOfType<T>(); }
        }

        public override void SetSelected(object item)
        {
            //ensure selection is of the correct type
            var attempt = item as T;
            if (attempt != null) { myObject = attempt; }
        }

        [ShowIf("@myObject != null")]
        [GUIColor(0.7f, 1f, 0.7f)]
        [ButtonGroup("Top Button", -1000)]
        private void SelectSceneObject()
        {
            if (myObject != null) { Selection.activeGameObject = myObject.gameObject; }
        }

        [ShowIf("@myObject == null")]
        [Button]
        private void CreateManagerObject()
        {
            var newManager = new GameObject(typeof(T).Name);
            myObject = newManager.AddComponent<T>();
        }

        public override bool IsSceneObject => true;

        public override string AssetsPath => string.Empty;

        public override Type Type => typeof(T);
    }
}
