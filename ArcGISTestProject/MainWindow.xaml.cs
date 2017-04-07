using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.Query;
using Esri.ArcGISRuntime.Tasks.NetworkAnalyst;
using Esri.ArcGISRuntime.Tasks.Geocoding;

namespace ArcGISTestProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowsViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowsViewModel();
            this.DataContext = viewModel;
            viewModel.AddMap("Projects");
            viewModel.AddMap("Feature");
            viewModel.AddMap("Route");
            viewModel.AddMap("Map 1");
            viewModel.AddMap("Map 3");
        }

        private async void BtnClevlandGo_Click(object sender, RoutedEventArgs e)
        {
            Envelope clevlandEnvelope = new Envelope(-9113800, 5077300, -9078300, 5091800);
            await mapView.SetViewAsync(clevlandEnvelope);
        }
        
        private void cBoxBaseMaps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnGetParvelAddress.Visibility = Visibility.Collapsed;
            pnlParcelAddressInfo.Visibility = Visibility.Collapsed;
            btnGetRoute.Visibility = Visibility.Collapsed;
            txtFrom.Visibility = Visibility.Collapsed;
            txtTo.Visibility = Visibility.Collapsed;

            Esri.ArcGISRuntime.Controls.Map map = new Esri.ArcGISRuntime.Controls.Map();
            Esri.ArcGISRuntime.Layers.ArcGISTiledMapServiceLayer baseMaplayer = new Esri.ArcGISRuntime.Layers.ArcGISTiledMapServiceLayer();
            baseMaplayer.ServiceUri = "http://services.arcgisonline.com/arcgis/rest/services/World_Street_Map/MapServer";
            baseMaplayer.ID = "BaseMap";

            map.Layers.Add(baseMaplayer);

            if (cBoxBaseMaps.SelectedItem.ToString() == "Projects")
            {
                ArcGISDynamicMapServiceLayer layer2 = new ArcGISDynamicMapServiceLayer();
                layer2.ServiceUri = "http://192.168.1.47:6080/arcgis/rest/services/PROJECTS/MapServer";
                layer2.ID = "Projects";
                map.Layers.Add(layer2);
            }
            else if (cBoxBaseMaps.SelectedItem.ToString() == "Feature")
            {
                btnGetParvelAddress.Visibility = Visibility.Visible;
                pnlParcelAddressInfo.Visibility = Visibility.Visible;
                ServiceFeatureTable featureTable = new ServiceFeatureTable();
                featureTable.ServiceUri = "http://sampleserver6.arcgisonline.com/arcgis/rest/services/PoolPermits/FeatureServer/0";
                featureTable.Where = "has_pool = 1 AND pool_permit = 0";
                FeatureLayer layer = new FeatureLayer(featureTable);
                SimpleRenderer renderer = new SimpleRenderer();
                renderer.Symbol = new SimpleFillSymbol() { Color = Color.FromRgb(66, 134, 244), Style = SimpleFillStyle.DiagonalCross };
                layer.Renderer = renderer;

                map.InitialViewpoint = new Viewpoint(new Envelope(-13075816.40, 4014771.46, -13073005.67, 4016869.78));

                layer.ID = "Feature";
                map.Layers.Add(layer);
            }
            else if (cBoxBaseMaps.SelectedItem.ToString() == "Route") {
                btnGetRoute.Visibility = Visibility.Visible;
                txtFrom.Visibility = Visibility.Visible;
                txtTo.Visibility = Visibility.Visible;

                GraphicsLayer layer2 = new GraphicsLayer();
                layer2.ID = "Graphics";
                map.Layers.Add(layer2);
            }
            else if (cBoxBaseMaps.SelectedItem.ToString() == "Map 1")
            {
                map = new Map();
                ArcGISTiledMapServiceLayer ArcGISTiledMapServiceLayer1 = new ArcGISTiledMapServiceLayer();
                ArcGISTiledMapServiceLayer1.ServiceUri = "http://services.arcgisonline.com/ArcGIS/rest/services/Canvas/World_Light_Gray_Base/MapServer";
                ArcGISTiledMapServiceLayer1.ID = "World_Light_Gray_Base";

                Esri.ArcGISRuntime.Layers.ArcGISDynamicMapServiceLayer ArcGISDynamicMapServiceLayer1 = new Esri.ArcGISRuntime.Layers.ArcGISDynamicMapServiceLayer();
                ArcGISDynamicMapServiceLayer1.ServiceUri = "http://services.arcgisonline.com/ArcGIS/rest/services/Demographics/USA_1990-2000_Population_Change/MapServer";
                ArcGISDynamicMapServiceLayer1.ID = "USA_1990-2000_Population_Change";

                map.Layers.Add(ArcGISTiledMapServiceLayer1);
                map.Layers.Add(ArcGISDynamicMapServiceLayer1);

                mapView.Map = map;
            }
            else if (cBoxBaseMaps.SelectedItem.ToString() == "Map 3")
            {
                map = new Esri.ArcGISRuntime.Controls.Map();
                Esri.ArcGISRuntime.Layers.ArcGISTiledMapServiceLayer ArcGISTiledMapServiceLayer3 = new Esri.ArcGISRuntime.Layers.ArcGISTiledMapServiceLayer();
                ArcGISTiledMapServiceLayer3.ServiceUri = "http://services.arcgisonline.com/ArcGIS/rest/services/Canvas/World_Light_Gray_Base/MapServer";
                ArcGISTiledMapServiceLayer3.ID = "World_Light_Gray_Base";

                Esri.ArcGISRuntime.Layers.FeatureLayer FeatureLayer3 = new Esri.ArcGISRuntime.Layers.FeatureLayer();
                Esri.ArcGISRuntime.Data.ServiceFeatureTable ServiceFeatureTable3 = new Esri.ArcGISRuntime.Data.ServiceFeatureTable();
                ServiceFeatureTable3.ServiceUri = "http://sampleserver3.arcgisonline.com/ArcGIS/rest/services/HomelandSecurity/operations/FeatureServer/0";
                ServiceFeatureTable3.OutFields = Esri.ArcGISRuntime.Tasks.Query.OutFields.All;
                FeatureLayer3.FeatureTable = ServiceFeatureTable3;

                map.Layers.Add(ArcGISTiledMapServiceLayer3);
                map.Layers.Add(FeatureLayer3);

                mapView.Map = map;
            }

            mapView.Map = map;
        }

        private async void DrawRoute(params Graphic[] graphics) {
            try
            {
                var routeTask = new OnlineRouteTask(new Uri("http://route.arcgis.com/arcgis/rest/services/World/Route/NAServer/Route_World"));
                var routeParams = await routeTask.GetDefaultParametersAsync();
                routeParams.OutSpatialReference = mapView.SpatialReference;
                routeParams.DirectionsLengthUnit = LinearUnits.Kilometers;
                routeParams.ReturnDirections = true;
                routeParams.SetStops(graphics);
                var routeResult = await routeTask.SolveAsync(routeParams);

                if (routeResult == null || routeResult.Routes == null || routeResult.Routes.Count == 0)
                {
                    throw new Exception("Невозможно расчитать маршрут");
                }
                var routeGraphic = new Graphic(routeResult.Routes[0].RouteFeature.Geometry, new SimpleLineSymbol { Color = Color.FromRgb(0, 0, 0), Style = SimpleLineStyle.Dash, Width = 2 });
                ((GraphicsLayer)mapView.Map.Layers["Graphics"]).Graphics.Add(routeGraphic);
                await mapView.SetViewAsync(routeGraphic.Geometry.Extent);
            }
            catch (Exception e) {
                System.Diagnostics.Debug.Write(e.ToString());
            }
        }

        private async void btnGetParvelAddress_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MapPoint mapPoint = await mapView.Editor.RequestPointAsync();
                string poolPermitUrl = "http://sampleserver6.arcgisonline.com/arcgis/rest/services/PoolPermits/FeatureServer/0";
                QueryTask queryTask = new QueryTask(new Uri(poolPermitUrl));
                Query queryFilter = new Query("apn");
                queryFilter.OutFields.Add("apn");
                queryFilter.OutFields.Add("address");
                var queryResult = await queryTask.ExecuteAsync(queryFilter);
                if (queryResult.FeatureSet.Features.Count > 0)
                {
                    var resultGraphic = queryResult.FeatureSet.Features[0] as Graphic;
                    txtApn.Text = resultGraphic.Attributes["apn"].ToString();
                    txtBlcAddress.Text = resultGraphic.Attributes["address"].ToString();
                }
            }
            catch (TaskCanceledException exc) {
                
            }
        }

        private async Task<Graphic> FindAddress(string address) {
            var uri = new Uri("http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer");
            var locator = new OnlineLocatorTask(uri, string.Empty);
            var findParams = new OnlineLocatorFindParameters(address);
            findParams.OutSpatialReference = new SpatialReference(4326);
            Graphic matchGraphic = new Graphic();
            var results = await locator.FindAsync(findParams, new System.Threading.CancellationToken());
            if (results.Count > 0) {
                matchGraphic.Geometry = results[0].Feature.Geometry;
                matchGraphic.Attributes.Add("Name", address);
            }
            return matchGraphic;
        }

        private async void btnGetRoute_Click(object sender, RoutedEventArgs e)
        {
            var from = FindAddress(txtFrom.Text);
            var to = FindAddress(txtTo.Text);

            DrawRoute(await from, await to);
        }
    }
}
