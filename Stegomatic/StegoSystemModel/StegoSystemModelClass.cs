﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using StegomaticProject.StegoSystemModel.Miscellaneous;
using StegomaticProject.StegoSystemModel.Cryptograhy;
using StegomaticProject.StegoSystemModel.Steganography;
using StegomaticProject.StegoSystemUI;
using StegomaticProject.CustomExceptions;

namespace StegomaticProject.StegoSystemModel
{
    public class StegoSystemModelClass : IStegoSystemModel
    {
        private ICompression _compressMethod;
        private ICryptoMethod _cryptoMethod;
        private IStegoAlgorithm _stegoMethod;

        public Func<int, int, bool, int> CalculateImageCapacity { get; set; }

        public StegoSystemModelClass()
        {
            _compressMethod = new GZipStreamCompression();
            _cryptoMethod = new RijndaelCrypto();
            _stegoMethod = new GraphTheoryBased(); //new LeastSignificantBit(); 

            CalculateImageCapacity = CalcCapacityWithCompressionAndStego;
        }

        public string DecodeMessageFromImage(Bitmap coverImage, string decryptionKey, string stegoSeed, 
            bool decrypt = true, bool decompress = true)
        {
            try
            {
                byte[] byteMessage = _stegoMethod.Decode(coverImage, stegoSeed);
                if (decompress)
                {
                    byteMessage = _compressMethod.Decompress(byteMessage);
                }
                string message = ByteConverter.ByteArrayToString(byteMessage);
                if (decrypt)
                {
                    message = _cryptoMethod.Decrypt(message, decryptionKey);
                }
                return message;
            }
            catch (NotifyUserException)
            {
                throw;
            }
            catch (AbortActionException)
            {
                throw;
            }
        }

        public Bitmap EncodeMessageInImage(Bitmap coverImage, string message, string encryptionKey, string stegoSeed, 
            bool encrypt = true, bool compress = true)
        {
            try
            {
                if (encrypt)
                {
                    message = _cryptoMethod.Encrypt(message, encryptionKey);
                }
                byte[] byteMessage = ByteConverter.StringToByteArray(message);
                if (compress)
                {
                    byteMessage = _compressMethod.Compress(byteMessage);
                }
                Bitmap stegoObject = _stegoMethod.Encode(coverImage, stegoSeed, byteMessage);
                return stegoObject;
            }
            catch (NotifyUserException)
            {
                throw;
            }
            catch (AbortActionException)
            {
                throw;
            }
        }

        public int CalcCapacityWithCompressionAndStego(int height, int width, bool compress)
        {
            int capacity = _stegoMethod.CalculateImageCapacity(height, width); 
            if (compress)
            {
                capacity = _compressMethod.ApproxSizeAfterCompression(capacity);
            }
            return capacity;
        }
    }
}
