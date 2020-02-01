﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PKHeX.Core;
using SysBot.Pokemon;

namespace SysBot.ConsoleApp
{
    internal static class Program
    {
        private const string Path = "config.txt";

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Starting up.");
            if (args.Length != 0)
            {
                Console.WriteLine($"Don't provide any args. Just use {Path}");
                Console.ReadKey();
                return;
            }

            if (!File.Exists(Path))
            {
                Console.WriteLine($"Create {Path}, with IP and Port on separate lines.");
                Console.ReadKey();
                return;
            }

            var lines = File.ReadAllLines("config.txt");
            if (lines.Length < 2)
            {
                Console.WriteLine($"Create {Path}, with IP and Port on separate lines.");
                Console.ReadKey();
                return;
            }

            await DoLinkTrade(lines).ConfigureAwait(false);
        }

        private static async Task DoSurpriseTrade(string[] lines)
        {
            // Default Bot: Surprise Trade bot. See associated files.
            await SurpriseTradeBotUtil.RunBotAsync(lines, CancellationToken.None).ConfigureAwait(false);
        }

        private static async Task DoLinkTrade(string[] lines)
        {
            // Default Bot: Code Trade bot. See associated files.
            var hub = new PokeTradeHub<PK8> {UseBarrier = true};

            var token = CancellationToken.None;

            var monitor = hub.MonitorQueueAddIfEmpty(lines[2], token);
            var bot1 = PokeTradeBotUtil.RunBotAsync(lines, hub, token);

            await Task.WhenAll(monitor, bot1).ConfigureAwait(false);
        }
    }
}