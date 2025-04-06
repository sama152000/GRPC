using Grpc.Core;
using PaymentRPC.Protos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentRPC.Services
{
    public class PaymentServiceImpl : Payment.PaymentBase
    {
        private static readonly Dictionary<string, double> ClientBalances = new Dictionary<string, double>
        {
            { "100", 1000.0 },
            { "101", 500.0 }
        };

        public override Task<PayResponse> Pay(PayRequest request, ServerCallContext context)
        {
            if (!ClientBalances.ContainsKey(request.ClientId))
            {
                return Task.FromResult(new PayResponse
                {
                    Success = false,
                    Message = $"Client {request.ClientId} not found!"
                });
            }

            if (ClientBalances[request.ClientId] < request.Amount)
            {
                return Task.FromResult(new PayResponse
                {
                    Success = false,
                    Message = $"Insufficient balance. Available: {ClientBalances[request.ClientId]}"
                });
            }

            ClientBalances[request.ClientId] -= request.Amount;

            return Task.FromResult(new PayResponse
            {
                Success = true,
                Message = $"Payment successful. Remaining balance: {ClientBalances[request.ClientId]}"
            });
        }
    }
}