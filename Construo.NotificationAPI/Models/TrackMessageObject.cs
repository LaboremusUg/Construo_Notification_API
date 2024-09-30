using System.ComponentModel;

namespace Construo.NotificationAPI.Models;

public class TrackMessageObject
{
    public string Context { get; set; }

    public int ContextId { get; set; }

    public TemplateType TemplateType { get; set; }

    public int? SentByContactId { get; set; }
}

public enum TemplateType
{
    /// <summary>
    /// 
    /// </summary>
    [Description("Default")]
    Default = 0,

    /// <summary>
    /// 
    /// </summary>
    [Description("Approval")]
    Approval = 1,

    /// <summary>
    /// 
    /// </summary>
    [Description("DeclineAmlOrFraud")]
    DeclineAmlOrFraud = 2,

    /// <summary>
    /// 
    /// </summary>
    [Description("DeclineOverAllAssessment")]
    DeclineOverAllAssessment = 3,

    /// <summary>
    /// 
    /// </summary>
    [Description("DeclineTaxIncomeDeviation")]
    DeclineTaxIncomeDeviation = 4,

    /// <summary>
    /// 
    /// </summary>
    [Description("AutoApproval")]
    AutoApproval = 5,

    /// <summary>
    /// 
    /// </summary>
    [Description("Custom")]
    Custom = 7,

    /// <summary>
    /// 
    /// </summary>
    [Description("LoanPaidOut")]
    LoanPaidOut = 8,

    /// <summary>
    /// 
    /// </summary>
    [Description("ApplicationWithdraw")]
    ApplicationWithdraw = 9,

    /// <summary>
    /// 
    /// </summary>
    [Description("AutoDecline")]
    AutoDecline = 11,

    /// <summary>
    /// 
    /// </summary>
    [Description("Signature")]
    Signature = 12,

    /// <summary>
    /// 
    /// </summary>
    [Description("DiscontinueApplication")]
    DiscontinueApplication = 13,

    /// <summary>
    /// 
    /// </summary>
    [Description("SavingsAccountCreated")]
    SavingsAccountCreated = 15,

    /// <summary>
    /// 
    /// </summary>
    [Description("SavingsAccountContractCreated")]
    SavingsAccountContractCreated = 16,

    /// <summary>
    /// 
    /// </summary>
    [Description("ResetPassword")]
    ResetPassword = 1000,

    /// <summary>
    /// 
    /// </summary>
    [Description("DocumentReminder")]
    DocumentReminder = 1001,

    /// <summary>
    /// 
    /// </summary>
    [Description("GrantAccess")]
    GrantAccess = 1002,

    /// <summary>
    /// 
    /// </summary>
    [Description("OfferAcceptance")]
    OfferAcceptance = 1003,

    /// <summary>
    /// 
    /// </summary>
    [Description("AccountNumber")]
    AccountNumber = 1004,

    /// <summary>
    /// Broker email address missing
    /// </summary>
    [Description("BrokerEmailMissing")]
    BrokerEmailMissing = 2001,

    /// <summary>
    /// OFPS finds duplicate when checking order files from UCA for duplicates
    /// </summary>
    [Description("DuplicateEntry")]
    DuplicateEntry = 2002,

    /// <summary>
    /// Missing fields to successfully determine instrument name/category
    /// </summary>
    [Description("FailureToDetermineInstrumentNameOrCategory")]
    FailureToDetermineInstrumentNameOrCategory = 2003,

    /// <summary>
    /// Order File Validation (Missing fields, invalid file format etc.)
    /// </summary>
    [Description("InvalidOrderFile")]
    InvalidOrderFile = 2004,

    /// <summary>
    /// Missing mandatory information to /runtest
    /// </summary>
    [Description("MissingInformationToRunTest")]
    MissingInformationToRunTest = 2005,

    /// <summary>
    /// STA link in case of a mismatch
    /// </summary>
    [Description("MismatchTestResult")]
    MismatchTestResult = 2006,

    /// <summary>
    /// Send PDF
    /// </summary>
    [Description("PdfGenerated")]
    PdfGenerated = 2007,

    /// <summary>
    /// Unable to determine if client is corporate or personal
    /// </summary>
    [Description("UnableToDetermineClientType")]
    UnableToDetermineClientType = 2008,

    /// <summary>
    /// Client Missing On Kyc DataConnector
    /// </summary>
    [Description("ClientMissingOnKycDataConnector")]
    ClientMissingOnKycDataConnector = 2009,

    /// <summary>
    ///
    /// </summary>
    [Description("InvestorDetailsMissing")]
    InvestorDetailsMissing = 2010
}
