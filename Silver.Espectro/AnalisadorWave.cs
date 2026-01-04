using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Espectro
{
    internal class AnalisadorWave
    {
        private const int AmplitudeMinima = -10000;

        public long Length { get; set; }
        public long DataLength { get; set; }
        public int SampleRate { get; set; }
        public short Channels { get; set; }
        public short BitsPerSample { get; set; }
        public byte[] Data { get; set; }

        public AnalisadorWave(string caminhoArquivoWave)
        {
            var fs = new FileStream(caminhoArquivoWave, FileMode.Open, FileAccess.Read);
            var br = new BinaryReader(fs);

            Length = fs.Length;
            fs.Position = 22;
            Channels = br.ReadInt16();
            fs.Position = 24;
            SampleRate = br.ReadInt32();
            fs.Position = 34;
            BitsPerSample = br.ReadInt16();
            DataLength = Length - 44;
            Data = new byte[DataLength];
            fs.Position = 44;
            fs.Read(Data, 0, Data.Length);

            fs.Close();
            br.Close();
        }

        public long TempoEmSilencio()
        {
            var InicioSilencio = Data.Length - 1;

            for (var i = InicioSilencio; i >= 0; i -= 2)
            {
                var Amplitude = AmplitudeEquivalente(Data, i - 1);
                if (Amplitude > AmplitudeMinima)
                {
                    InicioSilencio = i;
                }
                else
                {
                    break;
                }
            }

            return CalcularSegundos(InicioSilencio);
        }

        public string TipoAMD()
        {
            return string.Empty;
        }

        private long CalcularSegundos(long inicioSilencio)
        {
            return ((DataLength - inicioSilencio) / (SampleRate * (BitsPerSample / 8))) / Channels;
        }

        private short AmplitudeEquivalente(byte[] data, int posicaoInicial)
        {
            var Amplitude = BitConverter.ToInt16(data, posicaoInicial);
            if (Amplitude != 0)
            {
                Amplitude = Convert.ToInt16((~Amplitude | 1));
            }
            return Amplitude;
        }
    }
}
