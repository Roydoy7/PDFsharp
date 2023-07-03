namespace PdfSharp.Pdf.Layers
{
    public sealed class PdfLayer : PdfDictionary
    {
        internal PdfLayer(PdfDictionary dict)
            : base(dict)
        {
        }

        internal PdfLayer(string name)
        {
            Elements.Add(Keys.Type, new PdfName("/OCG"));
            Elements.Add(Keys.Name, new PdfString(name));
        }

        /// <summary>
        /// Get layer's name.
        /// </summary>
        public string Name
        {
            get
            {
                var name = Elements.GetString(Keys.Name);
                if (string.IsNullOrEmpty(name))
                    NRT.ThrowOnNull("Layer name must not be null or empty here.");
                return name;
            }
        }
        /// <summary>
        /// Gets or sets layer's visibility. True is visible, false is hidden in the viewer.
        /// </summary>
        public bool On { get; set; } = true;

        /// <summary>
        /// Gets or sets layer's lock state.
        /// </summary>
        public bool Locked { get; set; }

        internal class Keys : KeysBase
        {
            /// <summary>
            /// 
            /// </summary>
            [KeyInfo(KeyType.String | KeyType.Required)]
            public const string Name = "/Name";
            /// <summary>
            /// 
            /// </summary>
            [KeyInfo(KeyType.String | KeyType.Required)]
            public const string Type = "/Type";

            /// <summary>
            /// Gets the KeysMeta for these keys.
            /// </summary>
            public static DictionaryMeta Meta => _meta ??= CreateMeta(typeof(Keys));

            static DictionaryMeta? _meta;
        }

        /// <summary>
        /// Gets the KeysMeta of this dictionary type.
        /// </summary>
        internal override DictionaryMeta Meta => Keys.Meta;
    }
}
