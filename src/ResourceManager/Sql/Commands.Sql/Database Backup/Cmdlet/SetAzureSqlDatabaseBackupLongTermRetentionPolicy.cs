﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.Azure.Commands.Sql.Backup.Model;
using Microsoft.Azure.Commands.Sql.Database.Model;

namespace Microsoft.Azure.Commands.Sql.Backup.Cmdlet
{
    /// <summary>
    /// Cmdlet to create or update a new Azure Sql Database backup archival policy
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "AzureRmSqlDatabaseBackupLongTermRetentionPolicy",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Low)]
    public class SetAzureSqlDatabaseBackupLongTermRetentionPolicy : AzureSqlDatabaseBackupLongTermRetentionPolicyCmdletBase
    {
        /// <summary>
        /// Gets or sets the backup long term retention state
        /// </summary>
        [Parameter(Mandatory = true,
            HelpMessage = "The state of the long term retention backup policy, 'Enabled' or 'Disabled'")]
        [ValidateNotNullOrEmpty]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the name of the backup long term retention policy
        /// </summary>
        [Parameter(Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The Resource ID of the backup long term retention policy.")]
        [ValidateNotNullOrEmpty]
        [Alias("Id")]
        public string ResourceId { get; set; }

        /// <summary>
        /// Get the entities from the service
        /// </summary>
        /// <returns>The list of entities</returns>
        protected override IEnumerable<AzureSqlDatabaseBackupLongTermRetentionPolicyModel> GetEntity()
        {
            return new List<AzureSqlDatabaseBackupLongTermRetentionPolicyModel>() { 
                ModelAdapter.GetDatabaseBackupLongTermRetentionPolicy(this.ResourceGroupName, this.ServerName, this.DatabaseName) 
            };
        }

        /// <summary>
        /// Create the model from user input
        /// </summary>
        /// <param name="model">Model retrieved from service</param>
        /// <returns>The model that was passed in</returns>
        protected override IEnumerable<AzureSqlDatabaseBackupLongTermRetentionPolicyModel> ApplyUserInputToModel(IEnumerable<AzureSqlDatabaseBackupLongTermRetentionPolicyModel> model)
        {
            List<Model.AzureSqlDatabaseBackupLongTermRetentionPolicyModel> newEntity = 
                new List<AzureSqlDatabaseBackupLongTermRetentionPolicyModel>();
            newEntity.Add(new AzureSqlDatabaseBackupLongTermRetentionPolicyModel()
            {
                ResourceGroupName = ResourceGroupName,
                ServerName = ServerName,
                DatabaseName = DatabaseName,
                State = State,
                RecoveryServicesBackupPolicyResourceId = ResourceId,
                Location = model.FirstOrDefault().Location,
            });
            return newEntity;
        }

        /// <summary>
        /// Update the entity
        /// </summary>
        /// <param name="entity">The output of apply user input to model</param>
        /// <returns>The input entity</returns>
        protected override IEnumerable<AzureSqlDatabaseBackupLongTermRetentionPolicyModel> PersistChanges(IEnumerable<AzureSqlDatabaseBackupLongTermRetentionPolicyModel> entity)
        {
            if (ShouldProcess(DatabaseName))
            {
                return new List<AzureSqlDatabaseBackupLongTermRetentionPolicyModel>() {
                    ModelAdapter.SetDatabaseBackupLongTermRetentionPolicy(this.ResourceGroupName, this.ServerName, this.DatabaseName, entity.First())
                };
            }
            else
            {
                return null;
            }
        }
    }
}
