using System.Diagnostics;
using System.Drawing;

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();
Bitmap image, bitmap;
string inp = "bwexit4k.bmp";
image = new Bitmap(inp);
bitmap = new Bitmap(image.Width, image.Height);
Console.WriteLine($"h - {image.Height}, w - {image.Width}");
for (int y = 0; y < image.Height - 1; ++y)
{
    for (int x = 0; x < image.Width-1; ++x)
    {
        var color1 = image.GetPixel(x, y);
        var color2 = image.GetPixel(x + 1, y + 1);
        var color3 = image.GetPixel(x + 1, y);
        var color4 = image.GetPixel(x, y + 1);
        
        byte tmp1 = (byte)(color1.R + color1.G + color1.B);
        byte tmp2 = (byte)(color2.R + color2.G + color2.B);
        byte tmp3 = (byte)(color3.R + color3.G + color3.B);
        byte tmp4 = (byte)(color4.R + color4.G + color4.B);
        
        var res1 = tmp1 - tmp2;
        var res2 = tmp3 - tmp4;
        
        var result = (byte)(Math.Sqrt(Math.Pow(res1,2) + Math.Pow(res2,2)));
        bitmap.SetPixel(x, y, Color.FromArgb(color1.A, result, result, result));
    }    
}

//int n = Convert.ToInt32(Console.ReadLine());
int n = 2;
// Увелечение 
Bitmap newImage = new Bitmap(image.Width * n, image.Height * n);
Console.WriteLine($"h - {newImage.Height}, w - {newImage.Width}");
//Console.ReadLine();
for (int y = 0; y < bitmap.Height; y++)
{ 
    for (int x = 0; x < bitmap.Width; x++)
    {
        Color color = bitmap.GetPixel(x, y);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                newImage.SetPixel((x*n)+i,(y*n)+j,color);
            }
        }
            
    }
}
newImage.Save("test1.bmp");
bitmap.Save("image.bmp");
stopwatch.Stop();

TimeSpan ts = stopwatch.Elapsed;
string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds / 10);
Console.WriteLine($"Time: {elapsedTime}");