using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class DispatchRequestModel
    {
        public long IndentId { get; set; }
        public long IndentStatusId { get; set; }
        public string IndentStatus { get; set; }
        public string IndentNo { get; set; }
        public int NoOfKit { get; set; }
        public int DispatchedNoOfKit { get; set; }
        [Required(ErrorMessage = "Please enter number of kits")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please enter number of kits")]
        public int DispatchNoOfKit { get; set; }
        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        public IFormFile BranchDispatch { get; set; }
        public string BranchDispatchSuccess { get; set; }
        public string BranchDispatchError { get; set; }
        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        public IFormFile KitDispatch { get; set; }
        public string KitDispatchSuccess { get; set; }
        public string KitDispatchError { get; set; }
        public IFormFile KitDispatchReport { get; set; }
        public string KitDispatchReportSuccess { get; set; }
        public string KitDispatchReportError { get; set; }
    }
}
