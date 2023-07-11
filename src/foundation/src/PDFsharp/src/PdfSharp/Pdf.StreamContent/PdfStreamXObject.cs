namespace PdfSharp.Pdf.StreamContent
{
    public class PdfStreamXObject : PdfStreamItem
    {
        /// <summary>
        /// Content of XObject.
        /// </summary>
        public string? Content { get; set; }
        public override String ToString()
        {
            return Content == null ? string.Empty : Content;
        }
    }
}
