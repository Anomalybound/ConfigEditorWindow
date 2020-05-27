using System;

namespace ConfigEditor
{
    [Serializable]
    public abstract class DrawMenuModule
    {
        public abstract string Label { get; }

        public abstract bool IsSceneObject { get; }

        public abstract string AssetsPath { get; }

        public abstract Type Type { get; }

        public abstract void SetSelected(object selected);

        public virtual void Initialize() { }
    }
}
