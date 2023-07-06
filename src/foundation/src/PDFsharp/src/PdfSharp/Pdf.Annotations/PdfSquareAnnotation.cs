namespace PdfSharp.Pdf.Annotations
{
    /// <summary>
    /// Represents a square annotation.
    /// </summary>
    public class PdfSquareAnnotation : PdfAnnotation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdfSquareAnnotation"/> class.
        /// </summary>
        public PdfSquareAnnotation()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfSquareAnnotation"/> class.
        /// </summary>
        /// <param name="rect"></param>
        public PdfSquareAnnotation(PdfRectangle rect)
        {
            Initialize();
            Rect = rect;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfSquareAnnotation"/> class.
        /// </summary>
        /// <param name="dict"></param>
        public PdfSquareAnnotation(PdfDictionary dict) : base(dict)
        { }

        void Initialize()
        {
            Elements.SetName(PdfAnnotation.Keys.Subtype, "/Square");
            Elements.SetDateTime(Keys.M, DateTime.Now);
        }

        /// <summary>
        /// Gets or sets the rectangle of this annotation.
        /// </summary>
        public PdfRectangle Rect
        {
            get => Elements.GetRectangle(Keys.Rect);
            set
            {
                Elements.SetRectangle(Keys.Rect, value);
                Elements.SetDateTime(Keys.M, DateTime.Now);
            }
        }

        /// <summary>
        /// Gets or sets border of this annotation.
        /// </summary>
        public PdfArray Border
        {
            get => Elements.GetArray(Keys.Border);
            set
            {
                Elements.SetArray(Keys.Border, value);
                Elements.SetDateTime(Keys.M, DateTime.Now);
            }
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
        internal new class Keys : PdfAnnotation.Keys
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
