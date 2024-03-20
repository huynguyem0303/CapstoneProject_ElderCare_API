using AutoMapper;
using CorePush.Apple;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Service.Interfaces;
using ElderCare_Service.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly static string VNP_RETURNURL = "https://elder-care-api.monoinfinity.net/process-payment"; //URL nhan ket qua tra ve 
        private readonly static string VNP_URL = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
        private readonly static string VNP_TMNCODE = "NWYNAA42"; //Ma định danh merchant kết nối (Terminal Id)
        private readonly static string VNP_HASHSECRET = "XLTMAZINYXOVQKRVTNEIXAJIRVANWGZN"; //Secret Key

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> CreateTransaction(TrasactionDto dto, int accountId,int carerid,int cusid,int contractid)
        { 
            dto.DateTime = DateTime.Now;
            var id = _unitOfWork.TransactionRepo.GetAll().OrderByDescending(i => i.TransactionId).FirstOrDefault().TransactionId;
            Transaction? obj = _mapper.Map<Transaction>(dto);
            obj.AccountId = accountId;
            obj.TransactionId = id + 1;
            var check = _unitOfWork.CarerRepository.FindAsync(carerid, cusid);
            if (check.Result == null)
            {
                CarersCustomer carersCustomer = new CarersCustomer();
                carersCustomer.Datetime = DateTime.Now;
                carersCustomer.CarerId = carerid;
                carersCustomer.CustomerId = cusid;
                var carecusid = _unitOfWork.CarerRepository.GetLastest().Result.CarercusId;
                carersCustomer.CarercusId = carecusid + 1;
                await _unitOfWork.CarerRepository.AddCarerCusAsync(carersCustomer);
                obj.CarercusId = carersCustomer.CarercusId;
            }
            if (check.Result != null)
            {
                obj.CarercusId=check.Result.CarercusId;
            }
           
            obj.Status = "PENDING";
            if (dto.Type.Equals(TransactionType.CusContract))
            {
                obj.ContractId = contractid;
            }
            await _unitOfWork.TransactionRepo.AddAsync(obj);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                string vnp_Returnurl = dto.RedirectUrl; //URL nhan ket qua tra ve 
                //string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
                //string vnp_TmnCode = "NWYNAA42"; //Ma định danh merchant kết nối (Terminal Id)
                //string vnp_HashSecret = "XLTMAZINYXOVQKRVTNEIXAJIRVANWGZN"; //Secret Key

                VnPayLibrary vnpay = new VnPayLibrary();


                vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", VNP_TMNCODE);
                vnpay.AddRequestData("vnp_Amount", Math.Floor(decimal.Parse(obj.FigureMoney.ToString()) * 100).ToString());

                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
                vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_IpAddr", Utils.Utils.GetIpAddress(_httpContextAccessor));

                vnpay.AddRequestData("vnp_Locale", "vn");


                vnpay.AddRequestData("vnp_OrderInfo", "https://elder-care-api.monoinfinity.net/process-payment");
                vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                vnpay.AddRequestData("vnp_TxnRef", obj.TransactionId.ToString());



                string paymentUrl = vnpay.CreateRequestUrl(VNP_URL, VNP_HASHSECRET);
                return paymentUrl;
            }
            catch
            {
                throw new Exception("Error!");
            }
        }

        public string LinkPayment(int accountId)
        {
            var trans = _unitOfWork.TransactionRepo.GetLastestTransaction(accountId);

            //string vnp_Returnurl = "https://elder-care-api.monoinfinity.net/process-payment"; //URL nhan ket qua tra ve 
            //string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
            //string vnp_TmnCode = "NWYNAA42"; //Ma định danh merchant kết nối (Terminal Id)
            //string vnp_HashSecret = "XLTMAZINYXOVQKRVTNEIXAJIRVANWGZN"; //Secret Key

            VnPayLibrary vnpay = new VnPayLibrary();


            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", VNP_TMNCODE);
            vnpay.AddRequestData("vnp_Amount", Math.Floor(decimal.Parse(trans.Result.FigureMoney.ToString()) * 100).ToString());

            vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.Utils.GetIpAddress(_httpContextAccessor));

            vnpay.AddRequestData("vnp_Locale", "vn");


            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + trans.Result.TransactionId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", VNP_RETURNURL);
            vnpay.AddRequestData("vnp_TxnRef", trans.Result.TransactionId.ToString());



            string paymentUrl = vnpay.CreateRequestUrl(VNP_URL, VNP_HASHSECRET);
            return paymentUrl;
        }

        public async Task<string> ProcessPayment()
        {
            string returnContent;
            try
            {
                //string vnp_HashSecret = "XLTMAZINYXOVQKRVTNEIXAJIRVANWGZN";
                var vnpayData = new Dictionary<string, string>();

                foreach (var key in _httpContextAccessor.HttpContext.Request.Query.Keys)
                {
                    var values = _httpContextAccessor.HttpContext.Request.Query[key];
                    if (values.Count > 0)
                    {
                        vnpayData[key] = values[0];
                    }
                }

                VnPayLibrary vnpay = new VnPayLibrary();
                foreach (var entry in vnpayData)
                {
                    string key = entry.Key;
                    string value = entry.Value;

                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(key, value);
                    }
                }


                // Lấy thông tin từ Query String
                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                string vnp_SecureHash = _httpContextAccessor.HttpContext.Request.Query["vnp_SecureHash"].ToString(); // Lấy giá trị của tham số vnp_SecureHash và chuyển đổi thành chuỗi

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, VNP_HASHSECRET);

                var trans = await _unitOfWork.TransactionRepo.GetTransaction(orderId);

                if (checkSignature)
                {
                    if (trans != null)
                    {
                        if (trans.FigureMoney == vnp_Amount)
                        {
                            if (trans.Status == "PENDING")
                            {
                                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                                {
                                    // Thanh toán thành công
                                    trans.Status = "APPROVE";
                                    if (trans.Type == 1)
                                    {
                                        var contract = _unitOfWork.ContractRepository.FindAsync(x => x.ContractId == trans.ContractId).Result.FirstOrDefault() ?? throw new Exception("contract not found");
                                        contract.Status = (int)ContractStatus.Active;
                                        _unitOfWork.ContractRepository.Update(contract);
                                    }
                                    returnContent = "{\"RspCode\":\"00\",\"Message\":\"Confirm Success\"}";

                                }
                                else
                                {
                                    // Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                                    trans.Status = "REJECT";
                                    returnContent = "{\"RspCode\":\"02\",\"Message\":\"Order already confirmed\"}";
                                }

                                // Cập nhật thông tin đơn hàng vào CSDL
                                await _unitOfWork.TransactionRepo.UpdateOrderInfoInDatabase(trans);
                               
                            }
                            else
                            {
                                returnContent = "{\"RspCode\":\"02\",\"Message\":\"Order already confirmed\"}";
                            }
                        }
                        else
                        {
                            returnContent = "{\"RspCode\":\"04\",\"Message\":\"invalid amount\"}";
                        }
                    }
                    else
                    {
                        returnContent = "{\"RspCode\":\"01\",\"Message\":\"Order not found\"}";
                    }
                }
                else
                {
                    returnContent = "{\"RspCode\":\"97\",\"Message\":\"Invalid signature\"}";
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                returnContent = "{\"RspCode\":\"99\",\"Message\":\"An error occurred\"}";
            }
            return returnContent;
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _unitOfWork.TransactionRepo.GetAll();
        }
    }
}
