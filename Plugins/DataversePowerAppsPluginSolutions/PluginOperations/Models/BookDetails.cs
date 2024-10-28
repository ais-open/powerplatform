
using System;

namespace PluginOperations.Models
{
    public class BookDetails
    {
        public Guid BookId { get; set; }
        public string BookName { get; set; }
        public string BookDescription { get; set; }
        public Guid AuthorId { get; set; }
        public string Author { get; set; }
        public DateTime PublishingDate { get; set; }
    }
}
