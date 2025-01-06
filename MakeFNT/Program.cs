using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace MakeFNT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: makefnt .bmp");
                return;
            }
            string readName = args[0];
            if (!File.Exists(readName))
            {
                Console.WriteLine($"File {readName} not found.");

                return;
            }
            Bitmap bmp = (Bitmap)Image.FromFile(readName);

            int fx = bmp.Width / 16;
            int fy = bmp.Height / 16;
            Console.WriteLine($"/* {readName} chr size={fx}x{fy} */");
            Console.WriteLine($"unsigned char {Path.GetFileNameWithoutExtension(readName)}[][32]={{");
            for (int y = 0; y < fy; ++y)
            {
                for (int x = 0; x < fx; ++x)
                {
                    Console.Write("{");
                    for (int dy = 0; dy < 16; ++dy)
                    {
                        // 8bit単位での書き出し
                        for (int ox = 0; ox < 2; ++ox)
                        {
                            UInt16 ras = 0;
                            for (int dx = 0; dx < 8; ++dx)
                            {
                                ras <<= 1;
                                if (bmp.GetPixel(x * 16 + ox * 8 + dx, y * 16 + dy).R > 128)
                                {
                                    ras |= 1;
                                }
                            }
                            Console.Write($"0x{ras:x2},");
                        }
                    }
                    Console.WriteLine("},");
                }
            }
            Console.WriteLine("};");
        }
    }
}
