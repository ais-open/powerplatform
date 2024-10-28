using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace PluginOperations.DataverseHelpers
{
    public class CommonHelper
    {
        ContextBase context;

        public CommonHelper(ContextBase contextBase)
        {
            context = contextBase;
        }

        public void TraceMessage(string message)
        {
            context.Trace(message);
        }

        /// <summary>
        /// This method returns a specific column value of an entity where one of column name and value is provided
        /// </summary>
        /// <param name="entityName">Name of the entity</param>
        /// <param name="referenceColumnName">The column in condition to be checked</param>
        /// <param name="referenceColumnValue">The value of column in condition to be checked</param>
        /// <param name="returnColumnName">The name of column whose value is returned</param>
        /// <returns></returns>
        public string GetColumnValueFromEntity(string entityName, string referenceColumnName, string referenceColumnValue, string returnColumnName)
        {
            try
            {
                QueryExpression query = new QueryExpression();
                query.EntityName = entityName;
                query.Criteria = new FilterExpression();
                query.ColumnSet = new ColumnSet(allColumns: true);
                query.Criteria.AddCondition(new ConditionExpression(referenceColumnName, ConditionOperator.Equal, referenceColumnValue));

                var results = context.RetrieveMultiple(query);

                if (results.Entities.Count > 0)
                {
                    return results.Entities[0][returnColumnName].ToString();
                }

                return "";
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// This method returns the 1st row among all values returned while querying a value against a column
        /// </summary>
        /// <param name="entityName">The Entity name</param>
        /// <param name="columnName">The column name which is checked for value to be present</param>
        /// <param name="columnValue">The column value for which column is checked.</param>
        /// <returns>The first row of the resultset</returns>
        public Entity GetEntityRecord(string entityName, string columnName, string columnValue)
        {
            try
            {
                QueryExpression query = new QueryExpression();
                query.EntityName = entityName;
                query.Criteria = new FilterExpression();
                query.ColumnSet = new ColumnSet(allColumns: true);
                query.Criteria.AddCondition(new ConditionExpression(columnName, ConditionOperator.Equal, columnValue));

                var results = context.RetrieveMultiple(query);
                if (results.Entities.Count > 0)
                {
                    return results.Entities[0];
                }

                return null;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// This method returns list of all rows
        /// </summary>
        /// <param name="entityName">The entityname from which rows are fetched</param>
        /// <param name="columnName">The name of column  </param>
        /// <param name="id">the value of column</param>
        /// <returns>List of rows</returns>
        public DataCollection<Entity> GetEntityRecords(string entityName, string columnName, Guid id)
        {
            try
            {
                QueryExpression query = new QueryExpression();
                query.EntityName = entityName;
                query.Criteria = new FilterExpression();
                query.ColumnSet = new ColumnSet(allColumns: true);
                query.Criteria.AddCondition(new ConditionExpression(columnName, ConditionOperator.Equal, id));

                var results = context.RetrieveMultiple(query);
                return results.Entities;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// This method returns list of rows after processing a query condition.
        /// </summary>
        /// <param name="query">Query to be processed</param>
        /// <returns>List of filtered rows</returns>
        public DataCollection<Entity> GetEntityRecords(QueryExpression query)
        {
            try
            {
                var results = context.RetrieveMultiple(query);
                return results.Entities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This method executes a transaction
        /// </summary>
        /// <param name="requestCollection">Collection of requests which are processed as a transaction</param>
        /// <returns>Weather transaction is succesfull or not</returns>
        public bool RunTransaction(OrganizationRequestCollection requestCollection)
        {
            try
            {
                context.Trace("Transaction Execution Started");
                ExecuteTransactionRequest request = new ExecuteTransactionRequest()
                {
                    Requests = requestCollection,
                    ReturnResponses = true
                };

                var response = (ExecuteTransactionResponse)context.Service.Execute(request);

                return true;
            }
            catch (FaultException<ExecuteTransactionFault> ex)
            {
                int index = ((ExecuteTransactionFault)ex.Detail).FaultedRequestIndex + 1;
                string message = ex.Detail.Message;
                context.Trace($"Create request failed for the account {index} because: {message}");                
                throw;
            }
        }



        public bool ExecuteMultipleTransactions(CreateMultipleRequest requests)
        {
            try
            {
                context.Trace("Transaction Execution Started");
                var response = (CreateMultipleResponse)context.Service.Execute(requests);
                return true;
            }
            catch (FaultException<ExecuteTransactionFault> ex)
            {
                int index = ((ExecuteTransactionFault)ex.Detail).FaultedRequestIndex + 1;
                string message = ex.Detail.Message;
                context.Trace($"Create request failed for the account {index} because: {message}");                
                throw;
            }
        }

        public bool UpdateStatus(string tableName, Guid id,
            string statusColumnName, int statusValue, string statecode, int stateValue)
        {
            bool isSuccessfull = false;
            try
            {
                Entity ent = context.Retrieve(tableName, id, new ColumnSet(statusColumnName, statecode));
                var myoptionset = new OptionSetValue(statusValue);
                ent[statusColumnName] = myoptionset;
                var stateOptionSet = new OptionSetValue(stateValue);
                ent[statecode] = stateOptionSet;
                context.Update(ent);
                isSuccessfull = true;
            }
            catch (Exception ex)
            {
                context.Trace($"Unable to update the status in context table.Aborting the process.The exception received is {ex.Message}");
                throw ex;
            }

            return isSuccessfull;
        }

        public bool UpdateStatus(string tableName,
            string statusColumnName, int statusValue)
        {
            Entity targetEntity = context.Target;

            bool isSuccessfull = false;

            try
            {
                Entity ent = context.Retrieve(tableName, targetEntity.Id, new ColumnSet(allColumns: true));
                var myoptionset = new OptionSetValue(statusValue);
                ent[statusColumnName] = myoptionset;
                context.Update(ent);
                isSuccessfull = true;
            }
            catch (Exception ex)
            {
                context.Trace($"Unable to update the status in context table.Aborting the process.The exception received is {ex.Message}");
                throw ex;
            }

            return isSuccessfull;
        }

        public bool UpdateStatus(string tableName, Guid primaryId,
           string statusColumnName, int statusValue)
        {
            bool isSuccessfull = false;
            try
            {
                Entity ent = context.Retrieve(tableName, primaryId, new ColumnSet(allColumns: true));
                var myoptionset = new OptionSetValue(statusValue);
                ent[statusColumnName] = myoptionset;
                context.Update(ent);
                isSuccessfull = true;
            }
            catch (Exception ex)
            {
                context.Trace($"Unable to update the status in context table.Aborting the process.The exception received is {ex.Message}");
                throw ex;
            }
            return isSuccessfull;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public dynamic GetEnvironmentVariableValue(string variableName)
        {
            dynamic variableValue = null;
            QueryExpression query = new QueryExpression("environmentvariabledefinition")
            {
                ColumnSet = new ColumnSet("environmentvariabledefinitionid", "defaultvalue")
            };

            query.Criteria.AddCondition("schemaname", ConditionOperator.Equal, variableName);
            var records = this.GetEntityRecords(query);
            
            if (records != null && records.Count > 0)
            {
                variableValue=records[0].GetAttributeValue<dynamic>("defaultvalue");
                return variableValue;
            }

            return null;
        }

        /// <summary>
        /// This method gets the windows timezone from a given IANA Time Name
        /// </summary>
        /// <param name="ianaTimeName">IANA Time name</param>
        /// <returns>Windows time zone name</returns>
        public string GetWindowsTimeZone(string ianaTimeName)
        {
            string WindowsTzName = string.Empty;
            var request = new OrganizationRequest("wtwosna_IanatowindowstzConverter");
            request["IANAName"] = ianaTimeName;
            var response = context.Service.Execute(request);
            if (response != null)
            {
                WindowsTzName = (string)response["WindowsTimeZoneName"];
            }

            return WindowsTzName;
        }
    }
}
