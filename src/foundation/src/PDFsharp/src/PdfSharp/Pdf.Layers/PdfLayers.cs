using PdfSharp.Pdf.Advanced;
using System.Collections;

namespace PdfSharp.Pdf.Layers
{
    public sealed class PdfLayers : PdfDictionary, IEnumerable<PdfLayer>
    {
        private IList<PdfLayer> _layers = new List<PdfLayer>();

        internal PdfLayers(PdfDocument document)
            : base(document)
        { }

        internal PdfLayers(PdfDictionary dict)
            : base(dict)
        {
            Initialize();
        }

        private void Initialize()
        {
            var layerDict = new Dictionary<PdfReference, PdfLayer>();
            foreach (PdfReference pdfRef in LayersArray)
            {
                if (pdfRef.Value is PdfLayer layer)
                {
                    _layers.Add(layer);
                    layerDict.Add(layer.ReferenceNotNull, layer);
                }
                else if (pdfRef.Value is PdfDictionary dict)
                {
                    var layer1 = new PdfLayer(dict);
                    _layers.Add(layer1);
                    layerDict.Add(dict.ReferenceNotNull, layer1);
                }
            }

            // Invisible layers
            foreach (PdfReference pdfRef in LayerOrder.LayerOffArray)
            {
                if (pdfRef.Value is PdfDictionary dict)
                {
                    if (layerDict.TryGetValue(dict.ReferenceNotNull, out var layer))
                    {
                        layer.On = false;
                    }
                }
            }

            // Locked layers
            foreach (PdfReference pdfRef in LayerOrder.LayerLockedArray)
            {
                if (pdfRef.Value is PdfDictionary dict)
                {
                    if (layerDict.TryGetValue(dict.ReferenceNotNull, out var layer))
                    {
                        layer.Locked = true;
                    }
                }
            }
        }

        internal PdfLayer Add(PdfLayer layer)
        {
            Insert(layer);
            return layer;
        }

        private PdfLayer Insert(PdfLayer layer)
        {
            if (layer == null)
                throw new ArgumentNullException(nameof(layer));

            if (layer.Owner == Owner)
                return layer;
            else
            {
                layer.Document = Owner;
                Owner.IrefTable.Add(layer);
                LayersArray.Elements.Add(layer.ReferenceNotNull);
                LayerOrder.LayerOrderArray.Elements.Add(layer.ReferenceNotNull);
                _layers.Add(layer);
            }

            return layer;
        }

        /// <summary>
        /// Gets the number of pages.
        /// </summary>
        public int Count => _layers.Count;

        public PdfLayer this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index), index, PSSR.LayerIndexOutOfRange);
                return _layers[index];
            }
        }

        /// <summary>
        /// Gets a PdfArray containing all layers of this document. The array must not be modified.
        /// </summary>
        internal PdfArray LayersArray
            => _layersArray ??= (PdfArray?)Elements.GetValue(Keys.OCGs, VCF.Create) ?? NRT.ThrowOnNull<PdfArray>();

        PdfArray? _layersArray;

        internal PdfLayerEntry LayerOrder
            => _layerOrder ??= (PdfLayerEntry?)Elements.GetValue(Keys.D, VCF.Create) ?? NRT.ThrowOnNull<PdfLayerEntry>();

        PdfLayerEntry? _layerOrder;

        internal override void PrepareForSave()
        {
            var off = LayerOrder.LayerOffArray;
            off.Elements.Clear();

            var locked = LayerOrder.LayerLockedArray;
            locked.Elements.Clear();

            foreach (var layer in _layers)
            {
                if (!layer.On)
                    off.Elements.Add(layer.ReferenceNotNull);
                if (layer.Locked)
                    locked.Elements.Add(layer.ReferenceNotNull);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        public new IEnumerator<PdfLayer> GetEnumerator()
        {
            return new LayersIterator(this);
        }

        class LayersIterator : IEnumerator<PdfLayer>
        {
            public LayersIterator(PdfLayers layers)
            {
                _layers = layers;
                _index = -1;
            }

            public PdfLayer/*PdfAnnotation*/ Current => _layers[_index];

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                return ++_index < _layers.Count;
            }

            public void Reset()
            {
                _index = -1;
            }

            public void Dispose()
            {
                //throw new NotImplementedException();
            }

            readonly PdfLayers _layers;
            int _index;
        }

        internal class Keys : KeysBase
        {
            /// <summary>
            /// 
            /// </summary>
            [KeyInfo(KeyType.Dictionary | KeyType.Required, typeof(PdfLayerEntry))]
            public const string D = "/D";
            /// <summary>
            /// 
            /// </summary>
            [KeyInfo(KeyType.Array | KeyType.Required)]
            public const string OCGs = "/OCGs";

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
