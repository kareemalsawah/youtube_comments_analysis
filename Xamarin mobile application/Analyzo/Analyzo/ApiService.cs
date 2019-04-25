using System;
using System.Collections.Generic;
using System.Text;
using Refit;

// This class will be used to create the RestService to use to access the server using our api
namespace Analyzo
{
    class ApiService
    {
        public static IAnalysisApi analysis_api;
        public static string baseUrl = "https://hollow-orange.glitch.me";
        public static IAnalysisApi getApiService()
        {
            analysis_api = RestService.For<IAnalysisApi>(baseUrl);
            return analysis_api;
        }
    }
}
