namespace PdfKit
{
    public class PdfReference
    {
        readonly PdfDocument _document;
        readonly int _id;
        readonly object _data;

        public int Id => _id;
        public object Data => _data;

        public PdfReference(PdfDocument document, int id, object data)
        {
            _document = document;
            _id = id;
            _data = data;
        }
    }
}
