using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace overrated_client
{
    class ScreenshotParser
    {
        public string GetFpsFromImage(Bitmap bmp)
        {
            var src = bmp.Clone(new Rectangle(0, 0, 125, 25), bmp.PixelFormat);
            return ToString(src);
        }

        private string ToString(Bitmap bmp)
        {
            string res = "";
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = new BitmapToPixConverter().Convert(bmp))
                    {
                        using (var page = engine.Process(img))
                        {
                            res = page.GetText();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
            }
            return res.Trim();
        }
    }
}
