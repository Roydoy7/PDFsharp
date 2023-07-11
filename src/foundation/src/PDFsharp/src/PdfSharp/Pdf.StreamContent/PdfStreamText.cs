using System.Text;

namespace PdfSharp.Pdf.StreamContent
{
    public class PdfStreamText : PdfStreamItem
    {
        /// <summary>
        /// Content of text.
        /// </summary>
        public string? Content { get; set; }

        public Dictionary<string, string>? TextState { get; set; }

        public override String ToString()
        {
            if (TextState == null)
                return string.Empty;

            var sb = new StringBuilder();
            sb.Append("BT\n");
            foreach (var kvp in TextState)
                sb.Append(kvp.Value.TrimEnd() + " " + kvp.Key + "\n");
            sb.Append("ET\n");
            return sb.ToString();
        }
    }
}
