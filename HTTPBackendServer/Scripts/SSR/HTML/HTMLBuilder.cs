using System;
using System.Text;


namespace DDUKServer.HTML
{
    /// <summary>
    /// HTML 빌더.
    /// </summary>
    public static class HTMLBuilder
    {
        private static StringBuilder s_StringBuilder;

        static HTMLBuilder()
        {
            s_StringBuilder = new StringBuilder();
        }

        public static string BuildDocument(string title, string style, string body)
        {
            var bodies = body.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            s_StringBuilder.Clear();
			s_StringBuilder.AppendLine($"<!DOCTYPE html>");
            s_StringBuilder.AppendLine($"<html lang=\"en\">");
            s_StringBuilder.AppendLine($"	<head>");
            s_StringBuilder.AppendLine($"		<meta charset=\"UTF-8\">");
			s_StringBuilder.AppendLine($"		<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
			s_StringBuilder.AppendLine($"		<title>{title}</title>");
			s_StringBuilder.AppendLine($"		<style>{style}</style>");
            s_StringBuilder.AppendLine($"	</head>");
            s_StringBuilder.AppendLine($"	<body>");
            for (var i = 0; i < bodies.Length; ++i)
            {
                s_StringBuilder.Append($"		{bodies[i]}");
            }
            s_StringBuilder.AppendLine("	</body>");
            s_StringBuilder.AppendLine("</html>");

            return s_StringBuilder.ToString();
        }

        public static bool BuildLayout(Element element, out string html, out string css)
        {
            string Recursive(Element element, int indentLevel)
            {
                if (element == null)
                    return string.Empty;

                var indent = string.Empty;
                for (var i = 0; i < indentLevel; ++i)
                {
                    indent += "\t";
                }

                s_StringBuilder.Append($"{indent}<{element.Name} ");

                var index = 0;
                var count = element.Attributes.Count;
                var enumerator = element.Attributes.GetEnumerator();

				while (enumerator.MoveNext())
				{
					var attribute = enumerator.Current;
					s_StringBuilder.Append($"{attribute.Key}=\"{attribute.Value}\"");
					++index;

					if (index < count)
					{
                        s_StringBuilder.Append(" ");
					}
				}

				s_StringBuilder.AppendLine(">");

				if (!string.IsNullOrEmpty(element.Value))
                {
                    s_StringBuilder.AppendLine($"{indent}{element.Value}");
                }

                if (element.Children != null)
                {
                    foreach (var child in element.Children)
                    {
                        s_StringBuilder.Append(Recursive(child, indentLevel + 1));
                    }
                }

                s_StringBuilder.AppendLine($"{indent}</{element.Name}>");
                return string.Empty;
            }

			html = string.Empty;
			css = string.Empty;

			if (element == null)
                return false;

            s_StringBuilder.Clear();
            s_StringBuilder.Append(Recursive(element, 0));
            html = s_StringBuilder.ToString();
            css = string.Empty;
            return true;
        }
    }
}