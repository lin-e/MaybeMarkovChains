using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace ChainChat
{
    class Program
    {
        const string BrainFile = "brain.txt";
        const int MaxWords = 10000;
        const string StopWord = "[-?-]";
        const string DefaultReply = "i cant think of anything";
        const int MaxRecursionDepth = 50;
        const int MinWords = 4;

        static int ChainLength = 2;

        static bool write = true;
        static Brain brain = new Brain();

        static void Main(string[] args)
        {
            try
            {
                if (args[0].ToLower() == "false")
                {
                    write = false;
                }
                ChainLength = Convert.ToInt32(args[1]);
            } catch { }
            if (!File.Exists(BrainFile))
            {
                File.WriteAllText(BrainFile, "hello world");
            }
            foreach (string line in File.ReadAllLines(BrainFile))
            {
                addToBrain(line.ToLower(), false);
            }
            while (true)
            {
                string input = Console.ReadLine().ToLower();
                Console.WriteLine("BOT: " + generateSentence(input));
                addToBrain(input, write);
            }
        }
        static void addToBrain(string _content, bool write = true)
        {
            string content = Regex.Replace(_content, @"[^\w\.@\- ]", "").Replace(".", "").ToLower();
            if (content.Trim().Length < 1)
            {
                return;
            }
            if (content == "\r")
            {
                return;
            }
            if (content == "\n")
            {
                return;
            }
            if (write)
            {
                List<string> allLines = File.ReadAllLines(BrainFile).ToList();
                allLines.Add(content);
                File.WriteAllLines(BrainFile, allLines);
            }
            List<string> buffer = Enumerable.Repeat(StopWord, ChainLength).ToList();
            foreach (string word in content.Split(' '))
            {
                brain.GetValue(string.Join(" ", buffer.ToArray())).Add(word);
                buffer.RemoveAt(0);
                buffer.Add(word);
            }
            brain.GetValue(string.Join(" ", buffer.ToArray())).Add(StopWord);
        }
        static string generateSentence(string _input = "", int depth = 0)
        {
            if (depth >= MaxRecursionDepth)
            {
                return DefaultReply;
            }
            string input = Regex.Replace(_input, @"[^\w\.@\- ]", "").Replace(".", "").ToLower();
            List<string> buffer = new List<string>();
            List<string> message = new List<string>();
            buffer = input.Split(' ').Slice(0, ChainLength).ToList();
            List<string> toRemove = new List<string>();
            foreach (string bufferMessage in buffer)
            {
                if (bufferMessage.Trim().Length < 1)
                {
                    toRemove.Add(bufferMessage);
                }
            }
            foreach (string bufferMessage in toRemove)
            {
                buffer.Remove(bufferMessage);
            }
            if (buffer.Count() < ChainLength)
            {
                for (int i = 0; i < (ChainLength - buffer.Count()) + 1; i++)
                {
                    try
                    {
                        string rand = StopWord;
                        for (int j = 0; j < 10; j++)
                        {
                            rand = brain.Random();
                            if (rand != StopWord)
                            {
                                break;
                            }
                        }
                        buffer.Insert(0, rand);
                    } catch { }
                }
            }
            foreach (string bufferWord in buffer)
            {
                message.Add(bufferWord);
            }
            for (int i = 0; i < MaxWords; i++)
            {
                string next = "";
                try
                {
                    List<string> notStop = new List<string>();
                    foreach (string single in brain.GetValue(string.Join(" ", buffer.ToArray())))
                    {
                        if (single == StopWord)
                        {
                            continue;
                        }
                        notStop.Add(single);
                    }
                    if (notStop.Count() == 0)
                    {
                        next = StopWord;
                    }
                    else
                    {
                        next = notStop.RandomChoice();
                    }
                }
                catch
                {
                    continue;
                }
                if (next == StopWord)
                {
                    break;
                }
                message.Add(next);
                buffer.RemoveAt(0);
                buffer.Add(next);
            }
            string toReturn = string.Join(" ", message).Trim();
            toReturn = Regex.Replace(toReturn, @"\s+", " ");
            if (toReturn.Split(' ').Length < MinWords)
            {
                return generateSentence(input, depth + 1);
            }
            else
            {
                return toReturn;
            }
        }

        class Brain
        {
            private Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            public List<string> GetValue(string key)
            {
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, new List<string>());
                }
                return dictionary[key];
            }
            public string[] GetKeys()
            {
                return dictionary.Keys.ToArray();
            }
            public string Random()
            {
                return GetValue(GetKeys().RandomChoice()).RandomChoice();
            }
        }
    }
    static class Extensions
    {
        static Random globalRand = new Random();
        public static IEnumerable<A> Slice<A>(this IEnumerable<A> e, int from = 0, int to = 0)
        {
            return e.Take(to).Skip(from);
        }
        public static A RandomChoice<A>(this IEnumerable<A> e)
        {
            return e.ElementAt(globalRand.Next(0, e.Count()));
        }
    }
}
