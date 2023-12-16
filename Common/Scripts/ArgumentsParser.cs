using System.Collections.Generic;
using System.Linq;


namespace DDUKServer
{
    /// <summary>
    /// Arguments Parser.
    /// </summary>
    public class ArgumentsParser
    {
        private Dictionary<string, List<string>> m_Arguments;
        public List<string> this[string key]
        {
            get
            {
                key = key.ToLower();
                if (!m_Arguments.TryGetValue(key, out List<string> value))
                    return new List<string>();

                return value;
            }
        }

        ArgumentsParser(string arguments)
        {
            m_Arguments = new Dictionary<string, List<string>>();

            if (string.IsNullOrWhiteSpace(arguments))
                throw new System.Exception("Arguments Parse Exception : arguments is null");

            Parse(arguments.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries));
        }

        public ArgumentsParser(params string[] arguments)
        {
            m_Arguments = new Dictionary<string, List<string>>();

            if (arguments == null)
                throw new System.Exception("Arguments Parse Exception : arguments is null");

            Parse(arguments);
        }

        ArgumentsParser(params object[] arguments)
        {
            m_Arguments = new Dictionary<string, List<string>>();

            if (arguments == null)
                throw new System.Exception("Arguments Parse Exception : arguments is null");

            var args = arguments.Select(it => it.ToString()).ToArray();
            Parse(args);
        }

        public void Parse(params string[] arguments)
        {
            var currentArgument = default(List<string>);
            foreach (var argument in arguments)
            {
                var argumentText = argument.ToString().ToLower();
                if (IsCommand(argumentText))
                {
                    if (m_Arguments.ContainsKey(argumentText))
                        continue;

                    currentArgument = new List<string>();
                    m_Arguments.Add(argumentText, currentArgument);
                }
                else
                {
                    if (string.IsNullOrEmpty(argumentText))
                        continue;

                    currentArgument.Add(argumentText);
                }
            }
        }

        private bool IsCommand(string argument)
        {
            if (!argument.StartsWith("-"))
                return false;
            return true;
        }
    }
}