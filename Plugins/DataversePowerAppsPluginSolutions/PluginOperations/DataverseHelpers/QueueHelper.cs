using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using System.ServiceModel;

namespace PluginOperations.DataverseHelpers
{
    public class QueueHelper
    {
        ContextBase context;

        public QueueHelper(ContextBase contextBase)
        {
            context = contextBase;
        }

        /// <summary>
        /// Removes an item from the queue
        /// </summary>
        /// <returns>True if item is removed</returns>
        public bool RemoveCurrentItemFromQueue()
        {
            Entity targetEntity = context.Target;

            Guid stepId = targetEntity.GetAttributeValue<Guid>("wtwosna_procedurestepid");

            Guid queueItemId = Guid.Empty;

            EntityReference queueReference = new EntityReference("wtwosna_procedurestep", stepId);

            QueryExpression query = new QueryExpression("queueitem");
            query.ColumnSet = new ColumnSet(true);

            query.Criteria.AddCondition("objectid", ConditionOperator.Equal, queueReference.Id);

            EntityCollection queueItems = context.RetrieveMultiple(query);

            if (queueItems != null && queueItems.Entities != null && queueItems.Entities.Count > 0)
            {
                queueItemId = queueItems[0].Id;
            }

            bool success = false;

            try
            {
                if (queueItemId != Guid.Empty)
                {
                    RemoveFromQueueRequest request = new RemoveFromQueueRequest
                    {
                        QueueItemId = queueItemId,
                    };

                    var response = (RemoveFromQueueResponse)context.Service.Execute(request);

                    context.Trace($"Created account with ID:{response.Results}");
                    success = true;
                }
            }
            catch (FaultException<ExecuteTransactionFault> ex)
            {
                int index = ((ExecuteTransactionFault)ex.Detail).FaultedRequestIndex + 1;
                string message = ex.Detail.Message;
                context.Trace($"Remove from  request failed for the account {index} because: {message}");
                throw;
            }
            return success;
        }

        /// <summary>
        /// Adds Request to Queue (Note: this needs to be revisited)
        /// </summary>
        /// <param name="requestCollection">request collection</param>
        /// <returns>True if the queue gets added</returns>
        public bool RunTransactionForQueue(OrganizationRequestCollection requestCollection)
        {
            try
            {
                ExecuteTransactionRequest request = new ExecuteTransactionRequest()
                {
                    Requests = requestCollection,
                    ReturnResponses = true
                };

                var response = (ExecuteTransactionResponse)context.Service.Execute(request);

                response.Responses.ToList().ForEach(x =>
                {
                    var createResponse = (AddToQueueResponse)x;
                    context.Trace($"Created account with ID:{createResponse.QueueItemId}");
                });

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
    }
}
