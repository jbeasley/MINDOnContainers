namespace MINDOnContainers.Services.Attachment.API.Application.Queries
{
    using Dapper;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;

    public class AttachmentQueries: IAttachmentQueries
    {
        private string _connectionString = string.Empty;

        public AttachmentQueries(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }


        public Task<Attachment> GetAttachmentAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
