using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SLINTIC.Exceptions;
using SLINTIC.Console;

/// <summary>
/// Instruction sets for serializing and deserializing various files into C# compatable scriptable object instances
/// </summary>
namespace SLINTIC.IO.Interpreter
{
    public struct BatchFile
    {
        /// <summary>
        /// Represents an instance of an executable BatchFile
        /// </summary>
        public BatchFile(string lines, string path)
        {
            instances++;
            this.lines = lines;
            this.BatchID = $"btch{instances}";
            this.path = $"{path}\\{BatchID}.bat";

            File.Create(this.path);
        }

        /// <summary>
        /// Takes a string array and serializes it into a new batch file
        /// </summary>
        public BatchFile SerializeBatch(string[] input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in input) { sb.AppendLine(s); }
            return new BatchFile(sb.ToString(), $"{Directory.GetCurrentDirectory()}\\SLINTIC\\BatchTemp\\");;
        }

        /// <summary>
        /// Gets a pre-existing BatchFile and inverse-serializes it into a scriptable BatchFile
        /// </summary>
        public BatchFile InverseSerialize(string path)
        {
            if (!path.EndsWith(".bat"))
            {
                throw new GeneralException($"SLINTIC.Interpreter Inverse Serialization Exception: Inverse-Serialization request denied, {path} does not point to a valid .bat file");
            }

            StringBuilder sb = new StringBuilder();
            string[] filedata = File.ReadAllLines(path);
            foreach (string s in filedata) { sb.AppendLine(s); }
            BatchFile output = new BatchFile(sb.ToString(), path);
            return output;
        }

        /// <summary>
        /// Deserializes a batch file into a raw string array
        /// </summary>
        /// <returns>Script data from a pre-existing batch</returns>
        [ObsoleteAttribute("DeserializeBatch is obsolete and redundant. Get BatchFile.lines instead.", false)]
        public string[] DeserializeBatch()
        {
            if (this.Destroyed) throw new GeneralException($"SLINTIC.Interpreter Deserialize Batch Exception: Cannot deserialize BatchFile, the requested instance {this.BatchID} was destroyed");

            return File.ReadAllLines(this.path);
        }

        /// <summary>
        /// Executes the BatchFile instance
        /// </summary>
        public void Execute()
        {
            if (this.Destroyed) throw new GeneralException($"SLINTIC.Interpreter Deserialize Batch Exception: Cannot execute BatchFile, the requested instance {this.BatchID} was destroyed");
            Process.Start(this.path);
        }

        // Overload 1: non-object execute
        /// <summary>
        /// Compiles and executes an instance of a BatchFile
        /// </summary>
        public static void Execute(BatchFile batch)
        {
            if (batch.path is null) { LocalConsole.LogError("SLINTIC.Interpreter Batch Error: BatchFile reference not set to an instance of a BatchFile."); return; }
            Process.Start(batch.path);
        }

        /// <summary>
        /// Destroys a scriptable instance of a BatchFile and deletes its file
        /// </summary>
        public void DestroyInstance()
        {
            File.Delete(this.path);
            this.path = "";
            this.lines = "";
            this.Destroyed = true;
        }

        // Overload 1: provide script backup
        /// <summary>
        /// Destroys a scriptable instance of a BatchFile and outputs its script contents before it's deleted
        /// </summary>
        public void DestroyInstance(out string contents)
        {
            File.Delete(this.path);
            contents = this.lines;
            this.lines = "";
            this.Destroyed = true;
        }

        /// <summary>
        /// Array of lines to be serialized into a Batch file
        /// </summary>
        public string lines;
        /// <summary>
        /// Represents the path that the serialized batchfile is stored at
        /// </summary>
        public string path;
        /// <summary>
        /// UUID (Universally Unique Identifier) used for instance-specific processes
        /// </summary>
        public string BatchID { get; internal set; }
        /// <summary>
        /// Number of total instances used for generating BatchFile UUIDs
        /// </summary>
        public static int instances { get; internal set; }
        /// <summary>
        /// (Read Only) Boolean property representative of an instance's active state (true = instance destroyed and inert, false = instance active and usable)
        /// </summary>
        public bool Destroyed { get; internal set; } = false;
    }
}