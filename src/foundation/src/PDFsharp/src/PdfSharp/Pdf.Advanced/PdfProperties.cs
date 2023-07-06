
namespace PdfSharp.Pdf.Advanced
{
    /// <summary>
    /// A dictionary that maps resource names to property list
    /// dictionaries for marked content.
    /// PdfRef1.7 P.154
    /// </summary>
    public class PdfProperties : PdfResourceMap, IEnumerable<KeyValuePair<string, PdfItem?>>
    {
        public PdfProperties(PdfDocument document)
            : base(document)
        { }

        protected PdfProperties(PdfDictionary dict)
            : base(dict)
        { }

        public int Count => Elements.Count;

        public new IEnumerator<KeyValuePair<string, PdfItem?>> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }
    }
}
