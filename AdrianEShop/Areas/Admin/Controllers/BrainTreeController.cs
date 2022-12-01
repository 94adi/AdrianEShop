using AdrianEShop.Utility;
using Braintree;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdrianEShop.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class BrainTreeController : Controller
    {

        private IBrainTreeGate _brainTreeGate;

        public BrainTreeController(IBrainTreeGate brainTreeGate)
        {
            _brainTreeGate = brainTreeGate;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var gateway = _brainTreeGate.GetGateway();
            var clientToken = gateway.ClientToken.Generate();
            ViewBag.ClientToken = clientToken;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IFormCollection collection)
        {
            Random rnd = new Random();
            string nonceFromTheClient = collection["payment_method_nonce"];
            var request = new TransactionRequest
            {
                Amount = rnd.Next(1, 100),
                PaymentMethodNonce = nonceFromTheClient,
                OrderId = "55151",
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            var gateway = _brainTreeGate.GetGateway();
            Result<Transaction> result = gateway.Transaction.Sale(request);

            if (result.Target.ProcessorResponseText == "Approved")
            {
                TempData["Success"] = "Transaction was successful Transaction ID " + result.Target.Id + ", Amount Charged : $" + result.Target.Amount;
            }

            return RedirectToAction("Index");
        }
    }
}
