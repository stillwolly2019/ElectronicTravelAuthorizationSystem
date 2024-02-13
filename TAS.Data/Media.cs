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
    public sealed class Media
    {
        //Added by Walter
        public IDataReader GetTrainingFilesByTAID(string TAID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.GetTrainingFilesByTAID", TAID);
            return Reader;
        }

        public IDataReader GetTrainingAppFilesByTAID(string TAID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.GetTrainingAppFilesByTAID", TAID);
            return Reader;
        }

        public IDataReader GetBookingFilesByTAID(string TAID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.GetBookingFilesByTAID", TAID);
            return Reader;
        }

        public IDataReader GetAgendaFilesByTAID(string TAID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.GetAgendaFilesByTAID", TAID);
            return Reader;
        }

        public IDataReader GetLeaveFilesByTAID(string TAID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.GetLeaveFilesByTAID", TAID);
            return Reader;
        }

        public IDataReader GetRRFilesByTAID(string TAID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.GetRRFilesByTAID", TAID);
            return Reader;
        }

        public IDataReader GetMopFilesByTAID(string TAID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.GetMopFilesByTAID", TAID);
            return Reader;
        }

        public IDataReader GetTeleFilesByTAID(string TAID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.GetTeleFilesByTAID", TAID);
            return Reader;
        }

        public IDataReader GetOtherDocumentsFilesByTAID(string TAID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.GetOtherDocumentsFilesByTAID", TAID);
            return Reader;
        }


        public void InsertTrainingFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.InsertTrainingFiles", TAID, TANO, FileName, FileExtension, FileData, CreatedBy);
        }

        public void InsertTrainingAppFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.InsertTrainingAppFiles", TAID, TANO, FileName, FileExtension, FileData, CreatedBy);
        }

        public void InsertBookingFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.InsertBookingFiles", TAID, TANO, FileName, FileExtension, FileData, CreatedBy);
        }

        public void InsertAgendaFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.InsertAgendaFiles", TAID, TANO, FileName, FileExtension, FileData, CreatedBy);
        }

        public void InsertLeaveFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.InsertLeaveFiles", TAID, TANO, FileName, FileExtension, FileData, CreatedBy);
        }

        public void InsertOtherDocumentsFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.InsertOtherDocumentsFiles", TAID, TANO, FileName, FileExtension, FileData, CreatedBy);
        }

        public void InsertRRFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.InsertRRFiles", TAID, TANO, FileName, FileExtension, FileData, CreatedBy);
        }

        public void InsertMopFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.InsertMopFiles", TAID, TANO, FileName, FileExtension, FileData, CreatedBy);
        }

        public void InsertTeleFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.InsertTeleFiles", TAID, TANO, FileName, FileExtension, FileData, CreatedBy);
        }

        public void DeleteTrainingFiles(string TAID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.DeleteTrainingFiles", TAID, CreatedBy);
        }

        public void DeleteTrainingAppFiles(string TAID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.DeleteTrainingAppFiles", TAID, CreatedBy);
        }

        public void DeleteBookingFiles(string TAID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.DeleteBookingFiles", TAID, CreatedBy);
        }

        public void DeleteAgendaFiles(string TAID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.DeleteAgendaFiles", TAID, CreatedBy);
        }

        public void DeleteLeaveFiles(string TAID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.DeleteLeaveFiles", TAID, CreatedBy);
        }

        public void DeleteOtherDocumentsFiles(string TAID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.DeleteOtherDocumentsFiles", TAID, CreatedBy);
        }

        public void DeleteRRFiles(string TAID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.DeleteRRFiles", TAID, CreatedBy);
        }
        public void DeleteMopFiles(string TAID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.DeleteMopFiles", TAID, CreatedBy);
        }

        public void DeleteTeleFiles(string TAID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString,
                "TA.DeleteTeleFiles", TAID, CreatedBy);
        }



        //Added by Walter


        //get
        public IDataReader GetSecurityTrainingFilesByTAID(string TAID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "TA.GetSecurityTrainingFilesByTAID", TAID);
            return Reader;
        }
        
        public IDataReader GetPRISMLeaveRequestFilesByMRID(string MRID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "MR.GetPRISMLeaveRequestFilesByMRID", MRID);
            return Reader;
        }

        public IDataReader GetSecurityTrainingFilesByMRID(string MRID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "MR.GetSecurityTrainingFilesByMRID", MRID);
            return Reader;
        }
        public IDataReader GetMedicalFilesByTAID(string TAID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "TA.GetMedicalFilesByTAID", TAID);
            return Reader;
        }
        public DataSet GetFilesByTAID(string TAID)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "TA.[GetFilesByTAID]", TAID);
            return ds;

        }
        //delete
        public void DeleteSecurityTrainingFiles(string TAID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "TA.DeleteSecurityTrainingFiles", TAID, CreatedBy);
        }

        public void DeletePRISMLeaveRequestFiles(string MRID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "MR.DeletePRISMLeaveRequestFiles", MRID, CreatedBy);
        }

        public void DeleteMRSecurityTrainingFiles(string MRID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "MR.DeleteSecurityTrainingFiles", MRID, CreatedBy);
        }
        public void DeleteMedicalFiles(string TAID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "TA.DeleteMedicalFiles", TAID, CreatedBy);
        }
        //insert-update
        public void InsertSecurityTrainingFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "TA.InsertSecurityTrainingFiles", TAID, TANO, FileName, FileExtension, FileData, CreatedBy);
        }

        public void InsertMRSecurityTrainingFiles(string MRID, string MRNO, string FileName, string FileExtension, byte[] FileData, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "MR.InsertSecurityTrainingFiles", MRID, MRNO, FileName, FileExtension, FileData, CreatedBy);
        }

        public void InsertPRISMLeaveRequestFiles(string MRID, string MRNO, string FileName, string FileExtension, byte[] FileData, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "MR.InsertPRISMLeaveRequestFiles", MRID, MRNO, FileName, FileExtension, FileData, CreatedBy);
        }

        public void InsertMedicalFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "TA.[InsertMedicalFiles]", TAID, TANO, FileName, FileExtension, FileData, CreatedBy);
        }
        //get
        public IDataReader GetTECExpensesFilesTAID(string TAID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "TEC.GetTECExpensesFilesTAID", TAID);
            return Reader;
        }

        //delete
        public void DeleteTECExpensesFiles(string TAID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "TEC.DeleteTECExpensesFiles", TAID, CreatedBy);
        }

        //insert-update
        public void InsertTECExpensesFiles(string TAID, string TANO, string FileName, string FileExtension, byte[] FileData, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.TravelAuthorizationMediaConnectionString, "TEC.InsertTECExpensesFiles", TAID, TANO, FileName, FileExtension, FileData, CreatedBy);
        }
        
    }
}
