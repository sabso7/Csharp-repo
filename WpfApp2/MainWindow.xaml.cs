
using ClassLibrary1;
using Iteration2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Microsoft.Maps.MapControl.WPF;
using System.Globalization;

namespace WpfApp2
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private AppelApi _appelApi;     

        public void boutonClick(object sender, RoutedEventArgs e)
        {
            string lat = latitude.Text;
            String lon = longitude.Text;
            String dist = distance.Text;

            _appelApi = new AppelApi();

            String responseInfoBus = _appelApi.GetApiInfoBus($"https://data.metromobilite.fr/api/linesNear/json?x={lon}&y={lat}&dist={dist}&details=true");
            String responseDetailsBus = _appelApi.GetApiInfoBus("https://data.metromobilite.fr/api/routers/default/index/routes");
            List<RootObject> listObjet = JsonConvert.DeserializeObject<List<RootObject>>(responseInfoBus);
            List<BusDetails> busdetails = JsonConvert.DeserializeObject<List<BusDetails>>(responseDetailsBus);
            List<RootObject> busNames = listObjet.GroupBy(busname => busname.name).Select(x => x.First()).ToList();

          
            foreach (RootObject name in busNames)
            {
                foreach (RootObject bustop in listObjet)
                {
                    if (name.name.Equals(bustop.name))
                    {
                        IEnumerable<string> newLines = name.lines.Union(bustop.lines);
                        name.GetType().GetProperty("lines").SetValue(name, newLines);
                    }
                }
            }
 
            listbus.Items.Clear();
            map.Children.Clear();
           
            foreach (RootObject name in busNames )
            {
                listbus.Items.Add(name.name);

                Location location2 = new Location(name.lat , name.lon);
                Pushpin pin = new Pushpin();
                pin.Location = location2;
              
                map.Children.Add(pin);
                string line1 = "";

                foreach (String line in name.lines.Distinct())
                {
                   
                    line1 += $"\n{line}";
                    
                    foreach (BusDetails x in busdetails.Where(x => x.id == line).ToList())
                    {
                       
                    }
                }

                ToolTipService.SetToolTip(pin, $"{name.name} {line1}");

            }

            double lat2 = Convert.ToDouble(lat, new CultureInfo("en-GB"));
            double lon2 = Convert.ToDouble(lon, new CultureInfo("en-GB"));
            map.Center = new Location( lat2 , lon2);

            MapLayer pinHere = new MapLayer();
            Image image = new Image();
            image.Height = 150;
            //Define the URI location of the image
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri("http://www.stickpng.com/assets/images/5897005ccba9841eabab6102.png");
            myBitmapImage.DecodePixelHeight = 40;
            myBitmapImage.EndInit();
            image.Source = myBitmapImage;
            image.Opacity = 1;
            image.Stretch = System.Windows.Media.Stretch.None;
            String here = "vous êtes ICI";
            Location location = new Location(map.Center);
            PositionOrigin position = PositionOrigin.Center;
            pinHere.AddChild(image, location, position );
            map.Children.Add(pinHere);

           
        }

        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            e.Handled = true;

           

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a locatoin on the map
            Location pinLocation = map.ViewportPointToLocation(mousePosition);

            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;

          
          
        }


        public MainWindow()
        {
            InitializeComponent();
        }      
  }
}
