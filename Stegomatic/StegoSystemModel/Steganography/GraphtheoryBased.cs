﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace StegomaticProject.StegoSystemModel.Steganography
{
    class GraphTheoryBased : IStegoAlgorithm
    {
        public GraphTheoryBased() //constructor
        {
        }
        
        List<Pixel> PixelList = new List<Pixel>();
        List<Vertex> VertexList = new List<Vertex>();
        List<Edge> EdgeList = new List<Edge>();
        List<Edge> MatchedEdges = new List<Edge>();

        //Create list for values of bitpairs in message
        List<IEnumerable<int>> BitPairValueList = new List<IEnumerable<int>>();

        public const int SamplesVertexRatio = 3;
        public const int Modulo = 4;
        public const int MaxEdgeWeight = 10;
        public const int PixelsPerByte = 12;    

        public byte[] Decode(Bitmap coverImage, string seed)
        {
            
            
            throw new NotImplementedException();
        }

        public Bitmap Encode(Bitmap coverImage, string seed, byte[] message)
        {
            throw new NotImplementedException();
        }

        /*Method for calculating the weight of an edge*/
        public byte CalculateEdgeWeight(Pixel vertPixOne, Pixel vertPixTwo) 
        {
            byte weight = 0;
            return weight;
        }

        private int ShortenAndParsePassphraseToInt32(string passphrase) //converts user stego passphrase into an int32 seed
        {
            int seed;
            string temp = "";

            while (true)
            {
                bool b = Int32.TryParse(passphrase, out seed);
                Console.WriteLine(b);
                if (b == true)
                {
                    break;
                }
                else
                {
                    for (int i = 0; i < passphrase.Length; i += 2)
                    {
                        temp += passphrase[i];
                    }
                    passphrase = temp;
                    temp = "";
                }
            }
            return seed;
        }

        private void GetRandomPixelsAddToList2(Bitmap image, int pixelsNeeded, string passphrase)
        {
            int key = ShortenAndParsePassphraseToInt32(passphrase);
            int numberOfPixels = image.Width*image.Height;

            //generate sequence of numbers through seed
            Random r = new Random(key);

            //Generates a set of numbers {0,...,n}, where n = amount of pixels in an image. Then it randomly selects numbers from this list, which will correspond to a pixel position in that image
            List<int> pixelPositions = Enumerable.Range(0, numberOfPixels).OrderBy(x => r.Next(0, numberOfPixels)).Take(pixelsNeeded).ToList();

            int tempPosX;
            int tempPosY;
            for (int i = 0; i < pixelsNeeded; i++)
            {
                tempPosX = pixelPositions[i]%image.Width;
                tempPosY = pixelPositions[i]/image.Width;
                //make new pixel 
                Pixel pixel = new Pixel(image.GetPixel(tempPosX, tempPosY), tempPosX, tempPosY);
                PixelList.Add(pixel);
            }
        }

        private void GetRandomPixelsAddToList1(Bitmap image, string passphrase, int amount)
        {
            //Create array at the lenght of a 'amount of total pixels'
            int[] array = new int[Convert.ToInt32(image.Width * image.Height) * 2];

            int seed = ShortenAndParsePassphraseToInt32(passphrase);

            //Create random-object, based on incoming seed
            Random r = new Random(seed);

            //Load to array
            for (int j = 0; j < array.Length; j++)
            {
                //This interval controls density of selection
                array[j] = r.Next(2, 10);
            }

            //Reset all variables before each run
            int posx = 0;
            int posy = 0;
            int pixels = 0;
            int i = 0;
            Color color;

            for (int j = 0; j < array.Length; j++)
            {
                posx += array[j];
                pixels++;

                if (posx * posy >= (image.Height * image.Width))
                {
                    break;
                }
                if (posx >= image.Width)
                {
                    int remainder = (image.Width - posx) * -1;
                    posx = 0 + remainder;
                    posy++;

                    if (posy >= image.Height)
                    {
                        posy = 0;
                    }
                }

                if (j == array.Length)
                {
                    j = 0;
                    if (pixels >= (image.Height * image.Width))
                    {
                        break;
                    }
                }

                //Gets selected pixel data; and creates new Pixel-object and stores in list
                Pixel pix = new Pixel(image.GetPixel(posx, posy), posx, posy);
                PixelList.Add(pix);
                if (PixelList.Count >= amount)
                {
                    break;
                }
                i++;
            }

            //Print log when done
            Console.WriteLine("Pixels imported successfully!");
            Console.WriteLine("Pixels: " + i + " were successfully extracted.");

        }

        /*Method for getting the value of bitpairs into a list of ints from a byte-array*/
        public List< IEnumerable<int>> ChopBytesToBitPairs(byte[] byteArray)
        {
            /*List fo int values*/
            List< IEnumerable<int>> messageValues = new List<IEnumerable <int>>();

            foreach (byte value in byteArray)
            {
                messageValues.Add(ConvertBitsToInt(value));
            }

            return messageValues;
        }

        /*Method for converting bitpairs to ints from a byte*/
        public IEnumerable<int> ConvertBitsToInt(byte byteValue)
        {
            int value;
            BitArray bitValues = new BitArray(new byte[] { byteValue });

            for (int index = 7; index > -1; index -= 2)   
            {
                if(bitValues[index] == true && bitValues[index - 1] == true)
                {
                    value = 3;
                }
                else if(bitValues[index] == true && bitValues[index - 1] == false)
                {
                    value = 2;
                }
                else if(bitValues[index] == false && bitValues[index - 1] == true)
                {
                    value = 1;
                }
                else
                {
                    value = 0;
                }

                yield return value;
            }
        }

        /*Method for calculating the required amount of pixels to hide the input message*/
        public int CalculateRequredPixels(byte[] byteArray)
        {
            int amount, counter = 0;

            foreach(byte value in byteArray)
            {
                counter++;
            }

            return amount = counter * PixelsPerByte;
        }
    }
}
