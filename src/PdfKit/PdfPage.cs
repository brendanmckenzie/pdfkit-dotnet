using System;

namespace PdfKit
{
    public class PdfPageOptions
    {

    }

    public class PdfPage
    {
        public object Dictionary => null;
        public int Height => 0;
        public PdfPageMargins Margins { get; set; }
        public PdfPage(PdfDocument document, PdfPageOptions options)
        {

        }
    }

    public class PdfPageMargins
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }
    }
}
