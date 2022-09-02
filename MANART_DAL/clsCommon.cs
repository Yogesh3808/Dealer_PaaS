using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using ASPnetControls;
using System.Data.OleDb;

namespace MANART_DAL
{
    /// <summary>
    /// Summary description for clsCommon Data
    /// </summary>
    public class clsCommon : System.Web.UI.Page
    {
        public enum ComboQueryType
        {
            CountryAll = 1,
            CountryActive = 2,
            CountryForRegion = 3,
            PaymentTerms = 4,
            INCOTerms = 5,
            ModeofDispatch = 6,
            PortofDischargeAll = 7,
            PortofDischargeForCountry = 8,
            Region = 9,
            AllModels = 10,
            ModelsOfCountry = 11,
            ModelBodyType = 12,
            DealerAll = 13,
            DealerForCountry = 14,
            StateAll = 15,
            StateForRegion = 16,
            LCClause = 17,
            ModelCodeOfCountry = 18,//i.e FERT Code
            RegionUserWise = 19,
            CountryUserWise = 20,
            StateUserWise = 21,
            DealerForState = 22,
            ShipmentDays = 23,
            ValidityDays = 24,
            ModalDestinationForCountry = 25,
            ProformaNoDealerWise = 26,
            ShippingUnder = 27,
            PortOfLoading = 28,
            ContainerisationLoc = 29,
            ClearingAgents = 30,
            InspectionAgency = 31,
            NominatedAgency = 32,
            ThirdPartyInspectionAgency = 33,
            ReasonForProforma = 34,
            BillOfLading = 35,
            BillOfLadingDischarge = 36,
            BillOfLadingFinalDest = 37,
            ReasonForPreshipment = 38,
            PostShipmentInvNoForVehicleIn = 39,
            ExportPoType = 40,
            ExportClaimType = 41,
            ChassisOfDealer = 42,
            RouteType = 43,
            CustomerComplaints = 44,
            DealerInvistigation = 45,
            JobCode = 46,
            CulpritCode = 47,
            DefectCode = 48,
            TechnicalCode = 49,
            ModelOfDealer = 50,
            Unit = 51,
            DealerLubricant = 52,
            DealerSublet = 53,
            UnitDecimal = 54,
            UserType = 55,
            UserLevel = 56,
            Department = 57,
            RegionUserTypeWise = 58,
            WarrantyClaims = 59,
            RightsAll = 60,
            ClaimSlabAll = 61,
            AllDealerOfCountry = 62,
            AllDealerOfState = 63,
            DepartmentForActivity = 64,
            TypeOfActivity = 65,
            Activity = 66,
            ClaimReason = 67,
            LubricantType = 68,
            LubricantData = 69,
            PartClaimType = 70,
            DepotCode = 71,
            YearCode = 72,
            MonthCode = 73,
            AllModelCode = 74,
            ActivityName = 75,
            CustomerType = 76,
            IndustryType = 77,
            DriveType = 78,
            LoadType = 79,
            PrimaryApplication = 80,
            SecondaryApplication = 81,
            RoadType = 82,
            Financier = 83,
            ReportList = 84,
            CSMList = 85,
            CredPostKey = 86,
            SpecialGL = 87,
            ETBPostingKey = 88,
            CostCenter = 89,
            Account = 90,
            PreshipmentInvoice = 91,
            PostshipmentInvoice = 92,
            MPGDetails = 93,
            ModelCategory = 94,
            BankStateMent = 95,
            //sujata 25012011
            PartClaimRejectionReason = 96,
            //sujata 25012011
            //sujata 28012011
            InvoiceNoForPRN = 97,
            //sujata 28012011
            //Sujata 01022011
            MRNRejectionReason = 98,
            //Sujata 01022011      
            //Sujata 15022011      
            AllModelCategory = 99,
            //Sujata 15022011      
            //Sujata 28022011
            DepoWithCode = 100,
            ModelPlant = 101,
            //Sujata 28022011
            ClaimType = 102,
            DOMPOType = 103,
            //Sujata 20042011
            ReportOption = 104,
            //Sujata 20042011
            //Sujata 24082011
            DealerType = 105,
            //Sujata 24082011
            PartClaimErrorCode = 106,
            //Sujata 12122011
            PartScheme = 107,
            //Sujata 12122011
            PartCategory = 108,
            DealerUser = 109,
            //Megha 11062012
            WarrantyRole = 110,
            //Megha 11062012
            //Megha 22102012 -- Tax Master
            TaxType = 111,
            TaxApplicable = 112,
            ADDSalesTax1 = 113,
            ADDSalesTax2 = 114,
            //Megha 22102012
            ModelFOBRateOfCountry = 115,//i.e FERT Rate
            ModelCNFRateOfCountry = 116,//i.e FERT Rate
            ModelCIFRateOfCountry = 117//i.e FERT Rate
                //BranchReportchnages Added by megha
          ,
            MDDealerUser = 118
                //BranchReportchnages Added by megha
                ,
            DealerSubletCode = 119
                ,
            UsanceDays = 120
               ,
            ModelCategoryBasic = 121
                ,
            EGPCustpmer = 122
                ,
            EGPMainTax = 123
                ,
            EGPMainTaxPer = 124
                ,
            EGPAdditionalTax1 = 125
                ,
            EGPAdditionalTax1Per = 126
                ,
            EGPAdditionalTax2 = 127
                ,
            EGPAdditionalTax2Per = 128
                ,
            EGPMainTaxLSTCST = 129
                ,
            MANTaxType = 130
                ,
            MANTaxcategory = 131
                ,
            MANTaxApplicable = 132

                ,
            MANADDSalesTax1 = 133
                ,
            MANADDSalesTax2 = 134

          ,
            EGPCustpmerType = 135
                ,
            EGPInvType = 136
                ,
            ReportModelCat = 137
                ,
            EGPPartTax1 = 138
                ,
            EGPPartTax2 = 139
                ,
            PartGroup = 140
                ,
            PartGroupTaxType = 141
                ,
            Objective = 142
                ,
            HDTrialVehCondition = 143
                ,
            EGPPartAddTaxAppl = 144
                //vrushali 17102014_Begin dump data combo
                ,
            DUMPDet = 145
                //vrushali 17102014_End
                //Rucha 07012015
                ,
            DumpRegion = 146
                ,
            DealerLocation = 147
                ,
            DealerHOBRList = 148
                ,
            JobcardType = 149
                ,
            BayAllocation = 150
                ,
            VehSaleDealer = 151
                //vrushali19032015_Begin
             ,
            LeadArea = 152,

            Employee = 153
                ,
            InqSource = 154
                ,
            LeadName = 155
                ,
            Competitor = 156
                //vrushali19032015_End
                ,
            CustomerGlobal = 157
                ,
            DelayReason = 158
                //vrushali19032015_Begin
                ,
            LeadObjective = 159
                ,
            Platform = 160
                //vrushali19032015_End
                ,
            Make = 161
                //vrushali19032015_Begin
                ,
            ProsName = 162
                ,
            LeadClose = 163
                ,
            InqClose = 164
                //vrushali19032015_End
                , Dealer_ActionTaken = 165,
            Dealer_ParameterChecked = 166,
            Dealer_FOCReason = 167,
            Dealer_Supplier = 168,
            LubricantCapacity = 169,
            AddLaborDesc = 170,
            //vrushali07062016_Begin On M1 vehicle PO type
            VehPOTypeM1 = 171,
            //vrushali07062016_End
            LaborGroup = 172,
            //VIkram01.08.2016
            MTIFleet = 173,
            VehicleCondition = 174,
            Customer = 175,
            AllocationChassis = 176,
            Customer_Type = 177,
            InvType = 178,
            AllocatedChassis = 179,
            VATTax = 180,
            CSTTax = 181,
            DealerTypeCust = 186,

            DealerSaleChassis = 187,
            EstNo = 182,
            EstNoWithDate = 183,
            TaxTag_I = 184,
            TaxTag_O = 185,
            TaxTag_Per_I = 188,
            TaxTag_Per_O = 189,
            OpenJobcard = 190,
            SecondaryApplication_CRM = 191,
            //BranchTypeCust = 191,
            StkChallanNo = 192,
            VehSRNCust = 193,
            VehPRNSupplier = 194,
            DocType = 195,
            LubricantTypeJobcard = 196,
            ModelNoRate = 197,
            ModelCodeNoRate = 198,
            MTIFeedback = 199,
            MTIFeedbackScale = 200,
            CulpritCodeMTI = 201,
            DefectCodeMTI = 202,
            //GST Changes
            MANTaxType_GST = 203
                ,
            MANTaxcategory_GST = 204
                ,
            MANTaxApplicable_GST = 205

                ,
            MANADDSalesTax1_GST = 206
                ,
            MANADDSalesTax2_GST = 207
                ,
            MANServiceType = 208
            ,
            MANServiceType_GST = 209
                ,
            MANADDSalesTax1_GST_Dealer = 210
               ,
            MANADDSalesTax2_GST_Dealer = 211,
            InvJobDescription = 212,
            CustomerBranchGSTNO = 213,
            GSTOAType = 214,
            ModelNameForJobcard = 215,
            ModelCodeForJobcard = 216,
            JobcardRequisition = 217,
            SourceDlrVehOrderFormMTI = 218,
            ActivityExpensesHead = 219,
            MerchandizeRequirement = 220,
            DistrictList = 221,
            WarehouseList = 222,
            CustomerIncMTI = 223,
            ProformaInvExport = 224
            //GST Changes
        }

        public enum FixedComboType
        {
            ForRFPProecess = 1,
            ForProformaAcceptance = 2,
            ForRFQProcess = 3
        }

        public enum GetDataQueryType
        {
            ToGetCurrency = 1,
            ToGetVehicleDealerCode = 2,
            ToGetSpareDealerCode = 3,
            ToGetUserType = 4,
            ToGetHighValueAmt = 5,
            //Sujata 04022011
            ToGetHrsApplicable = 6,
            //Sujata 04022011
            ToGetHighValueAmtForNepal = 7,
            ToGetHighValueAmtForAllExceptNepal = 8,
            ToGetRemanDealerCode = 9,
            ToGetManufSuppID = 10,              //Manufacture type Supp for VOR/Accidental ORDER on Jobcard.
            // VIkram 18.08.2016
            ToGetDamageClaimAmt = 11,
            ToGetTaxPercentage = 12,
            ToGetTaxApplicableOn = 13,
            ToGetFileSizeLimit = 14,
            ToGetDealerFinYear = 15,
            ToGetUserPCRHeadApprv = 16,
            ToGetUserVehPOApprv = 17
        }

        private string _sUserRole;

        public string sUserRole
        {
            get
            {
                return _sUserRole;
            }
            set
            {
                _sUserRole = value;
            }
        }

        public clsCommon()
        {
            sUserRole = Func.Convert.sConvertToString(Session["UserRole"]);
        }

        #region Control Function


        public bool bConfirmAndSendMail(int DocID, string YesNo, string ConfirmFor, string AdditionalMessage)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_ConfirmAndMailSend", DocID, YesNo, ConfirmFor, AdditionalMessage);
                objDB.CommitTransaction();
                bSaveRecord = true;

                return bSaveRecord;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return bSaveRecord;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }


        public void BindDataToCombo(DropDownList ObjDropDownList, ComboQueryType ValComboQueryType, int iParentId)
        {
            BindDataToCombo(ObjDropDownList, ValComboQueryType, iParentId, "");
        }

        public void BindDataToCombo(DropDownList ObjDropDownList, ComboQueryType ValComboQueryType, int iParentId, string sAdditionalCondition)
        {

            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                DataRow tmpDataRow;
                if (ObjDropDownList == null) return;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetDataForCombo", (int)ValComboQueryType, iParentId, sAdditionalCondition);
                if (dt != null)
                {

                    tmpDataRow = dt.NewRow();
                    tmpDataRow["ID"] = 0;
                    tmpDataRow["Name"] = "--Select--";
                    dt.Rows.InsertAt(tmpDataRow, 0);

                    ObjDropDownList.DataSource = dt;
                    ObjDropDownList.DataValueField = "ID";
                    ObjDropDownList.DataTextField = "Name";
                    ObjDropDownList.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }
        public void BindDataToCheckBoxList(CheckBoxList objCheckBoxList, ComboQueryType ValComboQueryType, int iParentId, string sAdditionalCondition)
        {

            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                if (objCheckBoxList == null) return;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetDataForCombo", (int)ValComboQueryType, iParentId, sAdditionalCondition);
                if (dt != null)
                {

                    objCheckBoxList.DataSource = dt;
                    objCheckBoxList.DataValueField = "ID";
                    objCheckBoxList.DataTextField = "Name";
                    objCheckBoxList.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        //Megha30/05/2011 
        public DataSet GetVehicleSparesDocumentDetails(int sDealerID, string ModelPart, string FromDate, string ToDate)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("Sp_GetVehicleSpares_DocumentPrint", sDealerID, ModelPart, FromDate, ToDate);

                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }
        //Megha30/05/2011
        public void FillCheckBoxList(CheckBoxList ObjCheckBoxList, clsCommon.ComboQueryType ValComboQueryType, int iParentId)
        {
            FillCheckBoxList(ObjCheckBoxList, ValComboQueryType, iParentId, "");
        }

        public void FillCheckBoxList(CheckBoxList ObjCheckBoxList, clsCommon.ComboQueryType ValComboQueryType, int iParentId, string sAdditionalCondition)
        {
            clsDB objDB = new clsDB();
            try
            {
                ObjCheckBoxList.DataSource = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetDataForCombo", (int)ValComboQueryType, iParentId, sAdditionalCondition);
                ObjCheckBoxList.DataTextField = "Name";
                ObjCheckBoxList.DataValueField = "ID";
                ObjCheckBoxList.DataBind();
            }
            catch
            { }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }

        //Fill Lc Clause
        public void FillLCClause(CheckBoxList LCClauseList)
        {

            clsDB objDB = new clsDB();
            try
            {
                LCClauseList.DataSource = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetDataForCombo", (int)clsCommon.ComboQueryType.LCClause, 0, "");
                LCClauseList.DataTextField = "Name";
                LCClauseList.DataValueField = "ID";
                LCClauseList.DataBind();
            }
            catch
            { }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        //Sujata 
        //Bind Data To RadioButtonList
        public void FillRadioButtonList(RadioButtonList ObjRadioButtonList, clsCommon.ComboQueryType ValComboQueryType, int iParentId, string sAdditionalCondition)
        {
            clsDB objDB = new clsDB();
            try
            {
                ObjRadioButtonList.DataSource = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetDataForCombo", (int)ValComboQueryType, iParentId, sAdditionalCondition);
                ObjRadioButtonList.DataTextField = "Name";
                ObjRadioButtonList.DataValueField = "ID";
                ObjRadioButtonList.DataBind();
            }
            catch
            { }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }


        public string GetDealerList(string sSelectionFor, int iUserID, string sDomestic_Export)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt = null;
                string sDealerIds = "";

                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetMultiDealerRecords", iUserID, sSelectionFor, sDomestic_Export);

                sDealerIds = Func.Convert.sConvertToString(dt.Rows[0][0]);
                return sDealerIds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }
        public string GetDealersByUserID(int iUserID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt = null;
                string sDealerIds = "";

                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetDealersByUserID", iUserID);

                sDealerIds = Func.Convert.sConvertToString(dt.Rows[0][0]);
                return sDealerIds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }
        // Bind Data to fixed Combo
        public void BindDataToFixedCombo(DropDownList ObjDropDownList, FixedComboType ValFixedComboType)
        {

            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                DataRow tmpDataRow;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetDataForFixedCombo", (int)ValFixedComboType);
                if (dt != null)
                {

                    tmpDataRow = dt.NewRow();
                    tmpDataRow["ID"] = 0;
                    tmpDataRow["Name"] = "--Select--";
                    dt.Rows.InsertAt(tmpDataRow, 0);

                    ObjDropDownList.DataSource = dt;
                    ObjDropDownList.DataValueField = "ID";
                    ObjDropDownList.DataTextField = "Name";
                    ObjDropDownList.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        //Display LC Caluse
        public void DisplayLCClause(CheckBoxList ChkList, DataTable dtList)
        {
            int idtRowCnt = 0;
            bool bRecordUsed = true;
            if (dtList != null && ChkList.Items.Count != 0)
            {
                for (int iLCRowCnt = 0; iLCRowCnt < ChkList.Items.Count; iLCRowCnt++)
                {
                    bRecordUsed = false;
                    ChkList.Items[iLCRowCnt].Selected = false;
                    if (idtRowCnt < dtList.Rows.Count)
                    {
                        if (Func.Convert.sConvertToString(dtList.Rows[idtRowCnt]["ID"]) == ChkList.Items[iLCRowCnt].Value)
                        {
                            ChkList.Items[iLCRowCnt].Selected = true;
                            bRecordUsed = true;
                            idtRowCnt++;
                        }
                    }
                    //// If record is Confirm /Cancel the nShow only selected Record
                    //if (bEnable == false && bRecordUsed == false)
                    //{
                    //    ChkList.Items.Remove(ChkList.Items[iLCRowCnt]);
                    //}
                }
            }
        }


        #endregion

        #region Common Function
        public DataTable GetGridData(string sTableName, string sFieldNameForSelect, string sWhereClauseName, int iWhereClauseID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt = null;
                string ssql = "select Record_Used," + sFieldNameForSelect + " From " + sTableName + " where " + sWhereClauseName + " = " + iWhereClauseID + " Order by " + sFieldNameForSelect;
                dt = objDB.ExecuteQueryAndGetDataTable(ssql);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public DataSet GetFormControlData(string sMenuID)
        {
            clsDB objDB = new clsDB();
            try
            {
                return objDB.ExecuteStoredProcedureAndGetDataset("SP_GetFormControlData", sMenuID);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public DataSet GetGridDetails(int iControlTypeID)
        {
            clsDB objDB = new clsDB();
            try
            {
                return objDB.ExecuteStoredProcedureAndGetDataset("SP_GetGridDetails", iControlTypeID);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public string sGetDataFromDataQuery(GetDataQueryType ValGetDataQueryType, int iParentId, string sAdditionalCondition)
        {
            string sValue = "";
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetDataFromDataQuery", (int)ValGetDataQueryType, iParentId, sAdditionalCondition);
                if (iRowCntOfTable(dt) != 0)
                {
                    sValue = Func.Convert.sConvertToString(dt.Rows[0]["Value"]);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
            return sValue;
        }

        public int sGetMultiUserAccess(int iUserId, int iMenu)
        {
            int iValue = 0;
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_MultiUserAccess", iUserId, iMenu);
                if (iRowCntOfTable(dt) != 0)
                {
                    iValue = Func.Convert.iConvertToInt(dt.Rows[0]["UserID"]);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
            return iValue;
        }

        // Check Record exist
        public bool bCheckRecordExist(string sSql)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (objDB.ExecuteQuery(sSql) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        // To get max documnet number
        public int iGetMaxDocNo(string sFinancialYear, string sDocName, int iDealerID)
        {
            int iMaxDocNo = 0;
            clsDB objDB = new clsDB();
            try
            {
                //if (sFinancialYear != "" && sFinancialYear != null)
                if (sFinancialYear != "" && sFinancialYear != null)
                {
                    //sFinancialYear = Func.sGetFinancialYear(idealerID);
                    sFinancialYear = Func.sGetFinancialYear(iDealerID);
                }
                if (sDocName != "" && sDocName != null)
                {

                    iMaxDocNo = objDB.ExecuteStoredProcedure("SP_GetMaxDocNo", sFinancialYear, sDocName, iDealerID);

                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
            return iMaxDocNo;
        }

        // To update Max No In Document  No Series
        public void UpdateMaxNo(clsDB objDB, string sFinancialYear, string sDocName, int iDealerID)
        {
            // if (sFinancialYear != "" && sFinancialYear != null)
            if (sFinancialYear != "" && sFinancialYear != null)
            {
                sFinancialYear = Func.sGetFinancialYear(iDealerID);
            }
            if (sDocName != "" && sDocName != null)
            {

                objDB.ExecuteStoredProcedure("SP_UpdateMaxDocNo", sFinancialYear, sDocName, iDealerID);

            }
        }

        /// <summary>
        /// Method to Get Data on the base of Fixed Combo Value Selected
        /// </summary>
        /// <param name="iOptionId"> 1 Unprocess RFP, 2 Partial Process RFP, 3 Complete Process RFP</param>
        /// <param name="sModel_Part">M Model P Part</param>
        /// <param name="iDealerID">Delaer ID</param>
        /// <returns></returns>    
        public DataTable GetDataOnFixedComboValue(int iOptionId, string sModel_Part, int iDealerID, string sAllDealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetData_OnFixedComboValue", iOptionId, sModel_Part, iDealerID, sAllDealerID);
                if (ds == null) return null;
                if (ds.Tables.Count == 0) return null;
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }


        /// <summary>    
        /// To Get Max RFP No. 
        /// </summary>        
        /// <param name="sDealerCode"></param>
        /// <param name="sFinYear">  default value ""</param>
        /// <param name="sRFPModel_Part">"M" for Model /"P" for Part</param>
        /// <returns></returns>
        public string sGetMaxDocNo(string sDealerCode, string sFinYear, string sDocumentName, int iDealerID)
        {
            string sRFPDocNo = "";
            int iMaxDocNo = 0;
            string sMaxRFPNo = "";
            if (sFinYear == "")
            {
                sFinYear = Func.sGetFinancialYear(iDealerID);
            }

            // format is dlr Code(6)+/+document start digt(1) V-Vehicle S-Spares & RFP(Document short name 3)+/+financial year(2)+/+Serial No.(4)                 
            iMaxDocNo = iGetMaxDocNo(sFinYear, sDocumentName, iDealerID);
            sMaxRFPNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
            sMaxRFPNo = sMaxRFPNo.PadLeft(4, '0');

            //sRFPDocNo = sDealerCode + "/" + sDocumentName + "/" + sFinYear + "/" + sMaxRFPNo;
            if (sDocumentName == "WPP" || sDocumentName == "WPL" || sDocumentName == "GTP" || sDocumentName == "GTL" ||
                sDocumentName == "GCP" || sDocumentName == "GCL" || sDocumentName == "CPP" || sDocumentName == "CPL" ||
                sDocumentName == "WCP" || sDocumentName == "WCL" || sDocumentName == "RMP" || sDocumentName == "RML" ||
                sDocumentName == "COF" || sDocumentName == "COD" || sDocumentName == "FR")
            {
                sRFPDocNo = sDealerCode.Substring(2, 5) + sDocumentName + sFinYear + sMaxRFPNo;
            }
            else
            {
                sRFPDocNo = sDealerCode + "/" + sDocumentName + "/" + sFinYear + "/" + sMaxRFPNo;
            }
            return sRFPDocNo;
        }

        //Sujata 28042016 Begin
        // To get max documnet number
        public int iGetMaxSubDocNo(string sFinancialYear, string sDocName, int iDealerID)
        {
            int iMaxDocNo = 0;
            clsDB objDB = new clsDB();
            try
            {
                if (sFinancialYear != "" && sFinancialYear != null)
                {
                    sFinancialYear = Func.sGetFinancialYear(iDealerID);
                }
                if (sDocName != "" && sDocName != null)
                {

                    iMaxDocNo = objDB.ExecuteStoredProcedure("SP_GetMaxSubDocNo", sFinancialYear, sDocName, iDealerID);

                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
            return iMaxDocNo;
        }

        // To update Max No In subDocument  No Series like IPO in jobcard
        public void UpdateMaxNoSubDoc(clsDB objDB, string sFinancialYear, string sDocName, int iDealerID)
        {
            if (sFinancialYear != "" && sFinancialYear != null)
            {
                sFinancialYear = Func.sGetFinancialYear(iDealerID);
            }
            if (sDocName != "" && sDocName != null)
            {

                objDB.ExecuteStoredProcedure("SP_UpdateMaxSubDocNo", sFinancialYear, sDocName, iDealerID);

            }
        }

        // Added for IPO no creation
        public string sGetMaxSubDocNo(string sDealerCode, string sFinYear, string sDocumentName, int iDealerID)
        {
            string sRFPDocNo = "";
            int iMaxDocNo = 0;
            string sMaxRFPNo = "";
            if (sFinYear == "")
            {
                sFinYear = Func.sGetFinancialYear(iDealerID);
            }

            // format is dlr Code(6)+/+document start digt(1) V-Vehicle S-Spares & RFP(Document short name 3)+/+financial year(2)+/+Serial No.(4)                 
            iMaxDocNo = iGetMaxSubDocNo(sFinYear, sDocumentName, iDealerID);
            sMaxRFPNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
            sMaxRFPNo = sMaxRFPNo.PadLeft(4, '0');
            sRFPDocNo = sDealerCode + "/" + sDocumentName + "/" + sFinYear + "/" + sMaxRFPNo;
            return sRFPDocNo;
        }
        //Sujata 28042016 End

        /// <summary>
        /// To Get Sql For Fill Selection Grid
        /// </summary>
        /// <param name="sFromDate"></param>
        /// <param name="sToDate"></param>
        /// <param name="sModelPart">else ""</param>
        /// <param name="iDealerId">else 0 </param>
        /// <param name="sSqlFor"> "RFP/Proforma/preshipment/PackingList"</param>
        /// <returns></returns>
        public DataSet GetSqlToFillSelectionGrid(string sFromDate, string sToDate, string sModelPart, int iDealerId, string SqlFor)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDataToFillSelecionGrid", sFromDate, sToDate, sModelPart, iDealerId.ToString(), SqlFor, 0);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }


        public DataSet GetSqlToFillSelectionGrid(string sFromDate, string sToDate, string sModelPart, string sDealerId, string SqlFor)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDataToFillSelecionGrid", sFromDate, sToDate, sModelPart, sDealerId, SqlFor, 0);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public DataSet GetSqlToFillSelectionGrid(string sFromDate, string sToDate, string sModelPart, string sDealerId, string SqlFor, int HOBrID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDataToFillSelecionGrid", sFromDate, sToDate, sModelPart, sDealerId, SqlFor, HOBrID);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }


        public DataSet GetDataToFillSelectionGrid(string sFromDate, string sToDate, string sModelPart, int iDealerId, string SqlFor, int CurrentIndex, int PageSize)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDataToFillSelecionGrid", sFromDate, sToDate, sModelPart, iDealerId.ToString(), SqlFor, CurrentIndex, PageSize);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }


        //public DataSet GetDataToFillSelectionGrid(string sFromDate, string sToDate, string sModelPart, string sDealerId, string SqlFor,int CurrentIndex, int PageSize)
        //{
        //    clsDB objDB = new clsDB();
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDataToFillSelecionGrid", sFromDate, sToDate, sModelPart, sDealerId, SqlFor);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (objDB != null) objDB = null;
        //    }
        //}

        public DataTable GetRecordOnOpenClick(string sGetRecordFor, string sDealerIds, int UserRole)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt = new DataTable();
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetRecord_OnNewClick_VehSale", sGetRecordFor, sDealerIds, UserRole);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }


        // Get Count of the Tbale in Dataset
        public int iTableCntOfDatatSet(DataSet ds)
        {
            int iTableCntOfDatatSet = 0;
            try
            {
                if (ds == null) return iTableCntOfDatatSet;
                iTableCntOfDatatSet = ds.Tables.Count;
            }
            catch
            { }
            return iTableCntOfDatatSet;
        }
        // To Get Row Count of the Table 
        public int iRowCntOfTable(DataTable dt)
        {
            int iRowCntOfTable = 0;
            try
            {
                if (dt == null) return iRowCntOfTable;
                iRowCntOfTable = dt.Rows.Count;
            }
            catch
            {
            }
            return iRowCntOfTable;
        }
        // To Get Row Count Of the Table
        public string sRowCntOfTable(DataTable dt)
        {
            return Func.Convert.sConvertToString(iRowCntOfTable(dt));
        }
        #endregion

        public static int GridPageIndex(int iIndex)
        {
            return iIndex % 10;
        }

        #region Date function
        // Get Starting Date Of the month
        public string sGetMonthStartDate(string sCurrDate)
        {
            string sDate = "";
            string sMonth = "01";
            string sYear = "";
            sDate = sCurrDate;
            sMonth = Convert.ToDateTime(sCurrDate).Month.ToString("00");
            sYear = Convert.ToDateTime(sCurrDate).Year.ToString("0000");
            sDate = "01/" + sMonth + "/" + sYear;
            return sDate;
        }

        // Get Ending Date Of the month
        public string sGetMonthEndDate(string sCurrDate)
        {
            string sDate = "";
            string sDay = "";
            int iMonth = 0;
            int iYear = 0;
            sDate = sCurrDate;
            iMonth = Convert.ToDateTime(sCurrDate).Month;
            iYear = Convert.ToDateTime(sCurrDate).Year;
            if (iMonth == 2)
            {
                if ((iYear % 4 == 0 && iYear % 100 != 0) || iYear % 400 == 0)
                {
                    sDay = "29";
                }
                else
                {
                    sDay = "28";
                }
            }
            else if (iMonth == 1 || iMonth == 3 || iMonth == 5 || iMonth == 7 || iMonth == 8 || iMonth == 10 || iMonth == 12)
            {
                sDay = "31";
            }
            else
            {
                sDay = "30";
            }
            sDate = sDay + "/" + iMonth.ToString("00") + "/" + iYear.ToString("0000");
            return sDate;
        }
        /// <summary>
        /// To Get Current Date Time  as Per Country
        /// </summary>
        /// <param name="iCountryId"> Id of Country</param>
        /// <param name="bWithTime"> True : date with time false : only Date</param>
        /// <returns></returns>
        public string sGetCurrentDate(int iCountryId, bool bWithTime)
        {
            string sDate = "";
            if (Func.Convert.sConvertToString(HttpContext.Current.Session["DlrEOYDone"]) == "Y")
            {
                //if (bWithTime == true)
                //{
                //    sDate = DateTime.Today.Date.ToString();
                //}
                //else
                //{
                //    sDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
                //}

                if (bWithTime == true)
                {
                    //sDate = DateTime.Today.Date.ToString();
                    if (Func.Convert.sConvertToString(HttpContext.Current.Session["ISGST"]) == "Y")
                    {
                        sDate = DateTime.Today.Date.ToString();
                    }
                    else
                    {
                        //sDate = "30/06/2017" + " " + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss"));
                        sDate = (DateTime.Today.Date <= Convert.ToDateTime("30/06/2017")) ? DateTime.Today.Date.ToString() : "30/06/2017" + " " + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss"));
                    }
                }
                else
                {
                    //sDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
                    if (Func.Convert.sConvertToString(HttpContext.Current.Session["ISGST"]) == "Y")
                    {
                        sDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        //sDate = "30/06/2017" ;
                        sDate = (DateTime.Today.Date <= Convert.ToDateTime("30/06/2017")) ? DateTime.Today.Date.ToString("dd/MM/yyyy") : "30/06/2017";
                    }
                }


            }
            else
            {
                string sDlrMaxYear = Func.Convert.sConvertToString(HttpContext.Current.Session["DlrCurrMaxYear"]);
                if (bWithTime == true)
                {
                    sDate = "31/03/" + sDlrMaxYear + " " + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss"));
                }
                else
                {
                    sDate = "31/03/" + sDlrMaxYear;
                }
            }
            //
            //if (iCountryId ==1)
            //{

            //}
            //else(iCountryId ==2)
            //{

            //}
            return sDate;
        }

        public string sGetCurrentDateTime(int iCountryId, bool bWithTime)
        {
            string sDate = "";
            if (Func.Convert.sConvertToString(HttpContext.Current.Session["DlrEOYDone"]) == "Y")
            {
                if (bWithTime == true)
                {
                    //sDate = DateTime.Now.ToString();                    
                    if (Func.Convert.sConvertToString(HttpContext.Current.Session["ISGST"]) == "Y")
                    {
                        sDate = DateTime.Now.ToString();
                    }
                    else
                    {
                        //sDate = "30/06/2017" + " " + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss"));                        
                        sDate = (DateTime.Today.Date <= Convert.ToDateTime("30/06/2017")) ? DateTime.Today.Date.ToString() : "30/06/2017" + " " + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss"));
                    }
                }
                else
                {
                    //sDate = DateTime.Today.Date.ToString("dd/MM/yyyy");                    
                    if (Func.Convert.sConvertToString(HttpContext.Current.Session["ISGST"]) == "Y")
                    {
                        sDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        //sDate = "30/06/2017" ;
                        sDate = (DateTime.Today.Date <= Convert.ToDateTime("30/06/2017")) ? DateTime.Today.Date.ToString("dd/MM/yyyy") : "30/06/2017";
                    }
                }
            }
            else
            {
                string sDlrMaxYear = Func.Convert.sConvertToString(HttpContext.Current.Session["DlrCurrMaxYear"]);
                if (bWithTime == true)
                {
                    sDate = "31/03/" + sDlrMaxYear + " " + Func.Convert.sConvertToString(System.DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss"));
                }
                else
                {
                    sDate = "31/03/" + sDlrMaxYear;
                }
            }
            return sDate;
        }

        public string sGetCurrentDate()
        {
            if (Func.Convert.sConvertToString(HttpContext.Current.Session["DlrEOYDone"]) == "Y")
            {
                //return DateTime.Today.Date.ToString("dd-MMM-yyyy");
                if (Func.Convert.sConvertToString(HttpContext.Current.Session["ISGST"]) == "Y")
                {
                    return DateTime.Today.Date.ToString("dd-MMM-yyyy");
                }
                else
                {
                    //return "30/06/2017" ;                    
                    return (DateTime.Today.Date <= Convert.ToDateTime("30/06/2017")) ? DateTime.Today.Date.ToString("dd/MM/yyyy") : "30/06/2017";
                }
            }
            else
            {
                string sDlrMaxYear = Func.Convert.sConvertToString(HttpContext.Current.Session["DlrCurrMaxYear"]);
                return "31/03/" + sDlrMaxYear;
            }
            ///return DateTime.Today.Date.ToString("dd/MM/yyyy");
        }
        ///To Get Month Short Name
        public string sGetMonthShortName(string sDate)
        {
            string sMonthShortName = "";
            if (sDate == "")
            {
                sDate = sGetCurrentDate();
                DateTime dtm = Convert.ToDateTime(sDate);
                sMonthShortName = sGetMonthShortName(dtm.Month);
            }
            return sMonthShortName;
        }

        ///To Get Month Short Name
        public string sGetMonthShortName(int iMonthID)
        {
            string sMonthShortName = "";
            switch (iMonthID.ToString())
            {
                case "1": { sMonthShortName = "JAN"; break; }
                case "2": { sMonthShortName = "FEB"; break; }
                case "3": { sMonthShortName = "MAR"; break; }
                case "4": { sMonthShortName = "APR"; break; }
                case "5": { sMonthShortName = "MAY"; break; }
                case "6": { sMonthShortName = "JUN"; break; }
                case "7": { sMonthShortName = "JUL"; break; }
                case "8": { sMonthShortName = "AUG"; break; }
                case "9": { sMonthShortName = "SEP"; break; }
                case "10": { sMonthShortName = "OCT"; break; }
                case "11": { sMonthShortName = "NOV"; break; }
                case "12": { sMonthShortName = "DEC"; break; }

            }
            return sMonthShortName;
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public void ProcessUnhandledException(Exception ex1)
        {
            // An unhandled exception occured somewhere in our application. Let
            // the 'Global Policy' handler have a try at handling it.
            try
            {
                bool rethrow = false;

                if (ex1.Message != "Thread was being aborted.")
                {
                    rethrow = ExceptionPolicy.HandleException(ex1, "Global Policy");
                }
                //System.Web.HttpContext.Current.Response.Redirect("~/frmError.aspx");
                System.Web.HttpContext.Current.Response.Redirect("~/frmException.aspx?sType=" + "Error");
                if (rethrow)
                {
                    // Something has gone very wrong - exit the application.
                    //Application.Exit();
                }
            }
            catch (Exception ex)
            {
                // Something has gone wrong during HandleException (e.g. incorrect configuration of the block).
                // Exit the application
                string errorMsg = "An unexpected exception occured while calling HandleException with policy 'Global Policy'. ";
                errorMsg += "Please check the event log for details about the exception." + Environment.NewLine + Environment.NewLine;

                //MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                //  Application.Exit();
            }
            // QuickStartForm.AppForm.Cursor = System.Windows.Forms.Cursors.Default;
        }

        public string GetDealerWiseSystemParameters(int DealerID, int HoBr_Id, int iParaID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 29032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetSystemParameters_values", DealerID, HoBr_Id, iParaID);
                return Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Para_Value"]);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        //Created By Vikram Dated 12042018 Start
        #region CommanFunction for generate Doc No
        public string GenerateDocNo(string sDealerCode, int iDealerID, string sDocName, string Is_Distributor)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                if (sFinYear == "2016")
                    sFinYearChar = sFinYear.Substring(3);
                else
                    sFinYearChar = sFinYear;

                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                if (sFinYear == "2016")
                {
                    sMaxDocNo = sMaxDocNo.PadLeft(5, '0');
                    sDocNo = sDealerCode.Substring(2, 5) + sDocName + sFinYearChar + sMaxDocNo;
                }
                else if (sDealerCode != "")
                {
                    sMaxDocNo = sMaxDocNo.PadLeft(4, '0');
                    sDocNo = sDealerCode.Substring(2, 5) + sDocName + sFinYearChar + sMaxDocNo;
                }
                else
                    sDocNo = sDocName + sFinYearChar + sMaxDocNo;


                return sDocNo;
            }
            catch
            {
                return "0";
            }
        }
        public DataTable FillDatatableFromExcelSheet(string FilePath, string ext, string isHader)
        {
            DataTable dtLayout = new DataTable();
            string connectionString = "";
            if (ext == ".xls")
            {   //For Excel 97-03
                //connectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }
            else if (ext == ".xlsx")
            {    //For Excel 07 and greater
                //connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }
            connectionString = String.Format(connectionString, FilePath, isHader);
            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmd.Connection = conn;
            //Fetch 1st Sheet Name
            conn.Open();
            DataTable dtSchema;
            dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string ExcelSheetName = dtSchema.Rows[0]["TABLE_NAME"].ToString();
            conn.Close();
            //Read all data of fetched Sheet to a Data Table
            conn.Open();
            cmd.CommandText = "SELECT * From [" + ExcelSheetName + "]";
            dataAdapter.SelectCommand = cmd;
            dataAdapter.Fill(dt);
            conn.Close();
            dtLayout = dt;

            return dtLayout;
        }
        #endregion
        //END
    }
}
