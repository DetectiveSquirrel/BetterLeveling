using System.Windows.Forms;
using PoeHUD.Hud.Settings;
using PoeHUD.Plugins;
using SharpDX;

namespace BetterLeveling.Core
{
    public class Settings : SettingsBase
    {
        [Menu("Hotkeys", 20)]
        public EmptyNode EmptyHotkey { get; set; }
        [Menu("Toggle Show", 21, 20)]
        public HotkeyNode ShowHotkey { get; set; } = Keys.LMenu;
        //[Menu("Show", 22, 20)]
        public ToggleNode ShowToggle { get; set; } = true;

        [Menu("Weighting", 10)]
        public EmptyNode EmptyWeight { get; set; }
        [Menu("Life", 11, 10)]
        public RangeNode<float> LifeWeight { get; set; } = new RangeNode<float>(1, 1, 100);
        [Menu("Fire Resistance", 12, 10)]
        public RangeNode<float> FireResWeight { get; set; } = new RangeNode<float>(2, 1, 100);
        [Menu("Cold Resistance", 13, 10)]
        public RangeNode<float> ColdResWeight { get; set; } = new RangeNode<float>(2, 1, 100);
        [Menu("Lightning Resistance", 14, 10)]
        public RangeNode<float> LightResWeight { get; set; } = new RangeNode<float>(2, 1, 100);
        [Menu("Physical DPS", 15, 10)]
        public RangeNode<float> PhysicalDPSWeight { get; set; } = new RangeNode<float>(1, 1, 100);
        [Menu("Elemental DPS", 16, 10)]
        public RangeNode<float> EleDPSWeight { get; set; } = new RangeNode<float>(1, 1, 100);

        [Menu("Graphics", 30)]
        public EmptyNode EmptyGraphics { get; set; }
        [Menu("Line Thickness", 31, 30)]
        public RangeNode<int> LineThickness { get; set; } = new RangeNode<int>(1, 1, 20);
        [Menu("Line Thickness", 32, 30)]
        public RangeNode<int> OutlineThickness { get; set; } = new RangeNode<int>(1, 1, 20);
        [Menu("Sum Size", 33, 30)]
        public RangeNode<int> SumSize { get; set; } = new RangeNode<int>(15, 1, 50);
        [Menu("Sum Color", 35, 30)]
        public ToggleNode SumShow { get; set; } = true;

        [Menu("Colors", 36, 30)]
        public EmptyNode EmptyColor { get; set; }
        [Menu("Sum Color", 360, 36)]
        public ColorNode SumColor { get; set; } = Color.White;
        [Menu("Connector", 361, 36)]
        public ColorNode ConnectorColor { get; set; } = new Color(0, 255, 38, 255);
        [Menu("Fill", 362, 36)]
        public ColorNode FillColor { get; set; } = new Color(0, 255, 38, 35);
        [Menu("Frame", 363, 36)]
        public ColorNode FrameColor { get; set; } = new Color(0, 255, 38, 255);
    }
}