using astute.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IInvoiceRemarksService
    {
        Task<int> InsertInvoiceRemarks(Invoice_Remarks invoice_Remarks);
        Task<int> UpdateInvoiceRemarks(Invoice_Remarks invoice_Remarks);
        Task<int> DeleteInvoiceRemarks(int processId, DateTime startDate);
        Task<IList<Invoice_Remarks>> GetInvoiceRemarks(int processId, DateTime startDate);
        Task<int> InvoiceRemarksChangeStatus(int process_Id, bool status);
    }
}
