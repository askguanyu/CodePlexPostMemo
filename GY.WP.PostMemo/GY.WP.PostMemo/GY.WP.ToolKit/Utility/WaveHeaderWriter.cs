﻿//-----------------------------------------------------------------------
// <copyright file="WaveHeaderWriter.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.ToolKit.Utility
{
    using System;
    using System.IO.IsolatedStorage;

    /// <summary>
    ///
    /// </summary>
    public class WaveHeaderWriter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="targetStream"></param>
        /// <param name="byteStreamSize"></param>
        /// <param name="sampleRate"></param>
        public static void WriteHeader(IsolatedStorageFileStream targetStream, int byteStreamSize, int sampleRate)
        {
            const int channelCount = 1;
            const int bitsPerSample = 16;
            const int bytesPerSample = bitsPerSample / 8;
            var encoding = System.Text.Encoding.UTF8;

            // ChunkID Contains the letters "RIFF" in ASCII form (0x52494646 big-endian form).
            targetStream.Write(encoding.GetBytes("RIFF"), 0, 4);

            // NOTE this will be filled in later
            targetStream.Write(BitConverter.GetBytes(byteStreamSize + 36), 0, 4);

            // Format Contains the letters "WAVE"(0x57415645 big-endian form).
            targetStream.Write(encoding.GetBytes("WAVE"), 0, 4);

            // Subchunk1ID Contains the letters "fmt " (0x666d7420 big-endian form).
            targetStream.Write(encoding.GetBytes("fmt "), 0, 4);

            // Subchunk1Size 16 for PCM.  This is the size of therest of the Subchunk which follows this number.
            targetStream.Write(BitConverter.GetBytes(16), 0, 4);

            // AudioFormat PCM = 1 (i.e. Linear quantization) Values other than 1 indicate some form of compression.
            targetStream.Write(BitConverter.GetBytes((short)1), 0, 2);

            // NumChannels Mono = 1, Stereo = 2, etc.
            targetStream.Write(BitConverter.GetBytes((short)channelCount), 0, 2);

            // SampleRate 8000, 44100, etc.
            targetStream.Write(BitConverter.GetBytes(sampleRate), 0, 4);

            // ByteRate =  SampleRate * NumChannels * BitsPerSample/8
            targetStream.Write(BitConverter.GetBytes(sampleRate * channelCount * bytesPerSample), 0, 4);

            // BlockAlign NumChannels * BitsPerSample/8 The number of bytes for one sample including all channels.
            targetStream.Write(BitConverter.GetBytes((short)(channelCount * bytesPerSample)), 0, 2);

            // BitsPerSample    8 bits = 8, 16 bits = 16, etc.
            targetStream.Write(BitConverter.GetBytes((short)(bitsPerSample)), 0, 2);

            // Subchunk2ID Contains the letters "data" (0x64617461 big-endian form).
            targetStream.Write(encoding.GetBytes("data"), 0, 4);

            // NOTE to be filled in later
            targetStream.Write(BitConverter.GetBytes(byteStreamSize), 0, 4);
        }
    }
}
