using System.IO;

namespace trilc
{
    class Writer
    {
        public static void write(string path, Instruction[] instructions){
            void error(string msg){
                Error.assert(msg);
                return;
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    for (int i = 0; i < instructions.Length; i++)
                    {
                        sw.Write(instructions[i].type.ToString());
                        for (int j = 0; j < instructions[i].@params.Length; j++)
                        {
                            sw.Write(" "+instructions[i].@params[j]);
                        }
                        sw.Write("\n");
                    }

                    sw.Flush();
                }
            }
            catch (IOException)
            {
                error("The given output location is not valid!");
            }
        }
    }
}