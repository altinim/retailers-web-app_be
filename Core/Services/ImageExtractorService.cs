using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Xobject;

namespace Core.Services {
    public class ImageExtractionStrategy : IEventListener {
        private List<string> images = new List<string>();

        public void EventOccurred(IEventData data, EventType type) {
            if (type == EventType.RENDER_IMAGE) {
                var renderInfo = (ImageRenderInfo)data;
                var image = renderInfo.GetImage();
                string ImageToBase64 = ConvertImageToBase64(image);
                images.Add(ImageToBase64);
            }
        }
        private string ConvertImageToBase64(PdfImageXObject image) {
            using (MemoryStream stream = new MemoryStream()) {
                byte[] imageBytes = image.GetImageBytes();
                stream.Write(imageBytes, 0, imageBytes.Length);
                return Convert.ToBase64String(stream.ToArray());
            }
        }
        public ICollection<string> GetImages() {
            return images;
        }

        public ICollection<EventType> GetSupportedEvents() {
            return new List<EventType> { EventType.RENDER_IMAGE };
        }

        public void Dispose() { }
    }
}
