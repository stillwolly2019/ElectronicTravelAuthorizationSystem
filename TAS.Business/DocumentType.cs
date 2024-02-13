using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;

namespace Business
{
    public class DocumentTypes
    {
        private Data.DocumentTypes daDTypes = new Data.DocumentTypes();
        HttpContext context = HttpContext.Current;

        public void GetAllDocumentTypes(ref DataTable dt)
        {
            dt.Load(daDTypes.GetAllDocumentTypes());
        }

        public void CheckDuplicateDocumentType(string DocumentTypeID, string DocumentTypeName, ref DataTable dt)
        {
            dt.Load(daDTypes.CheckDuplicateDocumentType(DocumentTypeID, DocumentTypeName));
        }

        public void InsertUpdateDocumentType(string DocumentTypeID, string DocumentTypeName)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daDTypes.InsertUpdateDocumentType(DocumentTypeID, DocumentTypeName, ui.User_Id);
        }

        public void DeleteDocumentType(string DocumentTypeID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daDTypes.DeleteDocumentType(DocumentTypeID, ui.User_Id);
        }

    }

}
