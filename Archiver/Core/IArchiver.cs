namespace Archiver.Core
{
    interface IArchiver
    {
        void Compress(string inputFile, string outputFile);
        void Decompress(string inputFile, string outputFile);
    }
}
