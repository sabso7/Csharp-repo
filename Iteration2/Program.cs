using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using Newtonsoft.Json;
using System.Collections;
using ClassLibrary1;

namespace Iteration2
{
    class Program
    {
        static void Main(string[] args)
        {

            AppelApi appelApi = new AppelApi();
           
         String responseInfoBus = appelApi.GetApiInfoBus("https://data.metromobilite.fr/api/linesNear/json?x=5.709360123&y=45.176494599999984&dist=1000&details=true");
         String responseDetailsBus = appelApi.GetApiInfoBus("https://data.metromobilite.fr/api/routers/default/index/routes");

           

            ReadKey();
        }
    }
}
