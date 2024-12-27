namespace astute.CoreModel
{
    public static class CoreCommonMessage
    {
        public static string DataSuccessfullyFound = "Data successfully found.";
        public static string DataNotFound = " Data not found.";
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
        public static string EmailSendFromDefaultSuccessMessage = "User mail not found. Do you want to send mail with default Email.";
        public static string OldPasswordNotMatched = "Your old password does not match!";
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
        public static string CustomerDetailSavedSuccessfully = "Customer detail saved successfully.";
        public static string CustomerDetailRequired = "Must require API/FTP/URL Data.";
        public static string InvalidDate = "From Date should not be greater than To Date";
        public static string ApiFailed = "Failed to retrieve data from the API";
        public static string ApiError = "Error connecting to the API";
        public static string JobExc = "Job executed successfully.";
        public static string StkUpdate = "Data Update successfully.";
        public static string DataTransfer = "Data transferred successfully.";

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
        public static string MenuMasterExists = MenuMaster + AlreadyExists;
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
        public static string EmployeeStockCreated = "Employee stock saved successfully.";
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
        public static string ReportRolesDeleted = "Report roles " + DeletedSuccessfully;
        public static string SearchedReportCreated = "Search save has been added successfully.";
        public static string SearchedReportUpdated = "Search save has been updated successfully.";
        public static string SearchedReportDeleted = "Search save has been deleted successfully.";
        public static string SearchedReportNameExist = "Search save name" + AlreadyExists;
        public static string LayoutReportCreated = "Report layout has been added successfully.";
        public static string LayoutReportUpdated = "Report layout has been updated successfully.";
        public static string LayoutReportDeleted = "Report layout has been deleted successfully.";
        public static string LayoutReportNameExist = "Report layout name" + AlreadyExists;
        public static string ReportRolesSaveLayout = "Report roles layout saved successfully.";

        #endregion

        #region Cart/Review/Approval Management
        public static string CartAdded = "Stone has been added to cart successfully.";
        public static string CartUpdateValidityDate = "Stone validity date changed successfully.";
        public static string CartExists = "Stone alredy exists in to cart.";
        public static string ReviewAdded = "Stone has been added to review successfully.";
        public static string StockApproved = "Stone has been send to management successfully for the approval.";
        public static string StockOrderProcessing = "Stone has been send to order processing successfully.";
        public static string CartStockDeleted = "Stone hase been deleted from the cart successfully.";
        public static string ApprovalStockDeleted = "Stone hase been deleted from the approval list successfully.";
        public static string StokeApprovedByManagement = "Stone has been approved successfully.";
        public static string StokeRejectedByManagement = "Stone has been rejected successfully.";
        public static string StockStatusManagement = "Stone Status has been changed successfully.";
        public static string OrderStatusManagement = "Order Status has been changed successfully.";
        public static string OrderInactive = "Order has been inactive successfully.";
        public static string OrderProcessingFinalUpdate = "Order Processing Final" + UpdatedSuccessfully;
        #endregion

        #region Account Group
        public static string AccountGroup = "Account group";
        public static string AccountGroupCreated = AccountGroup + AddedSuccessfully;
        public static string AccountGroupUpdated = AccountGroup + UpdatedSuccessfully;
        public static string AccountGroupDeleted = AccountGroup + DeletedSuccessfully;
        public static string AccountGroupAlreadyExist = "Account group name" + AlreadyExists;
        #endregion

        #region Account Master
        public static string AccountMaster = "Account master";
        public static string AccountMasterCreated = AccountMaster + AddedSuccessfully;
        public static string AccountMasterUpdated = AccountMaster + UpdatedSuccessfully;
        public static string AccountMasterDeleted = AccountMaster + DeletedSuccessfully;
        #endregion

        #region First Voucher No
        public static string TransMaster = "First voucher no";
        public static string TransMasterCreated = TransMaster + AddedSuccessfully;
        public static string TransMasterUpdated = TransMaster + UpdatedSuccessfully;
        public static string TransMasterDeleted = TransMaster + DeletedSuccessfully;
        public static string TransExistsMaster = "Trans type already exists.";
        public static string TransExistsOMaster = " Other records have been";
        public static string TransExistsMasterOCreated = TransExistsMaster + TransExistsOMaster + AddedSuccessfully;
        public static string TransExistsMasterOUpdated = TransExistsMaster + TransExistsOMaster + UpdatedSuccessfully;
        public static string TransExistsPrefixMaster = "Prefix already exists.";
        public static string TransExistsTransPrefixMaster = "Trans type & Prefix already exists.";
        public static string TransExistsPrefixOMasterCreated = TransExistsPrefixMaster + TransExistsOMaster + AddedSuccessfully;
        public static string TransExistsPrefixOMasterUpdated = TransExistsPrefixMaster + TransExistsOMaster + UpdatedSuccessfully;
        public static string TransExistsTransPrefixMasterCreated = TransExistsTransPrefixMaster;
        public static string TransExistsTransPrefixMasterUpdated = TransExistsTransPrefixMaster;
        public static string TransExistsTransPrefixOMasterCreated = TransExistsTransPrefixMaster + TransExistsOMaster+ AddedSuccessfully;
        public static string TransExistsTransPrefixOMasterUpdated = TransExistsTransPrefixMaster + TransExistsOMaster + UpdatedSuccessfully;

        #endregion

        #region Oracle 
        public static string Fortune_Discount = "Fortune Discount Oracle";
        public static string Fortune_Discount_Added = "Fortune Discount Oracle" + AddedSuccessfully;
        public static string Fortune_Party = "Fortune Party Oracle";
        public static string Fortune_Party_Added = "Fortune Party Oracle" + AddedSuccessfully;
        public static string Fortune_Party_Master = "Fortune Party Master Oracle";
        public static string Fortune_Party_Master_Added = "Fortune Party Master Oracle" + AddedSuccessfully;
        public static string Fortune_Purchase_Disc = "Purchase Disc " + ApiFailed;
        public static string Fortune_Sale_Disc = "Sale Disc " + ApiFailed;
        public static string Fortune_Stock_Disc = "Stock Disc " + ApiFailed;
        public static string Fortune_Sale_Disc_Kts = "Sale Disc KTS " + ApiFailed;
        public static string Fortune_Stock_Disc_Kts = "Stock Disc KTS " + ApiFailed;
        public static string Oracle_Notification = "Notification get from oracle successfully";


        #endregion

        #region Account Trans Master

        public static string FirstAddFirstVoucherNo = "First voucher no not found.";
        public static string AccountCashbookMaster = "Cash book";
        public static string AccountCashbookMasterCreated = AccountCashbookMaster + AddedSuccessfully;
        public static string AccountCashbookMasterUpdated = AccountCashbookMaster + UpdatedSuccessfully;
        public static string AccountCashbookMasterDeleted = AccountCashbookMaster + DeletedSuccessfully;
        public static string AccountCashbookMasterDataNotFound = AccountCashbookMaster + DataNotFound;

        public static string AccountBankbookMaster = "Bank book";
        public static string AccountBankbookMasterCreated = AccountBankbookMaster + AddedSuccessfully;
        public static string AccountBankbookMasterUpdated = AccountBankbookMaster + UpdatedSuccessfully;
        public static string AccountBankbookMasterDeleted = AccountBankbookMaster + DeletedSuccessfully;
        public static string AccountBankbookMasterDataNotFound = AccountBankbookMaster + DataNotFound;

        public static string AccountJvMaster = "Jounral Voucher";
        public static string AccountJvMasterCreated = AccountJvMaster + AddedSuccessfully;
        public static string AccountJvMasterUpdated = AccountJvMaster + UpdatedSuccessfully;
        public static string AccountJvMasterDeleted = AccountJvMaster + DeletedSuccessfully;
        public static string AccountJvMasterDataNotFound = AccountJvMaster + DataNotFound;

        public static string AccountContraMaster = "Contra";
        public static string AccountContraMasterCreated = AccountContraMaster + AddedSuccessfully;
        public static string AccountContraMasterUpdated = AccountContraMaster + UpdatedSuccessfully;
        public static string AccountContraMasterDeleted = AccountContraMaster + DeletedSuccessfully;
        public static string AccountContraMasterDataNotFound = AccountContraMaster + DataNotFound;

        public static string AccountPurchaseMaster = "Purchase";
        public static string AccountPurchaseMasterCreated = AccountPurchaseMaster + AddedSuccessfully;
        public static string AccountPurchaseMasterUpdated = AccountPurchaseMaster + UpdatedSuccessfully;
        public static string AccountPurchaseMasterDeleted = AccountPurchaseMaster + DeletedSuccessfully;
        public static string AccountPurchaseMasterDataNotFound = AccountPurchaseMaster + DataNotFound;
        #endregion

        #region Notification
        public static string Notification_Updated_Read_By = "Notification has been read by user" + UpdatedSuccessfully;
        #endregion

        #region Import Master
        public static string ImportMaster = "Import Master";
        public static string ImportMasterCreated = ImportMaster + AddedSuccessfully;
        public static string ImportMasterUpdated = ImportMaster + UpdatedSuccessfully;
        public static string ImportMasterDeleted = ImportMaster + DeletedSuccessfully;
        public static string ImportMasterDataNotFound = ImportMaster + DataNotFound;
        public static string ImportMasterExists = ImportMaster + AlreadyExists;
        public static string DeleteImportDetail = "Delete Import Detail records first.";
        #endregion

        #region Import Detail
        public static string ImportDetail = "Import Detail";
        public static string ImportDetailCreated = ImportDetail + AddedSuccessfully;
        public static string ImportDetailUpdated = ImportDetail + UpdatedSuccessfully;
        public static string ImportDetailDeleted = ImportDetail + DeletedSuccessfully;
        public static string ImportDetailDataNotFound = ImportDetail + DataNotFound;
        public static string ImportDetailExists = ImportDetail + AlreadyExists;
        #endregion

        #region Import Excel
        public static string ImportExcel = "Import Excel";
        public static string ImportExcelCreated = ImportExcel + AddedSuccessfully;
        public static string ImportExcelUpdated = ImportExcel + UpdatedSuccessfully;
        public static string ImportExcelDeleted = ImportExcel + DeletedSuccessfully;
        public static string ImportExcelDataNotFound = ImportExcel + DataNotFound;
        public static string ImportExcelExists = ImportExcel + AlreadyExists;
        #endregion

        #region Terms Trans Det
        public static string TermsTransDet = "Terms Trans Det";
        public static string TermsTransDetCreated = TermsTransDet + AddedSuccessfully;
        public static string TermsTransDetUpdated = TermsTransDet + UpdatedSuccessfully;
        public static string TermsTransDetDeleted = TermsTransDet + DeletedSuccessfully;
        public static string IsExistTermsTransDet = TermsTransDet + AlreadyExists;
        #endregion

        #region Parcel Master
        public static string ParcelMaster = "Parcel Master";
        public static string ParcelMasterCreated = ParcelMaster + AddedSuccessfully;
        public static string ParcelMasterUpdated = ParcelMaster + UpdatedSuccessfully;
        public static string ParcelMasterDeleted = ParcelMaster + DeletedSuccessfully;
        public static string IsExistParcelMaster = ParcelMaster + AlreadyExists;
        #endregion

        #region Inward details
        public static string InwardDetails = "Inward Details";
        public static string FileNotFound = "File not found";
        public static string InvalidFileFormat = "Please upload a valid Excel file.";
        #endregion

        #region Hold
        public static string Transaction = "Transaction details";
        public static string TransactionCreated = Transaction + AddedSuccessfully;
        public static string TransactionUpdated = Transaction + UpdatedSuccessfully;
        public static string TransactionDeleted = Transaction + DeletedSuccessfully;
        public static string TransactionlMaster = Transaction + AlreadyExists;
        #endregion

        #region SupplierPriceUpdate
        public static string SupplierPriceUpdate = "Supplier Price Update successfully.";
        public static string SupplierPriceUpdateCheck = "Please check Price Update.";
        #endregion

        #region Party Url Format
        public static string Party_Url_Format = "Party Url Format";
        public static string Party_Url_Format_Created = Party_Url_Format + AddedSuccessfully;
        public static string Party_Url_Format_Updated = Party_Url_Format + UpdatedSuccessfully;
        public static string Party_Url_Format_Deleted = Party_Url_Format + DeletedSuccessfully;
        public static string Party_Url_Format_Master = Party_Url_Format + AlreadyExists;
        #endregion

        #region Latest Stock
        public static string Supplier_Id_Less_Than_0 = "Supplier Id cannot be less than 0.";
        public static string No_stock_uploaded_in_last = "No stock uploaded in last 48 hours";
        #endregion

        #region Supplier Stock Upload Status Email
        public static string Supplier_Stock_Upload_Status_Email = "Supplier Stock Upload Status Email";
        #endregion

        #region Employee Download Share Rights
        public static string EmployeeDownloadShareRights = "Employee Download Share Rights";
        public static string EmployeeDownloadShareRightsCreated = EmployeeDownloadShareRights + AddedSuccessfully;
        #endregion

        #region Lab Entry
        public static string Lab_Entry = "Lab Entry";
        public static string Lab_Entry_Created = Lab_Entry + AddedSuccessfully;
        public static string Lab_Entry_Updated = Lab_Entry + UpdatedSuccessfully;
        public static string Lab_Entry_Deleted = Lab_Entry + DeletedSuccessfully;
        public static string Lab_Entry_Auto_Deleted = "Cannot delete auto generated " + Lab_Entry;
        public static string Lab_Entry_Master = Lab_Entry + AlreadyExists;
        #endregion
    }
}
