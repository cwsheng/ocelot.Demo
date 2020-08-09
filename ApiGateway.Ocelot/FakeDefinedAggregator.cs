using Microsoft.AspNetCore.Http;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Ocelot
{
    public class FakeDefinedAggregator : IDefinedAggregator
    {
        public FakeDefinedAggregator()
        {
        }
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            List<string> result = new List<string>();
            foreach (var item in responses)
            {
                byte[] tmp = new byte[item.Response.Body.Length];
                await item.Response.Body.ReadAsync(tmp, 0, tmp.Length);
                var val = Encoding.UTF8.GetString(tmp);
                result.Add(val);
            }
            var merge = string.Join(";", result.ToArray());
            List<Header> headers = new List<Header>();
            return new DownstreamResponse(new StringContent(merge), HttpStatusCode.OK, headers, "some reason");
        }
    }
}
