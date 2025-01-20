namespace PdfSharp.Pdf.Annotations
{
    public class PdfFreeTextAnnotation : PdfMarkupAnnotation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdfFreeTextAnnotation"/> class.
        /// </summary>
        public PdfFreeTextAnnotation()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfFreeTextAnnotation"/> class.
        /// </summary>
        /// <param name="rect"></param>
        public PdfFreeTextAnnotation(PdfRectangle rect)
        {
            Initialize();
            Rectangle = rect;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfFreeTextAnnotation"/> class.
        /// </summary>
        /// <param name="dict"></param>
        public PdfFreeTextAnnotation(PdfDictionary dict) : base(dict)
        { }

        void Initialize()
        {
            Elements.SetName(PdfAnnotation.Keys.Subtype, "/FreeText");
            Elements.SetName(Keys.DA, "");
            Elements.SetDateTime(Keys.M, DateTime.Now);
        }

        /// <summary>
        /// Gets or sets CallOut of the annotation.
        /// Six numbers [ x1 y1 x2 y2 x3 y3 ] represent the starting, 
        /// knee point, and ending coordinates of the line in default user space,
        /// Four numbers [ x1 y1 x2 y2 ] represent the starting and 
        /// ending coordinates of the line.
        /// </summary>
        public PdfArray? CallOut
        {
            get => Elements.GetArray(Keys.CL);
            set => Elements.SetArray(Keys.CL, value);
        }

        /// <summary>
        /// Predefined keys of this dictionary.
        /// </summary>
        internal new class Keys : PdfMarkupAnnotation.Keys
        {
            /// <summary>
            /// (Required) The default appearance string to be used in formatting the text (see 
            /// “Variable Text” on page 677). 
            /// Note: The annotation dictionary’s AP entry, if present, takes precedence over the DA
            /// entry; see Table 8.15 on page 606 and Section 8.4.4, “Appearance Streams.” 
            /// </summary>
            [KeyInfo(KeyType.String | KeyType.Required)]
            public const string DA = "/DA";

            /// <summary>
            ///  (Optional; PDF 1.4) A code specifying the form of quadding (justification) to be 
            /// used in displaying the annotation’s text: 
            /// 0 Left-justified 
            /// 1 Centered 
            /// 2 Right-justified
            /// Default value: 0 (left-justified). 
            /// </summary>
            [KeyInfo("1.4", KeyType.Integer | KeyType.Optional)]
            public const string Q = "/Q";

            /// <summary>
            /// (Optional; PDF 1.5) A rich text string (see “Rich Text Strings” on page 680) to be 
            /// used to generate the appearance of the annotation.
            /// </summary>
            [KeyInfo("1.5", KeyType.TextString | KeyType.Stream | KeyType.Optional)]
            public const string RC = "/RC";

            /// <summary>
            /// (Optional; PDF 1.5) A default style string, as described in “Rich Text Strings” on 
            /// page 680.
            /// </summary>
            [KeyInfo("1.5", KeyType.TextString | KeyType.Optional)]
            public const string DS = "/DS";

            /// <summary>
            ///  (Optional; PDF 1.6) An array of four or six numbers specifying a callout line at-
            /// tached to the free text annotation.Six numbers[x1 y1 x2 y2 x3 y3] represent
            /// the starting, knee point, and ending coordinates of the line in default user space,
            /// as shown in Figure 8.4. Four numbers [x1 y1 x2 y2] represent the starting and
            /// ending coordinates of the line.
            /// </summary>
            [KeyInfo("1.6", KeyType.Array | KeyType.Optional)]
            public const string CL = "/CL";

            /// <summary>
            /// (Optional; PDF 1.6) A name describing the intent of the free text annotation (see 
            /// also Table 8.21). Valid values are FreeTextCallout, which means that the annota-
            /// tion is intended to function as a callout, and FreeTextTypeWriter, which means
            /// that the annotation is intended to function as a click-to-type or typewriter ob-
            /// ject.
            /// </summary>
            [KeyInfo("1.6", KeyType.Name | KeyType.Optional)]
            public const string IT = "/IT";

            /// <summary>
            /// (Optional; PDF 1.6) A border effect dictionary (see Table 8.18) used in conjunc-
            /// tion with the border style dictionary specified by the BS entry.
            /// </summary>
            [KeyInfo("1.6", KeyType.Dictionary | KeyType.Optional)]
            public const string BE = "/BE";

            /// <summary>
            /// (Optional; PDF 1.6) A set of four numbers describing the numerical differences 
            /// between two rectangles: the Rect entry of the annotation and a rectangle con-
            /// tained within that rectangle.The inner rectangle is where the annotation’s text
            /// should be displayed.Any border styles and/or border effects specified by BS and
            /// BE entries, respectively, are applied to the border of the inner rectangle.
            /// The four numbers correspond to the differences in default user space between
            /// the left, top, right, and bottom coordinates of Rect and those of the inner rectan-
            /// gle, respectively.Each value must be greater than or equal to 0. The sum of the
            /// top and bottom differences must be less than the height of Rect, and the sum of
            /// the left and right differences must be less than the width of Rect.
            /// </summary>
            [KeyInfo("1.6", KeyType.Rectangle | KeyType.Optional)]
            public const string RD = "/RD";

            /// <summary>
            /// (Optional; PDF 1.6) A border style dictionary (see Table 8.17 on page 611) speci-
            /// fying the line width and dash pattern to be used in drawing the annotation’s bor-
            /// der.
            /// Note: The annotation dictionary’s AP entry, if present, takes precedence over the In-
            /// kList and BS entries; see Table 8.15 on page 606 and Section 8.4.4, “Appearance
            /// Streams.” 
            /// </summary>
            [KeyInfo("1.6", KeyType.Dictionary | KeyType.Optional)]
            public const string BS = "/BS";

            /// <summary>
            ///  (Optional; PDF 1.6) An array of two names specifying the line ending styles to be 
            /// used in drawing the annotation’s border.The first and second elements of the ar-
            /// ray specify the line ending styles for the endpoints defined, respectively, by the
            /// first and second pairs of coordinates, (x1, y1) and (x2, y2), in the L array.Table
            /// 8.27 shows the possible values. Default value: [ /None /None]. 
            /// </summary>
            [KeyInfo("1.6", KeyType.Array | KeyType.Optional)]
            public const string LE = "/LE";

            public static DictionaryMeta Meta => _meta ??= CreateMeta(typeof(Keys));

            static DictionaryMeta? _meta;
        }
    }
}
