using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;

//Creating path variables
string timePath = "time.txt";
string intPath = "intensity.txt";

//Creating collection variables and assigning values to them
List<int> times = [];
Dictionary<int, double> intensity = [];
Dictionary<int, double> mappedIntensity = [];
times = GetTimesData(timePath);
double timesAverage = CalculateAverage(times);
intensity = getIntensityData(intPath);
mappedIntensity = MapIntensity(intensity);

//Converting each key value pair to DataPoint class in order to draw a plot
List<DataPoint> points = [];
foreach (var item in mappedIntensity)
{
    points.Add(new DataPoint((item.Key), item.Value));
}

//Creating a new plot
var plotModel = new PlotModel { Title = "Godzina największego ruchu" };

//Drawing a graph with the points we previously calculated. Configuring the look of the graph
var intensitySeries = new LineSeries
{
    Title = "Godzina największego ruchu",
    MarkerStroke = OxyColors.White,
    MarkerFill = OxyColors.Black,
    Background = OxyColors.White,
    ItemsSource = points,
    Color = OxyColors.Blue
};
plotModel.Series.Add(intensitySeries);

//Modifying X axis to show hours in the format 12:00, instead of minutes
plotModel.Axes.Add(new LinearAxis
{
    Position = AxisPosition.Bottom,
    Title = "Godzina",
    LabelFormatter = label => TimeSpan.FromMinutes(label).ToString(@"hh\:mm"),
    Minimum = 0,
    Maximum = 24 * 60
});

//Modifying Y axis to show the data that we want
plotModel.Axes.Add(new LinearAxis
{
    Position = AxisPosition.Left,
    Title = "Intensywność",
    Minimum = 0,
});

//Creating a variable of PngExporter class, which is used to export plot into in our case .png file
var exporter = new PngExporter { Width = 800, Height = 600 };

//Creating a directory to which files will be saved 
Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\website");

//Creating .png file from our plot and exporting it
using (var stream = File.Create(@"website\chart.png"))
{
    exporter.Export(plotModel, stream);
}

//Functions to read data from the czas.txt file and then convert it to a List<>
List<int> GetTimesData(string path)
{
    try
    {
        using (var reader = new StreamReader(timePath))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                times.Add(int.Parse(line.Trim()));
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    return times;
}

//Function to read data from int.txt file and the convert it to a Dictionary<><>
Dictionary<int, double> getIntensityData(string path)
{
    try
    {
        using (var reader = new StreamReader(intPath))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                string fixedLine = line.Trim().Replace('\t', ' ');
                var parts = fixedLine.Split(' ');
                var minutes = int.Parse(parts[0]);
                var calls = double.Parse(parts[1]);
                intensity[minutes] = calls;
            }
        }
        Instructions();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    return intensity;
}

//Function to calcualte an average of the given List<>
double CalculateAverage(List<int> times)
{
   return times.Average();
}

//Function to convert the given Dictionary<><> to another Dictionary<><> with the modified values. In this example we modify the Value for each Key.
Dictionary<int, double> MapIntensity(Dictionary<int, double> intensity)
{
    Dictionary<int, double> mappedIntensity = [];
    foreach (var item in intensity)
    {
        mappedIntensity[item.Key] = (item.Value / timesAverage);
    }
    return mappedIntensity; 
}

//Function displaying loading bar and further instructions
void Instructions()
{
    Console.Write("|");
    for (int i = 0; i < 30; i++)
    {
        Console.Write("#");
        Thread.Sleep(80);
    }
    Console.Write("|");
    Thread.Sleep(300);
    Console.Clear();
    Console.WriteLine(">Obliczenia udane");
    Thread.Sleep(1000);
    Console.WriteLine(">Wykres wygenerowany");
    Thread.Sleep(1000);
    Console.WriteLine(">Strona utworzona");
    Thread.Sleep(1000);
    Console.WriteLine(">Aby przejść do wyników wejdź do folderu website i otwórz plik index.html");
}