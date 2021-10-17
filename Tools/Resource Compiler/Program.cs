using System;
using System.IO;
using System.Text;

namespace ResourceCompiler
{
    class Program
    {
        static public string bitmap_dir = "..\\resources\\BITMAP";
        static public string[] bitmap_names = {
        };

        static public string shader_dir = "..\\resources\\SHADER";
        static public string[] shader_names = {
            "DEFAULT_FRAGMENT_SHADER",
            "DEFAULT_VERTEX_SHADER"
        };

        static public string wave_dir = "..\\resources\\WAVE";
        static public string[] wave_names = {
            null,
            "OH_NO_WAV"
        };

        static public string resource_header_dir = "..\\src\\resource.hpp";
        static void Main(string[] args)
        {
            StreamWriter resource_header = new StreamWriter(File.Create(resource_header_dir));
            resource_header.WriteLine(
                "#ifndef RESOURCE_HPP\n" +
                "#define RESOURCE_HPP\n" +
                "\n" +
                "struct glsl_shader\n" +
                "{\n" +
                "\tchar* text;\n" +
                "};\n" +
                "\n" +
                "struct wav_audio\n" +
                "{\n" +
                "\tunsigned short num_channels;\n" +
                "\tunsigned int sample_rate;\n" +
                "\tunsigned int siz;\n" +
                "\tshort data[];\n" +
                "};\n" +
                "\n" +
                "struct bmp_image\n" +
                "{\n" +
                "\tunsigned int width;\n" +
                "\tunsigned int height;\n" +
                "};\n"
                );

            DirectoryInfo di;

            di = new DirectoryInfo(shader_dir);

            Console.WriteLine();
            Console.WriteLine("shader files");

            long i = 0;
            foreach (FileInfo glsl in di.GetFiles("*.glsl?"))
            {
                StreamReader shader = new StreamReader(glsl.OpenRead());
                string name = "SHADER_" + i.ToString();
                if (i < shader_names.LongLength)
                {
                    name = shader_names[i];
                }
                i++;

                resource_header.WriteLine("static glsl_shader " + name + " = {\"" + shader.ReadToEnd().Replace("\n", "\\n").Replace("\r", "") + "\"};");

                Console.WriteLine(glsl.Name + " saved as " + name);
            }
            resource_header.WriteLine();

           di = new DirectoryInfo(wave_dir);

            Console.WriteLine();
            Console.WriteLine(".wav files");
            i = 0;
            foreach (FileInfo wav in di.GetFiles("*.wav"))
            {
                BinaryReader audio = new BinaryReader(wav.OpenRead());
                string name = "WAV_" + i.ToString();
                if (i < wave_names.LongLength && wave_names[i] != null)
                {
                    name = wave_names[i];
                }
                i++;

                ushort num_channels;
                uint sample_rate;

                if (new string(audio.ReadChars(4)) != "RIFF")
                {
                    Console.WriteLine(wav.Name + " failed");
                    Console.WriteLine("no RIFF header");
                    continue;
                }
                if ((audio.ReadInt32() + 8L) != wav.Length)
                {
                    Console.WriteLine(wav.Name + " failed");
                    Console.WriteLine("size mismatch");
                    continue;
                }
                if (new string(audio.ReadChars(4)) != "WAVE")
                {
                    Console.WriteLine(wav.Name + " failed");
                    Console.WriteLine("no WAVE header");
                    continue;
                }
                audio.ReadBytes(8);
                if(audio.ReadUInt16() != 1)
                {
                    Console.WriteLine(wav.Name + " failed");
                    Console.WriteLine("not pcm");
                    continue;
                }

                num_channels = audio.ReadUInt16();
                sample_rate = audio.ReadUInt32();
                audio.ReadBytes(6);
                if(audio.ReadUInt16() != 16)
                {
                    Console.WriteLine(wav.Name + " failed");
                    Console.WriteLine("convert to 16-PCM");
                    continue;
                }
                audio.ReadBytes(4);

                resource_header.Write("static wav_audio " + name + " {" + num_channels.ToString() + ", " + sample_rate.ToString() + ", " + audio.ReadUInt32().ToString()  + ", {\n\t" + audio.ReadInt16().ToString());
                while (audio.BaseStream.Position <= audio.BaseStream.Length - 16)
                {
                    resource_header.Write(", " + audio.ReadInt16().ToString());
                }
                resource_header.Write("\n}};\n");
                Console.WriteLine(wav.Name + " saved as " + name);
            }
            resource_header.WriteLine();

            di = new DirectoryInfo(bitmap_dir);

            Console.WriteLine();
            Console.WriteLine(".bmp files");
            i = 0;
            foreach (FileInfo bmp in di.GetFiles("*.bmp"))
            {
                Console.WriteLine(bmp.Name);
                i++;
            }
            resource_header.WriteLine("\n#endif");
            resource_header.Close();
        }
    }
}
