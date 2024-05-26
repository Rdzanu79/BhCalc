using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhCalcApp
{
    internal class GenerateWebsite
    {
        //Variables holding the html and css content
        const string html = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n<meta charset=\"UTF-8\">\r\n<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n<link rel=\"stylesheet\" href=\"style.css\">\r\n<title>Rush hour</title>\r\n</head>\r\n<body>\r\n<div id=\"chart\">\r\n<img src=\"chart.png\" alt=\"Rush hour chart\"/>\r\n</div>\r\n</body>\r\n</html>";
        const string css = "body { background-color: #0d0d0d; color: #fff } #chart {display: flex; justify-content: center; align-items: center; width: 100%; }";
        //Static method to generate a html and css files to a specified directory
        static public void GenerateHTML(string HtmlPath, string CssPath)
        {
            using (var stream = File.CreateText(HtmlPath))
            {
                stream.Write(html);
            }
            using (var stream = File.CreateText(CssPath))
            {
                stream.Write(css);
            }
        }
    }
}
