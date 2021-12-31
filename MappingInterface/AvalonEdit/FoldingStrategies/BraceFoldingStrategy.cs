using System.Collections.Generic;
using System.Linq;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;

namespace MappingFramework.MappingInterface.AvalonEdit.FoldingStrategies
{
    public class BraceFoldingStrategy : IFoldingStrategy
	{
        private class FoldingCharSet
        {
			public char OpeningBrace { get; }
            public char ClosingBrace { get; }

            public FoldingCharSet(char openingBrace, char closingBrace)
            {
                OpeningBrace = openingBrace;
                ClosingBrace = closingBrace;
            }
        }

        private List<FoldingCharSet> _foldingCharSets;
        
        public BraceFoldingStrategy()
        {
            _foldingCharSets = new List<FoldingCharSet>
            {
                new('{', '}'),
                new('[', ']')
            };
        }
		
		public void UpdateFoldings(FoldingManager manager, TextDocument document)
		{
			IEnumerable<NewFolding> newFoldings = CreateNewFoldings(document);
			manager.UpdateFoldings(newFoldings, -1);
		}
        
		private IEnumerable<NewFolding> CreateNewFoldings(ITextSource document)
		{
			var newFoldings = new List<NewFolding>();
			
			var startOffsets = new Stack<(int, FoldingCharSet)>();

            int lastNewLineOffset = 0;
			for (int i = 0; i < document.TextLength; i++) {
				char c = document.GetCharAt(i);
				
				FoldingCharSet set = _foldingCharSets.FirstOrDefault(f => f.OpeningBrace == c);
                
                if (set != null) {
					startOffsets.Push((i, set));
				} else if (_foldingCharSets.Any(f => f.ClosingBrace == c) && startOffsets.Count > 0) {
					(int startOffset, FoldingCharSet foundSet) = startOffsets.Pop();
					if (startOffset < lastNewLineOffset && foundSet.ClosingBrace == c) {
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