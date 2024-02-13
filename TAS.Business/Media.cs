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
    public class Media
    {
        private Data.Media daMedia = new Data.Media();
        HttpContext context = HttpContext.Current;


        //Aded by Walter

        //Get
        public void GetTrainingFilesByTAID(string TAID, ref DataTable dt)
        {
            dt.Load(daMedia.GetTrainingFilesByTAID(TAID));
        }

        public void GetTrainingAppFilesByTAID(string TAID, ref DataTable dt)
        {
            dt.Load(daMedia.GetTrainingAppFilesByTAID(TAID));
        }

        public void GetBookingFilesByTAID(string TAID, ref DataTable dt)
        {
            dt.Load(daMedia.GetBookingFilesByTAID(TAID));
        }

        public void GetAgendaFilesByTAID(string TAID, ref DataTable dt)
        {
            dt.Load(daMedia.GetAgendaFilesByTAID(TAID));
        }

        public void GetLeaveFilesByTAID(string TAID, ref DataTable dt)
        {
            dt.Load(daMedia.GetLeaveFilesByTAID(TAID));
        }

        public void GetOtherDocumentsFilesByTAID(string TAID, ref DataTable dt)
        {
            dt.Load(daMedia.GetOtherDocumentsFilesByTAID(TAID));
        }

        public void GetRRFilesByTAID(string TAID, ref DataTable dt)
        {
            dt.Load(daMedia.GetRRFilesByTAID(TAID));
        }

        public void GetMopFilesByTAID(string TAID, ref DataTable dt)
        {
            dt.Load(daMedia.GetMopFilesByTAID(TAID));
        }
        public void GetTeleFilesByTAID(string TAID, ref DataTable dt)
        {
            dt.Load(daMedia.GetTeleFilesByTAID(TAID));
        }

        //Get
        //Insert
        public void InsertTrainingFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.InsertTrainingFiles(TAID, TANO, FileName, FileExtension, FileData, ui.User_Id);
        }

        public void InsertTrainingAppFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.InsertTrainingAppFiles(TAID, TANO, FileName, FileExtension, FileData, ui.User_Id);
        }

        public void InsertBookingFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.InsertBookingFiles(TAID, TANO, FileName, FileExtension, FileData, ui.User_Id);
        }

        public void InsertAgendaFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.InsertAgendaFiles(TAID, TANO, FileName, FileExtension, FileData, ui.User_Id);
        }

        public void InsertLeaveFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.InsertLeaveFiles(TAID, TANO, FileName, FileExtension, FileData, ui.User_Id);
        }

        public void InsertOtherDocumentsFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.InsertOtherDocumentsFiles(TAID, TANO, FileName, FileExtension, FileData, ui.User_Id);
        }

        public void InsertRRFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.InsertRRFiles(TAID, TANO, FileName, FileExtension, FileData, ui.User_Id);
        }


        public void InsertMopFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.InsertMopFiles(TAID, TANO, FileName, FileExtension, FileData, ui.User_Id);
        }
        public void InsertTeleFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.InsertTeleFiles(TAID, TANO, FileName, FileExtension, FileData, ui.User_Id);
        }

        //Insert
        public void DeleteTrainingFiles(string TAID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.DeleteTrainingFiles(TAID, ui.User_Id);
        }

        public void DeleteTrainingAppFiles(string TAID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.DeleteTrainingAppFiles(TAID, ui.User_Id);
        }

        public void DeleteBookingFiles(string TAID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.DeleteBookingFiles(TAID, ui.User_Id);
        }

        public void DeleteAgendaFiles(string TAID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.DeleteAgendaFiles(TAID, ui.User_Id);
        }

        public void DeleteLeaveFiles(string TAID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.DeleteLeaveFiles(TAID, ui.User_Id);
        }

        public void DeleteOtherDocumentsFiles(string TAID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.DeleteOtherDocumentsFiles(TAID, ui.User_Id);
        }

        public void DeleteRRFiles(string TAID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.DeleteRRFiles(TAID, ui.User_Id);
        }

        public void DeleteMopFiles(string TAID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.DeleteMopFiles(TAID, ui.User_Id);
        }

        public void DeleteTeleFiles(string TAID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.DeleteTeleFiles(TAID, ui.User_Id);
        }

        //Aded by Walter






        //get
        public void GetSecurityTrainingFilesByTAID(string TAID, ref DataTable dt)
        {
            dt.Load(daMedia.GetSecurityTrainingFilesByTAID(TAID));
        }

        public void GetSecurityTrainingFilesByMRID(string MRID, ref DataTable dt)
        {
            dt.Load(daMedia.GetSecurityTrainingFilesByMRID(MRID));
        }

        public void GetPRISMLeaveRequestFilesByMRID(string MRID, ref DataTable dt)
        {
            dt.Load(daMedia.GetPRISMLeaveRequestFilesByMRID(MRID));
        }


        public void GetMedicalFilesByTAID(string TAID, ref DataTable dt)
        {
            dt.Load(daMedia.GetMedicalFilesByTAID(TAID));
        }

        public void GetFilesByTAID(string TAID, ref DataSet ds)
        {
            ds=daMedia.GetFilesByTAID(TAID);
        }

        //delete
        public void DeleteSecurityTrainingFiles(string TAID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.DeleteSecurityTrainingFiles(TAID, ui.User_Id);
        }

        public void DeletePRISMLeaveRequestFiles(string MRID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.DeletePRISMLeaveRequestFiles(MRID, ui.User_Id);
        }

        public void DeleteMedicalFiles(string TAID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.DeleteMedicalFiles(TAID, ui.User_Id);
        }

        //Insert-update
        public void InsertSecurityTrainingFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.InsertSecurityTrainingFiles(TAID, TANO, FileName, FileExtension, FileData, ui.User_Id);
        }

        public void InsertMRSecurityTrainingFiles(string MRID, string MRNO, string FileName, string FileExtension, byte[] FileData)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.InsertMRSecurityTrainingFiles(MRID, MRNO, FileName, FileExtension, FileData, ui.User_Id);
        }
        public void InsertPRISMLeaveRequestFiles(string MRID, string MRNO, string FileName, string FileExtension, byte[] FileData)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.InsertPRISMLeaveRequestFiles(MRID, MRNO, FileName, FileExtension, FileData, ui.User_Id);
        }
        public void InsertMedicalFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.InsertMedicalFiles(TAID, TANO, FileName, FileExtension, FileData, ui.User_Id);
        }
        public void GetTECExpensesFilesTAID(string TAID, ref DataTable dt)
        {
            dt.Load(daMedia.GetTECExpensesFilesTAID(TAID));
        }

        //delete
        public void DeleteTECExpensesFiles(string TAID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.DeleteTECExpensesFiles(TAID, ui.User_Id);
        }
        //Insert-update
        public void InsertTECExpensesFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daMedia.InsertTECExpensesFiles(TAID, TANO, FileName, FileExtension, FileData, ui.User_Id);
        }

    }
}
