namespace PdfSharp.Pdf.Annotations
{
    public class PdfCaretAnnotation : PdfMarkupAnnotation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdfCaretAnnotation"/> class.
        /// </summary>
        public PdfCaretAnnotation()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfCaretAnnotation"/> class.
        /// </summary>
        /// <param name="rect"></param>
        public PdfCaretAnnotation(PdfRectangle rect)
        {
            Initialize();
            Rectangle = rect;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfCaretAnnotation"/> class.
        /// </summary>
        /// <param name="dict"></param>
        public PdfCaretAnnotation(PdfDictionary dict) : base(dict)
        { }

        void Initialize()
        {
            Elements.SetName(PdfAnnotation.Keys.Subtype, "/Caret");
            Elements.SetName(Keys.Sy, "/None");
            Elements.SetDateTime(Keys.M, DateTime.Now);
        }

        /// <summary>
        /// (Optional) A set of four numbers describing the numerical differences 
        /// between two rectangles: the Rect entry of the annotation and the actual boundaries
        /// of the underlying caret.Such a difference can occur, for example, when a
        /// paragraph symbol specified by Sy is displayed along with the caret.        
        /// </summary>
        public PdfRectangle RD
        {
            get => Elements.GetRectangle(Keys.RD, true);
            set
            {
                Elements.SetRectangle(Keys.RD, value);
                Elements.SetDateTime(Keys.M, DateTime.Now);
            }
        }

        /// <summary>
        /// Predefined keys of this dictionary.
        /// </summary>
        internal new class Keys : PdfMarkupAnnotation.Keys
        {
            /// <summary>
            /// (Optional) A set of four numbers describing the numerical differences 
            /// between two rectangles: the Rect entry of the annotation and the actual boundaries
            /// of the underlying caret.Such a difference can occur, for example, when a
            /// paragraph symbol specified by Sy is displayed along with the caret.
            /// </summary>
            [KeyInfo(KeyType.Rectangle | KeyType.Optional)]
            public const string RD = "/RD";

            /// <summary>
            /// (Optional) A name specifying a symbol to be associated with the caret.
            /// </summary>
            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string Sy = "/Sy";

            public static DictionaryMeta Meta => _meta ??= CreateMeta(typeof(Keys));

            static DictionaryMeta? _meta;
        }
    }
}
