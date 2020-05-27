using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ConfigEditor
{
    public abstract class ConfigEditorWindow<TMenuModuleState> : OdinMenuEditorWindow where TMenuModuleState : Enum
    {
        public string Title { get; set; } = "Config Editor Window";

        public string Subtitle { get; set; }

        [LabelText("Manager Views")]
        [LabelWidth(100)]
        [ShowInInspector]
        [EnumToggleButtons]
        [OnValueChanged("StateChanged")]
        protected TMenuModuleState CurrentState;

        protected int StateIndex;

        protected bool ShouldRebuild;

        protected abstract List<DrawMenuModule> Modules { get; }

        protected DrawMenuModule CurrentModule
        {
            get
            {
                var index = Convert.ToInt16(CurrentState);
                if (index < 0) { return null; }

                return index >= Modules.Count ? null : Modules[index];
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            if (CurrentModule != null && !CurrentModule.IsSceneObject)
            {
                tree.AddAllAssetsAtPath(CurrentModule.Label, CurrentModule.AssetsPath, CurrentModule.Type);
            }

            return tree;
        }

        protected override void Initialize()
        {
            foreach (var menuModule in Modules) { menuModule.Initialize(); }

            StateIndex = -1;
        }

        protected override void OnGUI()
        {
            if (ShouldRebuild && Event.current.type == EventType.Layout)
            {
                ForceMenuTreeRebuild();
                ShouldRebuild = false;
            }

            if (string.IsNullOrEmpty(Title)) { Title = "Config Editor Window"; }

            SirenixEditorGUI.Title(Title, Subtitle, TextAlignment.Center, true);
            EditorGUILayout.Space();

            if (StateIndex >= 0 && CurrentModule != null && !CurrentModule.IsSceneObject) { DrawEditor(StateIndex); }

            EditorGUILayout.Space();

            base.OnGUI();
        }

        protected override void DrawEditors()
        {
            if (CurrentModule != null)
            {
                if (CurrentModule.IsSceneObject) { DrawEditor(StateIndex); }
                else { CurrentModule.SetSelected(MenuTree.Selection.SelectedValue); }
            }

            DrawEditor(Convert.ToInt32(CurrentState));
        }

        protected override void DrawMenu()
        {
            if (CurrentModule != null && !CurrentModule.IsSceneObject) { base.DrawMenu(); }
        }

        protected override IEnumerable<object> GetTargets()
        {
            var targets = new List<object>();
            foreach (var menuModule in Modules) { targets.Add(menuModule); }

            targets.Add(base.GetTarget());

            StateIndex = targets.Count - 1;
            return targets;
        }

        protected void StateChanged()
        {
            ShouldRebuild = true;
        }
    }
}
