using Microsoft.Xrm.Sdk;
using PluginOperations.Models;
using System;

namespace PluginOperations.Mapper
{
    public class BooksDetailMapper : IMapper<BookDetails, Entity>
    {
        public BookDetails MapFromEntity(Entity entity)
        {
            var bookDetail = new BookDetails();

            bookDetail.BookName = entity.GetAttributeValue<string>("new_name");
            bookDetail.BookId = entity.GetAttributeValue<Guid>("new_booksid");
            bookDetail.BookDescription = entity.GetAttributeValue<string>("new_bookdescription");
            var authorReference = entity.GetAttributeValue<EntityReference>("new_author");
            if(authorReference!=null)
            {
                bookDetail.AuthorId = authorReference.Id;
            }
            
            bookDetail.PublishingDate = entity.GetAttributeValue<DateTime>("new_publishingdate");

            return bookDetail;
        }

        public Entity MapToEntity(BookDetails model)
        {
            var entity = new Entity();
            return entity;
        }
    }
}
