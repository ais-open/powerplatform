using Microsoft.Xrm.Sdk;
using PluginOperations.Mapper;
using PluginOperations.Models;
using System.Collections.Generic;

namespace PluginOperations.Builder
{
    public class BooksDetailBuilder : IBuilder<BookDetails, Entity>
    {
        BooksDetailMapper _mapper;
        
        public BooksDetailBuilder()
        {
            _mapper = new BooksDetailMapper();
            
        }

        public List<Entity> BuildEntity(List<BookDetails> instances)
        {
            var entityList = new List<Entity>();

            foreach (var instance in instances)
            {
                entityList.Add(_mapper.MapToEntity(instance));
            }

            return entityList;
        }

        public BookDetails BuildModel(Entity entity)
        {   
            var bookDetail=_mapper.MapFromEntity(entity);          

            return bookDetail;
        }
    }
}
