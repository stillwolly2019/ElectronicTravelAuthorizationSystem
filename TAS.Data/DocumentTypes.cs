using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using TravelAuthorizationSystem.Utility;

namespace Data
{
    public sealed class DocumentTypes
    {
        public IDataReader GetAllDocumentTypes()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.GetAllDocumentTypes");
            return Reader;
        }

        public IDataReader CheckDuplicateDocumentType(string DocumentTypeID, string DocumentTypeName)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.CheckDuplicateDocumentType", DocumentTypeID, DocumentTypeName);
            return Reader;
        }

        public void InsertUpdateDocumentType(string DocumentTypeID, string DocumentTypeName, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.InsertUpdateDocumentType", DocumentTypeID, DocumentTypeName, CreatedBy);
        }

        public void DeleteDocumentType (string DocumentTypeID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.DeleteDocumentType", DocumentTypeID, CreatedBy);
        }
    }
}
