

using GameOverlay.Drawing;

namespace AntiMotionSickness
{
    internal struct LineConfig
    {
        public bool isVisible;
        public Color color;
        public float thickness;

        public bool hasBorder;
        public Color borderColor;
        public float borderThickness;

        public float size;
        public float distance;

        public override string ToString()
        {
            return $"{isVisible},{color},{thickness}/{hasBorder},{borderColor},{borderThickness}/{size},{distance}";
        }
    }

    internal struct LineConfigPreset
    {
        public LineConfig center;
        public LineConfig corner;
        public LineConfig cross;

        public override string ToString()
        {
            return $"center {center} / corner {corner} / cross {cross}";
        }
    }
}
