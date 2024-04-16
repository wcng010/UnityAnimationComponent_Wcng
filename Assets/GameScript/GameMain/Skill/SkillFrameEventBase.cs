namespace Wcng.SkillEditor
{
    public abstract class SkillFrameEventBase : IFrameEvent
    {
        public abstract string GetName();

        public abstract void SetName(string value);

        public abstract int GetFrameDuration(int frameRate);

        public abstract void SetFrameDuration(int value);

        public abstract object GetObject();

        public abstract void SetObject(object value);
    }
}