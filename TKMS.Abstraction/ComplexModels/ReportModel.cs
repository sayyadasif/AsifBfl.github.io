using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class ReportModel
    {
        public string IndentNumber { get; set; }
        public DateTime? IndentDate { get; set; }
        public string SchemeCode { get; set; }
        public string CardType { get; set; }
        public string C5Code { get; set; }
        public int? NoOfKit { get; set; }
        public string BranchType { get; set; }
        public string IblBranchName { get; set; }
        public string IblBranchCode { get; set; }
        public string DispatchAddress { get; set; }
        public string BfilBranchCode { get; set; }
        public string BfilBranchName { get; set; }
        public string PinCode { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string IndentedUserType { get; set; }
        public string IndentedBy { get; set; }
        public string BfilIndentHOStaus { get; set; }
        public string BfilIndentHoApproveBy { get; set; }
        public DateTime? HoApproveDate { get; set; }
        public string CpuIndentStaus { get; set; }
        public string CpuIndentHoApproveBy { get; set; }
        public DateTime? CpuApproveDate { get; set; }
        public DateTime? DateOfDispatch { get; set; }
        public DateTime? ReportUploadDate { get; set; }
        public string ReportUploadBy { get; set; }
        public string Vendor { get; set; }
        public int? Qty { get; set; }
        public string AccountStart { get; set; }
        public string AccountEnd { get; set; }
        public string SchemeType { get; set; }
        public string ReferenceNumber { get; set; }

        public IEnumerable<string> WayBills { get; set; }
        public string WayBillNo { get; set; }

        public string Status { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string ReceiverName { get; set; }
        public string CourierName { get; set; }
        public string CifNo { get; set; }
        public string AccountNo { get; set; }
        public string User { get; set; }
        public DateTime? UserConfirmationDate { get; set; }
        public DateTime? DateOfUpload { get; set; }

        public DateTime? ReceivedDate { get; set; }
        public string ReceivedBy { get; set; }

        public DateTime? ScannedDate { get; set; }
        public string ScannedBy { get; set; }
        public string AllocatedToStaffId { get; set; }
        public string AllocatedTo { get; set; }
        public string AllocatedBy { get; set; }
        public DateTime? AllocatedDate { get; set; }
        public string AssignedTo { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string Reason { get; set; }

        public string DesturtionApproved { get; set; }

        public string DestructionBy { get; set; }
        public DateTime? DestructionByDate { get; set; }

        public string ReturnedBy { get; set; }
        public string ReturnedAcceptBy { get; set; }
        
        public DateTime? ReturnedDate { get; set; }

        public DateTime? ReportDate { get; set; }
        public string ReportStatus { get; set; }
    }
}
