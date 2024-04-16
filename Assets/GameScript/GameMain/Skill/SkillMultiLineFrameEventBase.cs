using Wcng.SkillEditor;

namespace GameScript.GameMain.Skill
{
    public abstract class SkillMultiLineFrameEventBase : SkillFrameEventBase
    {
        public abstract string GetTrackName();

        public abstract void SetTrackName(string value);

        public abstract int GetFrameIndex();

        public abstract void SetFrameIndex(int value);
    }
}