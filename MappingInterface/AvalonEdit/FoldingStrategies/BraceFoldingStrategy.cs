using System.Collections.Generic;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;

namespace MappingFramework.MappingInterface.AvalonEdit.FoldingStrategies
{
    public class BraceFoldingStrategy : IFoldingStrategy
	{
        private char OpeningBrace { get; }
        private char ClosingBrace { get; }
        public BraceFoldingStrategy()
		{
			OpeningBrace = '{';
			ClosingBrace = '}';
		}
		
		public void UpdateFoldings(FoldingManager manager, TextDocument document)
		{
			IEnumerable<NewFolding> newFoldings = CreateNewFoldings(document);
			manager.UpdateFoldings(newFoldings, -1);
		}
        
		private IEnumerable<NewFolding> CreateNewFoldings(ITextSource document)
		{
			var newFoldings = new List<NewFolding>();
			
			Stack<int> startOffsets = new Stack<int>();
			
			char openingBrace = OpeningBrace;
			char closingBrace = ClosingBrace;

			int lastNewLineOffset = 0;
			for (int i = 0; i < document.TextLength; i++) {
				char c = document.GetCharAt(i);
				if (c == openingBrace) {
					startOffsets.Push(i);
				} else if (c == closingBrace && startOffsets.Count > 0) {
					int startOffset = startOffsets.Pop();
					if (startOffset < lastNewLineOffset) {
						newFoldings.Add(new NewFolding(startOffset, i + 1));
					}
				} else if (c == '\n' || c == '\r') {
					lastNewLineOffset = i + 1;
				}
			}
			newFoldings.Sort((a,b) => a.StartOffset.CompareTo(b.StartOffset));
			return newFoldings;
		}
	}
}