using System;
using System.Windows.Media;

namespace LunchViewer.Utils
{
    public static class ColorUtils
    {
        public static Color Lighten(Color input)
        {
            var hsl = RgbToHsl(input);
            hsl.L /= 0.8;
            return HslToRgb(hsl);
        }

        struct Hsl
        {
            private double h;
            private double s;
            private double l;

            public Hsl(double h, double s, double l)
                : this()
            {
                this.H = h;
                this.S = s;
                this.L = l;
            }

            public double H
            {
                get { return this.h; }
                set { this.h = Math.Max(0, Math.Min(1, value)); }
            }
            public double S
            {
                get { return this.s; }
                set { this.s = Math.Max(0, Math.Min(1, value)); }
            }
            public double L
            {
                get { return this.l; }
                set { this.l = Math.Max(0, Math.Min(1, value)); }
            }
        }

        private static Hsl RgbToHsl(Color value)
        {
            var c = System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
            return new Hsl
            {
                H = c.GetHue() / 360,
                S = c.GetSaturation(),
                L = c.GetBrightness()
            };
        }

        private static Color HslToRgb(Hsl hsl)
        {
            double r = 0, g = 0, b = 0;

            if (hsl.L == 0)
            {
                r = g = b = 0;
            }
            else
            {
                if (hsl.S == 0)
                {
                    r = g = b = hsl.L;
                }
                else
                {
                    double temp2 = ((hsl.L <= 0.5) ? hsl.L * (1.0 + hsl.S) : hsl.L + hsl.S - (hsl.L * hsl.S));
                    double temp1 = 2.0 * hsl.L - temp2;

                    var t3 = new double[] { hsl.H + 1.0 / 3.0, hsl.H, hsl.H - 1.0 / 3.0 };
                    var clr = new double[] { 0, 0, 0 };
                    for (int i = 0; i < 3; i++)
                    {
                        if (t3[i] < 0)
                            t3[i] += 1.0;
                        if (t3[i] > 1)
                            t3[i] -= 1.0;

                        if (6.0 * t3[i] < 1.0)
                            clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
                        else if (2.0 * t3[i] < 1.0)
                            clr[i] = temp2;
                        else if (3.0 * t3[i] < 2.0)
                            clr[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - t3[i]) * 6.0);
                        else
                            clr[i] = temp1;
                    }
                    r = clr[0];
                    g = clr[1];
                    b = clr[2];
                }
            }

            return Color.FromArgb(0xff, (byte)(0xff * r), (byte)(0xff * g), (byte)(0xff * b));
        }
    }
}
