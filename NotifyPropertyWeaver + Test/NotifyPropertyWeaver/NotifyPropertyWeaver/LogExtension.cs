using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotifyPropertyWeaver
{
    public static class LogExtension
    {
        private const int Indentation = 3;

        public static void Write(this NLog.Logger logger, int indent_level, string message, params object[] args)
        {
            string final_message = (args.Length == 0 ? message : string.Format(message, args));

            int total_length = final_message.Length + indent_level * Indentation;
            string indented_message = final_message.PadLeft(total_length);

            logger.Trace(indented_message);
        }

        public static void Write(this NLog.Logger logger, string message, params object[] args)
        {
            logger.Write(0, message, args);
        }
    }
}
