using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;

namespace MappingFramework.MappingInterface.AvalonEdit.FoldingStrategies
{
    public class XmlFoldingStrategy : IFoldingStrategy
	{
		public bool ShowAttributesWhenFolded { get; set; }
		
		public void UpdateFoldings(FoldingManager manager, TextDocument document)
		{
			int firstErrorOffset;
			IEnumerable<NewFolding> foldings = CreateNewFoldings(document, out firstErrorOffset);
			manager.UpdateFoldings(foldings, firstErrorOffset);
		}
		
		public IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
		{
			try {
				XmlTextReader reader = new XmlTextReader(document.CreateReader());
				reader.XmlResolver = null; // don't resolve DTDs
				return CreateNewFoldings(document, reader, out firstErrorOffset);
			} catch (XmlException) {
				firstErrorOffset = 0;
				return Enumerable.Empty<NewFolding>();
			}
		}
		
		private IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, XmlReader reader, out int firstErrorOffset)
		{
			Stack<XmlFoldStart> stack = new Stack<XmlFoldStart>();
			List<NewFolding> foldMarkers = new List<NewFolding>();
			try {
				while (reader.Read()) {
					switch (reader.NodeType) {
						case XmlNodeType.Element:
							if (!reader.IsEmptyElement) {
								XmlFoldStart newFoldStart = CreateElementFoldStart(document, reader);
								stack.Push(newFoldStart);
							}
							break;

						case XmlNodeType.EndElement:
							XmlFoldStart foldStart = stack.Pop();
							CreateElementFold(document, foldMarkers, reader, foldStart);
							break;

						case XmlNodeType.Comment:
							CreateCommentFold(document, foldMarkers, reader);
							break;
					}
				}
				firstErrorOffset = -1;
			} catch (XmlException ex) {
				if (ex.LineNumber >= 1 && ex.LineNumber <= document.LineCount)
					firstErrorOffset = document.GetOffset(ex.LineNumber, ex.LinePosition);
				else
					firstErrorOffset = 0;
			}
			foldMarkers.Sort((a, b) => a.StartOffset.CompareTo(b.StartOffset));
			return foldMarkers;
		}

		static int GetOffset(TextDocument document, XmlReader reader)
		{
			IXmlLineInfo info = reader as IXmlLineInfo;
			if (info != null && info.HasLineInfo()) {
				return document.GetOffset(info.LineNumber, info.LinePosition);
			} else {
				throw new ArgumentException("XmlReader does not have positioning information.");
			}
		}
		
		static void CreateCommentFold(TextDocument document, List<NewFolding> foldMarkers, XmlReader reader)
		{
			string comment = reader.Value;
			int firstNewLine = comment.IndexOf('\n');
			if (firstNewLine >= 0) 
            {
                int startOffset = GetOffset(document, reader) - 4;
				int endOffset = startOffset + comment.Length + 7;

				string foldText = $"<!--{comment.Substring(0, firstNewLine).TrimEnd('\r')}-->";
				foldMarkers.Add(new NewFolding(startOffset, endOffset) { Name = foldText });
			}
		}
		
		XmlFoldStart CreateElementFoldStart(TextDocument document, XmlReader reader)
		{
			XmlFoldStart newFoldStart = new XmlFoldStart();

			IXmlLineInfo lineInfo = (IXmlLineInfo)reader;
			newFoldStart.StartLine = lineInfo.LineNumber;
			newFoldStart.StartOffset = document.GetOffset(newFoldStart.StartLine, lineInfo.LinePosition - 1);

			if (this.ShowAttributesWhenFolded && reader.HasAttributes) {
				newFoldStart.Name = $"<{reader.Name} {GetAttributeFoldText(reader)}>";
			} else {
				newFoldStart.Name = $"<{reader.Name}>";
			}

			return newFoldStart;
		}
		
		static void CreateElementFold(TextDocument document, List<NewFolding> foldMarkers, XmlReader reader, XmlFoldStart foldStart)
		{
			IXmlLineInfo lineInfo = (IXmlLineInfo)reader;
			int endLine = lineInfo.LineNumber;
			if (endLine > foldStart.StartLine) {
				int endCol = lineInfo.LinePosition + reader.Name.Length + 1;
				foldStart.EndOffset = document.GetOffset(endLine, endCol);
				foldMarkers.Add(foldStart);
			}
		}
		
		static string GetAttributeFoldText(XmlReader reader)
		{
			StringBuilder text = new StringBuilder();

			for (int i = 0; i < reader.AttributeCount; ++i) {
				reader.MoveToAttribute(i);

				text.Append(reader.Name);
				text.Append("=");
				text.Append(reader.QuoteChar.ToString());
				text.Append(XmlEncodeAttributeValue(reader.Value, reader.QuoteChar));
				text.Append(reader.QuoteChar.ToString());
				
				if (i < reader.AttributeCount - 1) {
					text.Append(" ");
				}
			}

			return text.ToString();
		}
		
		static string XmlEncodeAttributeValue(string attributeValue, char quoteChar)
		{
			StringBuilder encodedValue = new StringBuilder(attributeValue);

			encodedValue.Replace("&", "&amp;");
			encodedValue.Replace("<", "&lt;");
			encodedValue.Replace(">", "&gt;");

			if (quoteChar == '"') {
				encodedValue.Replace("\"", "&quot;");
			} else {
				encodedValue.Replace("'", "&apos;");
			}

			return encodedValue.ToString();
		}

		private class XmlFoldStart : NewFolding
	    {
		    internal int StartLine;
	    }
	}
}