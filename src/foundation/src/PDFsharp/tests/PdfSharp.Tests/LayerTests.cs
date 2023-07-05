using PdfSharp.Drawing;
using PdfSharp.Pdf.Annotations;
using PdfSharp.Pdf.IO;
using Xunit;

namespace PdfSharp.Tests
{
    public class LayerTests
    {
        [Theory]
        [InlineData(@"path to pdf")]
        public void Read_exist_test(string filePath)
        {
            var doc = PdfReader.Open(filePath);
            foreach (var layer in doc.Layers)
                Console.WriteLine(layer.Name);
            doc.Close();
        }

        [Theory]
        [InlineData(@"path to pdf")]
        public void Create_layer_test(string filePath)
        {
            var doc = PdfReader.Open(filePath);
            var layerNormal = doc.AddLayer("Test layer normal");

            var layerHidden = doc.AddLayer("Test layer hidden");
            layerHidden.On = false;

            var layerLocked = doc.AddLayer("Test layer locked");
            layerLocked.Locked = true;

            var folderPath = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath) + "_modified.pdf";
            var filePathModified = Path.Combine(folderPath, fileName);
            doc.Save(filePathModified);
            doc.Close();

            doc = PdfReader.Open(filePathModified);
            if (doc.Layers.Count != 3) throw new Exception("Layers' count is incorrect.");

            var hiddenCnt = 0;
            var lockedCnt = 0;
            foreach (var layer in doc.Layers)
            {
                if (layer.Locked)
                    lockedCnt++;
                if (layer.On == false)
                    hiddenCnt++;
            }
            doc.Close();

            if (hiddenCnt <= 0) throw new Exception("Failed to create hidden layer.");
            if (lockedCnt <= 0) throw new Exception("Failed to create locked layer.");
        }

        [Theory]
        [InlineData(@"path to pdf")]
        public void Annotation_with_layer_test(string filePath)
        {
            var doc = PdfReader.Open(filePath);

            var anno = new PdfTextAnnotation();
            anno.Contents = "This is a test";
            anno.Title = "Title";
            anno.Rectangle = new Pdf.PdfRectangle(new XPoint(10, 10), new XPoint(100, 100));

            var layer = doc.AddLayer("Test layer");
            anno.SetLayer(layer);

            var page1 = doc.Pages[0];
            page1.Annotations.Add(anno);

            var folderPath = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath) + "_modified.pdf";
            var filePathModified = Path.Combine(folderPath, fileName);
            doc.Save(filePathModified);
            doc.Close();

            doc = PdfReader.Open(filePathModified);
            page1 = doc.Pages[0];

            var cnt = 0;
            foreach (PdfAnnotation item in page1.Annotations)
            {
                if (item.Layer?.Name == "Test layer")
                    cnt++;
            }

            doc.Close();

            if (cnt <= 0) throw new Exception("Failed to create annotation inside test layer.");
        }
    }
}
