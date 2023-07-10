using System.Text;

namespace PdfSharp.Pdf.StreamContent
{
    public class PdfStreamContainer : PdfStreamItem
    {
        /// <summary>
        /// Marked content name, starts with a '/'.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Marked content property list.
        /// </summary>
        public string? PropertyList { get; set; }
        public List<PdfStreamItem> Items { get; private set; } = new();
        public override String ToString()
        {
            if (Items.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();
            sb.Append("\n" + Name + " ");
            if (!string.IsNullOrEmpty(PropertyList))
            {
                sb.Append(PropertyList + " BDC\n");
            }
            else
            {
                sb.Append(" BMC\n");
            }
            foreach (var item in Items)
            {
                sb.Append(item.ToString());
            }
            sb.Append("EMC\n");
            return sb.ToString();
        }
    }
}
