using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfKit
{
    public class PdfDocumentOptions
    {
        public bool AutoFirstPage { get; set; }
    }

    public partial class PdfDocument
    {
        readonly ICollection<PdfPage> _pageBuffer;
        int _pageBufferStart;
        double _version;
        readonly ICollection<object> _offsets;
        int _waiting;
        bool _ended;
        int _offset;

        PdfPage _currentPage;
        readonly RootRef _rootRef;
        double _x, _y;

        Stream _stream;

        public PdfDocument(PdfDocumentOptions options)
        {
            // PDF version
            _version = 1.3;

            _pageBuffer = new List<PdfPage>();
            _pageBufferStart = 0;

            _offsets = new List<object>();
            _waiting = 0;
            _ended = false;
            _offset = 0;

            _currentPage = null;

            _rootRef = new RootRef();

            Ref(_rootRef);
            Ref(_rootRef.Pages);

            // InitColour();
            InitVector();
            // InitFonts();
            // InitText();
            // InitImages();

            _stream = new MemoryStream();

            Write($"%PDF-{_version}");
            Write(new byte[] { (byte)'%', 0xff, 0xff, 0xff, 0xff });

            if (options.AutoFirstPage)
            {
                AddPage(null);
            }
        }

        void Write(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);

            Write(bytes);
        }

        void Write(byte[] bytes)
        {
            _stream.Write(bytes, 0, bytes.Length);
        }

        public PdfReference Ref(object data)
        {
            var reference = new PdfReference(this, _offsets.Count + 1, data);

            _offsets.Add(null);
            _waiting++;

            return reference;
        }

        public bool RefEnd(PdfReference reference)
        {
            // _offsets[reference.Id - 1] = reference.Offset;

            return false;
        }

        PdfDocument AddPage(PdfPageOptions options)
        {
            /*
            addPage(options) {
      // end the current page if needed
      if (options == null) { ({ options } = this); }
      if (!this.options.bufferPages) { this.flushPages(); }
    }
     */
            // create a page object
            _currentPage = new PdfPage(this, options);
            _pageBuffer.Add(_currentPage);

            // add the page to the object store
            _rootRef.Pages.Kids.Add(_currentPage.Dictionary);
            _rootRef.Pages.Count++;

            // reset x and y coordinates
            _x = _currentPage.Margins.Left;
            _y = _currentPage.Margins.Top;

            // flip PDF coordinate system so that the origin is in
            // the top left rather than the bottom left
            _ctm = new double[] { 1, 0, 0, 1, 0, 0 };
            Transform(1, 0, 0, -1, 0, _currentPage.Height);

            return this;
        }

        void AddContent(string content)
        {

        }

        class RootRef
        {
            public string Type => "Catalog";
            public PagesRef Pages { get; set; } = new PagesRef();

        }

        class PagesRef
        {
            public string Type => "Pages";
            public int Count { get; set; } = 0;
            public ICollection<object> Kids = new List<object>();
        }
    }
}
