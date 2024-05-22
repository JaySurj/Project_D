using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Timers;
using System.Diagnostics;
using System.Media;
using NAudio.Wave;

// dotnet add package NAudio

public class Program
{
    private static string MusicFile = "";
    private static string PictureFile = "";
    private static bool isSportmode = false;
    private static bool isStressed = false;
    private static bool isPlaying = false;
    private static string MusicFile1 = "muziek_1.mp3";
    private static string MusicFile2 = "muziek_2.mp3";
    private static string MusicFile3 = "muziek_3.mp3";
    private static string DataHartslag = "hartslag.json";
    private static string PictureFile1 = "afbeelding_1.jpg";
    private static string PictureFile2 = "afbeelding_2.jpg";
    private static string PictureFile3 = "afbeelding_3.jpg";
    // als hartslag boven de limiet komt zal de muziek afspelen
    private static int HartslagLimiet = 100;
    // na hoeveel seconden hartslag boven de limiet is
    private static int SecondenLimiet = 3;
    //private static int next = 0;
    private static double Time = 0;
    // time + 1 second
    private static int TimePassed = 1;
    // TimeInterval is in milliseconds, dus 1000ms = 1s
    private static int TimeInterval = 1000;
    private static Stopwatch stopwatch = new Stopwatch();


    public static void Main(string[] args)
    {
        Console.WriteLine("Hartslag simuleren, druk op een knop om te starten");
        Console.WriteLine("Stressed mode is spacebar, normal mode is enter");
        Console.WriteLine("sport mode is backspace en settings is escape");
        ConsoleKeyInfo key = Console.ReadKey();
        if (key.Key == ConsoleKey.Spacebar)
        {
            Console.WriteLine("Stressed mode");
            isStressed = true;
        }
        if (key.Key == ConsoleKey.Enter)
        {
            Console.WriteLine("Normale mode");
            isStressed = false;
        }
        if (key.Key == ConsoleKey.Backspace)
        {
            Console.WriteLine("Sportmodus");
            isSportmode = true;
            // voor nu de logica nog niet goed, moet nog in de start() logica aanpassen
            HartslagLimiet = 200;
            // ^^^^
            return;
        }
        if (key.Key == ConsoleKey.Escape)
        {
            Console.WriteLine("settings");
            ChangeSettings();
            return;
        }
        Start();
    }

    private static void ChangeSettings()
    {
        Console.WriteLine();
        Console.WriteLine("Kies een afbeelding:");
        Console.WriteLine("1. Relaxing sea");
        Console.WriteLine("2. Keep calm");
        Console.WriteLine("3. Picture 3");

        switch (Console.ReadKey().KeyChar)
        {
            case '1':
                PictureFile = PictureFile1;
                break;
            case '2':
                PictureFile = PictureFile2;
                break;
            case '3':
                PictureFile = PictureFile3;
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
        Console.WriteLine();
        Console.WriteLine("Kies een muziek:");
        Console.WriteLine("1. Birds chirping");
        Console.WriteLine("2. Relaxing music");
        Console.WriteLine("3. Music 3");

        switch (Console.ReadKey().KeyChar)
        {
            case '1':
                MusicFile = MusicFile1;
                break;
            case '2':
                MusicFile = MusicFile2;
                break;
            case '3':
                MusicFile = MusicFile3;
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
        Main(null);
    }

    private static void Start()
    {
        //Random random = new Random();

        while (true)
        {
            int hartslag = SimuleerHartslag(Time);
            Time++;

            var hartslagData = new HartslagData
            {
                Tijdstip = DateTime.Now,
                Hartslag = hartslag
            };

            string json = JsonConvert.SerializeObject(hartslagData);
            File.AppendAllText(DataHartslag, json + "\n");

            Console.WriteLine($"Hartslag van {hartslag} bpm, gemeten om {DateTime.Now}");

            // Controleer of hartslag boven de drempel ligt
            if (hartslag > HartslagLimiet)
            {
                if (!stopwatch.IsRunning)
                {
                    stopwatch.Start();
                }

                if (stopwatch.Elapsed.TotalSeconds >= SecondenLimiet && !isPlaying)
                {
                    ShowPicture();
                    PlayMusic();
                    isPlaying = true;
                }

            }
            else
            {
                stopwatch.Reset();
                isPlaying = false;
            }
            Thread.Sleep(TimeInterval);
        }
    }

    private static int SimuleerHartslag(double tijd)
    {
        double baselineBpm, irregularity;
        if (isStressed)
        {
            baselineBpm = 90 + 20 * Math.Sin(2 * Math.PI * (tijd / 30));
            irregularity = 5 * Math.Sin(5 * Math.PI * (tijd / 30));
        }
        else
        {
            baselineBpm = 70 + 10 * Math.Sin(2 * Math.PI * (tijd / 60));
            irregularity = 2 * Math.Sin(5 * Math.PI * (tijd / 60));
        }

        double bpm = baselineBpm + irregularity;
        
        double pqrst;
        if (isStressed)
        {
            pqrst = AnxiousHeartbeat(tijd % (60.0 / bpm));
        }
        else
        {
            pqrst = NormalHeartbeat(tijd % (60.0 / bpm));
        }
        return (int)(bpm + pqrst * 10);
    }

    private static double NormalHeartbeat(double tijd)
    {
        if (tijd < 0.1) 
        {
            return 0.1 * Math.Sin(20 * Math.PI * tijd);
        }
        if (tijd < 0.12) 
        {
            return -0.5;
        }
        if (tijd < 0.2) 
        {
            return 1.0; 
        }
        if (tijd < 0.3)
        {
            return -0.3;
        }
        if (tijd < 0.4) 
        {
            return 0.1 * Math.Sin(20 * Math.PI * (tijd - 0.2));
        }
        else
        {
            return 0;
        }
    }

    // 
    private static double AnxiousHeartbeat(double tijd)
    {
        if (tijd < 0.1) 
        {
            return 0.2 * Math.Sin(20 * Math.PI * tijd);
        }
        if (tijd < 0.12) 
        {
            return -0.7;
        }
        if (tijd < 0.2) 
        {
            return 1.2;
        }
        if (tijd < 0.3) 
        {
            return -0.5;
        }
        if (tijd < 0.4) 
        {
            return 0.2 * Math.Sin(20 * Math.PI * (tijd - 0.2));
        }
        else
        {
            return 0;
        }
    }


    private static void ShowPicture()
    {
        var startInfo = new ProcessStartInfo(PictureFile)
        {
            UseShellExecute = true
        };
        Process.Start(startInfo);
    }

    private static void PlayMusic()
    {
        try
        {
            using (var audioFileReader = new AudioFileReader(MusicFile))
            {
                using (var waveOut = new WaveOutEvent())
                {
                    waveOut.Init(audioFileReader);
                    waveOut.Play();

                    Console.WriteLine("Speelt muziekje, klik een knop om te stoppen");
                    Console.ReadKey(); 

                    waveOut.Stop();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
