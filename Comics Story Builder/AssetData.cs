using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comics_Story_Builder
{
    [Serializable]
    public class AssetData
    {
        public string AssetType { get; set; } // "Image" ή "Background"
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ImagePath { get; set; }
        public string BubbleText { get; set; }

        public string TextContent { get; set; }

        public string TextRtf { get; set; } //για attributs των bubbles

        // ΝΕΕΣ ΜΕΤΑΒΛΗΤΕΣ ΓΙΑ ΤΟ TEXTBOX
        public int TxtX { get; set; }
        public int TxtY { get; set; }
        public int TxtWidth { get; set; }
        public int TxtHeight { get; set; }
    }
}
