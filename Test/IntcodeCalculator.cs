using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    public class IntcodeCalculator
    {
        private bool useNounVerb;
        private int noun;
        private int verb;

        public ( int diagnosticCode, List<int> output) Calculate(string intCode, int input = 0)
        {
            var final = new List<int>();
            var integers = intCode.Split(',').Select(int.Parse).ToArray();

            if (useNounVerb)
            {
                //set noun (max 99)
                integers[1] = noun;

                //set verb (max 99)
                integers[2] = verb;
            }

            var address = 0;
            (int modeParam3, int modeParam2, int modeParam1, int @operator) = ParseFirstValue(integers[0]);

            while (@operator != 99)
            {
                var jump = @operator switch
                {
                    1 => Add(address),
                    2 => Multiply(address),
                    3 => FromInput(address, input),
                    4 => ReturnValueAtAddress(address),
                    _ => 0
                };

                address += jump;
                @operator = ParseFirstValue(integers[address]).@operator;
            }



            final.Add(integers[0]);
            return (integers[0], final);

            int Add(int address)
            {
                var indexOutput = integers[address + 3];
                var indexA = integers[address + 1];
                var indexB = integers[address + 2];
                var result = (integers[indexA] + integers[indexB]);
                integers[indexOutput] = result;
                return 4;
            }

            int Multiply(int address)
            {
                var indexOutput = integers[address + 3];
                var indexA = integers[address + 1];
                var indexB = integers[address + 2];
                var result = (integers[indexA] * integers[indexB]);
                integers[indexOutput] = result;
                return 4;
            }

            int FromInput(int address, int input)
            {
                var indexA = integers[address + 1];
                integers[indexA] = input;
                return 2;
            }

            int ReturnValueAtAddress(int address)
            {
                var indexA = integers[address + 1];
                if (integers[indexA] == 0)
                {
                    final.Add(integers[indexA]);
                }
                return 2;
            }
        }

        private (int modeParam3, int modeParam2, int modeParam1, int @operator) ParseFirstValue(int firstValue)
        {
            if (firstValue <10)
            {
                return (0, 0, 0, firstValue);
            }

            var a = firstValue.ToString("00000");

            var modeParam3 = int.Parse(a[0].ToString());
            var modeParam2 = int.Parse(a[1].ToString());
            var modeParam1 = int.Parse(a[2].ToString());
            var @operator  = int.Parse(a[4].ToString());

            return (modeParam3, modeParam2, modeParam1, @operator);

        }

        public void SetNounVerb(int noun, int verb)
        {
            this.useNounVerb = true;
            this.noun = noun;
            this.verb = verb;
        }

        public int IetsInDieZin(string input, int desired)
        {
            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <= 99; verb++)
                {
                    SetNounVerb(noun, verb);
                    var result = Calculate(input);

                    if (result.diagnosticCode == desired)
                    {
                        return int.Parse($"{noun:00}{verb:00}");
                    }
                }
            }

            throw new Exception("Ja doei!");
        }
    }
}