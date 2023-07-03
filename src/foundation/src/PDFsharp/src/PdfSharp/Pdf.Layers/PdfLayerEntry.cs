namespace PdfSharp.Pdf.Layers
{
    /// <summary>
    /// Represents the /D dictionary entry under OCProperties.
    /// </summary>
    internal class PdfLayerEntry : PdfDictionary
    {
        internal PdfLayerEntry(PdfDocument document)
            : base(document)
        { }

        internal PdfLayerEntry(PdfDictionary dict)
            : base(dict)
        { }

        internal PdfArray LayerOrderArray
        => _layerOrderArray ??= (PdfArray?)Elements.GetValue(Keys.Order, VCF.Create) ?? NRT.ThrowOnNull<PdfArray>();

        PdfArray? _layerOrderArray;

        internal PdfArray LayerOffArray
            => _layerOffArray ??= (PdfArray?)Elements.GetValue(Keys.OFF, VCF.Create) ?? NRT.ThrowOnNull<PdfArray>();

        PdfArray? _layerOffArray;

        internal PdfArray LayerLockedArray
            => _layerLockArray ??= (PdfArray?)Elements.GetValue(Keys.Locked, VCF.Create) ?? NRT.ThrowOnNull<PdfArray>();

        PdfArray? _layerLockArray;

        internal class Keys : KeysBase
        {
            /// <summary>
            /// Layers' order array.
            /// </summary>
            [KeyInfo(KeyType.Array | KeyType.Required)]
            public const string Order = "/Order";

            /// <summary>
            /// Invisible layers' array.
            /// </summary>
            [KeyInfo(KeyType.Array | KeyType.Required)]
            public const string OFF = "/OFF";

            /// <summary>
            /// Locked layers' array.
            /// </summary>
            [KeyInfo(KeyType.Array | KeyType.Required)]
            public const string Locked = "/Locked";

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
