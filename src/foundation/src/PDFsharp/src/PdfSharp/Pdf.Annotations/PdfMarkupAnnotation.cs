
namespace PdfSharp.Pdf.Annotations
{
    public abstract class PdfMarkupAnnotation : PdfAnnotation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdfMarkupAnnotation"/> class.
        /// </summary>
        protected PdfMarkupAnnotation()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfMarkupAnnotation"/> class.
        /// </summary>
        protected PdfMarkupAnnotation(PdfDocument document)
            : base(document)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfMarkupAnnotation"/> class.
        /// </summary>
        internal PdfMarkupAnnotation(PdfDictionary dict)
            : base(dict)
        { }

        void Initialize()
        {
            Elements.SetName(Keys.Type, "/Annot");
            Elements.SetString(Keys.NM, Guid.NewGuid().ToString("D"));
            Elements.SetDateTime(Keys.M, DateTime.Now);
        }
                
        /// <summary>
        /// Gets or sets text representing a short description of the subject being
        /// addressed by the annotation.
        /// </summary>
        public string Subject
        {
            get => Elements.GetString(Keys.Subj, true);
            set
            {
                Elements.SetString(Keys.Subj, value);
                Elements.SetDateTime(Keys.M, DateTime.Now);
            }
        }

        /// <summary>
        /// Gets or sets the text label to be displayed in the title bar of the annotation's
        /// pop-up window when open and active. By convention, this entry identifies
        /// the user who added the annotation.
        /// </summary>
        public string Title
        {
            get => Elements.GetString(Keys.T, true);
            set
            {
                Elements.SetString(Keys.T, value);
                Elements.SetDateTime(Keys.M, DateTime.Now);
            }
        }

        /// <summary>
        /// Gets or sets the constant opacity value to be used in painting the annotation.
        /// This value applies to all visible elements of the annotation in its closed state
        /// (including its background and border) but not to the popup window that appears when
        /// the annotation is opened.
        /// </summary>
        public double Opacity
        {
            get
            {
                if (!Elements.ContainsKey(Keys.CA))
                    return 1;
                return Elements.GetReal(Keys.CA, true);
            }
            set
            {
                if (value is < 0 or > 1)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Opacity must be a value in the range from 0 to 1.");
                Elements.SetReal(Keys.CA, value);
                Elements.SetDateTime(Keys.M, DateTime.Now);
            }
        }

        /// <summary>
        /// Predefined keys of this dictionary.
        /// </summary>
        internal new class Keys : PdfAnnotation.Keys
        {
            // ----- Excerpt of entries specific to markup annotations ----------------------------------

            /// <summary>
            /// (Optional; PDF 1.1) The text label to be displayed in the title bar of the annotation's
            /// pop-up window when open and active. By convention, this entry identifies
            /// the user who added the annotation.
            /// </summary>
            [KeyInfo("1.1", KeyType.TextString | KeyType.Optional)]
            public const string T = "/T";

            /// <summary>
            /// (Optional; PDF 1.3) An indirect reference to a pop-up annotation for entering or
            /// editing the text associated with this annotation.
            /// </summary>
            [KeyInfo("1.3", KeyType.Dictionary | KeyType.Optional)]
            public const string Popup = "/Popup";

            /// <summary>
            /// (Optional; PDF 1.4) The constant opacity value to be used in painting the annotation.
            /// This value applies to all visible elements of the annotation in its closed state
            /// (including its background and border) but not to the popup window that appears when
            /// the annotation is opened.
            /// The specified value is not used if the annotation has an appearance stream; in that
            /// case, the appearance stream must specify any transparency. (However, if the viewer
            /// regenerates the annotation's appearance stream, it may incorporate the CA value
            /// into the stream's content.)
            /// The implicit blend mode is Normal.
            /// Default value: 1.0.
            /// </summary>
            [KeyInfo("1.4", KeyType.Real | KeyType.Optional)]
            public const string CA = "/CA";

            /// <summary>
            /// (Optional; PDF 1.5) A rich text string (see “Rich Text Strings” on page 680) to be 
            /// displayed in the pop-up window when the annotation is opened.
            /// </summary>
            [KeyInfo("1.5", KeyType.TextString | KeyType.Stream | KeyType.Optional)]
            public const string RC = "/RC";

            /// <summary>
            /// The date and time (Section 3.8.3, “Dates”) when the annota-
            /// tion was created.
            /// </summary>
            [KeyInfo("1.5", KeyType.Date | KeyType.Optional)]
            public const string CreationDate = "/CreationDate";

            /// <summary>
            /// (Required if an RT entry is present, otherwise optional; PDF 1.5) A reference to the 
            /// annotation that this annotation is “in reply to.” Both annotations must be on the
            /// same page of the document.The relationship between the two annotations is 
            /// specified by the RT entry.
            /// </summary>
            [KeyInfo("1.5", KeyType.Dictionary | KeyType.Optional)]
            public const string IRT = "/IRT";

            /// <summary>
            /// (Optional; PDF 1.5) Text representing a short description of the subject being
            /// addressed by the annotation.
            /// </summary>
            [KeyInfo("1.5", KeyType.TextString | KeyType.Optional)]
            public const string Subj = "/Subj";

            /// <summary>
            /// (Optional; meaningful only if IRT is present; PDF 1.6) A name specifying the rela-
            /// tionship (the “reply type”) between this annotation and one specified by IRT. Val-
            /// id values are:
            /// R The annotation is considered a reply to the annotation specified by 
            /// IRT.Viewer applications should not display replies to an annotation
            /// individually but together in the form of threaded comments.
            /// Group The annotation is grouped with the annotation specified by IRT; see
            /// discussion below.
            /// Default value: R.
            /// </summary>
            [KeyInfo("1.6", KeyType.Name | KeyType.Optional)]
            public const string RT = "/RT";

            /// <summary>
            /// (Optional; PDF 1.6) A name describing the intent of the markup annotation. In-
            /// tents allow viewer applications to distinguish between different uses and behav-
            /// iors of a single markup annotation type.If this entry is not present or its value is 
            /// the same as the annotation type, the annotation has no explicit intent and should
            /// behave in a generic manner in a viewer application.
            /// </summary>
            [KeyInfo("1.6", KeyType.Name | KeyType.Optional)]
            public const string IT = "/IT";

            /// <summary>
            /// (Optional; PDF 1.7) An external data dictionary specifying data to be associated 
            /// with the annotation.This dictionary contains the following entries:
            /// Type(optional) : If present, must be ExData.
            /// Subtype(required): a name specifying the type of data that the markup anno-
            /// tation is associated with.In PDF 1.7, the only defined value is 
            /// Markup3D.
            /// </summary>
            [KeyInfo("1.7", KeyType.Dictionary | KeyType.Optional)]
            public const string ExData = "/ExData";

            public static DictionaryMeta Meta => _meta ??= CreateMeta(typeof(Keys));

            static DictionaryMeta? _meta;
        }
    }
}
