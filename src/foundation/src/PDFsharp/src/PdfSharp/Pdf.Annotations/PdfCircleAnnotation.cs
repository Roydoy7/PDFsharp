namespace PdfSharp.Pdf.Annotations
{
    /// <summary>
    /// Represents a circle annotation.
    /// </summary>
    public class PdfCircleAnnotation : PdfMarkupAnnotation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdfCircleAnnotation"/> class.
        /// </summary>
        public PdfCircleAnnotation()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfCircleAnnotation"/> class.
        /// </summary>
        /// <param name="rect"></param>
        public PdfCircleAnnotation(PdfRectangle rect)
        {
            Initialize();
            Rectangle = rect;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfCircleAnnotation"/> class.
        /// </summary>
        /// <param name="dict"></param>
        public PdfCircleAnnotation(PdfDictionary dict) : base(dict)
        { }

        void Initialize()
        {
            Elements.SetName(PdfAnnotation.Keys.Subtype, "/Circle");
            Elements.SetDateTime(Keys.M, DateTime.Now);
        }

        /// <summary>
        /// Gets or sets the interior color of this annotation.
        /// </summary>
        public PdfArray InteriorColor
        {
            get => Elements.GetArray(Keys.IC);
            set
            {
                Elements.SetArray(Keys.IC, value);
                Elements.SetDateTime(Keys.M, DateTime.Now);
            }
        }

        /// <summary>
        /// Predefined keys of this dictionary.
        /// </summary>
        internal new class Keys : PdfMarkupAnnotation.Keys
        {
            /// <summary>
            /// (Optional) The annotation's interior color.
            /// </summary>
            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string IC = "/IC";

            public static DictionaryMeta Meta => _meta ??= CreateMeta(typeof(Keys));

            static DictionaryMeta? _meta;
        }
    }
}
