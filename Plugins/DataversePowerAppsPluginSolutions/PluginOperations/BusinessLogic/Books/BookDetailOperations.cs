using Microsoft.Xrm.Sdk;
using PluginOperations.Builder;
using PluginOperations.Helpers;
using PluginOperations.Models;
using System;
using System.Collections.Generic;

namespace PluginOperations.BusinessLogic.Books
{

    public class BookDetailOperations
    {
        BooksDetailBuilder _builder;
        BooksDetailsHelper _helper;
        ContextBase _context;

        public BookDetailOperations(ContextBase context)
        { 
            _context = context;
            _builder = new BooksDetailBuilder();
            _helper = new BooksDetailsHelper(context);
        }

        public List<BookDetails> GetBookDetailsByauthorId(Guid authorId)
        { 
            List<Entity> entities= _helper.GetBookDetails(authorId);
            List<BookDetails> books = new List<BookDetails>();
            foreach (Entity entity in entities) 
            {
                BookDetails book = _builder.BuildModel(entity);
                books.Add(book);
            }
            return books;
        }

        public List<BookDetails> GetAllBookDetails()
        {
            List<Entity> entities = _helper.GetAllBookDetails();
            List<BookDetails> books = new List<BookDetails>();
            foreach (Entity entity in entities)
            {
                BookDetails book= _builder.BuildModel(entity);

                books.Add(book);
            }

            return books;
        }
    }
}
