/// <summary>
/// Author:    Riley Kraabel and Lincoln Knowles
/// Date:      22-March-2023
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Riley Kraabel and Lincoln Knowles - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Riley Kraabel and Lincoln Knowles, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
///     This class contains methods for creating and implementing a file-specific Logger. Code implemented
///     contains information from the CS3500 ForStudents GitHub repository. 
///     https://github.com/uofu-cs3500-spring23/ForStudents
///     
/// </summary>
using Microsoft.Extensions.Logging;

namespace Logger
{
    public class FileLogger : ILogger
    {
        private readonly string _name;
        private readonly string _fileName;

        /// <summary>
        ///     This is a constructor method for the FileLogger class. It creates a new file with the path set in the filename variable. 
        ///     Note: This path is made for use by Windows OS, and will not work on Mac. 
        /// </summary>
        /// 
        /// <param name="name"> The name of the File that will be logged to. </param>
        public FileLogger(string name)
        {
            _name = name;
            _fileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
               + Path.DirectorySeparatorChar
               + $"CS3500-{name}.log";

            // _fileName = "C:\\Users\\riley\\source\\repos\\Agario\\logOutput\\" + $"CS3500-{name}.log";

            // -- Example of file-saving on Mac (used for debugging - left in for easy grading edit) -- //
            //_fileName = "/Users/lincolnknowles/Projects/chatting/assignment-seven---chatting-capricornious2/logoutput/"
            //   + $"CS3500-{name}.log";
            var myFile = File.Create(_fileName);
            myFile.Close();
        }

        /// <summary>
        ///     This method creates all future logs under the specified LogLabel until the 'BeginScope' instance is disposed. 
        /// </summary>
        /// 
        /// <typeparam name="TState">   the type of the LogLabel being set </typeparam>
        /// <param name="state">        the name of the LogLabel being set </param>
        /// <returns>                   returns IDisposable to be disposed of when done. </returns>
        /// 
        /// <exception cref="NotImplementedException"> the method is not implemented. </exception>
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Given some LogLevel, this method determines if logging at that level is Enabled and can be called.     
        /// </summary>
        /// 
        /// <param name="logLevel">     the log level desired. </param>
        /// <returns>                   returns true or false - depends on whether or not logging at some level can be done. </returns>
        /// 
        /// <exception cref="NotImplementedException"> the method is not implemented. </exception>
        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Handles the creation of the file with all of the Log information. All of the parameters are used by MAUI. 
        /// </summary>
        /// 
        /// <typeparam name="TState">   </typeparam>
        /// <param name="logLevel">     the level of the Log being searched for. </param>
        /// <param name="eventId">      the event that occurred. </param>
        /// <param name="state">        </param>
        /// <param name="exception">    </param>
        /// <param name="formatter">    </param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string logMessage = $"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss.fff} [{logLevel}] {_name}: {formatter(state, exception)}{Environment.NewLine}";
            File.AppendAllText(_fileName, logMessage);
        }

        /// <summary>
        ///     Handles the execution of the actual logging. 
        /// </summary>
        ///
        /// <param name="logLevel">     the level of the Log being sent to the File. </param>
        /// <param name="message">      the message to send to the file </param>
        /// <param name="exception">    should there be some exception, this is the one that should be thrown. </param>
        public void Log(LogLevel logLevel, string message, Exception? exception)
        {

            string logMessage = $"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss.fff} Log Commit-" + message + $"{Environment.NewLine}";
            File.AppendAllText(_fileName, logMessage);
        }
    }

    /// <summary>
    ///    This class contains methods from the LoggerProvider but are specified for File-implementation.  
    /// </summary>
    public class FileLoggerProvider : ILoggerProvider
    {
        /// <summary>
        ///    This is a wrapper method for creating a new Logger, but specifically for Files. This is used by MAUI to create/read a file.
        /// </summary>
        /// 
        /// <param name="categoryName">     string type, the name of the file. </param>
        /// <returns>                       a new ILogger. </returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName);
        }

        /// <summary>
        ///     This method ends the Logger that is currently in use. 
        /// </summary>
        public void Dispose() {; }
    }
}