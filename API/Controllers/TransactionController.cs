using API.DTO;
using API.Ultils;
using AutoMapper;
using ElderCare_Domain.Models;
using ElderCare_Service;
using ElderCare_Repository.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using ElderCare_Service.Interfaces;
using ElderCare_Service.Services;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ODataController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
   
        private readonly ICarerService _carerService;


        private readonly ITransactionService _transactionService;
        public static string? url;

        public TransactionController(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, ICarerService carerService, ITransactionService transactionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _carerService = carerService;
            _transactionService = transactionService;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult GetAllTransactions()
        {
            //var list = _unitOfWork.TransactionRepo.GetAll();
            var list = _transactionService.GetAll();

            return Ok(list);
        }
        [HttpPost]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> CreateTransaction([FromBody] TrasactionDto dto,int carerid)
        {
            
            var idClaim = _unitOfWork.AccountRepository.GetMemberIdFromToken(HttpContext.User);
            var userid = await _unitOfWork.AccountRepository.GetByIdAsync(idClaim);
            //dto.DateTime = DateTime.Now;
             
            //var id = _unitOfWork.TransactionRepo.GetAll().OrderByDescending(i=>i.TransactionId).FirstOrDefault().TransactionId;
            //Transaction obj = _mapper.Map<Transaction>(dto);
            //obj.AccountId = userid.AccountId;
            //obj.TransactionId = id+1;
            //obj.Status = "PENDING";
            //await _unitOfWork.TransactionRepo.AddAsync(obj);

            try
            {
                //await _unitOfWork.SaveChangeAsync();
                //string vnp_Returnurl = dto.RedirectUrl; //URL nhan ket qua tra ve 
                //string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
                //string vnp_TmnCode = "NWYNAA42"; //Ma định danh merchant kết nối (Terminal Id)
                //string vnp_HashSecret = "XLTMAZINYXOVQKRVTNEIXAJIRVANWGZN"; //Secret Key

                //VnPayLibrary vnpay = new VnPayLibrary();


                //vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                //vnpay.AddRequestData("vnp_Command", "pay");
                //vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                //vnpay.AddRequestData("vnp_Amount", Math.Floor(decimal.Parse(obj.FigureMoney.ToString()) * 100).ToString());

                //vnpay.AddRequestData("vnp_BankCode", "VNBANK");
                //vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                //vnpay.AddRequestData("vnp_CurrCode", "VND");
                //vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_httpContextAccessor));

                //vnpay.AddRequestData("vnp_Locale", "vn");


                //vnpay.AddRequestData("vnp_OrderInfo", "https://elder-care-api.monoinfinity.net/process-payment");
                //vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

                //vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                //vnpay.AddRequestData("vnp_TxnRef", obj.TransactionId.ToString());



                //string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                string paymentUrl = await _transactionService.CreateTransaction(dto, userid.AccountId,carerid);
                url= paymentUrl;
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Create success",
                    Data = paymentUrl
                }); ;
            }
            catch
            {
                return BadRequest("Error!");
            }
        }

        [HttpGet("/link-payment")]
        [Authorize]
        public async Task<IActionResult> LinkPayment()
        {
            try
            {
                var idClaim = _unitOfWork.AccountRepository.GetMemberIdFromToken(HttpContext.User);
                var userid = await _unitOfWork.AccountRepository.GetByIdAsync(idClaim);
                //var trans = _unitOfWork.TransactionRepo.GetLastestTransaction(userid.AccountId);

                //string vnp_Returnurl = "https://elder-care-api.monoinfinity.net/process-payment"; //URL nhan ket qua tra ve 
                //string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
                //string vnp_TmnCode = "NWYNAA42"; //Ma định danh merchant kết nối (Terminal Id)
                //string vnp_HashSecret = "XLTMAZINYXOVQKRVTNEIXAJIRVANWGZN"; //Secret Key

                //VnPayLibrary vnpay = new VnPayLibrary();


                //vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                //vnpay.AddRequestData("vnp_Command", "pay");
                //vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                //vnpay.AddRequestData("vnp_Amount", Math.Floor(decimal.Parse(trans.Result.FigureMoney.ToString()) * 100).ToString());

                //vnpay.AddRequestData("vnp_BankCode", "VNBANK");
                //vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                //vnpay.AddRequestData("vnp_CurrCode", "VND");
                //vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_httpContextAccessor));

                //vnpay.AddRequestData("vnp_Locale", "vn");


                //vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + trans.Result.TransactionId);
                //vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

                //vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                //vnpay.AddRequestData("vnp_TxnRef", trans.Result.TransactionId.ToString());



                //string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                string paymentUrl =  _transactionService.LinkPayment(userid.AccountId);

                return Ok(paymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/process-payment")]
        public async Task<IActionResult> ProcessPayment()
        {
            string returnContent = string.Empty;

            try
            {
                //string vnp_HashSecret = "XLTMAZINYXOVQKRVTNEIXAJIRVANWGZN";
                //var vnpayData = new Dictionary<string, string>();

                //foreach (var key in Request.Query.Keys)
                //{
                //    var values = Request.Query[key];
                //    if (values.Count > 0)
                //    {
                //        vnpayData[key] = values[0];
                //    }
                //}

                //VnPayLibrary vnpay = new VnPayLibrary();
                //foreach (var entry in vnpayData)
                //{
                //    string key = entry.Key;
                //    string value = entry.Value;

                //    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                //    {
                //        vnpay.AddResponseData(key, value);
                //    }
                //}


                //// Lấy thông tin từ Query String
                //long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                //long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                //long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                //string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                //string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                //string vnp_SecureHash = Request.Query["vnp_SecureHash"].ToString(); // Lấy giá trị của tham số vnp_SecureHash và chuyển đổi thành chuỗi

                //bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);


                //var trans = _unitOfWork.TransactionRepo.GetTransaction(orderId).Result;

          


                //if (checkSignature)
                //{
                //    if (trans != null)
                //    {
                //        if (trans.FigureMoney == vnp_Amount)
                //        {
                //            if (trans.Status == "PENDING")
                //            {
                //                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                //                {
                //                    // Thanh toán thành công
                //                    trans.Status = "APPROVE";
                //                    returnContent = "{\"RspCode\":\"00\",\"Message\":\"Confirm Success\"}";

                //                }
                //                else
                //                {
                //                    // Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                //                    trans.Status = "REJECT";
                //                    returnContent = "{\"RspCode\":\"02\",\"Message\":\"Order already confirmed\"}";
                //                }

                //                // Cập nhật thông tin đơn hàng vào CSDL
                //                await _unitOfWork.TransactionRepo.UpdateOrderInfoInDatabase(trans);
                //            }
                //            else
                //            {
                //                returnContent = "{\"RspCode\":\"02\",\"Message\":\"Order already confirmed\"}";
                //            }
                //        }
                //        else
                //        {
                //            returnContent = "{\"RspCode\":\"04\",\"Message\":\"invalid amount\"}";
                //        }
                //    }
                //    else
                //    {
                //        returnContent = "{\"RspCode\":\"01\",\"Message\":\"Order not found\"}";
                //    }
                //}
                //else
                //{
                //    returnContent = "{\"RspCode\":\"97\",\"Message\":\"Invalid signature\"}";
                //}
                returnContent = await _transactionService.ProcessPayment();
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                returnContent = "{\"RspCode\":\"99\",\"Message\":\"An error occurred\"}";
            }

            return Redirect(url);
        }
        [HttpGet("getTransactionHistoryByCarerId")]
        [EnableQuery]
        
        public async Task<IActionResult> GetCarerTransactionHistoryByCarer(int carerId)
        {
            try
            {
                //var transactionList = await _unitOfWork.CarerRepository.GetCarerTransactionHistoryAsync(carerId);
                //var carerTransactions = _mapper.Map<List<CarerTransactionDto>>(transactionList);
                //foreach (var transaction in carerTransactions)
                //{
                //    var carerCus = await _unitOfWork.CarerRepository.GetCarerCustomerFromIdAsync(transactionList[carerTransactions.IndexOf(transaction)].CarercusId);
                //    if(carerCus != null)
                //    {
                //        (transaction.CarerId, transaction.CustomerId) = (carerCus.CarerId, carerCus.CustomerId);
                //    }
                //}
                var carerTransactions = await _carerService.GetCarerTransactionHistoryAsyncByCarerId(carerId);
                return Ok(carerTransactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getTransactionHistoryByCustomerId")]
        [EnableQuery]
        public async Task<IActionResult> GetCarerTransactionHistoryByCus(int customerId)
        {
            try
            {
                //var transactionList = await _unitOfWork.CarerRepository.GetCarerTransactionHistoryAsync(carerId);
                //var carerTransactions = _mapper.Map<List<CarerTransactionDto>>(transactionList);
                //foreach (var transaction in carerTransactions)
                //{
                //    var carerCus = await _unitOfWork.CarerRepository.GetCarerCustomerFromIdAsync(transactionList[carerTransactions.IndexOf(transaction)].CarercusId);
                //    if(carerCus != null)
                //    {
                //        (transaction.CarerId, transaction.CustomerId) = (carerCus.CarerId, carerCus.CustomerId);
                //    }
                //}
                var carerTransactions = await _carerService.GetCarerTransactionHistoryAsyncByCustomerId(customerId);
                return Ok(carerTransactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}

