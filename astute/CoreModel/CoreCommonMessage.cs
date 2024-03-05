using Newtonsoft.Json.Serialization;
using System;

namespace astute.CoreModel
{
    public static class CoreCommonMessage
    {
        public static string DataSuccessfullyFound = "Data successfully found.";
        public static string DataNotFound = "Data not found.";
        public static string AddedSuccessfully = " added successfully.";
        public static string CopySuccessfully = " copied successfully.";
        public static string UpdatedSuccessfully = " updated successfully.";
        public static string DeletedSuccessfully = " deleted successfully.";
        public static string AlreadyExists = " already exists.";
        public static string ParameterMismatched = "Parameter mismatched!";
        public static string InteralServerError = "Internal server error.";
        public static string UnauthorizedUser = "User is unauthorized";
        public static string ForgetPasswordSuccessMessage = "Email successfully sent on your registered email address.";
        public static string EmailSendSuccessMessage = "Email successfully sent.";
        public static string OldPasswordNotMatched = "Your old password dose not match!";
        public static string PasswordChengedSuccessMessage = "Password changed successfully.";
        public static string LayoutAddedSuccessMessage = "Layout added successfully.";
        public static string LayoutUpdatedSuccessMessage = "Layout updated successfully.";
        public static string LayoutDeletedSuccessMessage = "Layout deleted successfully.";
        public static string LayoutAlreadyExist = "Layout already exist with the same user menu.";
        public static string StatusChangedSuccessMessage = "Status successfully changed.";
        public static string OrderNoAlreadyExist = "Order no already exist.";
        public static string SortNoAlreadyExist = "Sort no already exist.";
        public static string ReferenceFoundError = "Reference found, you can not delete this record.";
        public static string SupplierDetailSavedSuccessfully = "Supplier detail saved successfully.";
        public static string UserRegisteredSuccessfully = "User registered successfully";
        public static string UserUpdatedSuccessfully = "User updated successfully";
        public static string UserNameAlreadyExist = "User name already exist";
        public static string StockUploadedSuccessfully = "Stock uploaded successfully";
        public static string FileDownloadSuccessfully = "File downloaded successfully";

        #region Email Subjects
        public static string ForgetPasswordSubject = "Forget Password Email";
        public static string StoneSelectionSubject = "Stone Selection";
        #endregion

        #region Employee Master
        public static string EmployeeMaster = "Employee";
        public static string EmployeeMasterCreated = EmployeeMaster + AddedSuccessfully;
        public static string EmployeeMasterUpdated = EmployeeMaster + UpdatedSuccessfully;
        public static string EmployeeMasterDeleted = EmployeeMaster + DeletedSuccessfully;
        public static string EmployeeExists = "User Name" + AlreadyExists;
        #endregion

        #region Category Master
        public static string CategoryMaster = "Category";
        public static string CategoryMasterCreated = CategoryMaster + AddedSuccessfully;
        public static string CategoryMasterUpdated = CategoryMaster + UpdatedSuccessfully;
        public static string CategoryMasterDeleted = CategoryMaster + DeletedSuccessfully;
        public static string CategoryExists = CategoryMaster + AlreadyExists;
        #endregion

        #region Category Value
        public static string CategoryValue = "Category value";
        public static string CategoryValueCreated = CategoryValue + AddedSuccessfully;
        public static string CategoryValueUpdated = CategoryValue + UpdatedSuccessfully;
        public static string CategoryValueDeleted = CategoryValue + DeletedSuccessfully;
        public static string CategoryValueExists = CategoryValue + AlreadyExists;
        public static string CategoryValueOrderNoExists = "Order no" + AlreadyExists;
        public static string CategoryValueSortNoExists = "Sort no" + AlreadyExists;
        #endregion

        #region Supplier Value
        public static string SupplierValue = "Supplier value";
        public static string SupplierValueCreated = SupplierValue + AddedSuccessfully;
        public static string SupplierValueUpdated = SupplierValue + UpdatedSuccessfully;
        public static string SupplierValueDeleted = SupplierValue + DeletedSuccessfully;
        #endregion

        #region Country
        public static string Country = "Country";
        public static string CountryCreated = Country + AddedSuccessfully;
        public static string CountryUpdated = Country + UpdatedSuccessfully;
        public static string CountryDeleted = Country + DeletedSuccessfully;
        public static string CountryExists = Country + AlreadyExists;
        #endregion

        #region State
        public static string State = "State";
        public static string StateCreated = State + AddedSuccessfully;
        public static string StateUpdated = State + UpdatedSuccessfully;
        public static string StateDeleted = State + DeletedSuccessfully;
        public static string StateExists = State + AlreadyExists;
        #endregion

        #region City
        public static string City = "City";
        public static string CityCreated = City + AddedSuccessfully;
        public static string CityUpdated = City + UpdatedSuccessfully;
        public static string CityDeleted = City + DeletedSuccessfully;
        #endregion

        #region Terms
        public static string Terms = "Terms";
        public static string TermsCreated = Terms + AddedSuccessfully;
        public static string TermsUpdated = Terms + UpdatedSuccessfully;
        public static string TermsDeleted = Terms + DeletedSuccessfully;
        public static string IsExistTerms = Terms + AlreadyExists;
        #endregion

        #region Proccess Master
        public static string ProccessMaster = "Proccess master";
        public static string ProccessMasterCreated = ProccessMaster + AddedSuccessfully;
        public static string ProccessMasterUpdated = ProccessMaster + UpdatedSuccessfully;
        public static string ProccessMasterDeleted = ProccessMaster + DeletedSuccessfully;
        #endregion

        #region Currency
        public static string Currency = "Currency";
        public static string CurrencyCreated = Currency + AddedSuccessfully;
        public static string CurrencyUpdated = Currency + UpdatedSuccessfully;
        public static string CurrencyDeleted = Currency + DeletedSuccessfully;
        public static string CurrencyExists = Currency + AlreadyExists;
        #endregion

        #region BGM
        public static string BGM = "BGM";
        public static string BGMCreated = "BGM saved successfully.";
        public static string BGMUpdated = BGM + UpdatedSuccessfully;
        public static string BGMDeleted = BGM + DeletedSuccessfully;
        public static string IsExistBGM = BGM + AlreadyExists;
        public static string IsExistShade_Milky = "Shade and milky" + AlreadyExists;
        #endregion

        #region Menu Master
        public static string MenuMaster = "Menu";
        public static string MenuMasterCreated = MenuMaster + AddedSuccessfully;
        public static string MenuMasterUpdated = MenuMaster + UpdatedSuccessfully;
        public static string MenuMasterDeleted = MenuMaster + DeletedSuccessfully;
        public static string MenuMasterExists  = MenuMaster + AlreadyExists;
        #endregion

        #region Bank Master
        public static string BankMaster = "Bank";
        public static string BankMasterCreated = BankMaster + AddedSuccessfully;
        public static string BankMasterUpdated = BankMaster + UpdatedSuccessfully;
        public static string BankMasterDeleted = BankMaster + DeletedSuccessfully;
        #endregion

        #region Invoice Remarks
        public static string InvoiceRemarks = "Invoice remarks";
        public static string InvoiceRemarksCreated = InvoiceRemarks + AddedSuccessfully;
        public static string InvoiceRemarksUpdated = InvoiceRemarks + UpdatedSuccessfully;
        public static string InvoiceRemarksDeleted = InvoiceRemarks + DeletedSuccessfully;
        #endregion

        #region Employee Rights
        public static string EmployeeRights = "Employee Rights";
        public static string EmployeeRightsCreated = EmployeeRights + AddedSuccessfully;
        public static string EmployeeRightsCopy = EmployeeRights + CopySuccessfully;
        #endregion

        #region Holiday Master
        public static string HolidayMaster = "Holiday";
        public static string HolidayMasterCreated = HolidayMaster + AddedSuccessfully;
        public static string HolidayMasterUpdated = HolidayMaster + UpdatedSuccessfully;
        public static string HolidayMasterDeleted = HolidayMaster + DeletedSuccessfully;
        #endregion

        #region Year Master
        public static string YearMaster = "Year";
        public static string YearMasterCreated = YearMaster + AddedSuccessfully;
        public static string YearMasterUpdated = YearMaster + UpdatedSuccessfully;
        public static string YearMasterDeleted = YearMaster + DeletedSuccessfully;
        #endregion

        #region Quote Master
        public static string QuoteMaster = "Quote";
        public static string QuoteMasterCreated = QuoteMaster + AddedSuccessfully;
        public static string QuoteMasterUpdated = QuoteMaster + UpdatedSuccessfully;
        public static string QuoteMasterDeleted = QuoteMaster + DeletedSuccessfully;
        #endregion

        #region Employee Mail
        public static string EmployeeMail = "Employee mail";
        public static string EmployeeMailCreated = EmployeeMail + AddedSuccessfully;
        public static string EmployeeMailUpdated = EmployeeMail + UpdatedSuccessfully;
        public static string EmployeeMailDeleted = EmployeeMail + DeletedSuccessfully;
        #endregion

        #region Rapaport Master
        public static string RapaportMaster = "Rapaport";
        public static string RapaportMasterCreated = RapaportMaster + AddedSuccessfully;
        public static string RapaportMasterUpdated = RapaportMaster + UpdatedSuccessfully;
        public static string RapaportMasterDeleted = RapaportMaster + DeletedSuccessfully;
        #endregion

        #region Rapaport Detail
        public static string RapaportDetail = "Rapaport detail";
        public static string RapaportDetailCreated = RapaportDetail + AddedSuccessfully;
        public static string RapaportDetailUpdated = RapaportDetail + UpdatedSuccessfully;
        public static string RapaportDetailDeleted = RapaportDetail + DeletedSuccessfully;
        public static string WeightErrorMessage = "To weight must be greater than from weight.";
        #endregion

        #region Rapaport User
        public static string RapaportUser = "Rapaport user";
        public static string RapaportUserCreated = RapaportUser + AddedSuccessfully;
        public static string RapaportUserUpdated = RapaportUser + UpdatedSuccessfully;
        public static string RapaportUserDeleted = RapaportUser + DeletedSuccessfully;
        #endregion

        #region Pointer Master
        public static string PointerMaster = "Pointer";
        public static string PointerMasterCreated = PointerMaster + AddedSuccessfully;
        public static string PointerMasterUpdated = PointerMaster + UpdatedSuccessfully;
        public static string PointerMasterDeleted = PointerMaster + DeletedSuccessfully;
        public static string PointerMasterAlreadyExist = "Pointer Name" + AlreadyExists;
        #endregion

        #region Pointer Detail
        public static string PointerDetail = "Pointer detail";
        public static string PointerDetailCreated = PointerDetail + AddedSuccessfully;
        public static string PointerDetailUpdated = PointerDetail + UpdatedSuccessfully;
        public static string PointerDetailDeleted = PointerDetail + DeletedSuccessfully;
        #endregion

        #region Party Master
        public static string PartyMaster = "Party";
        public static string PartyMasterCreated = PartyMaster + " has been added successfully";
        public static string PartyMasterUpdated = PartyMaster + " has been updated successfully";
        public static string PartyMasterDeleted = PartyMaster + " has been deleted successfully";
        public static string PartyCodeAlreadyExist = "Party code" + AlreadyExists;
        public static string PartyAlreadyExist = PartyMaster + AlreadyExists;
        #endregion

        #region Party Contact
        public static string PartyContact = "Party contact";
        public static string PartyContactCreated = PartyContact + AddedSuccessfully;
        public static string PartyContactUpdated = PartyContact + UpdatedSuccessfully;
        public static string PartyContactDeleted = PartyContact + DeletedSuccessfully;
        #endregion

        #region Party Bank
        public static string PartyBank = "Party bank";
        public static string PartyBankCreated = PartyBank + AddedSuccessfully;
        public static string PartyBankUpdated = PartyBank + UpdatedSuccessfully;
        public static string PartyBankDeleted = PartyBank + DeletedSuccessfully;
        #endregion

        #region Party Shipping
        public static string PartyShipping = "Party shipping";
        public static string PartyShippingCreated = PartyShipping + AddedSuccessfully;
        public static string PartyShippingUpdated = PartyShipping + UpdatedSuccessfully;
        public static string PartyShippingDeleted = PartyShipping + DeletedSuccessfully;
        #endregion

        #region Party Document
        public static string PartyDocument = "Party document";
        public static string PartyDocumentCreated = PartyDocument + AddedSuccessfully;
        public static string PartyDocumentUpdated = PartyDocument + UpdatedSuccessfully;
        public static string PartyDocumentDeleted = PartyDocument + DeletedSuccessfully;
        #endregion

        #region Party Assist
        public static string PartyAssist = "Party assist";
        public static string PartyAssistCreated = PartyAssist + AddedSuccessfully;
        public static string PartyAssistUpdated = PartyAssist + UpdatedSuccessfully;
        public static string PartyAssistDeleted = PartyAssist + DeletedSuccessfully;
        public static string PartyAssistPercentageError = "First and Second Assist Percentage should equal to 100!";
        #endregion

        #region Terms & Conditions
        public static string TermsAndConditions = "Terms & Conditions";
        public static string TermsAndConditionsCreated = TermsAndConditions + AddedSuccessfully;
        public static string TermsAndConditionsUpdated = TermsAndConditions + UpdatedSuccessfully;
        public static string TermsAndConditionsDeleted = TermsAndConditions + DeletedSuccessfully;
        #endregion

        #region Loader Master
        public static string LoaderMaster = "Loader";
        public static string LoaderMasterCreated = LoaderMaster + AddedSuccessfully;
        public static string LoaderMasterDeleted = LoaderMaster + DeletedSuccessfully;
        public static string SetDefaultLoader = "Set default loader successfully.";
        #endregion

        #region Exchange Rate
        public static string ExchangeRate = "Exchange rate";
        public static string ExchangeRateCreated = "Exchange rate saved successfully.";
        public static string ExchangeRateUpdated = ExchangeRate + DeletedSuccessfully;
        public static string ExchangeRateDeleted = ExchangeRate + DeletedSuccessfully;
        #endregion

        #region Value Config
        public static string ValueConfig = "Value config";
        public static string ValueConfigCreated = ValueConfig + AddedSuccessfully;
        public static string ValueConfigUpdated = ValueConfig + UpdatedSuccessfully;
        public static string ValueConfigDeleted = ValueConfig + DeletedSuccessfully;
        #endregion

        #region Supplier Pricing
        public static string SupplierPricing = "Supplier pricing";
        public static string SupplierPricingCreated = SupplierPricing + " saved successfully.";
        public static string SupplierStockCreated = "Supplier stock saved successfully.";
        public static string SunrisePricingCreated = "Sunrise pricing saved successfully.";
        public static string CustomerPricingCreated = "Customer pricing saved successfully.";
        public static string SupplierPricingUpdated = SupplierPricing + UpdatedSuccessfully;
        public static string SupplierPricingDeleted = SupplierPricing + DeletedSuccessfully;
        public static string SunrisePricingDeleted = "Sunrise pricing" + DeletedSuccessfully;
        public static string CustomerPricingDeleted = "Customer pricing" + DeletedSuccessfully;
        public static string SupplierPricingIsExists = "Please check records exists in supplier pricing or stock!";
        #endregion

        #region Stock Number Generation
        public static string StockNumber = "Stock number";
        public static string StockNumberCreated = StockNumber + AddedSuccessfully;
        public static string StockNumberUpdated = StockNumber + UpdatedSuccessfully;
        public static string StockNumberDeleted = StockNumber + DeletedSuccessfully;
        public static string StockNumberAlreadyExistLab = "Front prefix already exists on lab";
        public static string StockNumberAlreadyExistOverses = "Front prefix already exists on overseas";
        public static string StockNumberAlreadyExistSunrise = "Front prefix already exists on sunrise";
        #endregion

        #region Temp Layout
        public static string TempLayout = "Temp layout";
        public static string TempLayoutCreated = TempLayout + AddedSuccessfully;
        public static string TempLayoutUpdated = TempLayout + UpdatedSuccessfully;
        public static string TempLayoutDeleted = TempLayout + DeletedSuccessfully;
        public static string TempLayoutStatusUpdate = "Layout status updated.";
        #endregion

        #region Report
        public static string ReportMaster = "Report Master";
        public static string ReportDetail = "Report Detail";
        public static string ReportNameExist = "Report Name" + AlreadyExists;
        public static string ReportMasterCreated = ReportMaster + AddedSuccessfully;
        public static string ReportMasterUpdated = ReportMaster + UpdatedSuccessfully;
        public static string ReportMasterDeleted = ReportMaster + DeletedSuccessfully;
        public static string ReportDetailCreated = ReportDetail + AddedSuccessfully;
        public static string ReportDetailUpdated = ReportDetail + UpdatedSuccessfully;
        public static string ReportDetailDeleted = ReportDetail + DeletedSuccessfully;
        public static string ReportRoles = "Report roles apply successfully.";
        public static string SearchedReportCreated = "Search save has been added successfully.";
        public static string SearchedReportUpdated = "Search save has been updated successfully.";
        public static string SearchedReportDeleted = "Search save has been deleted successfully.";
        public static string SearchedReportNameExist = "Search save name" + AlreadyExists;
        public static string LayoutReportCreated = "Report layout has been added successfully.";
        public static string LayoutReportUpdated = "Report layout has been updated successfully.";
        public static string LayoutReportDeleted = "Report layout has been deleted successfully.";
        public static string LayoutReportNameExist = "Report layout name"+ AlreadyExists;
        public static string ReportRolesSaveLayout = "Report roles layout saved successfully.";

        #endregion

        #region Cart/Review/Approval Management
        public static string CartAdded = "Stone has been added to cart successfully.";
        public static string CartExists = "Stone alredy exists in to cart.";
        public static string ReviewAdded = "Stone has been added to review successfully.";
        public static string StockApproved = "Stone has been send to management successfully for the approval.";
        public static string StockOrderProcessing= "Stone has been send to order processing successfully.";
        public static string CartStockDeleted = "Stone hase been deleted from the cart successfully.";
        public static string ApprovalStockDeleted = "Stone hase been deleted from the approval list successfully.";
        public static string StokeApprovedByManagement = "Stone has been approved successfully.";
        public static string StokeRejectedByManagement = "Stone has been rejected successfully.";
        public static string OrderInactive = "Order has been inactive successfully.";
        #endregion
    }
}
