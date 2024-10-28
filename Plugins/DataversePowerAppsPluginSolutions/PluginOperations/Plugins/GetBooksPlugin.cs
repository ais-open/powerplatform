using Newtonsoft.Json;
using PluginOperations.BaseClasses;
using PluginOperations.BusinessLogic.Books;

namespace PluginOperations.Plugins
{
    public class GetBooksPlugin : PluginBase
    {
        BookDetailOperations bookDetailOperations;

        public override void Execute(ContextBase context)
        {
            bookDetailOperations= new BookDetailOperations(context);
            var books = bookDetailOperations.GetAllBookDetails();

            string jsonBooks= JsonConvert.SerializeObject(books);

            if (!string.IsNullOrEmpty(jsonBooks)) 
            {
                context.Context.OutputParameters["BookCollections"] = jsonBooks;
            }            
        }
    }
}
