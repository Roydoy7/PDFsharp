namespace PdfSharp.Pdf.StreamContent
{
    public class PdfStreamGraphicState : PdfStreamItem
    {
        /// <summary>
        /// Content of graphic state.
        /// </summary>
        public string? Content { get; set; }
        public override String ToString()
        {
            return Content == null ? string.Empty : Content;
        }
    }
}
