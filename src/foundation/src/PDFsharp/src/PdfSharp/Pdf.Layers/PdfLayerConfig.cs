namespace PdfSharp.Pdf.Layers
{
    /// <summary>
    /// Represents the /D dictionary entry under OCProperties.
    /// </summary>
    internal class PdfLayerConfig : PdfDictionary
    {
        internal PdfLayerConfig(PdfDocument document)
            : base(document)
        { }

        internal PdfLayerConfig(PdfDictionary dict)
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
            /// (Optional) An array specifying the recommended order for presentation of
            /// optional content groups in a user interface.
            /// PdfRef1.7 P.377
            /// </summary>
            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string Order = "/Order";

            /// <summary>
            /// (Optional) An array of optional content groups whose state should be set to
            /// ON when this configuration is applied.
            /// PdfRef1.7 P.376
            /// </summary>
            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string OFF = "/OFF";

            /// <summary>
            /// (Optional; PDF 1.6) An array of optional content groups that should be
            /// locked when this configuration is applied.The state of a locked group cannot
            /// be changed through the user interface of a viewer application.Producers can
            /// use this entry to prevent the visibility of content that depends on these
            /// groups from being changed by users.
            /// PdfRef1.7 P.378
            /// </summary>
            [KeyInfo(KeyType.Array | KeyType.Optional)]
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
