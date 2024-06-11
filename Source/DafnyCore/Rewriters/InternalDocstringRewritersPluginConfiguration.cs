﻿using System.CommandLine;
using DafnyCore;
using Microsoft.Dafny.Compilers;
using Microsoft.Dafny.Plugins;

namespace Microsoft.Dafny;

public class InternalDocstringRewritersPluginConfiguration : Plugins.PluginConfiguration {
  public static readonly InternalDocstringRewritersPluginConfiguration Singleton = new();
  public static readonly Plugin Plugin = new ConfiguredPlugin(Singleton);

  static InternalDocstringRewritersPluginConfiguration() {
    DafnyOptions.RegisterLegacyBinding(UseJavadocLikeDocstringRewriterOption,
      (options, value) => { options.UseJavadocLikeDocstringRewriter = value; });
    DooFile.RegisterNoChecksNeeded(UseJavadocLikeDocstringRewriterOption, false);
  }

  public static readonly Option<bool> UseJavadocLikeDocstringRewriterOption = new("--javadoclike-docstring-plugin",
    "Rewrite docstrings using a simple Javadoc-to-markdown converter"
  );

  public override DocstringRewriter[] GetDocstringRewriters(DafnyOptions options) {
    if (options.UseJavadocLikeDocstringRewriter) {
      return new DocstringRewriter[] {
        new JavadocLikeDocstringRewriter()
      };
    } else {
      return new DocstringRewriter[] { };
    }
  }
}