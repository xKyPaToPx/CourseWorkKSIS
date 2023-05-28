using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

MPI.Environment.Run(ref args, comm =>
{
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();
    Bitmap image, bitmap;
    string inp = "bwexit4k.bmp";
    image = new Bitmap(inp);
    bitmap = new Bitmap(image.Width, image.Height);
    int newIndex = 0;
    switch (comm.Rank)
    {
        case 0:
            newIndex = 0;
            break;
        case 1:
            newIndex = 1000;
            break;
        case 2:
            newIndex = 2000;
            break;
        case 3:
            newIndex = 3000;
            break;
    }

    for (int y = newIndex; y < newIndex + (image.Height/4) -1; ++y)
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


    int n = 2;//размер увелечения
    
    
// Увелечение 
    Bitmap newImage = new Bitmap(image.Width * n, image.Height * n);
    for (int y = newIndex; y < newIndex + (image.Height/4); y++)
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

    Bitmap[] b1 = comm.Gather(newImage, 0);
    
    if (comm.Rank == 0)
    {
        Bitmap res = new Bitmap(image.Width * n, image.Height * n);
        var gr = Graphics.FromImage(res);
        gr.DrawImage(b1[0],0,0);
        gr.DrawImage(b1[1],0,0);
        gr.DrawImage(b1[2],0,0);
        gr.DrawImage(b1[3],0,0);
        res.Save($"RES.bmp");
        TimeSpan ts = stopwatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine($"Time: {elapsedTime}");
    }
    
});
