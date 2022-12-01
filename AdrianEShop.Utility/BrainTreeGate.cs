using Braintree;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Utility
{
    public class BrainTreeGate : IBrainTreeGate
    {
        public BrainTreeSettings Options { get; set; }

        private IBraintreeGateway _braintreeGateway;

        public BrainTreeGate(IOptions<BrainTreeSettings> options)
        {
            Options = options.Value;
        }

        public IBraintreeGateway CreateGateway()
        {
            _braintreeGateway = new BraintreeGateway(Options.Environment, Options.MerchantId, Options.PublicKey, Options.PrivateKey);
            return _braintreeGateway;
        }

        public IBraintreeGateway GetGateway()
        {
            if(_braintreeGateway == null)
            {
                _braintreeGateway = new BraintreeGateway(Options.Environment, Options.MerchantId, Options.PublicKey, Options.PrivateKey);
            }           
            return _braintreeGateway;
        }
    }
}
