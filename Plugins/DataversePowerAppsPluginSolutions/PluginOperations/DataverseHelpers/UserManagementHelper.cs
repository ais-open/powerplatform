using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PluginOperations.DataverseHelpers
{
    public class UserManagementHelper
    {
        ContextBase context;
        CommonHelper commonHelper;

        public UserManagementHelper(ContextBase contextBase)
        {
            context = contextBase;
            commonHelper= new CommonHelper(context);
        }

        /// <summary>
        /// Gets list of System roles
        /// </summary>
        /// <param name="systemUserId">The system User id</param>
        /// <returns>List of system roles</returns>
        public List<Guid> GetSystemRoles(Guid systemUserId)
        {
            var query = new QueryExpression("systemuserroles")
            {
                ColumnSet = new ColumnSet("roleid"),
                Criteria = new FilterExpression
                {
                    Conditions = { new ConditionExpression("systemuserid", ConditionOperator.Equal, systemUserId) }
                }
            };
            
            var teamRoles = context.Service.RetrieveMultiple(query);
            return teamRoles.Entities.Select(e => e.GetAttributeValue<Guid>("roleid")).ToList();
        }

        /// <summary>
        /// Gets list of team roles
        /// </summary>
        /// <param name="teamid">The Team Id</param>
        /// <returns>List of team Roles</returns>
        public List<Guid> GetTeamRoles(Guid teamid)
        {
            var query = new QueryExpression("teamroles")
            {
                ColumnSet = new ColumnSet("roleid"),
                Criteria = new FilterExpression
                {
                    Conditions = { new ConditionExpression("teamid", ConditionOperator.Equal, teamid) }
                }
            };

            var teamRoles = context.Service.RetrieveMultiple(query);

            return teamRoles.Entities.Select(e => e.GetAttributeValue<Guid>("roleid")).ToList();
        }

        /// <summary>
        /// Gets the role name
        /// </summary>
        /// <param name="roleId">The role id</param>
        /// <returns>Role Name</returns>
        private string GetRoleName(Guid roleId)
        {
            string roleName = string.Empty;

            var role = context.Service.Retrieve("role", roleId, new ColumnSet("name"));

            if (role != null)
            {
                roleName = role.GetAttributeValue<string>("name");
            }

            return roleName;
        }

        /// <summary>
        /// Checks if  a role has a privilege
        /// </summary>
        /// <param name="roleids">List of roleids</param>
        /// <param name="privilegeId">The Privilegeid</param>
        /// <returns>True if any role has a privilege</returns>
        public bool CheckPrivilegesForRoles(List<Guid> roleids, Guid privilegeId)
        {
            var privileges = new List<string>();
            foreach (var roleid in roleids)
            {
                var query = new QueryExpression("roleprivileges")
                {
                    ColumnSet = new ColumnSet("privilegeid"),
                    Criteria = new FilterExpression
                    {
                        Conditions = {
                            new ConditionExpression("roleid", ConditionOperator.Equal, roleid),
                            new ConditionExpression("privilegeid", ConditionOperator.Equal, privilegeId)
                        }
                    }
                };

                var rolePrivileges = context.Service.RetrieveMultiple(query).Entities;
                if (rolePrivileges.Count > 0)
                {
                    context.Trace($"Role id   {roleid} corresponding privilege id {privilegeId}");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns PrivilegeId
        /// </summary>
        /// <param name="privilegeName">The Privilege Name</param>
        /// <returns>The Privilege Id</returns>
        public Guid GetPrivilegeId(string privilegeName)
        {
            var query = new QueryExpression("privilege")
            {
                ColumnSet = new ColumnSet("privilegeid"),

                Criteria = new FilterExpression
                {
                    Conditions = { new ConditionExpression("name", ConditionOperator.Equal, privilegeName) }
                }
            };

            var privilegeEntities = context.Service.RetrieveMultiple(query).Entities;

            if (privilegeEntities != null && privilegeEntities.Count > 0)
            {
                return privilegeEntities[0].Id;
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Gets the privilege name
        /// </summary>
        /// <param name="privilegeId">The Privilege Id</param>
        /// <returns>The privilege name</returns>
        public string GetPrivilegeName(Guid privilegeId)
        {
            var query = new QueryExpression("privilege")
            {
                ColumnSet = new ColumnSet("privilegeid", "name"),

                Criteria = new FilterExpression
                {
                    Conditions = { new ConditionExpression("privilegeid", ConditionOperator.Equal, privilegeId) }
                }
            };

            var privilegeEntities = context.Service.RetrieveMultiple(query).Entities;

            if (privilegeEntities != null && privilegeEntities.Count > 0)
            {
                return privilegeEntities[0].GetAttributeValue<string>("name");
            }

            return string.Empty;
        }


        /// <summary>
        /// Gets roles for privilege
        /// </summary>
        /// <param name="Privilegeid">The privilegeid</param>
        /// <returns>List of role ids</returns>
        public List<Guid> GetRolesForPrivilegeId(Guid Privilegeid)
        {
            var roles = new List<Guid>();

            var query = new QueryExpression("roleprivileges")
            {
                ColumnSet = new ColumnSet("privilegeid", "roleid"),

                Criteria = new FilterExpression
                {
                    Conditions = {

                            new ConditionExpression("privilegeid", ConditionOperator.Equal, Privilegeid)
                    }
                }
            };

            var rolePrivileges = context.Service.RetrieveMultiple(query).Entities;

            if (rolePrivileges.Count > 0)
            {
                foreach (var entity in rolePrivileges)
                {
                    var roleId = entity.GetAttributeValue<Guid>("roleid");

                    context.Trace($"Role id   {Privilegeid} corresponding privilege id {roleId}");

                    roles.Add(roleId);
                }
            }
            return roles;
        }

        /// <summary>
        /// Gets list of privileges for a role
        /// </summary>
        /// <param name="roleid">The role id</param>
        /// <returns>List of privileges</returns>
        public List<Guid> GetPrivilegesForRole(Guid roleid)
        {
            var privileges = new List<Guid>();
            var query = new QueryExpression("roleprivileges")
            {
                ColumnSet = new ColumnSet("privilegeid", "roleid"),
                Criteria = new FilterExpression
                {
                    Conditions = {

                            new ConditionExpression("roleid", ConditionOperator.Equal, roleid)
                    }
                }
            };

            var rolePrivileges = context.Service.RetrieveMultiple(query).Entities;

            if (rolePrivileges.Count > 0)
            {
                foreach (var entity in rolePrivileges)
                {
                    var privilegeid = entity.GetAttributeValue<Guid>("privilegeid");
                    context.Trace($"Role id   {roleid} corresponding privilege id {privilegeid}");
                    privileges.Add(privilegeid);
                }
            }

            return privileges;
        }

        /// <summary>
        /// Returns weather user is privileged
        /// </summary>
        /// <param name="userid">The user id</param>
        /// <param name="usertype">Weather user is Teams/Individual</param>
        /// <param name="priviligeName">Name of privilege to be checked</param>
        /// <returns>True if user is privileged</returns>
        public bool IsUserPrivileged(Guid userid, string usertype, string priviligeName)
        {
            bool isPrivileged = false;
            if (usertype == "Individual")
            {
                isPrivileged = IsSystemUserPrivileged(userid, priviligeName);
                context.Trace($"The privilege status of system user is {isPrivileged}");
            }

            if (usertype == "Teams")
            {
                isPrivileged = IsTeamUserPrivileged(userid, priviligeName);
                context.Trace($"The privilege status of Team user is {isPrivileged}");
            }

            return isPrivileged;
        }

        /// <summary>
        /// Returns whether specified user has a named privilege
        /// </summary>
        /// <param name="service">The IOrganizationService instance to use.</param>
        /// <param name="systemUserId">The Id of the user.</param>
        /// <param name="privilegeName">The name of the privilege.</param>
        /// <returns>True if system user is privileged</returns>
        private bool IsSystemUserPrivileged(Guid systemUserId, string privilegeName)
        {
            Guid PrivilegeId = GetPrivilegeId(privilegeName);
            var roles = GetSystemRoles(systemUserId);
            GetParentRoleId(roles);
            return CheckPrivilegesForRoles(roles, PrivilegeId);
        }

        /// <summary>
        /// Returns weather team user is privileged
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="privilegeName"></param>
        /// <returns></returns>
        private bool IsTeamUserPrivileged(Guid userId, string privilegeName)
        {
            var roles = GetTeamRoles(userId);
            GetParentRoleId(roles);
            Guid privilegeId = GetPrivilegeId(privilegeName);
            return CheckPrivilegesForRoles(roles, privilegeId);
        }

        /// <summary>
        /// Adds parent roleid to the roleid list
        /// </summary>
        /// <param name="roleIds">List of roleids</param>
        private void GetParentRoleId(List<Guid> roleIds)
        {
            List<Guid> parentRoleIds = new List<Guid>();
            foreach (var roleId in roleIds)
            {
                var role = context.Service.Retrieve("role", roleId, new ColumnSet("name", "parentroleid"));

                if (role != null)
                {
                    var parentRole = role.GetAttributeValue<EntityReference>("parentroleid");

                    if (parentRole != null)
                    {
                        parentRoleIds.Add(parentRole.Id);
                    }
                }
            }
            if (parentRoleIds.Count > 0)
                roleIds.AddRange(parentRoleIds);
        }
    }
}
