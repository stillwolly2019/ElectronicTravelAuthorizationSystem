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
    public class TravelExpenseClaim
    {
        private Data.TravelExpenseClaim daTEC = new Data.TravelExpenseClaim();
        HttpContext context = HttpContext.Current;
        public void UpdateTravelIteneraryTime(string TravelItineraryID, TimeSpan Time, string DepArr)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.UpdateTravelIteneraryTime(TravelItineraryID, Time, DepArr, ui.User_Id);
        }
        public void UpdateTECItineraryNoOfKms(string TravelItineraryID, float NoOfKms)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.UpdateTECItineraryNoOfKms(TravelItineraryID, NoOfKms, ui.User_Id);
        }
        public void UpdateTECItineraryDSA(string TravelItineraryID, float NoOfDays, float DSARate, float RateAmount, float LocalAmount)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.UpdateTECItineraryDSA(TravelItineraryID, NoOfDays, DSARate, RateAmount, LocalAmount, ui.User_Id);
        }
        public void InsertTECExpenditure(string TravelAuthorizationNumber, DateTime ExpenditureDate, string ExpDetails, string CurrencyID, float ExpenseAmount)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.InsertTECExpenditure(TravelAuthorizationNumber, ExpenditureDate, ExpDetails, CurrencyID, ExpenseAmount, ui.User_Id);
        }
        public void UpdateTECExpenditureRates(string TECExpenditureID, float Rate, float RateAmount, float LocalAmount)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.UpdateTECExpenditureRates(TECExpenditureID, Rate, RateAmount, LocalAmount, ui.User_Id);
        }
        public void DeleteTECExpenditure(string TECExpenditureID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.DeleteTECExpenditure(TECExpenditureID, ui.User_Id);
        }
        public void InsertTECAdvances(string TravelAuthorizationNumber, string PayOfficeCode, DateTime DatePaid, string CurrencyID, float AdvanceAmount)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.InsertTECAdvances(TravelAuthorizationNumber, PayOfficeCode, DatePaid, CurrencyID, AdvanceAmount, ui.User_Id);
        }
        public void UpdateTECAdvancesRates(string TECAdvancesID, float Rate, float RateAmount, float LocalAmount)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.UpdateTECAdvancesRates(TECAdvancesID, Rate, RateAmount, LocalAmount, ui.User_Id);
        }
        public void DeleteTECAdvances(string TECAdvancesID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.DeleteTECAdvances(TECAdvancesID, ui.User_Id);
        }
        public void UpdateTECStatus(string TravelAuthorizationNumber, string StatusID, string Comments)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.UpdateTECStatus(TravelAuthorizationNumber, StatusID, ui.User_Id, Comments);
        }
        public void GetTECItineraryDSAByTECItineraryID(ref DataTable dt, string TravelItineraryID)
        {
            dt.Load(daTEC.GetTECItineraryDSAByTECItineraryID(TravelItineraryID));
        }
        public void InsertUpdateTECItineraryDSA(string TECItineraryDSAID, string TECItineraryID, float NoOfDays, float DSARate, float Percentage, float RateAmount, float ExchangeRate, float LocalAmount)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.InsertUpdateTECItineraryDSA(TECItineraryDSAID, TECItineraryID, NoOfDays, DSARate, Percentage, RateAmount, ExchangeRate, LocalAmount, ui.User_Id);
        }
        public void InsertEmptyTECItineraryDSA(string TravelItineraryID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.InsertEmptyTECItineraryDSA(TravelItineraryID, ui.User_Id);
        }
        public void DeleteTECItineraryDSA(string TECItineraryDSAID, string TECItineraryID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.DeleteTECItineraryDSA(TECItineraryDSAID, TECItineraryID, ui.User_Id);
        }
        public void CheckTravelIteneraryTime(ref DataTable dt, string TravelAuthorizationNumber)
        {
            dt.Load(daTEC.CheckTravelIteneraryTime(TravelAuthorizationNumber));
        }
        public void GetTECItineraryByTravelAuthorizationNumber(ref DataTable dt, string TravelAuthorizationNumber)
        {
            dt.Load(daTEC.GetTECItineraryByTravelAuthorizationNumber(TravelAuthorizationNumber));
        }

        public void GetTECExpenditureByTravelAuthorizationNumber(ref DataTable dt, string TravelAuthorizationNumber)
        {
            dt.Load(daTEC.GetTECExpenditureByTravelAuthorizationNumber(TravelAuthorizationNumber));
        }
        public void GetTECAdvancesByTravelAuthorizationNumber(ref DataTable dt, string TravelAuthorizationNumber)
        {
            dt.Load(daTEC.GetTECAdvancesByTravelAuthorizationNumber(TravelAuthorizationNumber));
        }
        public void GetTECItineraryExchangeRateByTravelAuthorizationNumber(ref DataTable dt, string TravelAuthorizationNumber)
        {
            dt.Load(daTEC.GetTECItineraryExchangeRateByTravelAuthorizationNumber(TravelAuthorizationNumber));
        }

        public void UpdateTECExpenditure(string TravelAuthorizationID, bool ExpenditureNotApplicable)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.UpdateTECExpenditure(TravelAuthorizationID,ExpenditureNotApplicable, ui.User_Id);
        }
        public void UpdateTECAdvances(string TravelAuthorizationID, bool AdvancesNotApplicable)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTEC.UpdateTECAdvances(TravelAuthorizationID, AdvancesNotApplicable, ui.User_Id);
        }
    }
}
