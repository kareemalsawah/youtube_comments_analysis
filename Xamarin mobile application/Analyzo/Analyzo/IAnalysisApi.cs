using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;

// Interface of our post method and the data type of the data sent to the server (Dictionary<string,object>)
namespace Analyzo
{
    interface IAnalysisApi
    {
        [Post("/sentiment_analysis")]
        Task<AnalysisRes> getAnalysis([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);
    }
}
