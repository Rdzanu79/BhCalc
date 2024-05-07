using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;
using RushHourCalculator;

//Creating path variables
string timePath = @"src\time.txt";
string intPath = @"src\intensity.txt";

//Creating collection variables and assigning values to them
List<int> times = [];
Dictionary<int, double> intensity = [];
Dictionary<int, double> mappedIntensity = [];
times = GetTimesData(timePath);
intensity = getIntensityData(intPath);

//Checking if the List and Dictionary aren't empty
if (times.Count != 0 && intensity.Count != 0)
{
    //Modifying the data
    double timesAverage = times.Average();
    mappedIntensity = MapIntensity(intensity, timesAverage);

    //Converting each key value pair to DataPoint class
    List<DataPoint> points = [];
    foreach (var item in mappedIntensity)
    {
        points.Add(new DataPoint((item.Key), item.Value));
    }
    //Creating a list of following max Y value DataPoints
    List<DataPoint> maxPoints = [];
    maxPoints = GetMaximumPoints(points);

    //Creating a new plot
    var plotModel = new PlotModel
    {
        Title = "Godzina największego ruchu",
        TitleColor = OxyColors.White,
    };

    //Drawing a graph with the calculated points. Configuring the look of the graph
    var intensitySeries = new LineSeries
    {
        Title = "Godzina największego ruchu",
        MarkerStroke = OxyColors.White,
        MarkerFill = OxyColors.Black,
        Background = OxyColors.White,
        ItemsSource = points,
        Color = OxyColors.SteelBlue,
    };
    plotModel.Series.Add(intensitySeries);

    //Modifying X axis to show hours in the format 24:00, instead of minutes
    plotModel.Axes.Add(new LinearAxis
    {
        Position = AxisPosition.Bottom,
        Title = "Godzina",
        LabelFormatter = label => TimeSpan.FromMinutes(label).ToString(@"hh\:mm"),
        Minimum = 0,
        Maximum = 24 * 60,
        TitleColor = OxyColors.White,
        TextColor = OxyColors.White,
        TicklineColor = OxyColors.White,

    });

    //Modifying Y axis to show the needed data
    plotModel.Axes.Add(new LinearAxis
    {
        Position = AxisPosition.Left,
        Title = "Natężenie ruchu",
        Minimum = 0,
        TitleColor = OxyColors.White,
        TextColor = OxyColors.White,
        TicklineColor = OxyColors.White,
    });

    //Vertical line at the highest Y value DataPoint from the left
    var maxLineAnnotationLeft = new LineAnnotation
    {
        Type = LineAnnotationType.Vertical,
        X = maxPoints[0].X,
        Y = plotModel.Axes[0].Minimum,
        Color = OxyColors.Red,
        StrokeThickness = 2
    };
    plotModel.Annotations.Add(maxLineAnnotationLeft);

    //Vertical line at the next highest Y value DataPoint
    var maxLineAnnotationRight = new LineAnnotation
    {
        Type = LineAnnotationType.Vertical,
        X = maxPoints[1].X,
        Y = plotModel.Axes[0].Minimum,
        Color = OxyColors.Red,
        StrokeThickness = 2
    };
    plotModel.Annotations.Add(maxLineAnnotationRight);

    //Text annotation for the interval
    string intervalAverage = GetIntervalAverage(points, maxPoints).ToString("F3");
    string text = $"Przedział godzin: {TimeSpan.FromMinutes(maxPoints[0].X):hh\\:mm} - {TimeSpan.FromMinutes(maxPoints[1].X):hh\\:mm}\nŚrednie natężenie ruchu: {intervalAverage}";
    var leftLineTextAnnotation = new TextAnnotation
    {
        Text = text,
        FontSize = 15,
        TextColor = OxyColors.DarkRed,
        TextPosition = new DataPoint(40, 0.335),
        TextHorizontalAlignment = HorizontalAlignment.Left,
        TextVerticalAlignment = VerticalAlignment.Top
    };
    plotModel.Annotations.Add(leftLineTextAnnotation);

    //Creating a variable of PngExporter class, which is used to export plot into .png file
    var exporter = new PngExporter { Width = 800, Height = 600 };

    //Creating a directory to which files will be saved 
    Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\website");

    //Creating .png file from the plot and exporting it
    using (var stream = File.Create(@"website\chart.png"))
    {
        exporter.Export(plotModel, stream);
    }
    //Generating website files
    GenerateWebsite.GenerateHTML(@"website\index.html", @"website\style.css");
    Instructions();
}

//Function to read data from the czas.txt file and then convert it to a List<>
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
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    return intensity;
}

//Function to convert the given Dictionary<><> to another Dictionary<><> with the modified values
Dictionary<int, double> MapIntensity(Dictionary<int, double> intensity, double timesAverage)
{
    Dictionary<int, double> mappedIntensity = [];
    foreach (var item in intensity)
    {
        mappedIntensity[item.Key] = (item.Value * timesAverage);
    }
    return mappedIntensity;
}

//Function to get the interval between two following highest Y value DataPoints
List<DataPoint> GetMaximumPoints(List<DataPoint> points)
{
    List<DataPoint> maxPoints = [];
    double biggestYLeft = 0;
    double xLeft = 0;
    double nextBiggestYLeft = 0;
    double nextXLeft = 0;

    points.Sort((a, b) => a.X.CompareTo(b.X));

    foreach (var point in points)
    {
        if (point.Equals(points[0]))
        {
            biggestYLeft = point.Y;
            xLeft = point.X;
        }
        else
        {
            if (point.Y > biggestYLeft)
            {
                nextBiggestYLeft = biggestYLeft;
                nextXLeft = xLeft;
                biggestYLeft = point.Y;
                xLeft = point.X;
            }
            else if (point.Y > nextBiggestYLeft)
            {
                nextBiggestYLeft = point.Y;
                nextXLeft = point.X;
            }
        }
    }
    maxPoints.Add(new DataPoint(xLeft, biggestYLeft));
    maxPoints.Add(new DataPoint(nextXLeft, nextBiggestYLeft));
    return maxPoints;
}

//Function to calculate an average inside the interval
double GetIntervalAverage(List<DataPoint> points, List<DataPoint> maxPoints)
{
    int startIndex = points.FindIndex(p => p.X == maxPoints[0].X);
    int endIndex = points.FindIndex(p => p.X == maxPoints[1].X);
    int counter = 0;
    double average = 0;
    for (int i = startIndex; i <= endIndex; i++)
    {
        average += points[i].Y;
        counter++;
    }
    return average / counter;
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