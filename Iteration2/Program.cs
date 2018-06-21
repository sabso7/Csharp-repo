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

            List <RootObject> listObjet = JsonConvert.DeserializeObject<List<RootObject>>(responseInfoBus);
            List<BusDetails> busdetails = JsonConvert.DeserializeObject<List<BusDetails>>(responseDetailsBus);
            List<RootObject> busNames = listObjet.GroupBy(busname => busname.name).Select(x => x.First()).ToList();

            foreach (RootObject name in busNames) {

                foreach (RootObject bustop in listObjet) {

                    if (name.name.Equals(bustop.name))
                    {
                        IEnumerable<string> newLines = name.lines.Union(bustop.lines);
                        name.GetType().GetProperty("lines").SetValue(name, newLines);
                    }
                }
            }
            foreach (RootObject name in busNames)
            { 
                WriteLine($"=====================  \n Nom : {name.name}   \n Id : {name.id} \n Lat: {name.lat} \n Lon :  {name.lon}");

                foreach (String line in name.lines.Distinct())
                {
                    WriteLine($" lignes : {line}");

                    foreach (BusDetails x in busdetails.Where(x => x.id == line).ToList())
                    {
                       WriteLine($" couleur : {x.color} \n Nom complet : { x.longName}    \n Type : {x.type} ");

                    }
                }
            }

            ReadKey();
        }
    }
}
