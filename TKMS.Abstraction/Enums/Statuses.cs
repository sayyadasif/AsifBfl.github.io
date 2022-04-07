using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.Enums
{
    public enum IndentStatuses
    {
        [Description("Pending Approval")]
        PendingApproval = 1,

        [Description("Approved")]
        Approved = 2,

        [Description("Rejected")]
        Rejected = 3,

        [Description("Approved")]
        IndentForDispatch = 4,

        [Description("Partial Dispatched")]
        PartialDispatched = 5,

        [Description("Dispatched")]
        Dispatched = 6,

        [Description("Indent Box Received")]
        IndentBoxReceived = 7,

        [Description("Cancelled")]
        Cancelled = 8,
    }

    public enum KitStatuses
    {
        [Description("Dispatched")]
        Dispatched = 1,

        [Description("Received")]
        Received = 2,

        [Description("Allocated")]
        Allocated = 3,

        [Description("Assigned")]
        Assigned = 4,

        [Description("Damaged")]
        Damaged = 5,

        [Description("Destruction")]
        Destruction = 6,

        [Description("Transfer")]
        Transfer = 7,
    }

    public enum IWorksStatuses
    {
        [Description("Dispatched")]
        Dispatched = 1,

        [Description("Received")]
        Received = 2,

        [Description("Allocated")]
        Allocated = 3,

        [Description("Re-Allocated")]
        ReAllocated = 4,

        [Description("Assigned")]
        Assigned = 5,

        [Description("Destroyed")]
        Destroyed = 6,

        [Description("Returned")]
        Returned = 7,
    }


    public enum DispatchStatuses
    {
        Dispatched = 1,
        ReceivedAtRo = 2,
        DispatchToBranch = 3,
        Received = 4,
    }

    public enum CourierStatuses
    {
        Norecords = 1,
        Rto = 2,
        ShipmentDelivered = 3,
        SpeedPost = 4,
        CneeShiftedFromTheGivenAddress = 5,
        Intransit = 6,
        ConsigneesAddressUnlocatableLandmarkNeeded = 7,
        NewReordAdded = 8
    }
}
