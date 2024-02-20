using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using DafnyCore.Verifier;
using DafnyDriver.Commands;
using Microsoft.Boogie;
using VC;

namespace Microsoft.Dafny;

public class JsonVerificationLogger {
  private TextWriter tw;
  private readonly TextWriter outWriter;
  private readonly ProofDependencyManager depManager;

  public JsonVerificationLogger(ProofDependencyManager depManager, TextWriter outWriter) {
    this.depManager = depManager;
    this.outWriter = outWriter;
  }

  public void Initialize(Dictionary<string, string> parameters) {
    tw = parameters.TryGetValue("LogFileName", out string filename) ? new StreamWriter(filename) : outWriter;
  }

  private static JsonNode SerializeAssertion(AssertCmd assertion) {
    return new JsonObject {
      ["filename"] = assertion.tok.filename,
      ["line"] = assertion.tok.line,
      ["col"] = assertion.tok.col,
      ["description"] = assertion.Description.SuccessDescription
    };
  }

  private JsonNode SerializeProofDependency(ProofDependency dependency) {
    return new JsonObject {
      ["startFile"] = dependency.Range.StartToken.Filepath,
      ["startLine"] = dependency.Range.StartToken.line,
      ["startCol"] = dependency.Range.StartToken.col,
      ["endFile"] = dependency.Range.EndToken.Filepath,
      ["endLine"] = dependency.Range.EndToken.line,
      ["endCol"] = dependency.Range.EndToken.col,
      ["description"] = dependency.Description,
      ["originalText"] = dependency.OriginalString()
    };
  }

  private JsonNode SerializeVcResult(IReadOnlyList<ProofDependency> potentialDependencies, VerificationRunResult vcResult) {
    var result = new JsonObject {
      ["vcNum"] = vcResult.VcNum,
      ["outcome"] = SerializeOutcome(vcResult.Outcome),
      ["runTime"] = SerializeTimeSpan(vcResult.RunTime),
      ["resourceCount"] = vcResult.ResourceCount,
      ["assertions"] = new JsonArray(vcResult.Asserts.Select(SerializeAssertion).ToArray()),
    };
    if (potentialDependencies is not null) {
      var fullDependencySet = depManager.GetOrderedFullDependencies(vcResult.CoveredElements).ToHashSet();
      var unusedDependencies = potentialDependencies.Where(dep => !fullDependencySet.Contains(dep));
      result["coveredElements"] = new JsonArray(fullDependencySet.Select(SerializeProofDependency).ToArray());
      result["uncoveredElements"] = new JsonArray(unusedDependencies.Select(SerializeProofDependency).ToArray());
    }
    return result;
  }

  private static JsonNode SerializeTimeSpan(TimeSpan timeSpan) {
    return timeSpan.ToString();
  }

  private static JsonNode SerializeOutcome(SolverOutcome outcome) {
    return outcome.ToString();
  }
  private static JsonNode SerializeOutcome(VcOutcome outcome) {
    return outcome.ToString();
  }

  private JsonNode SerializeVerificationResult(VerificationScope scope, IReadOnlyList<VerificationRunResult> results) {
    var trackProofDependencies =
      results.All(o => o.Outcome == SolverOutcome.Valid) &&
      results.Any(vcResult => vcResult.CoveredElements.Any());
    var potentialDependencies =
      trackProofDependencies ? depManager.GetPotentialDependenciesForDefinition(scope.Name).ToList() : null;
    var result = new JsonObject {
      ["name"] = scope.Name,
      ["outcome"] = SerializeOutcome(results.Aggregate(VcOutcome.Correct, (o, r) => MergeOutcomes(o, r.Outcome))),
      ["runTime"] = SerializeTimeSpan(TimeSpan.FromSeconds(results.Sum(r => r.RunTime.Seconds))),
      ["resourceCount"] = results.Sum(r => r.ResourceCount),
      ["vcResults"] = new JsonArray(results.Select(r => SerializeVcResult(potentialDependencies, r)).ToArray())
    };
    if (potentialDependencies is not null) {
      result["programElements"] = new JsonArray(potentialDependencies.Select(SerializeProofDependency).ToArray());
    }
    return result;
  }

  /// <summary>
  /// This method is copy pasted from a private Boogie method. It will be public Boogie version > 3.0.11
  /// Then this method can be removed
  public static VcOutcome MergeOutcomes(VcOutcome currentVcOutcome, SolverOutcome newOutcome) {
    switch (newOutcome) {
      case SolverOutcome.Valid:
        return currentVcOutcome;
      case SolverOutcome.Invalid:
        return VcOutcome.Errors;
      case SolverOutcome.OutOfMemory:
        if (currentVcOutcome != VcOutcome.Errors && currentVcOutcome != VcOutcome.Inconclusive) {
          return VcOutcome.OutOfMemory;
        }

        return currentVcOutcome;
      case SolverOutcome.TimeOut:
        if (currentVcOutcome != VcOutcome.Errors && currentVcOutcome != VcOutcome.Inconclusive) {
          return VcOutcome.TimedOut;
        }

        return currentVcOutcome;
      case SolverOutcome.OutOfResource:
        if (currentVcOutcome != VcOutcome.Errors && currentVcOutcome != VcOutcome.Inconclusive) {
          return VcOutcome.OutOfResource;
        }

        return currentVcOutcome;
      case SolverOutcome.Undetermined:
        if (currentVcOutcome != VcOutcome.Errors) {
          return VcOutcome.Inconclusive;
        }

        return currentVcOutcome;
      default:
        Contract.Assert(false);
        throw new cce.UnreachableException();
    }
  }

  private JsonObject SerializeVerificationResults(IEnumerable<IGrouping<VerificationScope, VerificationRunResult>> implementationResults) {
    return new JsonObject {
      ["verificationResults"] = new JsonArray(implementationResults.Select(k =>
        SerializeVerificationResult(k.Key, k.ToList())).ToArray())
    };
  }

  public void LogResults(IEnumerable<IGrouping<VerificationScope, VerificationRunResult>> verificationResults) {
    tw.Write(SerializeVerificationResults(verificationResults).ToJsonString());
  }
}