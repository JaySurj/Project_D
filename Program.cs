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
//
public class Program
{
    private static string MusicFile1 = "muziek_1.mp3";
    private static string MusicFile2 = "muziek_2.mp3";
    private static string MusicFile3 = "muziek_3.mp3";
    private static string DataHartslag = "hartslag.json";
    private static string PictureFile1 = "afbeelding_1.jpg";
    private static int HartslagLimiet = 70;
    private static int SecondenLimiet = 3;
    private static bool IsPlaying = false;
    private static int next = 0;
    private static double Time = 0;
    private static double TimeInterval = 1;
    private static Stopwatch stopwatch = new Stopwatch();


    public static void Main(string[] args)
    {
        Start();
    }

    private static void Start()
    {
        //Random random = new Random();

        while (true)
        {
            int hartslag = SimuleerHartslag(Time);
            Time += TimeInterval;

            var hartslagData = new HartslagData
            {
                Tijdstip = DateTime.Now,
                Hartslag = hartslag
            };

            string json = JsonConvert.SerializeObject(hartslagData);
            File.AppendAllText(DataHartslag, json + Environment.NewLine);

            Console.WriteLine($"Hartslag van {hartslag} bpm, gemeten om {DateTime.Now}");

            // Controleer of hartslag boven de drempel ligt
            if (hartslag > HartslagLimiet)
            {
                if (!stopwatch.IsRunning)
                {
                    stopwatch.Start();
                }

                if (stopwatch.Elapsed.TotalSeconds >= SecondenLimiet && !IsPlaying)
                {
                    ShowPicture();

                    PlayMP3File();
                    IsPlaying = true;
                }

            }
            else
            {
                stopwatch.Reset();
                IsPlaying = false;
            }

            Thread.Sleep((int)(TimeInterval * 1000)); // Omzetten naar milliseconden
        }
    }

    private static int SimuleerHartslag(double tijd)
    {
        double bpm = 70 + 10 * Math.Sin(2 * Math.PI * (tijd / 60)); // Simuleer een basislijn met lichte variatie
        double pqrst = PQRST(tijd % (60.0 / bpm)); // PQRST-golfvorm over een hartslagperiode
        return (int)(bpm + pqrst * 10); // Schaal de PQRST-golfvorm op
    }

    private static double PQRST(double tijd)
    {
        // Simuleer een eenvoudige PQRST-golfvorm
        if (tijd < 0.1) return 0.1 * Math.Sin(20 * Math.PI * tijd);       // P-golf
        if (tijd < 0.12) return -0.5;                                     // Q-daling
        if (tijd < 0.2) return 1.0;                                       // R-piek
        if (tijd < 0.3) return -0.3;                                      // S-daling
        if (tijd < 0.4) return 0.1 * Math.Sin(20 * Math.PI * (tijd - 0.2)); // T-golf
        return 0.0;
    }

    private static void ShowPicture()
    {
        var startInfo = new ProcessStartInfo(PictureFile1)
        {
            UseShellExecute = true
        };
        Process.Start(startInfo);
    }

    private static void PlayMP3File()
    {
        try
        {
            using (var audioFileReader = new AudioFileReader(MusicFile1))
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