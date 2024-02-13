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
    public sealed class TravelExpenseClaim
    {
        public void UpdateTravelIteneraryTime(string TravelItineraryID, TimeSpan Time, string DepArr, string ModifiedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.UpdateTravelIteneraryTime", TravelItineraryID, Time, DepArr, ModifiedBy);
        }
        public void UpdateTECItineraryNoOfKms(string TravelItineraryID, float NoOfKms, string ModifiedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.UpdateTECItineraryNoOfKms", TravelItineraryID, NoOfKms, ModifiedBy);
        }
        public void UpdateTECItineraryDSA(string TravelItineraryID, float NoOfDays, float DSARate, float RateAmount, float LocalAmount, string ModifiedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.UpdateTECItineraryDSA", TravelItineraryID, NoOfDays, DSARate, RateAmount, LocalAmount, ModifiedBy);
        }
        public void InsertTECExpenditure(string TravelAuthorizationNumber, DateTime ExpenditureDate, string ExpDetails, string CurrencyID, float ExpenseAmount, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.InsertTECExpenditure", TravelAuthorizationNumber, ExpenditureDate, ExpDetails, CurrencyID, ExpenseAmount, CreatedBy);
        }
        public void UpdateTECExpenditureRates(string TECExpenditureID, float Rate, float RateAmount, float LocalAmount, string ModifiedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.UpdateTECExpenditureRates", TECExpenditureID, Rate, RateAmount, LocalAmount, ModifiedBy);
        }
        public void DeleteTECExpenditure(string TECExpenditureID, string ModifiedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.DeleteTECExpenditure", TECExpenditureID, ModifiedBy);
        }
        public void InsertTECAdvances(string TravelAuthorizationNumber, string PayOfficeCode, DateTime DatePaid, string CurrencyID, float AdvanceAmount, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.InsertTECAdvances", TravelAuthorizationNumber, PayOfficeCode, DatePaid, CurrencyID, AdvanceAmount, CreatedBy);
        }
        public void UpdateTECAdvancesRates(string TECAdvancesID, float Rate, float RateAmount, float LocalAmount, string ModifiedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.UpdateTECAdvancesRates", TECAdvancesID, Rate, RateAmount, LocalAmount, ModifiedBy);
        }
        public void DeleteTECAdvances(string TECAdvancesID, string ModifiedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.DeleteTECAdvances", TECAdvancesID, ModifiedBy);
        }
        public void UpdateTECStatus(string TravelAuthorizationNumber, string StatusID, string ModifiedBy, string Comments)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.UpdateTECStatus", TravelAuthorizationNumber, StatusID, ModifiedBy, Comments);
        }
        public IDataReader GetTECItineraryDSAByTECItineraryID(string TravelItineraryID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.GetTECItineraryDSAByTECItineraryID", TravelItineraryID);
            return Reader;
        }
        public void InsertUpdateTECItineraryDSA(string TECItineraryDSAID, string TECItineraryID, float NoOfDays, float DSARate, float Percentage, float RateAmount, float ExchangeRate, float LocalAmount, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.InsertUpdateTECItineraryDSA", TECItineraryDSAID, TECItineraryID, NoOfDays, DSARate, Percentage, RateAmount, ExchangeRate, LocalAmount, CreatedBy);
        }
        public void InsertEmptyTECItineraryDSA(string TravelItineraryID, string ModifiedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.InsertEmptyTECItineraryDSA", TravelItineraryID, ModifiedBy);
        }
        public void DeleteTECItineraryDSA(string TECItineraryDSAID, string TECItineraryID, string ModifiedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.DeleteTECItineraryDSA", TECItineraryDSAID, TECItineraryID, ModifiedBy);
        }
        public IDataReader CheckTravelIteneraryTime(string TravelAuthorizationNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.CheckTravelIteneraryTime", TravelAuthorizationNumber);
            return Reader;
        }
        public IDataReader GetTECItineraryByTravelAuthorizationNumber(string TravelAuthorizationNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.GetTECItineraryByTravelAuthorizationNumber", TravelAuthorizationNumber);
            return Reader;
        }
        public IDataReader GetTECExpenditureByTravelAuthorizationNumber(string TravelAuthorizationNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.GetTECExpenditureByTravelAuthorizationNumber", TravelAuthorizationNumber);
            return Reader;
        }
        public IDataReader GetTECAdvancesByTravelAuthorizationNumber(string TravelAuthorizationNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.GetTECAdvancesByTravelAuthorizationNumber", TravelAuthorizationNumber);
            return Reader;
        }
        public IDataReader GetTECItineraryExchangeRateByTravelAuthorizationNumber(string TravelAuthorizationNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.GetTECItineraryExchangeRateByTravelAuthorizationNumber", TravelAuthorizationNumber);
            return Reader;
        }

        public void UpdateTECExpenditure(string TravelAuthorizationID, bool ExpenditureNotApplicable, string ModifiedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.UpdateTECExpenditure", TravelAuthorizationID, ExpenditureNotApplicable,ModifiedBy);
        }

        public void UpdateTECAdvances(string TravelAuthorizationID, bool AdvancesNotApplicable, string ModifiedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TEC.UpdateTECAdvances", TravelAuthorizationID, AdvancesNotApplicable, ModifiedBy);
        }

    }
}
