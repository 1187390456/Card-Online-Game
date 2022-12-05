using UnityEngine;

namespace Crosstales.RTVoice.Util
{
    /// <summary>AudioClip exporter class.</summary>
    public class AudioExporter
    {
        #region Variables

        private const int HEADER_SIZE = 44;
        private const float RESCALE_FACTOR = 32767f;

        private struct ClipData
        {
            public int samples;
            public int channels;
            public float[] samplesData;
        }

        #endregion


        #region Static methods

        public static bool SaveAsWav(string filename, AudioClip clip)
        {
            if (!filename.ToLower().EndsWith(".wav"))
            {
                filename += ".wav";
            }

            var filepath = filename;

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filepath));

            ClipData clipdata = new ClipData();
            clipdata.samples = clip.samples;
            clipdata.channels = clip.channels;
            float[] dataFloat = new float[clip.samples * clip.channels];
            clip.GetData(dataFloat, 0);
            clipdata.samplesData = dataFloat;

            using (var fileStream = createEmpty(filepath))
            {
                System.IO.MemoryStream memstrm = new System.IO.MemoryStream();
                convertAndWrite(memstrm, clipdata);
                memstrm.WriteTo(fileStream);
                writeHeader(fileStream, clip);
            }

            return true;
        }

        #endregion


        #region Private methods

        private static System.IO.FileStream createEmpty(string filepath)
        {
            var fileStream = new System.IO.FileStream(filepath, System.IO.FileMode.Create);
            byte emptyByte = new byte();

            for (int ii = 0; ii < HEADER_SIZE; ii++) //preparing the header
            {
                fileStream.WriteByte(emptyByte);
            }

            return fileStream;
        }

        private static void convertAndWrite(System.IO.MemoryStream memStream, ClipData clipData)
        {
            float[] samples = new float[clipData.samples * clipData.channels];

            samples = clipData.samplesData;

            short[] intData = new short[samples.Length];

            byte[] bytesData = new byte[samples.Length * 2];

            for (int ii = 0; ii < samples.Length; ii++)
            {
                intData[ii] = (short)(samples[ii] * RESCALE_FACTOR);
            }
            System.Buffer.BlockCopy(intData, 0, bytesData, 0, bytesData.Length);
            memStream.Write(bytesData, 0, bytesData.Length);
        }

        private static void writeHeader(System.IO.FileStream fileStream, AudioClip clip)
        {

            var hz = clip.frequency;
            var channels = clip.channels;
            var samples = clip.samples;

            fileStream.Seek(0, System.IO.SeekOrigin.Begin);

            byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
            fileStream.Write(riff, 0, 4);

            byte[] chunkSize = System.BitConverter.GetBytes(fileStream.Length - 8);
            fileStream.Write(chunkSize, 0, 4);

            byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
            fileStream.Write(wave, 0, 4);

            byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
            fileStream.Write(fmt, 0, 4);

            byte[] subChunk1 = System.BitConverter.GetBytes(16);
            fileStream.Write(subChunk1, 0, 4);

            //ushort two = 2;
            ushort one = 1;

            byte[] audioFormat = System.BitConverter.GetBytes(one);
            fileStream.Write(audioFormat, 0, 2);

            byte[] numChannels = System.BitConverter.GetBytes(channels);
            fileStream.Write(numChannels, 0, 2);

            byte[] sampleRate = System.BitConverter.GetBytes(hz);
            fileStream.Write(sampleRate, 0, 4);

            byte[] byteRate = System.BitConverter.GetBytes(hz * channels * 2);
            fileStream.Write(byteRate, 0, 4);

            ushort blockAlign = (ushort)(channels * 2);
            fileStream.Write(System.BitConverter.GetBytes(blockAlign), 0, 2);

            ushort bps = 16;
            byte[] bitsPerSample = System.BitConverter.GetBytes(bps);
            fileStream.Write(bitsPerSample, 0, 2);

            byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
            fileStream.Write(datastring, 0, 4);

            byte[] subChunk2 = System.BitConverter.GetBytes(samples * channels * 2);
            fileStream.Write(subChunk2, 0, 4);
        }

        #endregion
    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)