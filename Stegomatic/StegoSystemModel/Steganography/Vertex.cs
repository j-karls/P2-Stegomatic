﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegomaticProject.StegoSystemModel.Steganography
{
    class Vertex
    {
        public Vertex(byte partOfMessage, params Pixel[] pixels)
        {
            //assign unique ID
            this.Id = _id;
            _id++;

            //assign "NumberOfSamples" amount of samples (pixels) to this vertex
            for (int i = 0; i < GraphTheoryBased.SamplesVertexRatio; i++)
            {
                this.PixelsForThisVertex[i] = pixels[i];
            }

            this.VertexValue = CalculateVertexValue();
        }

        private static short _id = 1;
        public short Id { get; }

        public short LowestEdgeWeight { get; set; }
        public short NumberOfEdges { get; set; }

        
        public bool Active;
        public byte VertexValue; //value that has to correspond to a certain part of the secret message


        public Pixel[] PixelsForThisVertex;

        public byte[] TargetValues;

        public void CalculateTargetValues() //need to know a couple things more
        {
            for (int i = 0; i < GraphTheoryBased.SamplesVertexRatio; i++)
            {
                
            }
        }

        public byte CalculateVertexValue()
        {
            byte temp = 0;
            for (int i = 0; i < GraphTheoryBased.SamplesVertexRatio; i++)
            {
                temp += PixelsForThisVertex[i].EmbeddedValue;
            }

            return (byte)(temp%GraphTheoryBased.Modulo);
        }
        public void AssignWeightToVertex(short weight)
        {
            LowestEdgeWeight = weight;
        }
        public void AssignNumberOfEdgesToVertex(short edges)
        {
            NumberOfEdges = edges;
        }
    }
}
