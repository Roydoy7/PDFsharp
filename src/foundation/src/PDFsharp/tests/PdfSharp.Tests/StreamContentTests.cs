using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.StreamContent;
using System.Diagnostics;
using System.Text;
using Xunit;

namespace PdfSharp.Tests
{
    public class StreamContentTests
    {
        [Theory]
        //[InlineData(@"path to pdf", @"path to pdf")]
        [InlineData(@"D:\Work\2024\5.22 計器リストジャンプ\計器リスト_R3.05_Rapidus千歳_Phase1-2_水処理棟.pdf", @"path to pdf")]
        public void Parse_stream_contents_test(string filePathSrc, string filePathDest)
        {
            //Remove this layer's contents
            var layerName = "/oc24";
            var parser = new PdfStreamParser();

            var pdfDoc = PdfReader.Open(filePathSrc);
            var cnt = pdfDoc.PageCount;
            for (var i = 0; i < cnt; i++)
            {
                var page = pdfDoc.Pages[i];
                var contents = page.Contents;

                //Read stream contents
                var sb = new StringBuilder();
                foreach (var content in contents)
                {
                    var stream = content.Stream;
                    sb.Append(stream.ToString());
                }

                var str = sb.ToString();
                var objs = parser.Parse(str);
                for (var j = objs.Count - 1; j >= 0; j--)
                {
                    var obj = objs[j];
                    if (obj is PdfStreamContainer container)
                    {
                        //Remove this layer's content
                        if (container.Name == "/OC" && container.PropertyList == layerName)
                        {
                            for (var k = container.Items.Count - 1; k >= 0; k--)
                            {
                                var obj2 = container.Items[k];
                                if (obj2 is PdfStreamPath or PdfStreamText)
                                    container.Items.Remove(obj2);
                            }
                        }
                    }
                }

                //Replace with altered stream content
                page.Contents.Elements.Clear();
                var contentNew = page.Contents.AppendContent();
                contentNew.CreateStream(parser.GetBytes(objs));
            }
            pdfDoc.Save(filePathDest);
            pdfDoc.Close();
        }
    }
}
