using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace WinFormsTest.Core.Attributes
{
    public class FrameElementAttribute : Attribute
    {
        public string FrameName { get; private set; }

        public Color HeaderColor { get; private set; }

        public FrameElementAttribute(byte R, byte G, byte B, string FrameName) 
        {
            this.FrameName = FrameName;
            HeaderColor = Color.FromArgb(R, G, B);

            if (!Booff.Contains(HeaderColor))
                Booff.Add(HeaderColor);
        }

        public FrameElementAttribute(string FrameName)
        {
            this.FrameName = FrameName;
            HeaderColor = GetRandomColor();
        }

        //TODO thread save
        private static Color GetRandomColor() 
        {
            var ctype = typeof(Color);
            var colors = ctype.GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.PropertyType == ctype)
                .Select(x => (Color)x.GetValue(null))
                .ToList();

            colors = colors.Except(Booff).ToList();

            Color res;

            if (colors.Count > 0)
                res = colors[Rnd.Next(colors.Count)];
            else
                res = Color.FromArgb(Rnd.Next(256), Rnd.Next(256), Rnd.Next(256));

            if (!Booff.Contains(res))
                Booff.Add(res);

            return res;
        }

        private static Random Rnd = new Random();
        private static HashSet<Color> Booff;

        static FrameElementAttribute() 
        {
            Booff = new HashSet<Color>() 
            {
                Color.Transparent,
                Color.AliceBlue,
                Color.Azure,
                Color.Cornsilk,
                Color.FloralWhite,
                Color.GhostWhite,
                Color.Gainsboro,
                Color.Honeydew,
                Color.Ivory,
                Color.Lavender,
                Color.LavenderBlush,
                Color.LightCyan,
                Color.Linen,
                Color.MintCream,
                Color.OldLace,
                Color.SeaShell,
                Color.Snow,
                Color.White,
                Color.WhiteSmoke,
                Color.LawnGreen,
                Color.Lime,
                Color.Bisque,
                Color.Wheat,
                Color.PaleGoldenrod,
                Color.LightYellow,
                Color.PapayaWhip,
                Color.AntiqueWhite,
                Color.Beige,
                Color.BlanchedAlmond,
                Color.Khaki,
                Color.LemonChiffon,
                Color.LightBlue,
                Color.LightGoldenrodYellow,
                Color.LightGray,
                Color.LightGreen,
                Color.LightPink,
                Color.LightSteelBlue,
                Color.MistyRose,
                Color.Moccasin,
                Color.NavajoWhite,
                Color.PaleTurquoise,
                Color.PeachPuff,
                Color.PowderBlue,
                Color.Silver,
                Color.Thistle,
            };
        }
    }
}
