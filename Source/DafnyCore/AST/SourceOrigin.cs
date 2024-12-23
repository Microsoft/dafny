using System;
using System.Text;

namespace Microsoft.Dafny;

public class SourceOrigin : OriginWrapper {
  public override Token StartToken => (Token)WrappedToken;

  public override Token EndToken => endToken ?? StartToken;

  public override bool InclusiveEnd => endToken != null;
  public override bool IncludesRange => true;

  public override bool Equals(object obj) {
    if (obj is SourceOrigin other) {
      return StartToken.Equals(other.StartToken) && EndToken.Equals(other.EndToken);
    }
    return false;
  }

  public override int GetHashCode() {
    return HashCode.Combine(StartToken.GetHashCode(), EndToken.GetHashCode());
  }

  public SourceOrigin(Token startToken, Token endToken) : base(startToken) {
    this.endToken = endToken;
  }

  public string PrintOriginal() {
    var token = StartToken;
    var originalString = new StringBuilder();
    originalString.Append(token.val);
    while (token.Next != null && token.pos < EndToken.pos) {
      originalString.Append(token.TrailingTrivia);
      token = token.Next;
      originalString.Append(token.LeadingTrivia);
      originalString.Append(token.val);
    }

    return originalString.ToString();
  }

  public int Length() {
    return EndToken.pos - StartToken.pos;
  }

  // TODO rename to Generated, and Token.NoToken to Token.Generated, and remove AutoGeneratedToken.
  public static IOrigin NoToken => Token.NoToken;
  private readonly Token endToken;

  public override IOrigin WithVal(string newVal) {
    throw new NotImplementedException();
  }

  public override bool IsSourceToken => !ReferenceEquals(this, NoToken);
}