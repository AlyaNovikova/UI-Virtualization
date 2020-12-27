using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DogDatabase
{
	public class SqlQueryConverter
	{
		private static readonly XmlDocument xmlDocument;
		private static readonly string FileName = "DogDatabase.Queries.xml";

		static SqlQueryConverter()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();

			xmlDocument = new XmlDocument();
			var stream = assembly.GetManifestResourceStream(FileName);
			if (stream == null)
				throw new Exception("The resource " + FileName + " was not loaded properly.");

			xmlDocument.Load(stream);
		}

		public static string GetCommand(string tableName, string commandName)
		{
			XmlNode xNode = getXmlNode(tableName, commandName);

			string commandText = (xNode != null) ? 
				xNode.InnerText.Trim(' ', '\r', '\n', '\t') : string.Empty;

			return commandText;
		}

		private static XmlNode getXmlNode(string tableName, string commandName)
		{
			if (xmlDocument == null)
				return null;

			return xmlDocument.DocumentElement.SelectSingleNode(
				string.Format(
					"Table[@Name=\"{0}\"]/Command[@Name=\"{1}\"]",
					tableName,
					commandName)
				);
		}
	}
}
