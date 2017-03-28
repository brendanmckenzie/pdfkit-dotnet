using System.Collections.Generic;
using System.Linq;

namespace PdfKit
{
    public partial class PdfDocument
    {
        double[] _ctm;
        ICollection<object> _ctmStack;

        void InitVector()
        {
            _ctm = new double[] { 1, 0, 0, 1, 0, 0 };
            _ctmStack = new List<object>();
        }

        void Transform(double m11, double m12, double m21, double m22, double dx, double dy)
        {
            var m = _ctm;
            var transformed = new double[] {
                (m[0] * m11) + (m[2] * m12),
                (m[1] * m11) + (m[3] * m12),
                (m[0] * m21) + (m[2] * m22),
                (m[1] * m21) + (m[3] * m22),
                (m[0] * dx) + (m[2] * dy) + m[4],
                (m[1] * dx) + (m[3] * dy) + m[5],
            };

            _ctm = transformed;

            var values = string.Join(" ", new[] { m11, m12, m21, m22, dx, dy }.Select(d => d.ToString("0.00000")));

            AddContent($"{values} cm");
        }
    }
}
