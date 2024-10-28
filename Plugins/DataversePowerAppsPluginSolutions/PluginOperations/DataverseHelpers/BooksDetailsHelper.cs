using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PluginOperations.DataverseHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PluginOperations.Helpers
{
    public class BooksDetailsHelper:IHelper
    {
        CommonHelper helper;
        public BooksDetailsHelper(ContextBase contextBase) 
        {
            helper = new CommonHelper(contextBase);
        }

        public List<Entity> GetBookDetails(Guid id)
        {
            List<Entity> entities = new List<Entity>();

            QueryExpression query = new QueryExpression();
            query.EntityName = "new_books";
            query.Criteria = new FilterExpression();
            query.ColumnSet = new ColumnSet(allColumns: true);
            query.Criteria.AddCondition(new ConditionExpression("new_booksid", ConditionOperator.Equal, id));
            var records = helper.GetEntityRecords(query);
            if (records != null && records.Count>0) 
            {
                entities= records.ToList();
            }

            return entities;
        }

        public List<Entity> GetAllBookDetails()
        {
            List<Entity> entities = new List<Entity>();
            QueryExpression query = new QueryExpression();
            query.EntityName = "new_books";
            query.Criteria = new FilterExpression();
            query.ColumnSet = new ColumnSet(allColumns: true);
            //query.Criteria.AddCondition(new ConditionExpression("new_booksid", ConditionOperator.Equal, id));
            var records = helper.GetEntityRecords(query);
            if (records != null && records.Count > 0)
            {
                entities = records.ToList();
            }

            return entities;
        }
    }
}
