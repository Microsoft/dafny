
using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace Microsoft.Dafny;

public interface IOrigin : Microsoft.Boogie.IToken, IComparable<IOrigin> {
  public RangeToken To(IOrigin end) => new RangeToken(this, end);

  /*
  int kind { get; set; }
  int pos { get; set; }
  int col { get; set; }
  int line { get; set; }
  string val { get; set; }
  bool IsValid { get; }*/
  string Boogie.IToken.filename {
    get => Uri == null ? null : Path.GetFileName(Uri.LocalPath);
    set => throw new NotSupportedException();
  }

  public string ActualFilename => Uri.LocalPath;
  string Filepath => Uri.LocalPath;

  Uri Uri { get; set; }

  /// <summary>
  /// TrailingTrivia contains everything after the token,
  /// until and including two newlines between which there is no commment
  /// All the remaining trivia is for the LeadingTrivia of the next token
  ///
  /// ```
  /// const /*for const*/ x /*for x*/ := /* for := */ 1/* for 1 */
  /// // for 1 again
  /// // for 1 again
  ///
  /// // Two newlines, now all the trivia is for var y
  /// // this line as well.
  /// var y := 2
  /// ```
  /// </summary>
  string TrailingTrivia { get; set; }
  string LeadingTrivia { get; set; }
  IOrigin Next { get; set; } // The next token
  IOrigin Prev { get; set; } // The previous token

  public IOrigin WithVal(string val);  // create a new token by setting the given val.
}

/// <summary>
/// Has one-indexed line and column fields
/// </summary>
public class Token : IOrigin {

  public Token peekedTokens; // Used only internally by Coco when the scanner "peeks" tokens. Normally null at the end of parsing
  public static readonly Token NoToken = new();
  public static readonly Token Cli = new(1, 1);
  public static readonly Token Ide = new(1, 1);
  public Token() : this(0, 0) { }

  public Token(int linenum, int colnum) {
    this.line = linenum;
    this.col = colnum;
    this.val = "";
  }

  public bool IsSourceToken => this != NoToken;
  public int kind { get; set; } // Used by coco, so we can't rename it to Kind

  public string ActualFilename => Filepath;
  public string Filepath => Uri?.LocalPath;
  public Uri Uri { get; set; }

  public int pos { get; set; } // Used by coco, so we can't rename it to Pos

  /// <summary>
  /// One-indexed
  /// </summary>
  public int col { get; set; } // Used by coco, so we can't rename it to Col

  /// <summary>
  /// One-indexed
  /// </summary>
  public int line { get; set; } // Used by coco, so we can't rename it to Line

  public string val { get; set; } // Used by coco, so we can't rename it to Val

  public string LeadingTrivia { get; set; } = "";

  public string TrailingTrivia { get; set; } = "";

  public IOrigin Next { get; set; } // The next token

  public IOrigin Prev { get; set; } // The previous token

  public bool IsValid => this.ActualFilename != null;

  public IOrigin WithVal(string newVal) {
    return new Token {
      pos = pos,
      col = col,
      line = line,
      Prev = Prev,
      Next = Next,
      Uri = Uri,
      kind = kind,
      val = newVal
    };
  }

  public int CompareTo(Boogie.IToken other) {
    if (line != other.line) {
      return line.CompareTo(other.line);
    }
    return col.CompareTo(other.col);
  }

  public override int GetHashCode() {
    return pos;
  }

  public override string ToString() {
    return $"'{val}': {Path.GetFileName(Filepath)}@{pos} - @{line}:{col}";
  }

  public int CompareTo(IOrigin other) {
    if (line != other.line) {
      return line.CompareTo(other.line);
    }
    return col.CompareTo(other.col);
  }
}

public abstract class TokenWrapper : IOrigin {

  public readonly IOrigin WrappedToken;
  protected TokenWrapper(IOrigin wrappedToken) {
    Contract.Requires(wrappedToken != null);
    WrappedToken = wrappedToken;
  }

  public abstract IOrigin WithVal(string newVal);

  public virtual int col {
    get { return WrappedToken.col; }
    set { WrappedToken.col = value; }
  }

  public string ActualFilename => WrappedToken.ActualFilename;

  public virtual string Filepath => WrappedToken.Filepath;

  public Uri Uri {
    get => WrappedToken.Uri;
    set => WrappedToken.Uri = value;
  }

  public bool IsValid {
    get { return WrappedToken.IsValid; }
  }

  public virtual bool IsSourceToken => false;

  public int kind {
    get { return WrappedToken.kind; }
    set { WrappedToken.kind = value; }
  }
  public virtual int line {
    get { return WrappedToken.line; }
    set { WrappedToken.line = value; }
  }
  public virtual int pos {
    get { return WrappedToken.pos; }
    set { WrappedToken.pos = value; }
  }

  public virtual string val {
    get { return WrappedToken.val; }
    set { WrappedToken.val = value; }
  }
  public virtual string LeadingTrivia {
    get { return WrappedToken.LeadingTrivia; }
    set { throw new NotSupportedException(); }
  }
  public virtual string TrailingTrivia {
    get { return WrappedToken.TrailingTrivia; }
    set { throw new NotSupportedException(); }
  }
  public virtual IOrigin Next {
    get { return WrappedToken.Next; }
    set { throw new NotSupportedException(); }
  }
  public virtual IOrigin Prev {
    get { return WrappedToken.Prev; }
    set { throw new NotSupportedException(); }
  }

  public int CompareTo(IOrigin other) {
    return WrappedToken.CompareTo(other);
  }

  public int CompareTo(Boogie.IToken other) {
    return WrappedToken.CompareTo(other);
  }

  /// <summary>
  ///  Removes token wrappings from a given token, so that it returns the bare token
  /// </summary>
  public static IOrigin Unwrap(IOrigin token, bool includeRanges = false) {
    if (token is TokenWrapper wrapper
        && (includeRanges || token is not RangeToken)) {
      return Unwrap(wrapper.WrappedToken);
    }

    if (token is RangeToken rangeToken) {
      return new RangeToken(Unwrap(rangeToken.StartToken), Unwrap(rangeToken.EndToken));
    }

    return token;
  }
}

public static class TokenExtensions {


  public static bool IsSet(this IOrigin token) => token.Uri != null;

  public static string TokenToString(this IOrigin tok, DafnyOptions options) {
    if (tok == Token.Cli) {
      return "CLI";
    }

    if (tok.Uri == null) {
      return $"({tok.line},{tok.col - 1})";
    }

    var currentDirectory = Directory.GetCurrentDirectory();
    string filename = tok.Uri.Scheme switch {
      "stdin" => "<stdin>",
      "transcript" => Path.GetFileName(tok.Filepath),
      _ => options.UseBaseNameForFileName
        ? Path.GetFileName(tok.Filepath)
        : (tok.Filepath.StartsWith(currentDirectory) ? Path.GetRelativePath(currentDirectory, tok.Filepath) : tok.Filepath)
    };

    return $"{filename}({tok.line},{tok.col - 1})";
  }

  public static RangeToken ToRange(this IOrigin token) {
    if (token is BoogieRangeToken boogieRangeToken) {
      return new RangeToken(boogieRangeToken.StartToken, boogieRangeToken.EndToken);
    }

    if (token is NestedToken nestedToken) {
      return ToRange(nestedToken.Outer);
    }
    return token as RangeToken ?? new RangeToken(token, token);
  }
}

public class BoogieRangeToken : TokenWrapper {
  // The wrapped token is the startTok
  public IOrigin StartToken { get; }
  public IOrigin EndToken { get; }

  /// <summary>
  /// If only a single position is used to refer to this piece of code, this position is the best
  /// </summary>
  public IOrigin Center { get; }

  // Used for range reporting
  public override string val => new(' ', Math.Max(EndToken.pos + EndToken.val.Length - pos, 1));

  public BoogieRangeToken(IOrigin startTok, IOrigin endTok, IOrigin center) : base(
    center ?? startTok) {
    StartToken = startTok;
    EndToken = endTok;
    Center = center;
  }

  public override IOrigin WithVal(string newVal) {
    return this;
  }

  public string PrintOriginal() {
    return new RangeToken(StartToken, EndToken).PrintOriginal();
  }
}

public class NestedToken : TokenWrapper {
  public NestedToken(IOrigin outer, IOrigin inner, string message = null)
    : base(outer) {
    Contract.Requires(outer != null);
    Contract.Requires(inner != null);
    Inner = inner;
    this.Message = message;
  }
  public IOrigin Outer { get { return WrappedToken; } }
  public readonly IOrigin Inner;
  public readonly string Message;

  public override IOrigin WithVal(string newVal) {
    return this;
  }
}

/// <summary>
/// A token wrapper used to produce better type checking errors
/// for quantified variables. See QuantifierVar.ExtractSingleRange()
/// </summary>
public class QuantifiedVariableDomainToken : TokenWrapper {
  public QuantifiedVariableDomainToken(IOrigin wrappedToken)
    : base(wrappedToken) {
    Contract.Requires(wrappedToken != null);
  }

  public override string val {
    get { return WrappedToken.val; }
    set { WrappedToken.val = value; }
  }

  public override IOrigin WithVal(string newVal) {
    return new QuantifiedVariableDomainToken((WrappedToken.WithVal(newVal)));
  }
}

/// <summary>
/// A token wrapper used to produce better type checking errors
/// for quantified variables. See QuantifierVar.ExtractSingleRange()
/// </summary>
public class QuantifiedVariableRangeToken : TokenWrapper {
  public QuantifiedVariableRangeToken(IOrigin wrappedToken)
    : base(wrappedToken) {
    Contract.Requires(wrappedToken != null);
  }

  public override string val {
    get { return WrappedToken.val; }
    set { WrappedToken.val = value; }
  }

  public override IOrigin WithVal(string newVal) {
    return new QuantifiedVariableRangeToken(WrappedToken.WithVal(newVal));
  }
}
