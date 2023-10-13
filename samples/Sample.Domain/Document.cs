namespace Sample.Domain
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
    }
}
