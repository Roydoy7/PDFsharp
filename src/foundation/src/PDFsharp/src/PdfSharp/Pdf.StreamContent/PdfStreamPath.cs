namespace PdfSharp.Pdf.StreamContent
{
    public class PdfStreamPath : PdfStreamItem
    {
        /// <summary>
        /// Content of path.
        /// </summary>
        public string Content { get; set; }

        public override String ToString()
        {
            return Content;
        }
    }
}
