using AutoMapper;
using CORTNE.EFModelCORTNEDB;
using CORTNE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CORTNE.Helpers
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<CrCashReceiptDetail, CashReceiptDetailViewModel>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FirstName ?? "" + s.LastName ?? ""))
                .ForMember(d => d.FullAddress, opt => opt.MapFrom(detail => (detail.Address1 ?? "") + " " + (detail.Address2 ?? "") + " " + (string.IsNullOrEmpty(detail.City) == false ? detail.City + "," : "")
                                + " " + (string.IsNullOrEmpty(detail.State) == false ? detail.State + "," : "") + " " + (string.IsNullOrEmpty(detail.Zip) == false ? detail.Zip + "," : "")));
            CreateMap<CrCashReceipt, CashReceiptViewModel>();
            CreateMap<CrCashReceiptDetail, CashReceiptDetailDto>();
            CreateMap<CrAccountCode, AccountCodeListDto>();
            CreateMap<CrAccountCode, AccountCodeDto>();
            CreateMap<CrAccountCodeReceipt, AccountCodeReceiptsDto>();

            CreateMap<CrDeposit, DepositReceiptViewModel>();
            CreateMap<CrTransfer, TransferViewModel>();
            CreateMap<CrAccountCode, AccountCodeListDto>();
            CreateMap<CrAccountCode, AccountCodeViewModel>();


        }
    }
}
